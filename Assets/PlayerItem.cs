using UnityEngine;

[System.Serializable]
public class PlayerItem
{

    [SerializeField] private string name;
    [SerializeField] private int amount;

    public void SetName(string newName)
    {
        this.name = newName;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetAmount(int newAmount)
    {
        this.amount = newAmount;
    }

    public int GetAmount()
    {
        return this.amount;
    }
}
