/*
 * Copyright (c) University of Sheffield AMRC 2025.
 * See LICENSE file
 */

using AMRC.UnityModBus.Classes;
using UnityEngine;
using UnityEngine.Events;

namespace AMRC.UnityModBus.UnityComponents
{
    /// <summary>
    /// A Unity component class to repeatedly poll a register over ModBus and alert at any change
    /// </summary>
    public class PollRegister : MonoBehaviour
    {
        [Tooltip("The active ModBus connection to use")]
        [SerializeField] private ModBusConnectionInstance connectionInstance;
        
        [Tooltip("The register to poll")]
        [SerializeField] private ushort registerAddress;
        
        [Tooltip("Events to call upon the register changing value")]
        [SerializeField] private UnityEvent<ushort[]> registerChanged;

        private ModBusPoll _poll;

        private void OnEnable()
        {
            if (!connectionInstance.IsConnected)
            {
                Debug.LogError("Cannot poll an inactive ModBus connection");
                return;
            }

            _poll = new ModBusPoll(registerAddress, connectionInstance.Connection);
            _poll.OnRegisterChange += RegisterChangeHandler;
            StartCoroutine(_poll.Poll());
        }

        private void OnDisable()
        {
            if (_poll == null) return;

            StopAllCoroutines();
            _poll.OnRegisterChange -= RegisterChangeHandler;
            _poll = null;
        }

        private void RegisterChangeHandler(ushort[] registerValues) => registerChanged?.Invoke(registerValues);
    }
}
