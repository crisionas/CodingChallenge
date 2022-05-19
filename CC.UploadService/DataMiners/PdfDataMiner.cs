using CC.Common;
using CC.UploadService.Interfaces;
using Hangfire;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace CC.UploadService.DataMiners
{
    public class PdfDataMiner : DataMiner
    {
        public PdfDataMiner(ILogger<PdfDataMiner> logger, IFileRepository repository,
            IUserRequestSettings userRequest, IBackgroundJobClient jobClient, IEmailSenderWorker emailWorker)
            : base(logger, repository, userRequest, jobClient, emailWorker)
        {
        }

        /// <summary>
        /// Parse Docx data from memoryStream
        /// </summary>
        /// <param name="byteArray">Byte array</param>
        /// <returns>string</returns>
        public override void ParseData(byte[] byteArray)
        {
            var ms = new MemoryStream(byteArray);
            var reader = new PdfReader(ms);
            var pdfDoc = new PdfDocument(reader);
            for (var page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                var mainPage = pdfDoc.GetPage(page);
                ParsedString += PdfTextExtractor.GetTextFromPage(mainPage);
            }
            reader.Close();
        }

        /// <summary>
        /// Analyze the data particular to CSV files
        /// </summary>
        /// <returns>Task</returns>
        public override void AnalyzeData()
        {
        }
    }
}
