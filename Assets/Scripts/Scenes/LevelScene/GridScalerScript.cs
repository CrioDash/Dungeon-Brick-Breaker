using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScalerScript : MonoBehaviour
{

    [Header("Base scale of game field")] [SerializeField]
    private float fieldScale;
    
    private float _aspect;
    
    private void Awake()
    {
        _aspect = Camera.main!.aspect;
        Application.targetFrameRate = int.MaxValue;
    }

    void Start()
    {
        float mult = _aspect * fieldScale / (9f / 16);
        transform.localScale = Vector3.one * mult;
    }
    
}
