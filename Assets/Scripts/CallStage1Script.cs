using UnityEngine;
using System.Collections;

public class CallStage1Script : MonoBehaviour {

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Application.LoadLevel("Stage1");
		}
	}
}
