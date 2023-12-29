using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;
using TMPro;

public class NFTDetail : MonoBehaviour
{
    public GameObject debuggerPanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public static NFTDetail Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        debuggerPanel.SetActive(false);
    }

    public void Show(string title, NFT nft)
    {
        debuggerPanel.SetActive(true);
        titleText.text = title;
        descriptionText.text = nft.ToString();
    }
}
