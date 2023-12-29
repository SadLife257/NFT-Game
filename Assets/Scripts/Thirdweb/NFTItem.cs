using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NFTItem : MonoBehaviour
{
    public Image nftImage;
    public TMP_Text nftName;
    public TMP_Text nftDescription;
    public TMP_Text nftSupply;
    public Button nftButton;
    public Button nftTransfer;

    private string CONTRACT_ADDRESS = "0x8D256999C2021898eC425BB07D26b199845e9375";

    public async void LoadNFT(NFT nft)
    {
        var address = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(CONTRACT_ADDRESS);

        nftImage.sprite = await ThirdwebManager.Instance.SDK.storage.DownloadImage(nft.metadata.image);
        nftName.text = nft.metadata.name;
        nftDescription.text = nft.metadata.description;
        nftSupply.text = nft.quantityOwned.ToString();
        nftButton.onClick.RemoveAllListeners();
        nftTransfer.onClick.RemoveAllListeners();

        nftButton.onClick.AddListener(() => GetDetail(nft));
        
        nftTransfer.onClick.AddListener(() => Transfer(nft));
    }

    void GetDetail(NFT nft)
    {
        NFTDetail.Instance.Show(nft.metadata.name, nft);
    }

    void Transfer(NFT nft)
    {
        NFTTransfer.Instance.Show(nft);
    }
}
