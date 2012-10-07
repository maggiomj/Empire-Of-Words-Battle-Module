using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

public class BattleScript : MonoBehaviour {
	
	bool showSelectionMenu;
	PlayerScript target;
	PlayerScript curActor;
	
	string action;
	Vector2 actionScrollPosition = Vector2.zero;
	Vector2 logScrollPosition = Vector2.zero;
	
	StringBuilder battlelog = new StringBuilder();
	//private string battlelog;
	
	int playerCursor = 0;
	int lineCounter = 3;
	
	OTAnimatingSprite animateMe;

	enum GameState {
    	PlayerTurn,
    	OponentTurn,
    	Win,
		Lose
	}	 
	
	GUIStyle wrapText;
	
	GameState state;
	static string prompt_skipString = "Skipped";
	static string prompt_wrongString = "Incorrect! No Damage!";
	static string prompt_rightString = "Correct! Extra Damage!";
	
	// player GUI
	int actionSelection = -1;
	int questionSelection = -1;
	string[] actionSelectionStrings;
	string[] questionSelectionStrings;
	DummyAttackList attacks;
	
	bool animating = false; 
	bool displayQuestion  = false;
	
	int boxDepth;
	
	// this will be all the players
	List<PlayerScript> players = new List<PlayerScript>();
	List<HeroScript> heroes = new List<HeroScript>();
	List<EnemyScript> enemies = new List<EnemyScript>();
	
	int screenWidth;
	int screenHeight;
	
	bool battleWon = false;
	bool battleLost = false;
	
	float questionStartTime = 20f;
	float questionRunningTime = 20f;
	
	//reference to the GameInterface Objects
	//GameInterface2 interfaceAccess;
	GameInterface gameInterfaceAccess;
	
	bool pickedAnswers = false;
	string skipString = "SKIP";
	bool firstTurn = true;
	
	Vector3 answerPrefabPos;
	Transform answerPrefab;
	
	Vector3 answerTextPos;
	int correctAnswerPos;
	
	string[] demoDefs;
	
	List<String> totalWords;
	List<String> totalDefs;
	
	int numDefs;
	
	public GUISkin battleSkin;
	
    // Use this for initialization
    IEnumerator Start () {
	
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		answerTextPos = new Vector3(-30, 50, 0);
		answerPrefab = Resources.Load("AnswerText", typeof(Transform)) as Transform;
		
		OT.RuntimeCreationMode(); // needed to create people at runtime
		
		answerPrefabPos = Camera.main.WorldToViewportPoint(answerTextPos);
		answerPrefabPos.z = 0;
		
		//First load the interface
		gameInterfaceAccess = GameInterface.Instance;
		
		// then load the characters
		LoadSpriteScene();
		
		// sort characters by speed
		SortPlayers();
		
		totalWords = gameInterfaceAccess.GetTotalWords();
		totalDefs = gameInterfaceAccess.GetTotalDefinitions();
		numDefs = totalDefs.Count;
		
		//dummy attack list
		actionSelectionStrings = gameInterfaceAccess.GetKnownWords().ToArray();
		if(actionSelectionStrings.Length <= 0) {
			Debug.Log("demo");
			actionSelectionStrings = new string[4]{"Dubitable", "Abhor", "Loquacious", "Periphery"};
		}
		demoDefs = new string[4]{"Subject to doubt or question; uncertain", "Regard with disgust and hatred", "Talkative", "The outer limits or edge of an area or object"};
		questionSelectionStrings = new string[5]{"good", "bad", "fast", "slow", skipString};
		
		AppendToBattleLog("The battle commences!");
		SetupFirstMove();
		
		return null;
    }
	
	void Update() {
			
		// hack to make the first player light up
		if(firstTurn) {
			curActor.SendMessage("MyTurn");
			firstTurn = false;
		}
		
		if(!animating){
		
			switch(state) {
			case GameState.PlayerTurn:
				showSelectionMenu = true;
				if(target != null) {
					AskQuestion();
				}
				if(target != null && questionSelection != -1) {
					PlayerAttack();
					DetermineNextState();
				}
				break;
				
			case GameState.OponentTurn:
				showSelectionMenu = false;
				AISelect();
				DetermineNextState();
				break;
			case GameState.Win:
				WinScenario();
				break;
			case GameState.Lose:
				LoseScenario();
				break;
			default:
				Debug.Log("unknown state");
				break;
			}
		} else {
			AnimateOnce();
		}	
	}
	
