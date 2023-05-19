using bs.Frmwrk.Core.ViewModels.Api;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Shared
{
    public static class ApiResponseExtensions
    {
        public static IApiResponse<T> SetError<T>(this IApiResponse<T> response, string errorMessage, long? errorCode = null, ILogger? logger = null, Exception? ex = null)
        {
            response.Success = false;
            response.ErrorMessage = errorMessage;
            response.ErrorCode = errorCode;

            logger?.LogError(ex, $"{(errorCode != null ? $"[{errorCode}] " : "")}{errorMessage}");

            return response;
        }

        public static IApiResponse SetError(this IApiResponse response, string errorMessage, long? errorCode = null, ILogger? logger = null, Exception? ex = null)
        {
            response.Success = false;
            response.ErrorMessage = errorMessage;
            response.ErrorCode = errorCode;

            logger?.LogError(ex, $"{(errorCode != null ? $"[{errorCode}] " : "")}{errorMessage}");

            return response;
        }

        public static IApiPagedResponse<T> SetError<T>(this IApiPagedResponse<T> response, string errorMessage, long? errorCode = null, ILogger? logger = null, Exception? ex = null)
        {
            response.Success = false;
            response.ErrorMessage = errorMessage;
            response.ErrorCode = errorCode;

            logger?.LogError(ex, $"{(errorCode != null ? $"[{errorCode}] " : "")}{errorMessage}");

            return response;
        }




        
    }
}