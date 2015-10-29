using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Toast : MonoBehaviour
{
    public UILabel label;

    public UISprite[] spriteBalls;
    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setText(string mess)
    {
        this.gameObject.SetActive(true);
        label.text = mess;
        StartCoroutine(showT());
       
    }

    public void setText (string mess, int startIndex) {
        this.gameObject.SetActive (true);
        label.text = mess;
        if(startIndex > 0) {
            for(int i = 0; i < 7; i++) {
                spriteBalls[i].gameObject.SetActive (true);
                spriteBalls[i].spriteName = "bito" + (i + startIndex);
            }
        }
        StartCoroutine (showT ());
    }

    IEnumerator showT()
    {
        TweenAlpha.Begin(gameObject, 0f, 0);
        TweenAlpha.Begin(gameObject, 0.5f, 1);
        yield return new WaitForSeconds(3);
        TweenAlpha.Begin(gameObject, 0.5f, 0);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}
