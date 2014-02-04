using System;

using log4net;

namespace MapGenerator.Utils.Logging
{
    public class Logger
    {
        private ILog _logger;

        public Logger(Type type)
        {
            _logger = LogManager.GetLogger(type);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }
    }
}
