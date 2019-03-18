using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet_code_challenge.Helpers
{
    public interface ILogger
    {
        void LogDebug(string text);
        void LogInfo(string text);
        void LogWarn(string text);
        void LogError(string text);
    }
}
