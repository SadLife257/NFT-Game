using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField] Image img;
        [SerializeField] TMP_Text title;
        [SerializeField] TMP_Text description;
        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            //gameObject.SetActive(false);
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            img.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            //gameObject.SetActive(true);
            gameObject.transform.localScale = new Vector3(1, 1, 0);
            img.gameObject.SetActive(true);
            img.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
        }
    }
}