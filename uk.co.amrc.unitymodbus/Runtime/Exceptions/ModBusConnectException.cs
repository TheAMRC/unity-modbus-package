using System;

namespace AMRC.UnityModBus.Exceptions
{
    public class ModBusConnectException : Exception
    {
        public ModBusConnectException() { }

        public ModBusConnectException(string message) : base(message) { }

        public ModBusConnectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
