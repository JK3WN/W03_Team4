using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform targetTf;
    [SerializeField] private Transform cameraTf;

    [SerializeField] private float lerpFactor;

    void Start()
    {
        if (cameraTf == null)
            cameraTf = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        cameraTf.position =
            new Vector3(Mathf.Lerp(cameraTf.position.x, targetTf.position.x, Time.deltaTime * lerpFactor),
                cameraTf.position.y, cameraTf.position.z);
    }
}
