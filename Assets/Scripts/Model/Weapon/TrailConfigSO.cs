using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Configuration", menuName = "Weapons/Trail Configuration", order=2)]
public class TrailConfigSO : ScriptableObject
{
    public Material Trail;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public Gradient Color;

    public float MissDistance = 100f;
    public float Speed = 100f;
}
