using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{

    public float delay;

    private void OnEnable()
    {
        Invoke("DestroyWithDelay", delay);
    }

    private void DestroyWithDelay()
    {
        gameObject.SetActive(false);
    }

}
