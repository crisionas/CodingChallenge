using CC.Common;
using CC.UploadService.Interfaces;
using System.Text;

namespace CC.UploadService.DataMiners
{
    public class CsvDataMiner : DataMiner
    {
        public CsvDataMiner(ILogger<CsvDataMiner> logger, IFileRepository repository, IUserRequestSettings userRequest) : base(logger, repository, userRequest)
        {
        }

        protected override string? ParseData(MemoryStream stream)
        {
            var byteArray = stream.ToArray();
            var str = Encoding.UTF8.GetString(byteArray);
            return str;
        }

        protected override Task AnalyzeData(string data)
        {
            return Task.CompletedTask;
        }
    }
}
