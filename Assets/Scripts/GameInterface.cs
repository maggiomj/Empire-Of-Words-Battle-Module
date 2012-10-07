using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameInterface
{
	#region Field
	private static GameInterface instance;
	private static List<string> totalWords ;
	private static List<string> knownWords ;
	private static List<string> unknownWords;
	
	private static List<string> totalDefinition ;
	private static List<string> knownDefinition ;
	private static List<string> unknownDefinition;
	
	private static List<string> totalArmy ;
	private static List<string> activeAllies ;
	private static List<string> battleEnemies ;
	
	private static int gameState = 0 ; //0 means game start state...
    #endregion
    
    
    public GameInterface() 
    {
        if (instance != null)
        {
            Debug.LogError ("Cannot have two instances of singleton. Self destruction in 3...");
            return;
        }
        
        instance = this;
    }
    
    public static GameInterface Instance
    {
        get
        {
            if (instance == null)
            {
                new GameInterface();
            }
			
			if(totalWords == null)
			{
				string[] words = {"dubitable", "missive", "obviate", "aspiration","inexorable","calamity",
				"imperative","chronology","subjugate","perfidious","belligerent","commandeered","presently",
				"unabridged","imprudent","deleterious","intrepid","sagacious","denizen"};
				totalWords = new List<string>(words);
			}
		
			if(unknownWords == null)
			{
				string[] words = {"dubitable", " missive", " obviate", "aspiration","inexorable","calamity",
				"imperative","chronology","subjugate","perfidious","belligerent","commandeered","presently",
				"unabridged","imprudent","deleterious","intrepid","sagacious","denizen"};
				unknownWords = new List<string>(words);
			}
		
			if(knownWords == null)
			{
				string[] words = {};
				knownWords = new List<string>(words);
			}
		
			if(totalDefinition == null)
			{
				string[] definition = {"Subject to doubt or question; uncertain.", "A written message; letter.", 
				"Avoid; prevent.", "A hope or ambition of achieving something.","Impossible to stop or prevent.",
				"Disaster and distress.","Of vital importance; necessary; crucial.",
				"The arrangement of events or dates in the order of their occurrence.",
				"Bring under domination or control, esp. by conquest."
				,"Deceitful and untrustworthy.","Hostile and aggressive.","Took possession of (something) without authority."
				,"After a short time; soon.","Not cut or shortened; complete.",
				"Not showing care for the consequences of an action; rash.","Causing harm or damage.",
				"Fearless; adventurous.","Having or showing keen mental discernment and good judgment; shrewd.",
				"An inhabitant or occupant of a particular place."};
				totalDefinition  = new List<string>(definition);
			}
		
			if(unknownDefinition == null)
			{
				string[] definition = {"Subject to doubt or question; uncertain.", "A written message; letter.", 
				"Avoid; prevent.", "A hope or ambition of achieving something.","Impossible to stop or prevent.",
				"Disaster and distress.","Of vital importance; necessary; crucial.",
				"The arrangement of events or dates in the order of their occurrence.",
				"Bring under domination or control, esp. by conquest."
				,"Deceitful and untrustworthy.","Hostile and aggressive.","Took possession of (something) without authority."
				,"After a short time; soon.","Not cut or shortened; complete.",
				"Not showing care for the consequences of an action; rash.","Causing harm or damage.",
				"Fearless; adventurous.","Having or showing keen mental discernment and good judgment; shrewd.",
				"An inhabitant or occupant of a particular place."};
				unknownDefinition = new List<string>(definition);
			}
		
			if(knownDefinition == null)
			{
				string[] definition = {};
				knownDefinition = new List<string>(definition);
			}
		
		
			if(totalArmy == null) 
			{
				string[] allies = {} ;
				totalArmy = new List<string>(allies);
			}
		
			if(activeAllies == null) 
			{
				string[] allies = {"good guy", "good guy"} ;
				activeAllies = new List<string>(allies);
			}
		
			if(battleEnemies == null) 
	    	{
				string[] enemies = {"ptero", "trice"};
				battleEnemies = new List<string>(enemies);
			}
            return instance;
        }
    }


	
    
    /*public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }*/
	// Use this for initialization
	
	#region function
	public void pushNewWord(string word) {
		knownWords.Add(word);
	}
	
	public void pushNewDefinition(string definition)
	{
		knownDefinition.Add(definition) ;
	}
	
	public void popOldWord(int i)
	{
		unknownWords.RemoveAt(i);
	}
	
	public void popOldDefinition(int i)
	{
		unknownDefinition.RemoveAt(i) ;
	}
	
	public string getWord(int i)
	{
		return totalWords[i] ;
	}
	public string getDefinition(int i)
	{
		return totalDefinition[i] ;
	}
		
		
	public List<string> GetKnownWords() {
		return knownWords;
	}
	public List<string> GetKnownDefinition() {
		return knownDefinition ;
	}
	
	public List<string> GetTotalWords() {
		return totalWords;
	}
	public List<string> GetTotalDefinitions() {
		return totalDefinition ;
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
	
	public List<string> GetEnemies() {
		return battleEnemies;
	}
	
	public void setGameState(int state)
	{
		gameState = state ;
	}
	
	public int getGameState()
	{
		return gameState ;
	}
	
	#endregion
}
