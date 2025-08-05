using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityModBus.Classes
{
    /// <summary>
    /// Class to handle polling a register address over ModBus.
    /// </summary>
    public class ModBusPoll
    {
        /// <summary>
        /// Called whenever the register specified in the constructor changes value.
        /// </summary>
        public Action<ushort[]> OnRegisterChange;

        private readonly ushort _registerAddress;
        private readonly ModBusConnection _connection;

        private ushort[] _previousRegisterValues;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="registerAddress">The register address to poll</param>
        /// <param name="connection">The active ModBus connection</param>
        public ModBusPoll(ushort registerAddress, ModBusConnection connection)
        {
            _registerAddress = registerAddress;
            _connection = connection;
        }

        /// <summary>
        /// Enumerator to continuously poll a register address.
        /// </summary>
        public IEnumerator Poll()
        {
            while (true)
            {
                Task<ushort[]> task;
                try
                {
                    task = _connection.ReadInputRegister(_registerAddress);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Couldn't read register. Stopping poll! : {ex.Message}");
                    break;
                }

                yield return new WaitUntil(() => task.IsCompleted || task.IsCanceled);

                ushort[] result;

                try
                {
                    result = task.Result;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Couldn't read register. Stopping poll! : {ex.Message}");
                    break;
                }

                HandleRegister(result);
            }
        }

        private void HandleRegister(ushort[] registerValues)
        {
            if (registerValues == null) return;
            
            if (_previousRegisterValues == null)
            {
                CallChange(registerValues);
                return;
            }
            
            if (registerValues.SequenceEqual(_previousRegisterValues)) return;

            CallChange(registerValues);
        }

        private void CallChange(ushort[] registerValues)
        {
            _previousRegisterValues = registerValues;
            OnRegisterChange?.Invoke(registerValues);
        }
    }
}
