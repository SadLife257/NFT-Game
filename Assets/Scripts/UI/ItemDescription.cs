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
            img.gameObject.SetActive(false);
            title.text = "";
            description.text = "";
        }

        public void SetDescription(Sprite sprite, string itemName, string itemDescription)
        {
            img.gameObject.SetActive(true);
            img.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
        }
    }
}