using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System.Collections.Generic;

public class CueForce : MonoBehaviour {
    public float cueForceValue = 0.0f;
    [SerializeField]
    private float startValue = 1.0f;
    [SerializeField]
    private CueController cueController;

    //UISlider sliderCue;
	public UnityEngine.UI.Slider sliderCue;
    void Awake () {
        //if(MenuControllerGenerator.controller) {
        //    if(!MenuControllerGenerator.controller.isTouchScreen) {
        //        transform.parent.gameObject.SetActive (false);
        //        return;
        //    } 
        //}
		//sliderCue = GetComponent<UnityEngine.UI.Slider> ();
          //EventDelegate.Set (sliderCue.onChange, delegate { MoveForceSlider (); });
          //sliderCue.onDragFinished += finishDrag;

		sliderCue.onValueChanged.AddListener (delegate {MoveForceSlider ();});
    }
    void Start () {
        sliderCue.value = startValue;
        cueController.inTouchForceSlider = false;
        cueController.cueForceisActive = false;
    }
    
    public void MoveForceSlider () {
        if(!cueController.allIsSleeping)
            return;
        if(ServerController.serverController && !ServerController.serverController.isMyQueue)
            return;

        MenuControllerGenerator.controller.canRotateCue = false;
        cueController.inTouchForceSlider = true;
        cueController.cueForceisActive = true;

		cueForceValue = 1 - sliderCue.value;
		cueController.cueDisplacement = cueController.cueMaxDisplacement * cueForceValue;
    }

	void finishDrag () {        
		cueForceValue = 1 - sliderCue.value;
		cueController.cueDisplacement = cueController.cueMaxDisplacement * cueForceValue;
		sliderCue.value = startValue;
	}

    public void Resset () {
        StartCoroutine (WaitAndRessetValue ());
    }
    IEnumerator WaitAndRessetValue () {
        yield return new WaitForEndOfFrame ();
		if(sliderCue.gameObject.activeInHierarchy)
        sliderCue.value = startValue;
    }
}
