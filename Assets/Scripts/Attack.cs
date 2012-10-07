using UnityEngine;
using System.Collections;

public class Attack {
			
	public enum PartOfSpeech {
		Noun,
		Verb,
		Adj,
		Adv
	};
	
	public string name;
	public string animation;
	public PartOfSpeech partOfSpeech;
	public int level;
	
	public Attack(string n, string a, PartOfSpeech pos, int l) {
		name = n;
		animation = a;
		partOfSpeech = pos;
		level = l;
	}
	
}

