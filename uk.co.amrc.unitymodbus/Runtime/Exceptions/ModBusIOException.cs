/*
 * Copyright (c) University of Sheffield AMRC 2025.
 * See LICENSE file
 */

using System;

namespace AMRC.UnityModBus.Exceptions
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
