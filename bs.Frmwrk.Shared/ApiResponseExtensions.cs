using bs.Frmwrk.Core.ViewModels.Api;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Shared
{
    public static class ApiResponseExtensions
    {
        public static void ToLog(this Dictionary<string, ApiResponse> resultList, ILogger? logger = null)
        {
            foreach (var result in resultList)
            {
                if (result.Value.Success)
                { logger?.LogDebug($"'{result.Key}' registered succeffulli in DI container"); }
                else { logger?.LogError($"'{result.Key}' error registering service in DI container: {result.Value.ErrorMessage}"); }
            }
        }

        public static void ToLog(this Dictionary<string, ApiResponse> resultList, Action<string> logSuccess = null, Action<string> logError = null, string successMessage = "registered succeffulli in DI container", string errorMessage= "error registering service in DI container")
        {
            foreach (var result in resultList)
            {
                if (result.Value.Success)
                    logSuccess?.Invoke($"'{result.Key}' {successMessage}");
                else { 
                    logSuccess?.Invoke($"'{result.Key}' {errorMessage}: {result.Value.ErrorMessage}");
                }
            }
        }


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