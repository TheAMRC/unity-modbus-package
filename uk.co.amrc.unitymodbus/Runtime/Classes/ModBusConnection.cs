using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityModBus.Exceptions;

namespace UnityModBus.Classes
{
    /// <summary>
    /// A class representing a connection to a device over ModBus TCP
    /// </summary>
    public class ModBusConnection
    {
        private const int CONNECTION_TIMEOUT_MS = 1000;

        private readonly string _remoteIpAddress;
        private readonly ushort _port;

        private readonly TcpClient _tcpClient = new();
        private IModbusMaster _master;
        
        private readonly Action _onConnect;
        private readonly Action _onDisconnect;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="remoteIpAddress">IP address of modbus device to connect to over TCP</param>
        /// <param name="port">Open modbus port on remote device</param>
        /// <param name="onConnect">Action to call on a successful connection</param>
        /// <param name="onDisconnect">Action to call on a successful connection termination</param>
        public ModBusConnection(string remoteIpAddress, ushort port, Action onConnect, Action onDisconnect)
        {
            _remoteIpAddress = remoteIpAddress;
            _port = port;
            
            _onConnect = onConnect;
            _onDisconnect = onDisconnect;
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
                _onConnect?.Invoke();
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
            _onDisconnect?.Invoke();

            if (_master == null) return;

            _master.Dispose();
            _master = null;
        }

        /// <summary>
        /// Read a ModBus input register at a given address
        /// </summary>
        /// <param name="address">The ModBus address to read</param>
        /// <returns>An awaitable task containing in the response from ModBus</returns>
        public async Task<ushort[]> ReadInputRegister(ushort address)
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
        /// Read a ModBus holding register at a given address
        /// </summary>
        /// <param name="address">The ModBus address to read</param>
        /// <returns>An awaitable task containing in the response from ModBus</returns>
        public async Task<ushort[]> ReadHoldingRegister(ushort address)
        {
            try
            {
                return await _master.ReadHoldingRegistersAsync(1, address, 1);
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
        /// <exception cref="ModBusIOException">Triggered if application cannot write a value to a register</exception>
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
