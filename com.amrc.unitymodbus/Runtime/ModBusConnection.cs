using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ModBus
{
    /// <summary>
    /// A class representing a connection to a device over ModBus TCP
    /// </summary>
    public class ModBusConnection : MonoBehaviour
    {
        [Tooltip("The IP address to connect to over ModBus TCP")]
        [SerializeField] private string remoteIpAddress;
        
        [Tooltip("The port of the ModBus connection")]
        [SerializeField] private ushort port = 502;

        private readonly TcpClient _tcpClient = new();
        private IModbusMaster _master;

        // Start is called before the first frame update
        private void Awake()
        {
            try
            {
                var factory = new ModbusFactory();
                _tcpClient.BeginConnect(remoteIpAddress, port, null, null);
                _master = factory.CreateMaster(_tcpClient);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void OnDisable()
        {
            _tcpClient.Close();
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
            catch { return null; }
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
            catch
            {
                // ignored
            }
        }
    }
}
