using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Thirdweb;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //[SerializeField] TMP_Text address;
    [SerializeField] private Button claimButton;
    private GameObject pauseMenu;

    private ThirdwebSDK sdk;

    private void Start()
    {
        pauseMenu = gameObject.transform.Find("PauseMenu").gameObject;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        sdk = ThirdwebManager.Instance.SDK;
        //UpdateWalletAddress();
    }

    public void ToggleMenu()
    {
        if(pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            return;
        }
        pauseMenu.SetActive(true);
    }

    public void LoadSceneNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadSceneBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene("Scenes/" + name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public async void Claim()
    {
        Pack pack = sdk.GetContract("0xa719Ed04bed1c4783A1A95775d57A27DA9b639DF").pack;
        await pack.Open("0", "1");

        //var data = await pack.GetPackContents("0");
        //Debug.LogError(data.erc1155Rewards);

        claimButton.gameObject.SetActive(false);
    }

    /*public async void UpdateWalletAddress()
    {
        var result = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        if(result != null)
            address.text = result;
    }*/
}
