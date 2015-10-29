using UnityEngine;
using System.Collections;

public class BallPivotController : MonoBehaviour {
    [SerializeField]
    private CircularSlider circularSlider;
    public float radius = 0.75f;
    private Vector3 strPosition = Vector3.zero;
    [SerializeField]
    private CueController cueController;



    void Start () {
        circularSlider.CircularSliderPress += SlideBallPivot;
        strPosition = transform.position;
    }

    void SlideBallPivot (CircularSlider circularSlider) {
        if(ServerController.serverController && !ServerController.serverController.isMyQueue)
            return;

        MenuControllerGenerator.controller.canControlCue = false;
        transform.localPosition = new Vector3 (-circularSlider.displacementZ, circularSlider.displacementX, 0.0f);
        float distance = Vector3.Distance (transform.position, strPosition);
        if(distance > radius) {
            transform.position -= (distance - radius) * (transform.position - strPosition).normalized;
        }
    }
    public void SetPosition (Vector3 localPosition) {
        transform.localPosition = radius * localPosition;
    }
    public void Reset () {
        transform.position = strPosition;
    }
}
