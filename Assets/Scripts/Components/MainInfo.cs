using UnityEngine;
using System.Collections;

public class MainInfo {
	public string nick = ServerController.serverController.myName.ToString();
    public long userid = 0;
	public string displayname = ServerController.serverController.myName.ToString();
    public int gender = 0;
    public string birthday = "";
    public string address = "";
    public string status = "";
    public string email = "admin@admin.admin";
    public string cmnd = "";
    public string phoneNumber = "0123456789";
    public string link_Avatar = "";
    public int idAvata = 1;

    public long exp = 0;
    public long score_vip;
    public long total_money_charging = 0;
    public long total_time_play = 0;


	public long moneyChip = ServerController.serverController.coins;
    public int level = 1;
    public int[][] score;
    public int[] listAvatar;
    public string[] listNameAvatar;
    // public int idAvatar = -1;
    public long timeRuongServer = 0;

    // ----- new
    public string soLanThang;
    public string soLanThua;
    public long soTienMax;
    public long soChipMax;
    public int soGDThanhCong;
    public string LanDangNhapCuoi;
    public int level_vip = 0;

}
