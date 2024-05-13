using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModBus
{
    public class PollRegister : MonoBehaviour
    {
        [SerializeField] ModBusConnection connection;
        [SerializeField] ushort registerAddress;
        [SerializeField] UnityEvent<ushort> registerChanged;

        private ushort _previousRegisterValue;

        private void Start()
        {
            StartCoroutine(PollRoutine());
        }

        private IEnumerator PollRoutine()
        {
            while (true)
            {
                var task = ReadStatus();
                yield return new WaitUntil(() => task.IsCompleted);
            }
        }

        private async Task ReadStatus()
        {
            var registerValues = await connection.ReadRegister(registerAddress);

            if (registerValues == null) return;

	    var status = registerValues[0];
            if (status == _previousRegisterValue) return;

            registerChanged?.Invoke(status);
            _previousRegisterValue = status;

        }
    }
}
