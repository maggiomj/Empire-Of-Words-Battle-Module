using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyAttackList {

	 public static List<Attack> attackList = new List<Attack>();
	
	public DummyAttackList() {
		LoadDummyWords();
	}
	
	
	void LoadDummyWords() {
		
		attackList.Add(new Attack("Gregarious", "", Attack.PartOfSpeech.Adj, 2));
		attackList.Add(new Attack("Loqacious", "", Attack.PartOfSpeech.Adj, 2));
		attackList.Add(new Attack("Figment", "", Attack.PartOfSpeech.Noun, 1));
		attackList.Add(new Attack("Abhor", "", Attack.PartOfSpeech.Verb, 3));
		
		attackList.Sort(delegate(Attack p1, Attack p2) {
        	return p1.level.CompareTo(p2.level); // sort by Level
		});
		
	}
	
	public string[] GetNames() {
	
		string[] names = new string[attackList.Count];
		
		int i = 0;
		foreach(Attack a in attackList) {
			names[i] = a.name;
			i++;
		}
		
		return names;
	
	}

	
}
