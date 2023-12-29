using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Thirdweb;
using System;

[Serializable]
public enum NFTType
{
    ERC721,
    ERC1155
}

[Serializable]
public struct MultiQuery
{
    public string contractAddress;
    public int startID;
    public int count;
    public NFTType type;
}

[Serializable]
public struct OwnedQuery
{
    public string contractAddress;
    //public string owner;
    public NFTType type;
}

[Serializable]
public struct NFTQuery
{
    public List<MultiQuery> loadMultipleNfts;
    public List<OwnedQuery> loadOwnedNfts;
}

public class HomePage : MonoBehaviour
{
    [Header("SETTINGS")]
    public NFTQuery query;

    private ThirdwebSDK sdk;
    //private Contract contract;
    private string TEST_CONTRACT_ADDRESS = "0x8D256999C2021898eC425BB07D26b199845e9375";
    [SerializeField] private Button startBtn;
    [SerializeField] private GameObject itemList;
    [SerializeField] private NFTItem nftPrefab;
    [SerializeField] private GameObject loadingPanel;

    [SerializeField] private RectTransform container;
    private string ownerAddress;

    //TODO: when back to main menu, player is logout
    // Start is called before the first frame update
    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
        startBtn.interactable = false;
        itemList.SetActive(false);

        foreach (Transform child in container)
            Destroy(child.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void CheckConnected()
    {
        var isConnected = await sdk.wallet.IsConnected();
        if(isConnected)
        {
            startBtn.interactable = true;
            itemList.SetActive(true);
            ownerAddress = await sdk.wallet.GetAddress();
            return;
        }
        startBtn.interactable = false;
        itemList.SetActive(false);
    }

    public void ShowCollection()
    {
        loadingPanel.SetActive(true);
        itemList.SetActive(true);

        GetOwned();

        loadingPanel.SetActive(false);
    }

    private async void GetOwned()
    {
        List<NFT> nftsToLoad = new List<NFT>();
        try
        {
            var address = await sdk.wallet.GetAddress();
            Contract myContract = sdk.GetContract("0x8D256999C2021898eC425BB07D26b199845e9375");
            nftsToLoad = await myContract.ERC1155.GetOwned(address);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error Loading OwnedQuery NFTs: {e.Message}");
        }

        foreach (NFT nft in nftsToLoad)
        {
            if (!Application.isPlaying)
                return;

            NFTItem prefab = Instantiate(nftPrefab, container);
            prefab.LoadNFT(nft);
        }

        loadingPanel.SetActive(false);
    }
}
