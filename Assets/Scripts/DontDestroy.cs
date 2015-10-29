using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	private static DontDestroy _instance;
	public static DontDestroy instance {
		get {
			if(_instance == null) {
				_instance = new DontDestroy ();
				//_instance = GameObject.Find("bkgChung").GetComponent<GameControl>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad (_instance.gameObject);
			}

//			if(this != _instance.gameObject) DestroyObject(this.gameObject);
			
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		//DontDestroy ();
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
