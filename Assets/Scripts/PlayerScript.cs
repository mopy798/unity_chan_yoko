using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnitySampleAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour {
	
	public float speed = 4f;//ユニティちゃん速度
	public float jumpPower = 700;//ユニティちゃんジャンプ力
	public LayerMask groundLayer;//着地判定
	public GameObject mainCamera;
	public GameObject bullet;
	public LifeScript lifeScript;
	
	private Rigidbody2D rigidbody2D;//リジッドボディ
	private Animator anim;//アニメーター
	private Renderer renderer;//レンダラー
	private bool isGrounded;
	private bool gameClear = false;
	public Text clearText;
	
	void Start () {
		anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		renderer = GetComponent<Renderer>();
	}

	void Update ()
	{
		Debug.Log(isGrounded);
		//着地判定
		isGrounded = Physics2D.Linecast (
			transform.position + transform.up * 1,
			transform.position - transform.up * 0.05f,
			groundLayer);
		
		if (!gameClear) {
			//ジャンプボタン押下(スマホ対応)
			if (CrossPlatformInputManager.GetButtonDown("Jump")) {
				if (isGrounded) {
					anim.SetTrigger ("Jump");
					isGrounded = false;
					rigidbody2D.AddForce (Vector2.up * jumpPower);
				}
			}
		}
		
		//ジャンプアニメーション
		float velY = rigidbody2D.velocity.y;
		bool isJumping = velY > 0.1f ? true : false;
		bool isFalling = velY < -0.1f ? true : false;
		anim.SetBool ("isJumping", isJumping);
		anim.SetBool ("isFalling", isFalling);

		if (!gameClear) {
			//ショットボタン押下
			if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
				anim.SetTrigger ("Shot");
				Instantiate (bullet, transform.position + new Vector3 (0f, 1.2f, 0f), Quaternion.identity);
			}
			//画面外に落ちた時にゲームオーバー
			if (gameObject.transform.position.y < Camera.main.transform.position.y - 8) {
				lifeScript.LifeDown(240);
			}
		}
	}
	
	void FixedUpdate ()
	{
		if (!gameClear) {
			int x = 0;
			if (CrossPlatformInputManager.GetAxisRaw ("Horizontal") >= 0.01f) {
				x = 1;
			} else if (CrossPlatformInputManager.GetAxisRaw ("Horizontal") <= -0.01f) {
				x = -1;
			}

			//ユニティちゃんが左右どちらかに進んでいたら
			if (x != 0) {
				//歩きアニメーション
				anim.SetBool ("Dash", true);
				//移動
				rigidbody2D.velocity = new Vector2 (x * speed, rigidbody2D.velocity.y);
				//進行方向を向かせる
				Vector2 temp = transform.localScale;
				temp.x = x;
				transform.localScale = temp;
				//カメラ位置調整
				if (transform.position.x > mainCamera.transform.position.x - 4) {
					Vector3 cameraPos = mainCamera.transform.position;
					cameraPos.x = transform.position.x + 4;
					mainCamera.transform.position = cameraPos;
				}
				Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
				Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
				Vector2 pos = transform.position;
				pos.x = Mathf.Clamp (pos.x, min.x + 0.5f, max.x);
				transform.position = pos;
			} else {
				//移動しない
				rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
				//歩きアニメーションOFFに
				anim.SetBool ("Dash", false);
			}
		//ゲームをクリアした時
		} else {
			//クリアテキスト表示
			clearText.enabled = true;
			//ユニティちゃんを自動的に前進させる
			anim.SetBool ("Dash", true);
			rigidbody2D.velocity = new Vector2 (speed, rigidbody2D.velocity.y);
			//クリアの5秒後にタイトルへ戻る
			Invoke("CallTitle", 5);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (!gameClear) {
			//敵と衝突時
			if (col.gameObject.tag == "Enemy") {
				StartCoroutine ("Damage");
			}
		}
	}
	
	IEnumerator Damage ()
	{
		//ユニティちゃんを点滅させる
		//PlayerDamageレイヤーの時は、敵との衝突判定無効
		gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
		int count = 10;
		while (count > 0){
			renderer.material.color = new Color (1,1,1,0);
			yield return new WaitForSeconds(0.05f);
			renderer.material.color = new Color (1,1,1,1);
			yield return new WaitForSeconds(0.05f);
			count--;
		}
		//レイヤーを元に戻す
		gameObject.layer = LayerMask.NameToLayer("Player");
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		//ゲームクリア
		if (col.tag == "ClearZone") {
			gameClear = true;
		}
	}
	
	void CallTitle ()
	{
		Application.LoadLevel("Title");
	}
}