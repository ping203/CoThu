using UnityEngine;
using System.Collections;

public class SetNetwork : MonoBehaviour {
	public UILabel lableNetWork;
	UIButton button;
	// Use this for initialization
	void Start () {
		button = GetComponent<UIButton> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick(){
		lableNetWork.text = this.gameObject.name;
	}
}
