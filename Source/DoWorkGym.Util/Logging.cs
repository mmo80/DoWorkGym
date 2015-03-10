using System;
using NLog;

namespace DoWorkGym.Util
{
    /*
     http://stackoverflow.com/questions/576185/logging-best-practices
     */
    public static class Logging
    {
        private static readonly Logger Logger = LogManager.GetLogger("TrainingAppLogger");


        public static void Error(string message)
        {
            Logger.Error(message);
        }
        public static void Error(string message, Exception exception)
        {
            Logger.ErrorException(message, exception);
        }
        public static void Error(Exception exception)
        {
            Logger.ErrorException(exception.Message, exception);
        }

        public static void Warn(string message)
        {
            Logger.Warn(message);
        }
        public static void Warn(string message, Exception exception)
        {
            Logger.Warn(message, exception);
        }
        public static void Warn(Exception exception)
        {
            Logger.Warn(exception);
        }

        public static void Info(EventType eventType, string message)
        {
            Logger.Info(string.Format("[{0}] {1}", eventType, message));
        }

        public static void Info(string message)
        {
            Logger.Info(message);
        }

        public static void Debug(EventType eventType, string message)
        {
            Logger.Debug(string.Format("[{0}] {1}", eventType, message));
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Trace(string message)
        {
            Logger.Trace(message);
        }
    }


    public enum EventType
    {
        Login,
        Logout,
        NewUser,
        ResetPassword
    }
}
