using CC.Common.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CC.Common
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region protected methods

        /// <summary>
        /// Prepare IActionResult with Ok Response
        /// </summary>
        /// <param name="response">Object</param>
        /// <returns>IActionResult</returns>
        protected IActionResult PrepareActionResult(object? response)
        {
            return response == null ? NoContent() : response is BaseResponse baseResponse ? GetResponse(baseResponse) : Ok(response);
        }

        /// <summary>
        /// Prepare IActionResult with NoContent response
        /// </summary>
        /// <param name="response">Object</param>
        /// <returns>IActionResult</returns>
        protected IActionResult PrepareNoContentResult(BaseResponse response) => response.HasError ? GetResponse(response) : NoContent();

        #endregion

        #region private methods

        /// <summary>
        /// Return a IActionResult based if baseResponse has error or not
        /// </summary>
        /// <param name="response">BaseResponse</param>
        /// <returns>IActionResult</returns>
        private IActionResult GetResponse(BaseResponse response)
        {
            if (response.HasError)
                return BadRequest(response);
            return Ok(response);
        }

        #endregion

    }
}