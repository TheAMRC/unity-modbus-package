using System;
using UnityEngine;
using UnityEngine.Events;
using UnityModBus.Classes;

namespace UnityModBus.UnityComponents
{
    /// <summary>
    /// A GameObject representing a connection to a device over ModBus TCP
    /// </summary>
    public class ModBusConnectionInstance : MonoBehaviour
    {
        /// <summary>
        /// The ModBus connection object. Created using parameters set in the Inspector.
        /// </summary>
        public ModBusConnection Connection { get; private set; }

        /// <summary>
        /// Whether the ModBus connection is currently operational.
        /// </summary>
        public bool IsConnected { get; private set; }

        [Tooltip("The IP address to connect to over ModBus TCP")]
        [SerializeField] private string remoteIpAddress;
        
        [Tooltip("The port of the ModBus connection")]
        [SerializeField] private ushort port = 502;

        [Tooltip("Event triggered upon a successful Modbus connection")]
        [SerializeField] private UnityEvent onConnect;

        [Tooltip("Event triggered upon the termination of a Modbus connection")]
        [SerializeField] private UnityEvent onDisconnect;

        private void OnEnable() => TryOpenConnection();

        private void OnDisable() => CloseConnection();

        private void Update()
        {
            if (!IsConnected) return;
            Connection?.Tick();
        }

        private void TryOpenConnection()
        {
            try
            {
                OpenConnection();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OpenConnection()
        {
            Connection = new ModBusConnection(remoteIpAddress, port, OnConnectHandler, OnDisconnectHandler);
            Connection.OpenModBusConnection();
        }

        private void CloseConnection()
        {
            if (Connection == null) return;

            Connection.CloseModBusConnection();
            Connection = null;
        }
        
        private void OnConnectHandler()
        {
            IsConnected = true;
            onConnect?.Invoke();
        }

        private void OnDisconnectHandler()
        {
            IsConnected = false;
            onDisconnect?.Invoke();
        }
    }
}
