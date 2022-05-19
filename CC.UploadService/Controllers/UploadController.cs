using CC.Common;
using CC.UploadService.Interfaces;
using CC.UploadService.Models.Requests;
using CC.UploadService.Models.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = MaxValue, MultipartBodyLengthLimit = MaxValue)]
        [DisableRequestSizeLimit]
        [ProducesResponseType(200, Type = typeof(FileUploadResponse))]
        [ProducesResponseType(400, Type = typeof(FileUploadResponse))]

        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
        {
            var validationResult = await _fileReqValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);

            var response = await _fileUploaderWorker.UploadFileAsync(request);
            return PrepareActionResult(response);
        }
    }
}
