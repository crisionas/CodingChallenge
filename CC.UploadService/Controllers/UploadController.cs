using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CC.UploadService.Controllers
{
    [Authorize]
    [Route("upload")]
    [ApiController]
    public class UploadController : BaseController
    {
        private readonly IValidator<FileUploadRequest> _fileReqValidator;
        private readonly IFileUploaderWorker _fileUploaderWorker;

        private const int MaxValue = 1073741824; // Equal to 1Gb

        public UploadController(IValidator<FileUploadRequest> fileReqValidator, IFileUploaderWorker fileUploaderWorker)
        {
            _fileReqValidator = fileReqValidator;
            _fileUploaderWorker = fileUploaderWorker;
        }

        [SwaggerOperation(Summary = "Upload file. Supported files: CSV or PDF")]
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = MaxValue, MultipartBodyLengthLimit = MaxValue)]
        [DisableRequestSizeLimit]
        [ProducesResponseType(204, Type = typeof(void))]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
        {
            await _fileUploaderWorker.UploadFileAsync(request);
            return NoContent();
        }
    }
}
