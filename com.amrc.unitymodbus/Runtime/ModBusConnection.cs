using NModbus;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ModBus
{
    public class ModBusConnection : MonoBehaviour
    {
        [SerializeField] private string remoteIpAddress;
        [SerializeField] private ushort port = 502;

        private TcpClient _tcpClient = new TcpClient();
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

        public async Task<ushort[]> ReadRegister(ushort address)
        {
            try
            {
                return await _master.ReadInputRegistersAsync(1, address, 1);
            }
            catch { return null; }
        }

        public async void WriteRegister(ushort address, ushort value)
        {
            try
            {
                await _master.WriteSingleRegisterAsync(1, address, value);
            }
            catch { }
        }
    }
}
