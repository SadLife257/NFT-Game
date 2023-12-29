using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Configuration", menuName = "Weapons/Damage Configuration", order = 2)]
public class DamageSO : ScriptableObject
{
    public MinMaxCurve Damage;

    private void Reset()
    {
        Damage.mode = ParticleSystemCurveMode.Curve;
    }

    public float GetDamage(float distance = 0)
    {
        return Mathf.CeilToInt(Damage.Evaluate(distance, Random.value));
    }
}
