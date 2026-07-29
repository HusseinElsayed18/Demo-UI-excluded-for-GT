using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GT
{
    [System.Serializable]
    public struct Item
    {
        public Sprite img;
        public int Quantity;
        public float price;
    }
    [System.Serializable]
    public struct ShopKeeperStruct
    {
      public  Button shopKeeperBtn;
      public List<Item> items;
    }
    public class ShopKeeper : Singletone<ShopKeeper>
    {
        public List<ShopKeeperStruct> sellers = new List<ShopKeeperStruct>();
        [SerializeField]public Transform inventorySeytem , itemsContainer;
        [SerializeField]public GameObject itemPrefab;
        [Header("purchasing")]
        [SerializeField] Transform purchasingSystem;
        [SerializeField] Button buy;
        [SerializeField] Text purchasingOperationStatus;
    
        Player player;
        private void Awake()
        {
            if (inventorySeytem != null)
            {
                inventorySeytem.gameObject.SetActive(false);
            }
            if (purchasingSystem != null)
            {
                purchasingSystem.gameObject.SetActive(false);
            }
            player = FindObjectOfType<Player>();
            UIPanelsActions.action += ClosePurchasingSystem;
        }
        void ClosePurchasingSystem()
        {
            purchasingSystem.gameObject.SetActive(false);
        }
        private void Start()
        {
            InitSellers(sellers);
        }

        void InitSellers(List<ShopKeeperStruct> sellers)
        {
            foreach (ShopKeeperStruct seller in sellers)
            {
                seller.shopKeeperBtn.onClick.AddListener(delegate
                {
                    inventorySeytem.gameObject.SetActive(true);
                    LoadItemsToInventory(seller, itemsContainer, itemPrefab);
                });
            }
        }
        public void LoadItemsToInventory(ShopKeeperStruct seller , Transform itemsContainer , GameObject itemPrefab)
        {
            List<Item> items = seller.items;

            foreach(Transform child in itemsContainer)
            {
                Destroy(child.gameObject);
            }

            for(int i =0; i < items.Count; i++)
            {
                if(items[i].Quantity > 0)
                {
                   Transform obj = Instantiate(itemPrefab, itemsContainer).transform;
                    obj.GetChild(1).GetComponent<Image>().sprite = items[i].img;
                    obj.GetChild(2).GetComponent<Text>().text = ""+ items[i].price;
                    obj.GetChild(5).GetComponent<Text>().text = "" + items[i].Quantity;
                    Item itemInfo = items[i];
                   obj.GetComponent<Button>().onClick.AddListener(delegate { ItemClick(obj , seller.shopKeeperBtn.transform.name , itemInfo); });
                }
            }
        }

        public void ItemClick(Transform item , string sellerName , Item itemInfo)
        {
            UIPanelsActions.action?.Invoke();
            purchasingSystem.gameObject.SetActive(true);
            if (itemInfo.price <= player.coins)
            {
                purchasingOperationStatus.text = "You will buy this item with " + itemInfo.price + " $ "+"from : "+sellerName;
                buy.interactable = true;
                buy.GetComponent<Button>().onClick.RemoveAllListeners();
                buy.GetComponent<Button>().onClick.AddListener(delegate { BuyItem(item , sellerName , itemInfo); });
            }
            else
            {
                purchasingOperationStatus.text = "You need more money to buy";
                buy.interactable = false;
            }
        }

        public void BuyItem(Transform item , string sellerName, Item itemInfo)
        {
            buy.onClick.RemoveAllListeners(); 
            purchasingSystem.gameObject.SetActive(false);
            player.OnFinishedPurchasing?.Invoke(itemInfo , sellerName);
            int seller, itm;
           (seller,itm) = GetItem(sellerName, itemInfo.img);
            itemInfo.Quantity -= 1;
            if (seller != -1 && itm != -1)
            {
                sellers[seller].items[itm] = itemInfo;
                inventorySeytem.gameObject.SetActive(true);
                LoadItemsToInventory(GetSeller(sellerName), itemsContainer, itemPrefab);

            }
        }

        public (int,int) GetItem(string sellerName , Sprite item)
        {
            int sellerIndex = -1, itemIndex = -1;
            for(int i = 0; i < sellers.Count; i++)
            {
                if (sellers[i].shopKeeperBtn.transform.name == sellerName)
                {
                    sellerIndex = i;
                    for (int j = 0; j < sellers[i].items.Count; j++)
                    {
                        if (sellers[i].items[j].img ==  item)
                        {
                            itemIndex = j;
                            break;
                        }
                    }
                }
            }

            return (sellerIndex, itemIndex);
        }
        public ShopKeeperStruct GetSeller(string sellerName)
        {
            for (int i = 0; i < sellers.Count; i++)
            {
                if (sellers[i].shopKeeperBtn.transform.name == sellerName)
                {
                    return sellers[i];
                }
            }

            return new ShopKeeperStruct() { };
        }


    } //end of class
}

