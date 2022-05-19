using CC.Common;
using CC.UploadService.Interfaces;
using Hangfire;
using System.Text;

namespace CC.UploadService.DataMiners
{
    public class CsvDataMiner : DataMiner
    {
        public CsvDataMiner(ILogger<CsvDataMiner> logger, IFileRepository repository,
            IUserRequestSettings userRequest, IBackgroundJobClient jobClient) : base(logger, repository, userRequest, jobClient)
        {
        }

        public override void ParseData(byte[] byteArray)
        {
            ParsedString = Encoding.UTF8.GetString(byteArray);
        }

        public override void AnalyzeData()
        {
        }
    }
}
