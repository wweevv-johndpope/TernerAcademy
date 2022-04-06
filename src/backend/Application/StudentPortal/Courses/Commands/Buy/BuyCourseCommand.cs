using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.Commands.Buy
{
    public class BuyCourseCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }

        public class BuyCourseCommandHandler : IRequestHandler<BuyCourseCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDomainEventService _domainEventService;
            private readonly IRpcApiService _rpcApiService;
            private readonly IConfiguration _configuration;
            private readonly IAzureStorageQueueService _queueService;
            public BuyCourseCommandHandler(ICallContext context, IApplicationDbContext dbContext, IDomainEventService domainEventService, IRpcApiService rpcApiService, IConfiguration configuration, IAzureStorageQueueService queueService)
            {
                _context = context;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
                _rpcApiService = rpcApiService;
                _configuration = configuration;
                _queueService = queueService;
            }

            public async Task<IResult> Handle(BuyCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CourseId && x.ListingStatus == CourseListingStatus.Approved);

                if (course == null) return await Result.FailAsync("Course not found.");

                var subscription = await _dbContext.CourseSubscriptions.AsQueryable().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.StudentId == _context.UserId);

                if (subscription != null) return await Result.FailAsync("You've already bought this course.");

                BigInteger valueInWei = (BigInteger)(course.PriceInTFuel * Math.Pow(10, 18));
                var valueInHex = valueInWei.ToHex();

                var studentWallet = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETSTUDENT);
                var platformWallet = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETPLATFORM);

                _rpcApiService.GetTransactionCount(studentWallet);

                var sendPaymentResponse = _rpcApiService.SendPayment(studentWallet, platformWallet, valueInHex);
                if (sendPaymentResponse.Error != null) return await Result.FailAsync(sendPaymentResponse.Error.Message);

                do
                {
                    Task.Delay(2000).Wait();

                    var txResult = _rpcApiService.GetTransactionByHash(sendPaymentResponse.Result);

                    if (txResult.Error != null) return await Result.FailAsync(txResult.Error.Message);

                    if (!string.IsNullOrEmpty(txResult.Result.BlockHash) && !string.IsNullOrEmpty(txResult.Result.BlockNumber))
                        break;

                    Task.Delay(2000).Wait();
                } while (true);

                subscription = new CourseSubscription()
                {
                    CourseId = request.CourseId,
                    StudentId = _context.UserId,
                    Price = course.PriceInTFuel,
                    BuyTx = sendPaymentResponse.Result
                };

                _dbContext.CourseSubscriptions.Add(subscription);
                await _dbContext.SaveChangesAsync();

                await _domainEventService.Publish(new NewCourseSubscriptionEvent(request.CourseId));
                await _domainEventService.Publish(new NewCourseReviewEvent(request.CourseId));

                _queueService.InsertMessage(QueueNames.PaymentCashout, JsonConvert.SerializeObject(new PaymentCashoutQueueMessage() { SubscriptionId = subscription.Id }));

                return await Result.SuccessAsync();
            }
        }
    }
}
