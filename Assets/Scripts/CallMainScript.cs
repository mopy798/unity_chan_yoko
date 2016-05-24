using UnityEngine;
using System.Collections;

public class CallMainScript : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(2);
		Application.LoadLevel("Main");
	}
}