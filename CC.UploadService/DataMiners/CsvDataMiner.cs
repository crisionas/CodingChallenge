using CC.Common;
using CC.UploadService.Interfaces;
using Hangfire;
using System.Text;

namespace CC.UploadService.DataMiners
{
    public class CsvDataMiner : DataMiner
    {
        public CsvDataMiner(ILogger<CsvDataMiner> logger, IFileRepository repository,
            IUserRequestSettings userRequest, IBackgroundJobClient jobClient, IEmailSenderWorker emailWorker) 
            : base(logger, repository, userRequest, jobClient, emailWorker)
        {
        }

        /// <summary>
        /// Parse Csv data from memoryStream
        /// </summary>
        /// <param name="byteArray">Byte array</param>
        /// <returns>string</returns>
        public override void ParseData(byte[] byteArray)
        {
            ParsedString = Encoding.UTF8.GetString(byteArray);
        }

        /// <summary>
        /// Analyze the data particular to CSV files
        /// </summary>
        /// <returns>Task</returns>
        public override void AnalyzeData()
        {
            // Ignore
        }
    }
}
