using CC.UploadService.Models.Responses;

namespace CC.UploadService.Interfaces
{
    public interface ITrackWorker
    {
        Task<TrackStatusResponse> GetTrackStatusAsync(string trackId);
    }
}
