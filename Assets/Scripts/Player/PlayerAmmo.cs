using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAmmo : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    private WeaponAgent weapon;
    
    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<WeaponAgent>();
        ammoText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(weapon.ActiveWeapon != null)
        {
            ammoText.enabled = true;
            ammoText.SetText(
                $"{weapon.ActiveWeapon.ammo.CurrentClipAmmo}/" +
                $"{weapon.ActiveWeapon.ammo.CurrentAmmo}"
                );
        }
        else
        {
            ammoText.enabled = false;
        }
    }
}