	void SetupFirstMove() {
	
		curActor = players[0];
		if(curActor.GetType() == typeof(EnemyScript)) {
			state = GameState.OponentTurn;
		} else {
			state = GameState.PlayerTurn;
		}
		
		AppendToBattleLog(curActor.name+"'s turn");
		
	}
	
	void AnimateOnce() {
		Debug.Log("animating");
		GameObject animateObj  = OT.CreateObject("fireball");
		animateMe = animateObj.GetComponent("OTAnimatingSprite") as OTAnimatingSprite;
		animateMe.PlayOnce("fire");
	}
	
	// The OnAnimationFinish delegate will be called when an animation or 
	// animation frameset finishes playing.
	public void OnAnimationFinish(OTObject owner) {
	    if (owner == animateMe) {
	        animateMe = null;
			animating = false;
	    }
	}
	
	void AppendToBattleLog(string s) {
		
		battlelog.AppendLine(s);
		lineCounter++;
		logScrollPosition.y += 40;
	}
	
	void LoadSpriteScene() {
		
		List<string> allyStrings = gameInterfaceAccess.GetAllies();
		List<string> enemyStrings = gameInterfaceAccess.GetEnemies();
		
		int verticalPos = -170;
		
		foreach(string enemy in enemyStrings) {
			
			GameObject enemyObj = OT.CreateObject(enemy);
			OTSprite sprite = enemyObj.GetComponent("OTSprite") as OTSprite;
			
			sprite.position = new Vector2(75, verticalPos);
			sprite.depth = -10;
			
			verticalPos -= 60;
			
			InitEnemy(enemyObj, enemy);
		}
		
		verticalPos = -200;
		
		foreach(string ally in allyStrings) {
			
			GameObject allyObj = OT.CreateObject(ally);
			OTSprite sprite = allyObj.GetComponent("OTSprite") as OTSprite;
			
			sprite.position = new Vector2(-220, verticalPos);
			sprite.depth = -10;
			
			verticalPos -= 60;
				
			InitAlly(allyObj, ally);
		}
	}
	
	
	void InitEnemy(GameObject obj, string type) {
		EnemyScript sc = obj.GetComponent("EnemyScript") as EnemyScript;
		sc.type = type;

		enemies.Add(sc);
		players.Add(sc);
	
	}
	
	void InitAlly(GameObject obj, string type) {
		HeroScript sc = obj.GetComponent("HeroScript") as HeroScript;
		sc.type = type;
		
		heroes.Add(sc);
		players.Add(sc);
	
	}
	
	void SortPlayers() {
		
		players.Sort(delegate(PlayerScript p1, PlayerScript p2) {
        	return p1.speed.CompareTo(p2.speed); // sort by speed
		});
		
	}
	
	
	/******************************
	 * 
	 * Attacking Logic, including
	 *   
	 *   ** Asking Questions
	 *   ** Determining Correctness
	 *  
	 * 
	 ******************************/
	
	void AskQuestion() {
		displayQuestion = true;
		showSelectionMenu = false;
		
		if(!pickedAnswers) {
			PickAnswersOnce();
		}
		
		questionRunningTime = questionRunningTime - Time.deltaTime;
		if(questionRunningTime < 0) {
			questionSelection = 4; // skip
		}
		
	}
	
	void AnswerPrompt(string t) {
		Debug.Log("Caught "+t);
		Transform answerText = Instantiate(answerPrefab, answerPrefabPos, Quaternion.identity) as Transform;
		if(t == prompt_skipString)
			answerText.guiText.material.color = Color.white;
		if(t == prompt_wrongString)
			answerText.guiText.material.color = Color.red;
		// green is default color (Correct color)
		answerText.guiText.text = t;
	}
	
