using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponAgent : NetworkBehaviour
{
    /*[SerializeField]
    private WeaponSO weapon;*/

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    //[SerializeField] WeaponType type;
    [SerializeField] Transform container;
    [SerializeField] List<WeaponConfigSO> weapons;
    public WeaponConfigSO ActiveWeapon;

    private bool AutoReload = true;
    private bool isReloading;

    void Update()
    {
        if (ActiveWeapon != null)
        {
            ActiveWeapon.Tick(Input.GetButton("Fire1"));

            if (isManualReload() || isAutoReload())
            {
                isReloading = true;
                Reload();
            }
        }
    }

    private void Reload()
    {
        StartCoroutine(LoadingWait());
        isReloading = false;
    }

    private bool isManualReload()
    {
        return !isReloading &&
               Input.GetKeyUp(KeyCode.R) &&
               ActiveWeapon.CanReload();
    }

    private bool isAutoReload()
    {
        return !isReloading &&
               AutoReload &&
               ActiveWeapon.CanReload() &&
               ActiveWeapon.ammo.CurrentClipAmmo == 0;
    }

    public void SetWeapon(WeaponConfigSO ActiveWeapon, List<ItemParameter> itemState)
    {
        this.ActiveWeapon = ActiveWeapon;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        RequestSetWeaponServerRpc(container.position, container.rotation, container.localScale);
        ExecuteSetItem(container.position, container.rotation, container.localScale);
    }

    [ServerRpc]
    private void RequestSetWeaponServerRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        SetWeaponClientRpc(position, rotation, scale);
    }

    [ClientRpc]
    private void SetWeaponClientRpc(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (!IsOwner) ExecuteSetItem(position, rotation, scale);
    }

    private void ExecuteSetItem(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        container.position = position;
        container.rotation = rotation;
        container.localScale = scale;

        if (ActiveWeapon != null)
        {
            inventoryData.AddItem(ActiveWeapon, 1, itemCurrentState);
            ActiveWeapon.Despawn(container);
        }

        ActiveWeapon.Spawn(container, this);
    }

    private IEnumerator LoadingWait()
    {
        yield return new WaitForSeconds(ActiveWeapon.ammo.ReloadTime);
        //yield return null;
        ActiveWeapon.ammo.Reload();
    }
}
