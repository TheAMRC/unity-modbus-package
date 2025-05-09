using UnityModBus.Classes;
using PlasticPipe.PlasticProtocol.Messages;
using System;
using UnityEngine;
using UnityEngine.Events;

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
        public ModBusConnection Connection { get => _connection; }

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

        private ModBusConnection _connection;

        private void OnEnable()
        {
            TryOpenConnection();
        }

        private void OnDisable() {
            CloseConnection();
        }

        private void Update()
        {
            if (!IsConnected) return;
            _connection?.Tick();
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
            AddConnection();
            AddEvents();
            IsConnected = true;
        }

        private void CloseConnection()
        {
            RemoveEvents();
            RemoveConnection();
            IsConnected = false;
        }

        private void AddConnection()
        {
            _connection = new ModBusConnection(remoteIpAddress, port);
            _connection.OpenModBusConnection();
        }

        private void RemoveConnection()
        {
            if (_connection == null) return;

            _connection.CloseModBusConnection();
            _connection = null;
        }

        private void AddEvents()
        {
            if (_connection == null) return;

            _connection.onConnect += OnConnectHandler;
            _connection.onDisconnect += OnDisconnectHandler;
        }

        private void RemoveEvents()
        {
            if (_connection == null) return;

            _connection.onConnect -= OnConnectHandler;
            _connection.onDisconnect -= OnDisconnectHandler;
        }

        private void OnConnectHandler()
        {
            _connection.onConnect?.Invoke();
        }

        private void OnDisconnectHandler()
        {
            _connection.onDisconnect?.Invoke();
        }
    }
}
