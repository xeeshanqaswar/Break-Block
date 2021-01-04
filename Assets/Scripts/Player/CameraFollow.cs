using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    #region FIELDS DECLERATION

    public Transform followTarget;
    public Vector3 cameraOffset;

    #endregion

    private void Update()
    {
        transform.position = new Vector3(0f, 0f, followTarget.position.z) + cameraOffset;
    }

}
