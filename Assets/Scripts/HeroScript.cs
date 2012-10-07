using UnityEngine;
using System.Collections;

public class HeroScript : PlayerScript {

	/*void TakeDamage(int dam) {
	
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
		sprite.tintColor = Color.green;
	}
	
	void NotMyTurn() {
		isMyTurn = false;
		sprite.tintColor = Color.grey;
	}*/
}
