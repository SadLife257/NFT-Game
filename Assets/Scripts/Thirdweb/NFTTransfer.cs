using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;
using TMPro;

public class NFTTransfer : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text titleText;

    public TMP_Text address;
    public TMP_Text amount;

    public static NFTTransfer Instance;

    private NFT nft;
    private string CONTRACT_ADDRESS = "0x8D256999C2021898eC425BB07D26b199845e9375";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Show(NFT nft)
    {
        this.nft = nft;
        panel.SetActive(true);
        titleText.text += nft.metadata.name;
    }

    public async void Transfer()
    {
        if (string.IsNullOrEmpty(address.text) || string.IsNullOrEmpty(amount.text))
        {
            Debug.LogError("Safe Guard 1");
            throw new System.Exception();
        }   
        if (int.TryParse(amount.text, out int n))
        {
            Debug.LogError("Safe Guard 2");
            throw new System.Exception();
        } 
        if (n > nft.quantityOwned)
        {
            Debug.LogError("Safe Guard 3");
            throw new System.Exception();
        }

        Contract contract = ThirdwebManager.Instance.SDK.GetContract(CONTRACT_ADDRESS);
        await contract.ERC1155.Transfer(address.text.Trim(), nft.metadata.id, n);
    }
}
