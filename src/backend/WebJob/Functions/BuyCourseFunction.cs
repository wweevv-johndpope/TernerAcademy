using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.QueueMessages;
using Application.Queues.Commands;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebJob.Base;

namespace WebJob.Functions
{
    public class BuyCourseFunction : FunctionBase
    {
        public BuyCourseFunction(IMediator mediator, ICallContext context) : base(mediator, context)
        {
        }

        [FunctionName("BuyCourse_PaymentCashout")]
        public async Task PaymentCashout([QueueTrigger(QueueNames.PaymentCashout)] PaymentCashoutQueueMessage message, ILogger log, ExecutionContext context)
        {
            PaymentCashoutCommand commandArg = new() { SubscriptionId = message.SubscriptionId };
            await ExecuteAsync<PaymentCashoutCommand, IResult>(context, commandArg);
        }

        [FunctionName("BuyCourse_PaymentDev")]
        public async Task PaymentDev([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            PaymentDevCommand commandArg = new();
            await ExecuteAsync<PaymentDevCommand, IResult>(context, commandArg);
        }

        [FunctionName("BuyCourse_PaymentBurn")]
        public async Task PaymentBurn([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            PaymentBurnCommand commandArg = new();
            await ExecuteAsync<PaymentBurnCommand, IResult>(context, commandArg);
        }
    }
}
