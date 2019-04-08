using System;
using Newtonsoft.Json;
using ProfileApi.Models;

namespace ProfileApi.Services.LoggerService
{
    public static class LoggingExtensions
    {
        public static string ToErrorResponse(this Exception ex, int errorCode)
        {
            return JsonConvert.SerializeObject(new CustomErrorResponse
            {
                ErrorCode = errorCode,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}