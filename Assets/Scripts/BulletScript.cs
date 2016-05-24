using UnityEngine;
using System.Collections;

public class BulletScript: MonoBehaviour {
	
	private GameObject player;
	private int speed = 10;
	
	void Start () {
		player = GameObject.FindWithTag("UnityChan");
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		rigidbody2D.velocity = new Vector2 (speed * player.transform.localScale.x, rigidbody2D.velocity.y);
		Vector2 temp = transform.localScale;
		temp.x = player.transform.localScale.x;
		transform.localScale = temp;
		Destroy(gameObject, 5);
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Enemy") {
			Destroy(gameObject);
		}
	}
}