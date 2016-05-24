using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	
	public int healPoint = 20;
	//Prefab化するとInspectorから指定できないためprivate化
	private LifeScript lifeScript;
	
	void Start ()
	{
		//HPタグの付いているオブジェクトのLifeScriptを、スクリプトから取得
		lifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<LifeScript>();
	}
	
	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "UnityChan") {
			lifeScript.LifeUp(healPoint);
			Destroy(gameObject);
		}
	}
}
