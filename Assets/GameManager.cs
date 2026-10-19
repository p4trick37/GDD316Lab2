using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemData[] items;
    [SerializeField] private int maxMonthCount;
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform content; 

    [SerializeField] private TMP_Text monthText;

    [SerializeField] private GameObject gameplayScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Player player;
    [SerializeField] private PriceRecorder priceRecorder;
    private List<Item> allItems = new List<Item>();
    
    private int currentMonth = 0;


    public Action OnMonthPass; 

    public Action<List<Item>> GetAllItems;
    public Action<int> OnEndScreen;


    private void Awake()
    {
        for(int i = 0; i < items.Length; i++)
        {
            Item item = Instantiate(itemPrefab, content);
            item.SetData(items[i]);
            allItems.Add(item);
        }
        priceRecorder.SetItems(allItems);
        player.SetItems(allItems);
        monthText.text = MonthText(currentMonth);
    }

    private void OnEnable()
    {
        //GetAllItems?.Invoke(allItems); 
    }

    public void ChangeMonth()
    {
        currentMonth++; 
        if(currentMonth >= maxMonthCount)
        {
            GoToEndScreen();
        }
        else
        {
            OnMonthPass?.Invoke();
        }

        monthText.text = MonthText(currentMonth);
    }

    public ItemData[] GetItems()
    {
        return this.items;
    }

    private void GoToEndScreen()
    {
        gameplayScreen.SetActive(false);
        endScreen.SetActive(true);
        OnEndScreen?.Invoke(currentMonth);
    }

    public string MonthText(int month)
    {
        int monthMod = month % 12;
        int year = month / 12;
        string monthName = "";
        string yearName = "";
        switch(monthMod)
        {
            case 0:
                monthName = "January";
                break;
            case 1:
                monthName = "February";
                break;
            case 2:
                monthName = "March";
                break;
            case 3:
                monthName = "April";
                break;
            case 4:
                monthName = "May";
                break;
            case 5:
                monthName = "June";
                break;
            case 6:
                monthName = "July";
                break;
            case 7:
                monthName = "August";
                break;
            case 8:
                monthName = "September";
                break;
            case 9:
                monthName = "October";
                break;
            case 10:
                monthName = "November";
                break;
            case 11:
                monthName = "December";
                break;
        }

        switch(year)
        {
            case 0:
                yearName = "2022";
                break;
            case 1:
                yearName = "2023";
                break;
            case 2:
                yearName = "2024";
                break;
            case 3:
                yearName = "2025";
                break;
            case 4:
                yearName = "2026";
                break;
            case 5:
                yearName = "2027";
                break;
        }

        string text = monthName + ", " + yearName;
        return text;
    }
}
