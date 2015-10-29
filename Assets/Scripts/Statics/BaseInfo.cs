using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class BaseInfo {
    private static BaseInfo instance;
    public static BaseInfo gI () {
        if(instance == null) {
            instance = new BaseInfo ();

        }
        return instance;
    }
    public string cskh = "";
    public bool isView = false;
    public string pass = "";
    public string username = "";

    public string SMS_CHANGE_PASS_SYNTAX;
    public string SMS_CHANGE_PASS_NUMBER;
    public sbyte isDoiThuong = 0;// disable, 1: enable

    public int idRoom = 1;
    public string nameTale;

    public MainInfo mainInfo = new MainInfo ();

    protected static string strMoney = "";
    public int choinhanh = 0;
    public long moneyNeedTable;
    public int numberPlayer = 4;
    public short idTable;
    public long moneyTable, moneyMinTo;
    public string moneyName = "";
    public long betMoney;
    public int timerTurnTable = 30;
    public long needMoney = 0;
    public long maxMoney = 0;

    public int soTinNhan = 0;

    public String syntax10, syntax15;
    public String port10 = "", port15 = "";
    public int sms10 = 0, sms15 = 0;
    public int tyle_xu_sang_chip = 0, tyle_chip_sang_xu = 0;
    public bool isCharging = false;

    public bool tuDongRutTien = false;
    public long soTienRut;

    public long currentMaxMoney;
    public long currentMinMoney;
    public long moneyto;
    public bool isHideTabeFull = true;
    public bool nhacnen = true;
    public bool rung = true;
    public bool isNhanLoiMoiChoi = true;
    public int soDu = 50000;
    public bool isPurchase = false;

    public int type_sort = 0;
    public bool sort_giam_dan_bancuoc, sort_giam_dan_muccuoc, sort_giam_dan_nguoichoi;

    public bool isSound = PlayerPrefs.GetInt ("sound") == 0 ? true : false;
    public bool isVibrate = PlayerPrefs.GetInt ("rung") == 0 ? true : false;

    public int TELCO_CODE=1;

    public static string formatMoney (long money) {
        try {
            if(money < 0) {
                money = 0;
            }
            // strMoney.delete(0, strMoney.length());
            long strm = (long) (money / 1000000);
            long strk = 0;
            long strh = 0;
            if(strm > 0) {
                strk = (long) ((money % 1000000) / 1000);
                if(strk > 100) {
                    strMoney = strm + "," + strk + "M";
                } else if(strMoney.Length > 0) {
                    strMoney = strm + "," + "0" + strk + "M";
                }

            } else {
                strk = (long) (money / 1000);
                if(strk > 0) {
                    strh = (money % 1000 / 100);
                    if(strh > 0) {
                        strMoney = strk + "," + strh + "K";
                    } else if(strMoney.Length >= 0) {
                        strMoney = strk + "K";
                    }

                } else if(strMoney.Length >= 0) {
                    strMoney = money + "";
                }
            }
        } catch(Exception e) {
            Debug.LogException (e);

        }
        return strMoney.ToString ();
    }

    public static string formatMoneyNormal (long m) {
        //return m.ToString ("###.###");
        string str = m + "";// = m.ToString("000,000");//String.Format ("{0: 000.000}", m).ToString ();


        if(m < 1000000 && m > 0) {
            str = m.ToString("0,0");
        } else if(m >= 1000000 && m < 100000000) {
            str = (m / 1000).ToString ("0,0K");
        } else if(m >= 100000000) {
            str = (m / 1000000).ToString ("0,0M");
        }
        return str;
    }

    public static string formatMoneyNormal2 (long money) {
        //try {
            if(money < 0) {
                money = 0;
            }

            string str = money + "";
            string s = str;
            if(money < 1000000 && money > 0) {
                if(str.Length >= 4)
                s = str.Substring (0, str.Length - 3) + "," + str.Substring (str.Length - 3, 3);
                else
                    s = str.Substring (str.Length - 3, 3);
            } else if(money >= 1000000 && money < 100000000) {
                str = money / 1000 + "";
                if(str.Length >= 4)
                s = str.Substring (0, str.Length - 3) + "," + str.Substring (str.Length - 3, 3) + "K";
                else
                    s = str.Substring (str.Length - 3, 3) + "K";
            } else if(money >= 100000000) {
                str = (money / 1000000) + "";
                if(str.Length >=4)
                s = str.Substring (0, str.Length - 3) + "," + str.Substring (str.Length - 3, 3) + "M";
                else
                    s = str.Substring (str.Length - 3, 3) + "M";
            }

            return s.ToString ();
    }

    public static string formatMoneyDetail (long money) {
        if(money < 0) {
            money = 0;
        }
        String st = "";
        String rs = "";
        st = money + "";
        for(int i = 0; i < st.Length; i++) {
            rs = rs + st[(st.Length - i - 1)];
            if((i + 1) % 3 == 0 && i < st.Length - 1) {
                rs = rs + ",";
            }
        }
        st = "";
        for(int i = 0; i < rs.Length; i++) {
            st = st + rs[(rs.Length - i - 1)];
        }
        return st;

    }

    public static string formatMoneyDetailDot (long money) {
        if(money < 0) {
            money = 0;
        }
        String st = "";
        String rs = "";
        st = money + "";
        for(int i = 0; i < st.Length; i++) {
            rs = rs + st[(st.Length - i - 1)];
            if((i + 1) % 3 == 0 && i < st.Length - 1) {
                rs = rs + ".";
            }
        }
        st = "";
        for(int i = 0; i < rs.Length; i++) {
            st = st + rs[(rs.Length - i - 1)];
        }
        return st;

    }
    public bool checkNumber (string test) {
        for(int i = 0; i < test.Length; i++) {
            char c = test[i];
            if((('0' > c) || (c > '9'))) {
                return false;
            }
        }
        return true;
    }
}
