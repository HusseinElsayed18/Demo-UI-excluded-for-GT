using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GT
{
    [System.Serializable]
    public struct PurchasedItems
    {
        public string sellerName;
        public List<Item> items;
    }
    public class Player : Singletone<Player>
    {
        [SerializeField] float _coins, _bankBalance;
        [SerializeField] Text coinsTxt, bankBalanceTxt;
        public  Action<Item,string> OnFinishedPurchasing;
        public Action<Item, string> OnFinishedSelling;
        public List<PurchasedItems> purshasedItems = new List<PurchasedItems>();
        [Header("purchased items")]
        public Transform purshasedItemsPanel, purchasedItemsContainer;
        public float coins
        {
            set
            {
                _coins = value;
            }
            get
            {
                return _coins;
            }
        }
        public float bankBalance
        {
            set
            {
                _bankBalance = value;
            }
            get
            {
                return _bankBalance;
            }
        }

        [Header("selling")]
        [SerializeField] Transform sellingSystem;
        [SerializeField] Button sell;
        [SerializeField] Text sellingOperationStatus;
        [Header("sleeping")]
        [SerializeField] Button sleep;

        ShopKeeper shopKeeper;
        private void Awake()
        {
            coins = 1000;
            bankBalance = 3000;
            Init();
            if (purshasedItemsPanel != null)
            {
                purshasedItemsPanel.gameObject.SetActive(false);
            }
            if (sellingSystem != null)
            {
                sellingSystem.gameObject.SetActive(false);
            }
            OnFinishedPurchasing += PurchasingItem;
            OnFinishedSelling += SellingItem;
            shopKeeper = FindObjectOfType<ShopKeeper>();
            sleep.onClick.AddListener(delegate { Sleeping(); });
            UIPanelsActions.action += CloseSellingSystem;
        }
        void CloseSellingSystem()
        {
            sellingSystem.gameObject.SetActive(false);
        }
        public void Init()
        {
            coinsTxt.text = "" + coins;
            bankBalanceTxt.text = "" + bankBalance;
        }
        void PurchasingItem(Item item , string sellerName)
        {
            coins -= item.price;
            coinsTxt.text = "" + coins;
            int seller, itm;
            (seller, itm) = GetItem(sellerName, item.img);
            if (seller == -1 )
            {
                item.Quantity = 1;
                purshasedItems.Add(new PurchasedItems { sellerName = sellerName, items = new List<Item>() { item } });
            }
            else
            {
                if (itm == -1)
                {
                    item.Quantity = 1;
                    GetSeller(sellerName).items.Add(item);
                }
                else
                {
                   Item item1 = purshasedItems[seller].items[itm];
                    item1.Quantity += 1;
                    purshasedItems[seller].items[itm] = item1;
                }
            }
           
            InitPurchasedItems(purshasedItems);
            purshasedItemsPanel.gameObject.SetActive(true);
        }
        void InitPurchasedItems(List<PurchasedItems> PurchasedItems)
        {
            foreach (Transform child in purchasedItemsContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (PurchasedItems item in PurchasedItems)
            {
                LoadItemsToPurchasedPanel(item, purchasedItemsContainer, shopKeeper.itemPrefab);
            }
        }
        void LoadItemsToPurchasedPanel(PurchasedItems purshasedItems, Transform itemsContainer, GameObject itemPrefab)
        {
            List<Item> items = purshasedItems.items;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Quantity > 0)
                {
                    Transform obj = Instantiate(itemPrefab, itemsContainer).transform;
                    obj.GetChild(1).GetComponent<Image>().sprite = items[i].img;
                    obj.GetChild(2).GetComponent<Text>().text = "" + items[i].price;
                    obj.GetChild(5).GetComponent<Text>().text = "" + items[i].Quantity;
                    Item itemInfo = items[i];
                    obj.GetComponent<Button>().onClick.AddListener(delegate { ItemClick(obj, purshasedItems.sellerName, itemInfo); });
                }
                
            }
        }

        public void ItemClick(Transform item, string sellerName, Item itemInfo)
        {
            UIPanelsActions.action?.Invoke();
            sellingSystem.gameObject.SetActive(true);
            sell.interactable = true;
            sellingOperationStatus.text = "You will sell this item with " + itemInfo.price + " $ " + "to : " + sellerName;
            sell.GetComponent<Button>().onClick.RemoveAllListeners();
            sell.GetComponent<Button>().onClick.AddListener(delegate { SellItem(item, sellerName, itemInfo); });
        }

        public void SellItem(Transform item, string sellerName, Item itemInfo)
        {
            sell.onClick.RemoveAllListeners(); 
            sellingSystem.gameObject.SetActive(false);
            OnFinishedSelling?.Invoke(itemInfo, sellerName);
            int seller, itm;
            (seller, itm) = GetItem(sellerName, itemInfo.img);
            itemInfo.Quantity -= 1;
            if (seller != -1 && itm != -1)
            {
                purshasedItems[seller].items[itm] = itemInfo;
                InitPurchasedItems(purshasedItems);
                purshasedItemsPanel.gameObject.SetActive(true);
            }
        }
        void SellingItem(Item item, string sellerName)
        {
            coins += item.price;
            coinsTxt.text = "" + coins;
            int seller, itm;
            (seller, itm) =shopKeeper. GetItem(sellerName, item.img);
            if (seller == -1)
            {
                item.Quantity = 1;
                shopKeeper.sellers.Add(new ShopKeeperStruct { shopKeeperBtn = null, items = new List<Item>() { item } });
            }
            else
            {
                if (itm == -1)
                {
                    item.Quantity = 1;
                   shopKeeper. GetSeller(sellerName).items.Add(item);
                }
                else
                {
                    Item item1 = shopKeeper.sellers[seller].items[itm];
                    item1.Quantity += 1;
                    shopKeeper.sellers[seller].items[itm] = item1;
                }
            }

           shopKeeper. inventorySeytem.gameObject.SetActive(true);
            shopKeeper. LoadItemsToInventory(shopKeeper.GetSeller(sellerName), shopKeeper.itemsContainer,shopKeeper. itemPrefab);

        }
        public (int, int) GetItem(string sellerName, Sprite item)
        {
            int sellerIndex = -1, itemIndex = -1;
            for (int i = 0; i < purshasedItems.Count; i++)
            {
                if (purshasedItems[i].sellerName == sellerName)
                {
                    sellerIndex = i;
                    for (int j = 0; j < purshasedItems[i].items.Count; j++)
                    {
                        if (purshasedItems[i].items[j].img == item)
                        {
                            itemIndex = j;
                            break;
                        }
                    }
                }
            }

            return (sellerIndex, itemIndex);
        }
        public PurchasedItems GetSeller(string sellerName)
        {
            for (int i = 0; i < purshasedItems.Count; i++)
            {
                if (purshasedItems[i].sellerName == sellerName)
                {
                    return purshasedItems[i];
                }
            }

            return new PurchasedItems() { };
        }
        public void Sleeping()
        {
            bankBalance += (bankBalance / 10f);
            bankBalanceTxt.text = "" + bankBalance;
        }
    } //end of class

    public static class UIPanelsActions
    {
        public static Action action;
    }
}

