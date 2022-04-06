using Application.Common.Dtos.Response;

namespace Application.Common.Interfaces
{
    public interface IThetaVideoService
    {
        ThetaVideoProgressResultDto Transcode(string sourceUri);
        ThetaVideoProgressResultDto GetVideoProgress(string id);
    }
}
