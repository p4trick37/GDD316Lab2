using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite image;
    
    public float startPrice;
    [Range(1, 10)]
    public int volitility; 

    public Color graphColor;

    
}
