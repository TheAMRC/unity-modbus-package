using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
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
        [SerializeField] private UnityEvent<ushort[]> registerChanged;

        private ushort[] _previousRegisterValues; // Zero is a possible value so use something else

        private void OnEnable() => StartCoroutine(PollRoutine());

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator PollRoutine()
        {
            while (true)
            {
                var task = connection.ReadRegister(registerAddress);

                yield return new WaitUntil(() => task.IsCompleted || task.IsCanceled);

                ushort[] result;

                try
                {
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    break;
                }

                HandleRegister(result);
            }

            yield break;
        }



        private void HandleRegister(ushort[] registerValues)
        {
            if (registerValues == null) return;
            if (registerValues.SequenceEqual(_previousRegisterValues)) return;

            _previousRegisterValues = registerValues;
            registerChanged?.Invoke(registerValues);
        }
    }
}
