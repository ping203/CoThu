using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemXepHang : MonoBehaviour {

    public UISprite spriteAvata, sp_medal;
    public UILabel lb_name, lb_info, lb_num_top;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    public void setInfo (string link_ava, int idAvata, string name, string info, int medal) {
        spriteAvata.GetComponent<UISprite> ().enabled = true;
        spriteAvata.GetComponent<UITexture> ().enabled = false;
        spriteAvata.spriteName = idAvata + "";
        if(name.Length > 10) {
            name = name.Substring (0, 7) + "...";
        }
        lb_name.text = name;
        lb_info.text = "Win: " + info;
        lb_num_top.text = (medal + 1) + "";

        if(0 == medal % 2){
            lb_num_top.color = Color.cyan;
        } else {
            lb_num_top.color = Color.yellow;
        }
        sp_medal.gameObject.SetActive (true);
        if(medal == 0) {
            sp_medal.spriteName = "top1";
        } else if(medal == 1) {
            sp_medal.spriteName = "top2";
        } else if(medal == 2) {
            sp_medal.spriteName = "top3";
        } else {
            sp_medal.gameObject.SetActive (false);
        }
    }
}
