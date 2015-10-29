using UnityEngine;
using System.Collections;

public class ActionsManager : MonoBehaviour {

	public GameObject panelActions;
	public UIToggle toggleAction;
	public UIButton buttonInvite;

	// Use this for initialization
	void Start () {
		panelActions.gameObject.SetActive (false);
		/*toggleAction.value = false;
		buttonInvite.gameObject.SetActive (true);*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onHide (bool av) {
		panelActions.gameObject.SetActive (!av);
		toggleAction.value = !av;
		buttonInvite.gameObject.SetActive (av);
	}
}
