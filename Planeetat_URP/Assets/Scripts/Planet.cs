using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float gravitationalPull;
    public float GravitationalPull { get => gravitationalPull; }
}
