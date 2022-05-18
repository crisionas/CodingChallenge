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

        /// <summary>
        /// Write Response
        /// </summary>
        /// <param name="response">BaseResponse</param>
        /// <param name="e">Exception</param>
        /// <param name="text">Error text</param>
        protected void WriteBaseResponse(BaseResponse? response, Exception e, string text)
        {
            Logger.LogError(e, text);
            if (response == null)
                return;
            response.ErrorMessage = e.Message;
        }
    }
}
