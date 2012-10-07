using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	
	public int hp;
	public int str;
	public int speed;
	
	public string type;
	
	private BattleScript script;
	public OTSprite sprite;
	public Behaviour halo;
	
	public Transform damagePrefab;
	
	public bool isMyTurn = false;
	
	Color myTurnColor = new Color(.9f, .9f, .8f, 1f);
	
	// Use this for initialization
	public virtual void Start () {
		
		hp = Random.Range (20, 40);
		str = Random.Range(1, 5);
		speed = Random.Range(1, 20);
		
		sprite  = GetComponent<OTSprite>();
		
		script = (BattleScript)Camera.mainCamera.GetComponent("BattleScript");
		if(!script) 
			Debug.Log("dang");
		
		damagePrefab = Resources.Load("DamageText", typeof(Transform)) as Transform;
		
	}
	
	public virtual void OnMouseUp() {
	
		script.SendMessage("Clicked", this);
		
	}
	
	// Orthello Input handler
	public void OnInput(OTObject owner)
	{
	    // check if we clicked the left mouse button
	    if (Input.GetMouseButtonDown(0)) {
			script.SendMessage("Clicked", this);
	    }
	}
	
	void OnMouseOver() {
		if(hp > 0) {
			sprite.tintColor = Color.white;
		}
	}
	
	void OnMouseExit() {
		if(!isMyTurn)
			sprite.tintColor = Color.grey;
		else
			sprite.tintColor = myTurnColor;
	}
	
	//Orthello input handlers
	void onMouseEnterOT() {
		if(hp > 0) {
			sprite.tintColor = Color.white;
		}
	}
	
	void onMouseExitOT() {
		if(!isMyTurn)
			sprite.tintColor = Color.grey;
		else
			sprite.tintColor = myTurnColor;
	}
	
	void TakeDamage(int dam) {
	
		hp -= dam;
		
		// blink?
		sprite.alpha = .5f;
		sprite.alpha = 1f;
		sprite.alpha = .5f;
		sprite.alpha = 1f;
		
		Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
		Transform gui = (Transform) Instantiate(damagePrefab, new Vector3(v.x, v.y, 0), Quaternion.identity); 
		
		if(dam > 0)
			gui.guiText.text = dam.ToString();
		else
			gui.guiText.text = "Miss";
	}
	
	void MyTurn() {
		isMyTurn = true;
		sprite.tintColor = myTurnColor;
	}
	
	void NotMyTurn() {
		isMyTurn = false;
		sprite.tintColor = Color.grey;
	}
	
}
