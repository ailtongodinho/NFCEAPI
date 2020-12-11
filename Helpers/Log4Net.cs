using System;
using log4net;

namespace NFCE.API.Helpers
{
    public class Log4Net
    {
        private readonly ILog log = log4net.LogManager.GetLogger(typeof(Log4Net));
    }
}