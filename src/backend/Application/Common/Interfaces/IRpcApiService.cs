using Application.Common.Dtos.Response;

namespace Application.Common.Interfaces
{
    public interface IRpcApiService
    {
        RpcApiResultDto<string> GetTransactionCount(string address);
        RpcApiResultDto<RpcApiTransactionResultDto> GetTransactionByHash(string txHash);
        RpcApiResultDto<string> GetBalance(string address);
        RpcApiResultDto<string> SendPayment(string from, string to, string valueInHex);
    }
}