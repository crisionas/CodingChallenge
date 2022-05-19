using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Responses;
using Hangfire;

namespace CC.UploadService.Workers
{
    public class TrackWorker : BaseWorker, ITrackWorker
    {
        public TrackWorker(ILogger<TrackWorker> logger) : base(logger)
        {
        }

        public async Task<TrackStatusResponse> GetTrackStatusAsync(string trackId)
        {
            var response = new TrackStatusResponse();
            await Task.Run(() =>
            {
                try
                {
                    var connection = JobStorage.Current.GetConnection();
                    var jobData = connection.GetJobData(trackId);
                    response.Status = jobData.State;
                }
                catch (Exception e)
                {
                    WriteBaseResponse(response, e, "GetTrackStatusAsync error");
                }
            });
            return response;
        }
    }
}
