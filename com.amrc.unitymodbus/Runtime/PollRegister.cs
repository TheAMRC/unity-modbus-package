using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModBus
{
    /// <summary>
    /// A class to repeatedly poll a register over ModBus and alert at any change
    /// </summary>
    public class PollRegister : MonoBehaviour
    {
        [Tooltip("The active ModBus connection to use")]
        [SerializeField] private ModBusConnection connection;
        
        [Tooltip("The register to poll")]
        [SerializeField] private ushort registerAddress;
        
        [Tooltip("Events to call upon the register changing value")]
        [SerializeField] private UnityEvent<ushort> registerChanged;

        private ushort _previousRegisterValue = ushort.MaxValue; // Zero is a possible value so use something else

        private void Start() => StartCoroutine(PollRoutine());

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator PollRoutine()
        {
            while (true)
            {
                var task = connection.ReadRegister(registerAddress);
                yield return new WaitUntil(() => task.IsCompleted);
                
                var registerValues = task.Result;
                HandleRegister(registerValues);
            }
        }

        private void HandleRegister(IReadOnlyList<ushort> registerValues)
        {
            if (registerValues == null) return;

            var status = registerValues[0];
            if (status == _previousRegisterValue) return;

            _previousRegisterValue = status;
            registerChanged?.Invoke(status);
        }
    }
}
