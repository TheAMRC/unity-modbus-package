using System;

namespace ModBus.Exceptions
{
    public class ModBusIOException : Exception
    {
        public ModBusIOException() { }

        public ModBusIOException(string message) : base(message) { }

        public ModBusIOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

