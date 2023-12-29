using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Inventory.Model;
using Thirdweb;
using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class InventoryLoader : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    private List<InventoryItem> initialItems;
    private InventoryController controller;
    private ThirdwebSDK sdk;
    private void Initialize()
    {
        controller = GetComponent<InventoryController>();
        initialItems = controller.initialItems;
        //data = controller.inventoryData;
        sdk = ThirdwebManager.Instance.SDK;
    }

    public async void LoadItem()
    {
        Initialize();
        
        List<NFT> nft = new List<NFT>();

        try
        {
            var address = await sdk.wallet.GetAddress();
            Contract myContract = sdk.GetContract("0x8D256999C2021898eC425BB07D26b199845e9375");
            nft = await myContract.ERC1155.GetOwned(address);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error Loading OwnedQuery NFTs: {e.Message}");
        }

        foreach (NFT e in nft)
        {
            StartCoroutine(CreateItem(e.metadata.name.ToLower().Replace(" ", string.Empty), e.metadata.image, e.quantityOwned));
        }
    }

    private IEnumerator CreateItem(string name, string url, int quantity)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            //GameObject gameObject = bundle.LoadAsset<GameObject>(name);
            //WeaponConfigSO item = gameObject.GetComponent<WeaponConfigSO>();
            WeaponConfigSO item = bundle.LoadAsset<WeaponConfigSO> (name);
            if (item != null)
                inventoryData.AddItem(item, quantity);
            Debug.LogError("Adding " + name);
        }
    }
}
