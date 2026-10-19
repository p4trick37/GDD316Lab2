
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public float CurrentPrice => currentPrice;
    [SerializeField] private float currentPrice;
    private float priceWhenBought;
    [SerializeField] private ItemData data;

    [Header("UI References")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text percentText;

    public Action<Item, int> OnBuy;
    public Action<Item, int> OnSell;

    public Action<Item> OnChangePrice;
    private GameManager gameManager;
    private PriceRecorder priceRecorder;

    private int minPercent;
    private int maxPercent;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        if(data != null)
        {
            currentPrice = data.startPrice;
            VolitileRange(data.volitility);
            priceWhenBought = data.startPrice;
            UpdateUI();
        }
        else
        {
            Debug.LogError("No reference to the data");
        }
    }
    
    private void OnEnable()
    {
        gameManager.OnMonthPass += ChangePrice;
    }

    private void OnDisable()
    {
        gameManager.OnMonthPass -= ChangePrice;
    }

    private void ChangePrice()
    {
        
        int randomPercentage = UnityEngine.Random.Range(minPercent, maxPercent + 1);
        float factor = 1 + (float)randomPercentage / 100;
        bool shouldIncrease = true;
        int randomValue = UnityEngine.Random.Range(0, 2);

        if(randomValue == 0)
        {
            shouldIncrease = true;
        }
        else
        {
            shouldIncrease = false;
        }

        if(shouldIncrease == true)
        {
            currentPrice = currentPrice * factor;
            currentPrice = Mathf.Clamp(currentPrice, data.startPrice * 0.2f, data.startPrice * 3f);
        }
        else
        {
            currentPrice = currentPrice / factor;
            currentPrice = Mathf.Clamp(currentPrice, data.startPrice * 0.2f, data.startPrice * 3f);
        }
        OnChangePrice?.Invoke(this);
        UpdateUI();
    }

    public void BuyItem(int amount)
    {
        priceWhenBought = currentPrice;
        OnBuy?.Invoke(this, amount);
    }

    public void SellItem(int amount)
    {
        priceWhenBought = currentPrice;
        OnSell?.Invoke(this, amount);
    }

    public void SetData(ItemData data)
    {
        this.data = data;
    }

    public ItemData GetData()
    {
        return this.data;
    }

    private void UpdateUI()
    {
        if(data.image != null)
        {
            image.sprite = data.image;
        }
        float percentIncrease = ((currentPrice - priceWhenBought) / priceWhenBought) * 100;
        string percentString = percentIncrease.ToString("F1");
        if(percentIncrease > 0)
        {
            percentText.color = Color.limeGreen;
            percentText.text = "+" + percentString + "%";
        }
        else if(percentIncrease < 0)
        {
            percentText.color = Color.red;
            percentText.text = percentString + "%";
        }
        else
        {
            percentText.color = Color.white;
            percentText.text = percentString + "%";
        }
        priceText.text = "$" + currentPrice.ToString("F2");
        
        nameText.text = data.itemName;
        
    }

    private void VolitileRange(int value)
    {
        switch(value)
        {
            case 1:
                minPercent = 0;
                maxPercent = 5;
                break;
            case 2:
                minPercent = 3;
                minPercent = 7;
                break;
            case 3:
                minPercent = 5;
                minPercent = 12;
                break;
            case 4:
                minPercent = 8;
                minPercent = 20;
                break;
            case 5:
                minPercent = 10;
                minPercent = 25;
                break;
            case 6:
                minPercent = 15;
                minPercent = 30;
                break;
            case 7:
                minPercent = 25;
                minPercent = 40;
                break;
            case 8:
                minPercent = 35;
                minPercent = 50;
                break;
            case 9:
                minPercent = 45;
                minPercent = 65;
                break;
            case 10:
                minPercent = 50;
                minPercent = 100;
                break;
        }
    }
}
