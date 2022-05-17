using CC.Common.Models;
using Microsoft.Extensions.Logging;

namespace CC.Common
{
    public class BaseWorker
    {
        protected readonly ILogger Logger;

        public BaseWorker(ILogger logger)
        {
            Logger = logger;
        }

        protected void WriteBaseResponse(BaseResponse? response, Exception e, string text)
        {
            Logger.LogError(e, text);
            if (response == null)
                return;
            response.ErrorMessage = e.Message;
        }
    }
}
