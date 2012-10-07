using UnityEngine;
using System.Collections;

public class AnswerScript : MonoBehaviour {

	private float ttl;
	private float duration = 1.5f;
	
	void Start () {
		guiText.material.color = new Color(0f, .5f, 0f, 1f);
		ttl = 1;
	}
	
	void Update () {
		if (ttl > 0){
			ttl -= Time.deltaTime/duration; 
		} else {
    		Destroy(transform.gameObject);
		}
	}
}
