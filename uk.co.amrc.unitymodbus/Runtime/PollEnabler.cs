using ModBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollEnabler : MonoBehaviour
{

    [SerializeField] private PollRegister[] _pollers;

    private void OnEnable()
    {
        foreach (PollRegister poller in _pollers)
        {
            poller.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (PollRegister poller in _pollers)
        {
            poller.gameObject.SetActive(false);
        }
    }
}
