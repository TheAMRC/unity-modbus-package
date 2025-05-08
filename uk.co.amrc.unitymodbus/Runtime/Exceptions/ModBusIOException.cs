using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

