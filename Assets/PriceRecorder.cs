using System;
using System.Collections.Generic;
using UnityEngine;

public class PriceRecorder : MonoBehaviour
{
    [SerializeField] private Dictionary<Item, List<float>> data = new Dictionary<Item, List<float>>();
    [SerializeField] private List<Item> allItems;
    
    [SerializeField] private GameManager gameManager;

    public Action<Dictionary<Item, List<float>>, int> DataForGraph;
    private int totalMonths;

    private void OnEnable()
    {
        
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    public void AddPrice(Item item)
    {
        data[item].Add(item.CurrentPrice);
    }


    public float MaxPrice()
    {
        float highestPrice = 0;
        foreach(List<float> valueList in data.Values)
        {
            foreach(float value in valueList)
            {
                if(value > highestPrice)
                {
                    highestPrice = value;
                }
            }
        }

        return highestPrice;
    }

    public void SetItems(List<Item> items)
    {
        UnSubscribe();
        data.Clear();
        allItems = items;
        foreach(Item item in allItems)
        {
            data[item] = new List<float>();
        }

        if(isActiveAndEnabled)
        {
            Subscribe();
        }
    }
    
    private void Subscribe()
    {
        gameManager.OnEndScreen += GiveGraph;
        foreach(Item item in allItems)
        {
            item.OnChangePrice += AddPrice;
        }
    }

    private void UnSubscribe()
    {
        gameManager.OnEndScreen -= GiveGraph;
        foreach(Item item in allItems)
        {
            item.OnChangePrice -= AddPrice;
        }
    }

    private void GiveGraph(int month)
    {
        DataForGraph?.Invoke(data, month);
        
        totalMonths = month;
    }
}
