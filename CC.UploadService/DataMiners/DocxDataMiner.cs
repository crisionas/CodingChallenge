using CC.Common;
using CC.UploadService.Interfaces;
using System.Xml;

namespace CC.UploadService.DataMiners
{
    public class DocxDataMiner : DataMiner
    {
        public DocxDataMiner(ILogger<DocxDataMiner> logger, IFileRepository repository, IUserRequestSettings userRequest) : base(logger, repository, userRequest)
        {
        }

        /// <summary>
        /// Parse Docx data from memoryStream
        /// </summary>
        /// <param name="stream">MemoryStream</param>
        /// <returns>string</returns>
        protected override string? ParseData(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var doc = new XmlDocument();
            doc.Load(stream);
            var content = doc.DocumentElement?.InnerText;
            return content;
        }

        /// <summary>
        /// Analyze the data particular to CSV files
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>Task</returns>
        protected override Task AnalyzeData(string data)
        {
            return Task.CompletedTask;
        }
    }
}
