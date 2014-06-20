using System;

namespace Template.Components.Logging
{
    public interface ILogger : IDisposable
    {
        void Log(String message);
    }
}
