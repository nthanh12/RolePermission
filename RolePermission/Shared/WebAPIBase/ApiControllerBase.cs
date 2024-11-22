using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RolePermission.Shared.Consts.Exceptions;

namespace RolePermission.Shared.WebAPIBase
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected ILogger _logger;
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        [NonAction]
        public ApiResponse OkException(Exception ex)
        {
            var errorMessage = ex.Message.ToString();
            var errorCode = ErrorCode.GetErrorCode(errorMessage);
            if (!errorCode.ToString().IsNullOrEmpty() || errorCode == 0)
            {
                return new ApiResponse(
                        WebAPIBase.StatusCode.Error,
                        null,
                        errorCode,
                        errorMessage
                );
            }
            else
            {
                _logger?.LogError(ex, $"{ex.GetType()}: {ex}, ErrorCode = {ErrorCode.InternalServerError}, Message: {ErrorMessage.InternalServerError}");
            }
            return new ApiResponse(
                WebAPIBase.StatusCode.Error,
                null,
                ErrorCode.InternalServerError,
                ErrorMessage.InternalServerError
                );
        }
    }
}
