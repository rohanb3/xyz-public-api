using System;

namespace Xyzies.TWC.Public.Data.Providers.Behaviour
{
    public interface IAccessPointProvider<TProvider> : IDisposable where TProvider : class, IDisposable
    {
        /// <summary>
        /// Data access provider
        /// </summary>
        TProvider Provider { get; }
    }
}