	void PickAnswersOnce() {
		
		List<string> potentialAnswers = new List<string>();
		int correctIndex = totalWords.IndexOf(actionSelectionStrings[actionSelection]);
		
		// get correct definition
		// potentialAnswers.Add(totalDefs[correctIndex]);
		potentialAnswers.Add(demoDefs[actionSelection]);
		
		int incorrectOne = RandomRange(0, numDefs - 1);
		potentialAnswers.Add(totalDefs[incorrectOne]);
		
		int incorrectTwo;
		do incorrectTwo = RandomRange(0, numDefs- 1); while (incorrectOne == incorrectTwo || incorrectTwo == correctIndex);
		potentialAnswers.Add(totalDefs[incorrectTwo]);
		
		int incorrectThree;
		do incorrectThree = RandomRange(0, numDefs - 1); while (incorrectOne == incorrectThree || incorrectTwo == incorrectThree || incorrectThree == correctIndex);
		potentialAnswers.Add(totalDefs[incorrectThree]);
		
		List<string> shuffledAnswers = new List<string>(Shuffle(potentialAnswers.ToArray()));
		shuffledAnswers.Add(skipString);
		
		questionSelectionStrings = shuffledAnswers.ToArray();
		
		pickedAnswers = true;
	}
	
	bool IsQuestionCorrect() {
		
		// works for demo only, rework logic for real game
		return questionSelectionStrings[questionSelection] == demoDefs[actionSelection];
	}
	
	void PlayerAttack() {
		
		int damage = 0;
		string correctText;
		
		if(questionSelection == 4) {
			correctText = prompt_skipString;
			damage = curActor.str + RandomRange(5, 15);
		} else if (IsQuestionCorrect()) {
			correctText = prompt_rightString;
			damage = curActor.str + RandomRange(15, 25); // double damage
		} else {
			correctText = prompt_wrongString;
			damage = 0; // dud
		}
		
		this.SendMessage("AnswerPrompt", correctText);
		target.SendMessage("TakeDamage", damage);
		
		AppendToBattleLog(curActor.name+" uses "+actionSelectionStrings[actionSelection]+" against "+ target.name+" for "+damage+" damage!");
		
		adjustDead(target);
		actionSelection = -1;
		questionSelection = -1;
		questionRunningTime = questionStartTime; // start the timer over for next time
		
		//animating = true;
	}
	
	IEnumerator AISelect() {
	
		// read state and act
		target = HeroSelection();
		int damage = curActor.str + RandomRange (0,10);
		target.SendMessage("TakeDamage", damage);
		AppendToBattleLog("Enemy " + curActor.name + " attacks "+ target.name+" for "+damage+" damage!");
		adjustDead(target);
		
		return null;
	}
	
	void adjustDead(PlayerScript t) {
		if (t.hp <= 0) {
			t.hp = 0;
			//play dead
			t.sprite.alpha = .5f;
			AppendToBattleLog(t.name+ " has fallen!");
		}
	}
	
	PlayerScript HeroSelection() {
	// in case we want better AI later
		StartCoroutine(SecondBreather(2.0f));
		return heroes[RandomRange(0, heroes.Count-1)];
		
	}
	
	IEnumerator SecondBreather(float n) {
		yield return new WaitForSeconds(n);
	}
	
	/**
	 * 
	 * 
	 * Determining the next state of battle
	 * 
	 * 
	 * */
	
	void DetermineNextState() {
	
		// resetting stuff
		target = null;
		displayQuestion = false;
		pickedAnswers = false;
		
		curActor.SendMessage("NotMyTurn");
		
		if(DeadChecker(heroes.ToArray())) {
			state = GameState.Lose;
			return;
		}
		
		if(DeadChecker(enemies.ToArray())) {
			state = GameState.Win;
			return;
		}
		
		// skip dead people
		do {
			playerCursor = (playerCursor + 1) % players.Count;
			curActor = players[playerCursor];
		} while(curActor.hp <=0);
		
		if(curActor.GetType() == typeof(EnemyScript)) {
			state = GameState.OponentTurn;
		} else {
			state = GameState.PlayerTurn;
		}
		
		AppendToBattleLog(curActor.name+"'s turn");
		curActor.SendMessage("MyTurn");
	}
	
	
	bool DeadChecker(PlayerScript[] plys) {
	
		foreach(PlayerScript p in plys) {
			if(p.hp > 0) return false;
		}
		return true;
		// everyone's dead!
	}
	
