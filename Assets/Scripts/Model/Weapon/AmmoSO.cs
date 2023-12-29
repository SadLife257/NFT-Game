using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Configuration", menuName = "Weapons/Ammo Configuration", order = 4)]
public class AmmoSO : ScriptableObject
{
    public int MaxAmmo = 120;
    public int ClipSize = 30;

    public int CurrentAmmo = 120;
    public int CurrentClipAmmo = 30;

    public float ReloadTime = 1f;

    public void Reload()
    {
        int maxReloadAmount = Mathf.Min(ClipSize, CurrentAmmo);
        int availableBulletsInCurrentClip = ClipSize - CurrentClipAmmo;
        int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
        CurrentClipAmmo += reloadAmount;
        //CurrentAmmo -= reloadAmount;
        Debug.LogError("Actual Loading...");
    }

    public bool CanReload()
    {
        return CurrentClipAmmo < ClipSize && CurrentAmmo > 0;
        //return CurrentClipAmmo < ClipSize;
        //return CurrentClipAmmo == 0;
    }
}
