using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Reflection;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text startMonthText;
    [SerializeField] private TMP_Text endMonthText;
    [SerializeField] private TMP_Text highPrice;
    [SerializeField] private RectTransform bottomLeft;
    [SerializeField] private RectTransform bottomRight;
    [SerializeField] private RectTransform topLeft;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private PriceRecorder priceRecorder;
    
    private GameManager gameManager;

    private Vector2 origin;
    private Vector2 endXAxis;
    private Vector2 endYAxis;

    private Dictionary<Item, List<float>> data = new Dictionary<Item, List<float>>();
    private int monthsCompleted;

    private void Awake()
    {
        origin = transform.InverseTransformPoint(bottomLeft.position);
        endYAxis = transform.InverseTransformPoint(topLeft.position);
        endXAxis = transform.InverseTransformPoint(bottomRight.position);
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnEnable()
    {
        priceRecorder.DataForGraph += GetData;
    }

    private void OnDisable()
    {
        priceRecorder.DataForGraph -= GetData;
    }

    private void Start()
    {
        endScreen.SetActive(false);
    }

    public void DrawGraph()
    {
        CreateAxis();
        startMonthText.text = gameManager.MonthText(0);
        endMonthText.text = gameManager.MonthText(monthsCompleted);
        highPrice.text = "$" + priceRecorder.MaxPrice().ToString("F2");
        foreach(KeyValuePair<Item, List<float>> entry in data)
        {
            Color color = entry.Key.GetData().graphColor;
            DrawItem(color, entry.Value);
        }
    }

    private void DrawItem(Color lineColor, List<float> prices)
    {
        List<Vector2> positions = new List<Vector2>();
        for(int i = 0; i < prices.Count; i++)
        {
            float xTValue = Mathf.InverseLerp(0, prices.Count, i);
            float xPosition = Mathf.Lerp(origin.x, endXAxis.x, xTValue);
            float highestPrice = priceRecorder.MaxPrice();
            float yTValue = Mathf.InverseLerp(0, highestPrice, prices[i]);
            float yPosition = Mathf.Lerp(origin.y, endYAxis.y, yTValue);
            positions.Add(new Vector2(xPosition, yPosition));
        }

        for(int i = 0; i < positions.Count - 1; i++)
        {
            DrawLine(positions[i], positions[i + 1], 1f, lineColor);
        }
        
    }

    private void CreateAxis()
    {
        DrawLine(origin, endYAxis, 2, Color.white);
        DrawLine(origin, endXAxis, 2, Color.white);
    }

    private void DrawLine(Vector2 startPos, Vector2 endPos, float width, Color lineColor)
    {
        float xMid = (startPos.x + endPos.x) / 2;
        float yMid = (startPos.y + endPos.y) / 2;
        Vector2 mid = new Vector2(xMid, yMid);
        RectTransform newLine = Instantiate(linePrefab, transform).GetComponent<RectTransform>();
        newLine.localPosition = new Vector3(mid.x, mid.y, 0);
        Vector2 axisDistance = new Vector2(endPos. x - startPos.x, endPos.y - startPos.y);
        float angle = Mathf.Atan(axisDistance.y / axisDistance.x) * Mathf.Rad2Deg;
        newLine.rotation = Quaternion.Euler(0, 0, angle);

        float distance = Vector2.Distance(startPos, endPos);
        newLine.sizeDelta = new Vector2(distance, width);
        newLine.GetComponent<Image>().color = lineColor;
    }

    private void GetData(Dictionary<Item, List<float>> newData, int months)
    {
        data = newData;
        monthsCompleted = months;
        DrawGraph();
    }


}
