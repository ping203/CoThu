using UnityEngine;
using System.Collections;

public class ReflactionChecker : MonoBehaviour
{

	void OnTriggerEnter(Collider other) 
	{
		BallController ballController = other.GetComponent<BallController>();
		if(ballController && !ballController.ballIsOut && ballController.ballReflaction)
		{
			ballController.ballReflaction.GetComponent<Renderer>().enabled = false;
		}
	}
	void OnTriggerExit(Collider other) 
	{
		BallController ballController = other.GetComponent<BallController>();
		if(ballController && !ballController.ballIsOut && ballController.ballReflaction)
		{
			ballController.ballReflaction.GetComponent<Renderer>().enabled = true;
		}
	}
}
