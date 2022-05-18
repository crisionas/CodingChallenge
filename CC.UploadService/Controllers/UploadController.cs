using CC.Common;
using CC.UploadService.Models.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CC.UploadService.Controllers
{
    //[Authorize]
    [Route("upload")]
    [ApiController]
    public class UploadController : BaseController
    {
        private readonly IValidator<FileUploadRequest> _fileReqValidator;

        public UploadController(IValidator<FileUploadRequest> fileReqValidator)
        {
            _fileReqValidator = fileReqValidator;
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
        {
            var validationResult = await _fileReqValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return GetResponseFromValidationResult(validationResult);
            return Ok("test");
        }
    }
}
