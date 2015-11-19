using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RuleControl : MonoBehaviour {

    public  Image sprite;
    Vector3 offsetDown, offsetUp;

    public CueController cueController;
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }


    public void onDown () {
        Debug.Log ("===================== onDown");
        offsetDown = Input.mousePosition;
    }
    public void onDrag () {
        Debug.Log ("===================== onDrag");
        offsetUp = Input.mousePosition;

        Vector2 siz =  sprite.rectTransform.sizeDelta;
        float delta = offsetDown.y - offsetUp.y;
        if(delta > 0) {
            siz.y += 2;
            //sprite.rectTransform.sizeDelta = siz;
            if(siz.y > 600)
                siz.y = 496;
        } else {
            siz.y -= 2;
            if(siz.y < 400)
                siz.y = 496;
        }

        sprite.rectTransform.sizeDelta = siz;
    }
    public void onUp () {
        Debug.Log ("===================== onUp");
        offsetDown = new Vector3 (0, 0, 0);
        offsetUp = new Vector3 (0, 0, 0);
    }
}
