using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float currTime = 0;
    public float maxTime = 0;

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > maxTime)
        {
            Destroy(gameObject);
        }
    }
}
