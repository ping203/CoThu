using UnityEngine;
using System.Collections;

public class StageControl : MonoBehaviour {
    public GameControl gameControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public virtual void Appear()
    {
        gameObject.SetActive(true);
    }
    public void DisAppear() {
        gameObject.SetActive(false);
    }

    public virtual void onBack()
    {

    }
}
