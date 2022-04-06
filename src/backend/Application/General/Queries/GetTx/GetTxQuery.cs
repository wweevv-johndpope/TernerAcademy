using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.General.Queries.GetTx
{
    public class GetTxQuery : IRequest<Result<string>>
    {
        public string TxHash { get; set; }

        public class GetTxQueryHandler : IRequestHandler<GetTxQuery, Result<string>>
        {
            private readonly IRpcApiService _rpcApiService;

            public GetTxQueryHandler(IRpcApiService rpcApiService)
            {
                _rpcApiService = rpcApiService;
            }

            public async Task<Result<string>> Handle(GetTxQuery request, CancellationToken cancellationToken)
            {
                var tx = _rpcApiService.GetTransactionByHash(request.TxHash);
                var serialTx = JsonConvert.SerializeObject(tx.Result, Formatting.Indented);
                return await Result<string>.SuccessAsync(data: serialTx);
            }
        }
    }

}
