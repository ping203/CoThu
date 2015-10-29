using UnityEngine;
using System.Collections;

public class RoomInfo {

    public RoomInfo()
    {
        // TODO Auto-generated constructor stub
    }

    public sbyte typeRoom;
    private sbyte level = 0;
    public short notRoom = 1984;
    private sbyte id;
    private long money;
    private long needMoney;
    private int nUser;
    private string name;

    public void setLevel(sbyte level)
    {
        this.level = level;
    }

    public sbyte getLevel()
    {
        return level;
    }

    public sbyte getId()
    {
        return id;
    }

    public void setId(sbyte id)
    {
        this.id = id;
    }

    public long getMoney()
    {
        return money;
    }

    public void setMoney(long money)
    {
        this.money = money;
    }

    public int getnUser()
    {
        return nUser;
    }

    public void setnUser(int nUser)
    {
        this.nUser = nUser;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public long getNeedMoney()
    {
        return needMoney;
    }

    public void setNeedMoney(long needMoney)
    {
        this.needMoney = needMoney;
    }
}
