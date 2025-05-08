using Codice.Utils;
using ModBus.Exceptions;
using NModbus;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModBus
{
    /// <summary>
    /// A class representing a connection to a device over ModBus TCP
    /// </summary>
    public class ModBusConnection : MonoBehaviour
    {
        public bool IsConnected { get; private set; }

        [Tooltip("The IP address to connect to over ModBus TCP")]
        [SerializeField] private string remoteIpAddress;
        
        [Tooltip("The port of the ModBus connection")]
        [SerializeField] private ushort port = 502;

        [Tooltip("Event triggered upon a successful Modbus connection")]
        [SerializeField] private UnityEvent onConnect;

        [Tooltip("Event triggered upon the termination of a Modbus connection")]
        [SerializeField] private UnityEvent onDisconnect;

        private const int CONNECTION_TIMEOUT_MS = 1000;

        private readonly TcpClient _tcpClient = new();
        private IModbusMaster _master;

        // Start is called before the first frame update
        private void OnEnable()
        {
            try
            {
                _master = SetupConnection();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            if (_master != null)
            {
                IsConnected = true;
                onConnect?.Invoke();
            }
        }

        private void OnDisable() {
            _tcpClient.Close();
            IsConnected = false;
            onDisconnect?.Invoke();
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
                throw new ModBusIOException("Couldn't read register " + address + " of device at " + remoteIpAddress + " : " + e.Message);
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
            catch(Exception e)
            {
                throw new ModBusIOException("Couldn't write " + value + " to " + address + " : " + e.Message);
            }
        }
        
        private IModbusMaster SetupConnection()
        {
            var factory = new ModbusFactory();
            var result = _tcpClient.BeginConnect(remoteIpAddress, port, null, null);

            var connected = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(CONNECTION_TIMEOUT_MS));
            if (connected) {
                return factory.CreateMaster(_tcpClient);
            }

            else
            {
                _tcpClient.Close();
                throw new ModBusConnectException("Couldn't connect to device : " + remoteIpAddress + " at port : " + port);
            }
        }
    }
}
