using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using TMPro;
using System.Threading.Tasks;

public class ClaimArea : MonoBehaviour
{
    private string playerName;
    private int count;

    private ThirdwebSDK sdk;
    private Pack pack;
    
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async Task<ERC1155Reward> Claim()
    {
        sdk = ThirdwebManager.Instance.SDK;
        pack = sdk.GetContract("0x8D256999C2021898eC425BB07D26b199845e9375").pack;
        var result = pack.Open("0", "1").Result.erc1155Rewards[0];
        return result;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            count++;

            TMP_Text playerNameTxt = collision.gameObject.transform.Find("PlayerName").GetComponent<TMP_Text>();
            
            if (string.IsNullOrEmpty(playerName) || playerName != playerNameTxt.text)
            {
                playerName = playerNameTxt.text;
                //restart claim timer
            }

            //start claim timer
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            count--;

            TMP_Text playerNameTxt = collision.gameObject.transform.Find("PlayerName").GetComponent<TMP_Text>();

            if (string.IsNullOrEmpty(playerName) || playerName != playerNameTxt.text)
            {
                playerName = playerNameTxt.text;
                //restart claim timer
            }

            //start claim timer
        }
    }

    void ManageTimer()
    {
        if(count > 1)
        {
            //stop claim timer
        }
        //start claim timer
    }
}
