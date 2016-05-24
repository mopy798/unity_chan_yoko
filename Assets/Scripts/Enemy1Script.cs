using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {
	
	Rigidbody2D rigidbody2D;
	public int speed = -3;
	public GameObject explosion;
	public int attackPoint = 10;
	public GameObject item;
	
	private LifeScript lifeScript;
	private const string MAIN_CAMERA_TAG_NAME = "MainCamera";
	private bool _isRendered = false;
	
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
		lifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<LifeScript>();
	}
	
	void Update ()
	{
		if (_isRendered) {
			rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
		}
//********** 開始 **********//
		if (gameObject.transform.position.y < Camera.main.transform.position.y - 8 ||
		gameObject.transform.position.x < Camera.main.transform.position.x - 10) {
			Destroy(gameObject);
		}
//********** 終了 **********//
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (_isRendered) {
			if (col.tag == "Bullet") {
				Destroy (gameObject);
				Instantiate (explosion, transform.position, transform.rotation);
				if (Random.Range (0, 4) == 0) {
					Instantiate (item, transform.position, transform.rotation);
				}
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "UnityChan") {
			lifeScript.LifeDown (attackPoint);
		}
	}
	
	void OnWillRenderObject()
	{
		if(Camera.current.tag == MAIN_CAMERA_TAG_NAME){
		_isRendered = true;
		}
	}
}