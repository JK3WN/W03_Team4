using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    private IEnumerator Shaking()
    {
        start = true;
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/duration);
            Vector2 temp = (Vector2)startPos + Random.insideUnitCircle * strength * 5;

            transform.position = new Vector3(temp.x, temp.y, -10);
            yield return null;
        }

        transform.position = startPos;
        start = false;
    }

    public void Shake()
    {
        if (!start)
        {
            StartCoroutine(Shaking());
        }
    }
    
}
