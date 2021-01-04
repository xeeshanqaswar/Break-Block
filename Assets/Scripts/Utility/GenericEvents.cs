using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEvents : MonoBehaviour
{

    public UnityEvent onEnable, start, onDisable;

    private void OnEnable()
    {
        onEnable?.Invoke();
    }

    private void Start()
    {
        start?.Invoke();
    }

    private void OnDisable()
    {
        onDisable?.Invoke();
    }

}
