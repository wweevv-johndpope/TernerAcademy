using Application.Common.Constants;
using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Wallets.Queries.Get
{
    public class GetWalletQuery : IRequest<Result<WalletInfoDto>>
    {
        public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, Result<WalletInfoDto>>
        {
            private readonly IRpcApiService _rpcApiService;
            private readonly IConfiguration _configuration;
            public GetWalletQueryHandler(IRpcApiService rpcApiService, IConfiguration configuration)
            {
                _rpcApiService = rpcApiService;
                _configuration = configuration;
            }

            public async Task<Result<WalletInfoDto>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
            {
                var instructorWalletAddress = _configuration.GetValue<string>(EnvironmentVariableKeys.WALLETSTUDENT);
                var response = _rpcApiService.GetBalance(instructorWalletAddress);

                if (response.Error != null) return await Result<WalletInfoDto>.FailAsync(response.Error.Message);

                var amount = response.Result.ToTFuel();
                return await Result<WalletInfoDto>.SuccessAsync(new WalletInfoDto()
                {
                    Address = instructorWalletAddress,
                    Balance = amount
                });
            }
        }
    }
}
