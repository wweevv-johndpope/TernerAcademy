using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queues.Commands
{
    public class PaymentCashoutCommand : IRequest<IResult>
    {
        public int SubscriptionId { get; set; }

        public class PaymentCashoutCommandHandler : IRequestHandler<PaymentCashoutCommand, IResult>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IRpcApiService _rpcApiService;
            private readonly IConfiguration _configuration;

            public PaymentCashoutCommandHandler(IRpcApiService rpcApiService, IConfiguration configuration, IApplicationDbContext dbContext)
            {
                _rpcApiService = rpcApiService;
                _configuration = configuration;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(PaymentCashoutCommand request, CancellationToken cancellationToken)
            {
                var subscription = await _dbContext.CourseSubscriptions.AsQueryable().Include(x => x.Course).ThenInclude(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == request.SubscriptionId);

                if (subscription == null) return await Result.FailAsync("Subscription not found.");

                if (!string.IsNullOrEmpty(subscription.CashoutPaymentTx))
                    return await Result.FailAsync("Cashout has been processed.");

                var platformWallet = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETPLATFORM);
                var instructorWallet = subscription.Course.Instructor.WalletAddress;

                var value = subscription.Price * 0.90;
                BigInteger valueInWei = (BigInteger)(value * Math.Pow(10, 18));
                var valueInHex = valueInWei.ToHex();

                string txHash = "";
                do
                {
                    _rpcApiService.GetTransactionCount(platformWallet);
                    _rpcApiService.GetTransactionCount(instructorWallet);

                    var sendPaymentResponse = _rpcApiService.SendPayment(platformWallet, instructorWallet, valueInHex);
                    if (sendPaymentResponse.Error != null)
                    {
                        Task.Delay(5000).Wait();
                        continue;
                    }

                    txHash = sendPaymentResponse.Result;
                    break;
                } while (true);

                do
                {
                    Task.Delay(2000).Wait();

                    var txResult = _rpcApiService.GetTransactionByHash(txHash);

                    if (txResult.Error != null) return await Result.FailAsync(txResult.Error.Message);

                    if (!string.IsNullOrEmpty(txResult.Result.BlockHash) && !string.IsNullOrEmpty(txResult.Result.BlockNumber))
                        break;

                    Task.Delay(2000).Wait();
                } while (true);

                subscription.AmountCashout = value;
                subscription.CashoutPaymentTx = txHash;
                _dbContext.CourseSubscriptions.Update(subscription);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
