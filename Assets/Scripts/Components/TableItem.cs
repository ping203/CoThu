using UnityEngine;
using System.Collections;

public class TableItem {

    public int id;
	public int status;
	public string name = "";
	public string masid = "";
	public int nUser;
	public int maxUser;
	public long money;
	public long needMoney;
	public long maxMoney;
	public int Lock = 0;
    public int typeTable; // chip=1||kim cuong=0
    public int choinhanh = 0;//0:choi binh thuong, 1:choi nhanh
}
