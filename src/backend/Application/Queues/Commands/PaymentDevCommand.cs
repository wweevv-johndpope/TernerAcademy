using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queues.Commands
{
    public class PaymentDevCommand : IRequest<IResult>
    {
        public class PaymentDevCommandHandler : IRequestHandler<PaymentDevCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IRpcApiService _rpcApiService;
            private readonly IConfiguration _configuration;
            private readonly IAzureStorageQueueService _queueService;
            public PaymentDevCommandHandler(IRpcApiService rpcApiService, IConfiguration configuration, IApplicationDbContext dbContext, IAzureStorageQueueService queueService)
            {
                _rpcApiService = rpcApiService;
                _configuration = configuration;
                _dbContext = dbContext;
                _queueService = queueService;
            }

            public async Task<IResult> Handle(PaymentDevCommand request, CancellationToken cancellationToken)
            {
                var subscriptions = await _dbContext.CourseSubscriptions.AsQueryable().Where(x => !string.IsNullOrEmpty(x.CashoutPaymentTx) && string.IsNullOrEmpty(x.SendToDevTx)).Take(10).ToListAsync();

                if (!subscriptions.Any()) return await Result.SuccessAsync();

                var platformWallet = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETPLATFORM);
                var devWallet = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETDEV);

                var value = subscriptions.Sum(x => x.Price) * 0.03;
                BigInteger valueInWei = (BigInteger)(value * Math.Pow(10, 18));
                var valueInHex = valueInWei.ToHex();

                string txHash = "";
                int execCount = 0;

                do
                {
                    _rpcApiService.GetTransactionCount(platformWallet);
                    _rpcApiService.GetTransactionCount(devWallet);

                    var sendPaymentResponse = _rpcApiService.SendPayment(platformWallet, devWallet, valueInHex);
                    if (sendPaymentResponse.Error != null)
                    {
                        if (execCount == 3) return await Result.FailAsync();
                        Task.Delay(5000).Wait();
                        continue;
                    }
                    if (sendPaymentResponse.Error != null) return await Result.FailAsync();

                    txHash = sendPaymentResponse.Result;
                    break;
                } while (true);

                foreach (var subscription in subscriptions)
                {
                    subscription.PriceDev = subscription.Price * 0.03;
                    subscription.SendToDevTx = txHash;
                    _dbContext.CourseSubscriptions.Update(subscription);
                }

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
