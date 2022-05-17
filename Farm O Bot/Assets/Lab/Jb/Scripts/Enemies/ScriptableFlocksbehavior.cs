using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlocksBehavior", menuName = "Enemies/FlocksBehavior")]
public class ScriptableFlocksbehavior : ScriptableObject
{
    [Header("Speed")]
    [Range(0, 100)]
    [SerializeField] public float speed;

    [Header("Detection Distances")]
    [Range(0, 10)]
    public float cohesionDistance;

    [Range(0, 100)]
    public float avoidanceDistance;

    [Range(0, 10)]
    public float aligementDistance;

    [Range(0, 10)]
    public float obstacleDistance;


    [Header("Behaviour Weights")]
    [Range(0, 10)]
    public float cohesionWeight;

    [Range(0, 10)]
    public float avoidanceWeight;

    [Range(0, 10)]
    public float aligementWeight;

    [Range(0, 10)]
    public float boundsWeight;

    [Range(0, 100)]
    public float obstacleWeight;

    [Range(0, 10)]
    public float targetWeight;
}
