using System;
using Microsoft.AspNetCore.Hosting;

namespace Xyzies.TWC.Public
{
    /// <summary>
    /// Extension methods for Microsoft.AspNetCore.Hosting.IHostingEnvironment.
    /// </summary>
    public static class HostingEnvironmentExtension
    {
        /// <summary>
        /// Checks if the current hosting environment name is Microsoft.AspNetCore.Hosting.EnvironmentName.Development.
        /// </summary>
        /// <param name="hostingEnvironment">An instance of Microsoft.AspNetCore.Hosting.IHostingEnvironment.</param>
        /// <returns>True if the environment name is Microsoft.AspNetCore.Hosting.EnvironmentName.Local, otherwise false.</returns>
        public static bool IsLocal(this IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment == null)
            {
                throw new NullReferenceException(nameof(hostingEnvironment));
            }

            return hostingEnvironment.EnvironmentName.ToLower().Equals("local");
        }
    }
}
