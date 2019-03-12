using System;

namespace TwoCaptcha.Utils
{
    internal static class Preconditions
    {
        //Objects
        public static void NotNull<T>(T obj, string name, string msg = null) where T : class
        {
            if (obj == null) throw CreateNotNullException(name, msg);
        }

        private static ArgumentNullException CreateNotNullException(string name, string msg) =>
            msg == null ? new ArgumentNullException(name) : new ArgumentNullException(name, msg);
    }
}