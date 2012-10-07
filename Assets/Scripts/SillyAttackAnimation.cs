using UnityEngine;
using System.Collections;

public class SillyAttackAnimatin : MonoBehaviour {

	private float ttl;
	private float duration = 2f;
	private float scroll = 0.005f;
	
	// Use this for initialization
	void Start () {
		guiText.material.color = Color.red;
		ttl = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (ttl > 0){
			transform.Translate(scroll, 0, Time.deltaTime); 
			ttl -= Time.deltaTime/duration; 
		} else {
    		Destroy(transform.gameObject);
		}
	}
}