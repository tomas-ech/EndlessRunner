using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ColorToSell
{
    public Color color;
    public float price;
}

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private ColorToSell[] platformColors;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
