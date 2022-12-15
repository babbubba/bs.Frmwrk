using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Application.Middlewares
{
    /// <summary>
    /// Enrich log with extra info like RemoteIpAddress and current user
    /// </summary>
    public class SerilogMiddleware
    {
        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <exception cref="System.ArgumentNullException">next</exception>
        public SerilogMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        /// <summary>
        /// Invokes the specified HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <exception cref="System.ArgumentNullException">httpContext</exception>
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            //var sw = Stopwatch.StartNew();
            try
            {
                LogContext.PushProperty("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
                LogContext.PushProperty("RequestHost", httpContext.Request.Host);
                LogContext.PushProperty("RequestProtocol", httpContext.Request.Protocol);

                // Add user if exists
                if (httpContext.User.Identity != null)
                {
                    var userIdValue = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    LogContext.PushProperty("UserId", userIdValue ?? "*");

                    var userName = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                    LogContext.PushProperty("UserName", userName ?? "*");
                }
                await _next(httpContext);
            }
            catch (Exception)
            {
            }
        }
    }
}
