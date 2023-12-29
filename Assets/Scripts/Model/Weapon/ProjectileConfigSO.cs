using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="Projectile Configuration", menuName ="Weapons/Projectile Configuration", order=3)] 
public class ProjectileConfigSO : ScriptableObject
{
    public LayerMask HitMask;
    public float FireRate = 0.3f;
    public float RecoilRecoverySpeed = 1f;
    public float MaxSpreadTime = 1f;

    public ProjectileSpreadType SpreadType = ProjectileSpreadType.Simple;
    [Header("Simple Spread")]
    public Vector2 Spread = new Vector2(0.1f, 0.1f);

    [Header("Texture-Based Spread")]
    [Range(0.001f, 5f)]
    public float SpreadMultiplier = 0.1f;
    public Texture2D SpreadTexture;

    public Vector3 GetSpread(float shootTime)
    {
        Vector3 spread = Vector3.zero;

        if (SpreadType == ProjectileSpreadType.Simple)
        {
            spread = Vector3.Lerp(
                Vector3.zero,
                new Vector3(
                    Random.Range(-Spread.x, Spread.x),
                    Random.Range(-Spread.y, Spread.y),
                    0f
                    //Random.Range(-Spread.y, Spread.y)
                ),
                Mathf.Clamp01(shootTime / MaxSpreadTime)
            );
        }
        else if (SpreadType == ProjectileSpreadType.TextureBased)
        {
            spread = GetTextureDirection(shootTime);
            spread *= SpreadMultiplier;
        }

        return spread;
    }

    private Vector2 GetTextureDirection(float ShootTime)
    {
        Vector2 halfSize = new Vector2(SpreadTexture.width / 2f, SpreadTexture.height / 2f);

        int halfSquareExtents = Mathf.CeilToInt(Mathf.Lerp(0.01f, halfSize.x, Mathf.Clamp01(ShootTime / MaxSpreadTime)));

        int minX = Mathf.FloorToInt(halfSize.x) - halfSquareExtents;
        int minY = Mathf.FloorToInt(halfSize.y) - halfSquareExtents;

        Color[] sampleColors = SpreadTexture.GetPixels(
            minX,
            minY,
            halfSquareExtents * 2,
            halfSquareExtents * 2
        );

        float[] colorsAsGrey = System.Array.ConvertAll(sampleColors, (color) => color.grayscale);
        float totalGreyValue = colorsAsGrey.Sum();

        float grey = Random.Range(0, totalGreyValue);
        int i = 0;
        for (; i < colorsAsGrey.Length; i++)
        {
            grey -= colorsAsGrey[i];
            if (grey <= 0)
            {
                break;
            }
        }

        int x = minX + i % (halfSquareExtents * 2);
        int y = minY + i / (halfSquareExtents * 2);

        Vector2 targetPosition = new Vector2(x, y);

        Vector2 direction = (targetPosition - new Vector2(halfSize.x, halfSize.y)) / halfSize.x;

        return direction;
    }
}
