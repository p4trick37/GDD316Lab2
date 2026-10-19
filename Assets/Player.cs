using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float startingBalance;
    public float Balance => balance;
    
    [SerializeField] private float balance;
    
    private GameManager gameManager;
    [SerializeField] private PlayerItem[] playerItems;
    private List<Item> allItems = new List<Item>();

    [SerializeField] private TMP_Text itemsText;
    [SerializeField] private TMP_Text balanceText;
    


    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        
    }
    private void Start()
    {
        playerItems = new PlayerItem[gameManager.GetItems().Length];
        
        for(int i = 0; i < playerItems.Length; i++)
        {
            playerItems[i] = new PlayerItem();
            string itemName = gameManager.GetItems()[i].itemName;
            playerItems[i].SetName(itemName);
            playerItems[i].SetAmount(0);
        }

        balance = startingBalance;
        UpdatePlayerUI();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void BuyItem(Item item, int amount)
    {
        float totalPrice = item.CurrentPrice * amount;
        PlayerItem currentItem = null;

        for(int i = 0; i < playerItems.Length; i++)
        {
            if(playerItems[i].GetName().Equals(item.GetData().itemName))
            {
                currentItem = playerItems[i];
                break;
            }
        }

        if(totalPrice > balance)
        {
            return;
        }

        currentItem.SetAmount(currentItem.GetAmount() + amount);
        balance -= totalPrice;
        UpdatePlayerUI();
    }

    private void SellItem(Item item, int amount)
    {
        float price = item.CurrentPrice;
        float totalPrice = price * amount;
        PlayerItem currentItem = null;
        for(int i = 0; i < playerItems.Length; i++)
        {
            if(playerItems[i].GetName().Equals(item.GetData().itemName))
            {
                currentItem = playerItems[i];
                break;
            }
        }

        if(currentItem.GetAmount() < amount && amount != 100)
        {
            return;
        }

        if(amount == 100)
        {
            float worth = price * currentItem.GetAmount();
            balance += worth;
            currentItem.SetAmount(0);
        }
        else
        {
            currentItem.SetAmount(currentItem.GetAmount() - amount);
            balance += totalPrice;
        }

        UpdatePlayerUI();
        
    }



    public void UpdatePlayerUI()
    {
        string list = "";
        for(int i = 0; i < playerItems.Length; i++)
        {
            list += playerItems[i].GetName() + ": " + playerItems[i].GetAmount() + "\n";
        }
        itemsText.text = list;
        balanceText.text = "$" + balance.ToString("F2");
    }

    public void SetItems(List<Item> items)
    {
        UnSubscribe();
        allItems = items;

        if(isActiveAndEnabled)
        {
            Subscribe();
        }
    }

    private void Subscribe()
    {
        foreach(Item item in allItems)
        {
            item.OnBuy += BuyItem;
            item.OnSell += SellItem;
        }
    }

    private void UnSubscribe()
    {
        foreach(Item item in allItems)
        {
            item.OnBuy -= BuyItem;
            item.OnSell -= SellItem;
        }
    }

}
