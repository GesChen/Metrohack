using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaler : MonoBehaviour
{
    public float targetScale;
    public float smoothness;

    void Update()
    {
        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, targetScale, smoothness);
    }
    public void SetTarget(float scale)
    {
        targetScale = scale;
    }
}