	void OnGUI () {
		
		GUI.skin = battleSkin;
		
		wrapText = GUI.skin.button;
		wrapText.wordWrap = true;
		
		GUI.Box(new Rect(screenWidth/2 -150, 10, 300, 100), "");
		
		GUILayout.BeginArea(new Rect(screenWidth/2 -150, 10, 300, 100));
			logScrollPosition = GUILayout.BeginScrollView(logScrollPosition, GUILayout.MaxHeight(100), GUILayout.ExpandHeight(false));
				GUILayout.Label(battlelog.ToString(), GUILayout.ExpandHeight(true));
			GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		GUI.Box(new Rect(screenWidth-350, screenHeight-200, 360, 260), "Stats");
		
		int i = 0;
		for(i = 0; i < heroes.Count; i++) {
			GUI.Label(new Rect(screenWidth-330, screenHeight-170+30*i, 120, 30), heroes[i].name+": "+heroes[i].hp);
		}
		for(i = 0; i < enemies.Count; i++) {
			GUI.Label(new Rect(screenWidth-150, screenHeight-170+30*i, 120, 30), enemies[i].name+": "+enemies[i].hp);
		}
		
		
		if(showSelectionMenu) {
		
			GUI.Box(new Rect(10, 280, 200, 250), "Attacks");
    		actionScrollPosition = GUI.BeginScrollView(new Rect (10, 300, 200, 200), actionScrollPosition, new Rect (0, 0, 150, Math.Min(screenHeight - 350, actionSelectionStrings.Length*50)), false, true);
				actionSelection = GUI.SelectionGrid(new Rect(10, 10, 170, actionSelectionStrings.Length*40), actionSelection, actionSelectionStrings, 1);
			GUI.EndScrollView();
		}
		
		if(displayQuestion) {
			GUI.Box(new Rect(screenWidth-350, 0, 250, 500), "Define "+actionSelectionStrings[actionSelection]+ "    "+Mathf.CeilToInt(questionRunningTime).ToString());
			questionSelection = GUI.SelectionGrid(new Rect(screenWidth-330, 30, 220, 450), questionSelection, questionSelectionStrings, 2, wrapText);
		}
		
		if(battleWon) {
			// show some stuff probably
			if(GUI.Button(new Rect(screenWidth/2-50, screenHeight/2-30, 100, 60), "Exit Battle")) {
				CleanUp();
				//Application.LoadLevel ("main");
			}
		}
		if(battleLost) {
			// show some stuff probably
			if(GUI.Button(new Rect(screenWidth/2-50, screenHeight/2-30, 10, 60), "Exit Battle")) {
				CleanUp();
				//Application.LoadLevel ("main");
			}
		}
	}
	
	/****************************
	 * 
	 *  All PlayerScripts (Heroes and Enemies) send a message for the Clicked function
	 *  When they are clicked. For now the only action we take upon being clicked is if
	 *  The attack is happening
	 * 
	 * 
	 * */
	
	
	void Clicked(PlayerScript t) {
	
		// if the target is an alive enemy, an action has been select and the question isn't being asked
		if((t is EnemyScript) && (actionSelection >= 0) && (t.hp > 0) && !displayQuestion) {
			target = t;
		}
		
		//otherwise display info maybe?
	
	}
	
	int RandomRange(int min, int max) {
	
		return UnityEngine.Random.Range(min, max);
	}
	
	void WinScenario() {
		if(!battleWon) {
			AppendToBattleLog("You are Victorious!");
			battleWon = true;
			Debug.Log("win!");
		}
	}
	
	void LoseScenario() {
		if(!battleLost) {
			AppendToBattleLog("You have been vanquished...");
			battleLost = true;
			Debug.Log("lose!");
		}
	}
	
	void CleanUp() {
		gameInterfaceAccess.ClearBattleEnemies();
		OT.DestroyAll();
	}
	
	
	string[] Shuffle(string[] list)  {  
		
	    System.Random r = new System.Random();  
	    int n = list.Length;  
	    while (n > 1) {  
	        n--;  
	        int k = r.Next(n + 1);  
	        string value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
		}
		
		return list;
	}
}