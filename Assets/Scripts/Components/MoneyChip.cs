using UnityEngine;
using System.Collections;

public class MoneyChip {

    public string name;
    public long money;
    public bool isSkip;

    public MoneyChip(string name, long money, bool isSkip)
    {
        this.name = name;
        this.money = money;
        this.isSkip = isSkip;
    }
}
