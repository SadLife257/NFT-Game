using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] InventoryUI inventory;

        [SerializeField] InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        // Start is called before the first frame update
        void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventory.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventory.UpdateData(item.Key, item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventory.InitializeInventory(inventoryData.Size);
            inventory.OnDescriptionRequested += HandleDescriptionRequest;
            inventory.OnSwapItems += HandleSwapItems;
            inventory.OnStartDragging += HandleDragging;
            inventory.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventory.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventory.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            //string description = PrepareDescription(inventoryItem);
            inventory.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            throw new NotImplementedException();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventory.isActiveAndEnabled == false)
                {
                    inventory.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventory.UpdateData(item.Key,
                            item.Value.item.ItemImage,
                            item.Value.quantity);
                    }
                }
                else
                {
                    inventory.Hide();
                }

            }
        }
    }
}