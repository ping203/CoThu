using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetNameNetWork : MonoBehaviour {

	Text textName;
	public Text textShow;
	public GameObject popup;

	// Use this for initialization
	void Start () {
		textName = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setName(){
		textShow.text = textName.text;
		popup.SetActive (false);
	}
}
