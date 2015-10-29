using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour
{
	public UILabel lblContent;
	public UISprite spriteSmile;
	private String[] smileName = new String[] { "a1", "a2", "a3", "a4", "a5",
		"a6", "a7", "a8", "a9", "a10", "a11", "a12", "a13", "a14", "a15",
		"a16", "a17", "a18", "a19", "a20", "a21", "a22", "a23", "a24",
		"a25", "a26", "a27", "a28" };
	public static String[] smileys = { ":(", ";)", ":D", ";;)", ">:D<", ":-/",
		":x", ":-O", "X(", ":>", ":-S", "#:-S", ">:)", ":(|", ":))", ":|",
		"/:)", "=;", "8-|", ":-&", ":-$", "[-(", "(:|", "=P~", ":-?",
		"=D>", "@-)", ":-<" };

    Dictionary<string, string> emoticons = new Dictionary<string, string> ();

	// Use this for initialization
	void Awake ()
	{
        emoticons.Add (":(", "a1");
        emoticons.Add (";)", "a2");
        emoticons.Add (":D", "a3");
        emoticons.Add (";;)", "a4");
        emoticons.Add (">:D<", "a5");
        emoticons.Add (":-/", "a6");
        emoticons.Add (":x", "a7");
        emoticons.Add (":-O", "a8");
        emoticons.Add ("X(", "a9");
        emoticons.Add (":>", "a10");
        emoticons.Add (":-S", "a11");
        emoticons.Add ("#:-S", "a12");
        emoticons.Add (">:)", "a13");
        emoticons.Add (":(|", "a14");
        emoticons.Add (":))", "a15");
        emoticons.Add (":|", "a16");
        emoticons.Add ("/:)", "a17");
        emoticons.Add ("=;", "a18");
        emoticons.Add ("8-|", "a19");
        emoticons.Add (":-&", "a20");
        emoticons.Add (":-$", "a21");
        emoticons.Add ("[-(", "a22");
        emoticons.Add ("(:|", "a23");
        emoticons.Add ("=P~", "a24");
        emoticons.Add (":-?", "a25");
        emoticons.Add ("=D>", "a26");
        emoticons.Add ("@-)", "a27");
        emoticons.Add (":-<", "a28");
	}

    void Start(){
        this.gameObject.SetActive (false);
    }
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	internal void setText (string content)
	{
        string temp;
        bool check = emoticons.TryGetValue (content, out temp);
        if(check) {
            lblContent.gameObject.SetActive (false);
            spriteSmile.gameObject.SetActive (true);
            spriteSmile.spriteName = temp;
        } else {
            lblContent.gameObject.SetActive (true);
            spriteSmile.gameObject.SetActive (false);
            /*if(content.Length > 50) {
                lblContent.text = (content.Substring (0, 25) + "...");
            } else {*/
                lblContent.text = (content);
           // }
        }

		gameObject.SetActive (true);
		gameObject.transform.localScale = new Vector3 (0, 0, 0);
		TweenScale.Begin (gameObject, 0.2f, new Vector3 (1, 1, 1));
		StopAllCoroutines ();
		StartCoroutine (setInvisible ());
	}

	IEnumerator setInvisible ()
	{
		yield return new WaitForSeconds (2);
		TweenScale.Begin (gameObject, 0.2f, new Vector3 (0, 0, 0));
		yield return new WaitForSeconds (0.5f);
		gameObject.SetActive (false);
	}
}
