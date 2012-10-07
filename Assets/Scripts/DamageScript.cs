using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	private float ttl;
	private float duration = 1.5f;
	private float scroll = 0.001f;
	
	// Use this for initialization
	void Start () {
		guiText.material.color = Color.red;
		ttl = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (ttl > 0){
			transform.Translate(0, scroll, Time.deltaTime); 
			ttl -= Time.deltaTime/duration; 
		} else {
    		Destroy(transform.gameObject);
		}
	}
}
