using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] Item slot;

        [SerializeField] RectTransform content;

        [SerializeField] ItemDescription itemDescription;

        private List<Item> listOfSlot = new List<Item>();

        [SerializeField] MouseTracker mouseTracker;

        private int currentlyDraggedItemIndex = -1;

        public event Action<int, int> OnSwapItems;

        //[SerializeField] ItemActionPanel actionPanel;

        public event Action<int> OnDescriptionRequested,
                    OnItemActionRequested,
                    OnStartDragging;


        private void Awake()
        {
            Hide();
            mouseTracker.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventory(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Item prefab = Instantiate(slot, Vector3.zero, Quaternion.identity);
                prefab.transform.SetParent(content);
                listOfSlot.Add(prefab);
                prefab.OnItemClicked += HandleItemSelection;
                prefab.OnItemBeginDrag += HandleBeginDrag;
                prefab.OnItemDroppedOn += HandleSwap;
                prefab.OnItemEndDrag += HandleEndDrag;
                prefab.OnRightMouseBtnClick += HandleShowItemActions;

                //prefab.OnMouseHover += HandleMouseHover;
                //prefab.OnMouseLoseFocus += HandMouseLoseFocus;
            }
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfSlot)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfSlot[itemIndex].Select();
        }

        /*private void HandMouseLoseFocus(Item obj)
        {
            throw new NotImplementedException();
        }

        private void HandleMouseHover(Item obj)
        {
            throw new NotImplementedException();
        }*/

        public void UpdateData(int itemIndex,
                Sprite itemImage, int itemQuantity)
        {
            if (listOfSlot.Count > itemIndex)
            {
                listOfSlot[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(Item obj)
        {

        }

        private void HandleEndDrag(Item obj)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(Item obj)
        {
            int index = listOfSlot.IndexOf(obj);
            if (index == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(obj);
        }

        private void ResetDraggedItem()
        {
            mouseTracker.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(Item obj)
        {
            int index = listOfSlot.IndexOf(obj);
            if (index == -1)
                return;

            currentlyDraggedItemIndex = index;
            HandleItemSelection(obj);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseTracker.Toggle(true);
            mouseTracker.SetData(sprite, quantity);
        }

        private void HandleItemSelection(Item obj)
        {
            int index = listOfSlot.IndexOf(obj);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        /*public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButon(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }*/

        private void DeselectAllItems()
        {
            foreach (Item item in listOfSlot)
            {
                item.Deselect();
            }
            //actionPanel.Toggle(false);
        }

        public void Hide()
        {
            //actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }
    }
}