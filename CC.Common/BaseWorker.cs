using CC.Common.Models;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

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
        /// Create Logger instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static ILogger<T> GetLogger<T>()
        {
            var nf = new SerilogLoggerFactory();
            return nf.CreateLogger<T>();
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
