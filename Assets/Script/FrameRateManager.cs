using System;
using UnityEngine;
using System.Threading;
using System.Collections;
public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Setting")]
    public float TargetFrameRate = 60f;
    void Awake()
    {
        Application.targetFrameRate = Mathf.FloorToInt(TargetFrameRate);
    }

    private void Update()
    {
    }
}
