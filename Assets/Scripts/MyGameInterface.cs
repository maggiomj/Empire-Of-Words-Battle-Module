using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyGameInterface : MonoBehaviour {
	
	private List<string> activeAllies;
	private List<string> knownWords;
	private List<string> battleEnemies;
	
	private string goodGuy = "good guy";
	
	// Use this for initialization
	void Start () {
	
		DontDestroyOnLoad(this);
		
		if(activeAllies == null) {
			string[] allies = {goodGuy, goodGuy}; // temporary
			activeAllies = new List<string>(allies);
		}
		
		if(knownWords == null) {
			string[] words = {"Abhor", "Locquacious", "Libel", "Gregarious"}; // Temporary
			knownWords = new List<string>(words);
		}
		
		if(battleEnemies == null) {
			string[] enemies = {"ptero", "trice"}; // temporary
			battleEnemies = new List<string>(enemies);
		}
	}
	
	public void PushNewWord(string word) {
		knownWords.Add(word);
	}
	
	public void PushNewAlly(string ally) {
		activeAllies.Add(ally);
	}
	
	public void PushEnemy(string enemy) {
		battleEnemies.Add(enemy);
	}
	
	public void ClearBattleEnemies() {
		battleEnemies.Clear();
	}
	
	public List<string> GetAllies() {
		return activeAllies;
	}
	
	public List<string> GetKnownWords() {
		return knownWords;
	}
	
	public List<string> GetEnemies() {
		return battleEnemies;
	}
	
}
