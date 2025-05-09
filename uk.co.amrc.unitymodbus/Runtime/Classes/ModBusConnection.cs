using UnityModBus.Exceptions;
using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UnityModBus.Classes
{
    /// <summary>
    /// A class representing a connection to a device over ModBus TCP
    /// </summary>
    public class ModBusConnection
    {
        /// <summary>
        /// Called upon a successful connection to a modbus device
        /// </summary>
        public Action onConnect;

        /// <summary>
        /// Called whenever an active connection is dropped
        /// </summary>
        public Action onDisconnect;

        private const int CONNECTION_TIMEOUT_MS = 1000;

        private readonly string _remoteIpAddress;
        private readonly ushort _port;

        private readonly TcpClient _tcpClient = new();
        private IModbusMaster _master;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="remoteIpAddress">IP address of modbus device to connect to over TCP</param>
        /// <param name="port">Open modbus port on remote device</param>
        public ModBusConnection(string remoteIpAddress, ushort port)
        {
            _remoteIpAddress = remoteIpAddress;
            _port = port;
        }

        /// <summary>
        /// Sets up a ModBus connection over TCP
        /// </summary>
        /// <exception cref="ModBusConnectException">Thrown if device could not be connected to</exception>
        public void OpenModBusConnection()
        {
            var factory = new ModbusFactory();
            var result = _tcpClient.BeginConnect(_remoteIpAddress, _port, null, null);

            var connected = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(CONNECTION_TIMEOUT_MS));
            if (connected)
            {
                _master = factory.CreateMaster(_tcpClient);
                onConnect?.Invoke();
            }
            else
            {
                _tcpClient.Close();
                throw new ModBusConnectException("Couldn't connect to device : " + _remoteIpAddress + " at port : " + _port);
            }
        }

        /// <summary>
        /// Closes a ModBus connection over TCP
        /// </summary>
        public void CloseModBusConnection()
        {
            _tcpClient.Close();
            onDisconnect?.Invoke();

            if (_master == null) return;

            _master.Dispose();
            _master = null;
        }

        /// <summary>
        /// Read a ModBus register at a given address
        /// </summary>
        /// <param name="address">The ModBus address to read</param>
        /// <returns>A an awaitable task containing in the response from ModBus</returns>
        public async Task<ushort[]> ReadRegister(ushort address)
        {
            try
            {
                return await _master.ReadInputRegistersAsync(1, address, 1);
            }
            catch (Exception e)
            {
                throw new ModBusIOException("Couldn't read register " + address + " of device at " + _remoteIpAddress + " : " + e.Message);
            }
        }

        /// <summary>
        /// Write to a register over ModBus. The entire byte shall be written
        /// </summary>
        /// <param name="address">The address of the register to write to</param>
        /// <param name="value">The value to write</param>
        public async void WriteRegister(ushort address, ushort value)
        {
            try
            {
                await _master.WriteSingleRegisterAsync(1, address, value);
            }
            catch (Exception e)
            {
                throw new ModBusIOException("Couldn't write " + value + " to " + address + " : " + e.Message);
            }
        }

        /// <summary>
        /// Repeated functionality
        /// </summary>
        public void Tick()
        {
            CheckConnection();
        }

        private void CheckConnection()
        {
            if (_tcpClient.Connected) return;

            CloseModBusConnection();
        }
    }
}
