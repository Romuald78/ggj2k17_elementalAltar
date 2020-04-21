using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public GameObject scorePrefab;
	public Text scoreText;
	public GameObject wave_R;
	public GameObject wave_G;
	public GameObject wave_B;
	public GameObject wave_Y;
	private string[] actionButtonsList;
	private GameObject[] gameobjectList;
	public string id;
	public char type;
	public float axis;
	public float debug_axis;
	public float debug_axis_alt;
	public static float bound = 7;
	public static float speed=16f;	
	private RandomNumberGenerator myrandomgenerator;
	private float score = 0f;
	private List<GameObject> scoreToRemove = new List<GameObject>();
	public float intervalRandom = 0.05f;
	public float lastTimeRandom = 0.0f;
	public GameObject spawn;
	public bool running = true;
	public bool haswin = false;
	private Queue<GameObject> normalWaves;
	private List<GameObject> megaWaves;


	// Use this for initialization
	void Start () {
		this.normalWaves = new Queue<GameObject>();
		this.megaWaves = new List<GameObject>();
		this.myrandomgenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();
		this.actionButtonsList = new string[] {id+"_Fire_G",id+"_Fire_B",id+"_Fire_Y",id+"_Fire_R"};
		gameobjectList = new GameObject[]{ wave_G, wave_B, wave_Y, wave_R };
		scoreText.text = ""+score;
		spawn = GameObject.FindGameObjectsWithTag ("SPAWN")[0];
	}

	public GameObject CreateText(float x, float y, string text_to_print, int font_size, Color text_color)
	{
		GameObject[] canvas = GameObject.FindGameObjectsWithTag ("CANVA");
		Transform canvas_transform = canvas [0].transform;
		GameObject newgo = GameObject.Instantiate (scorePrefab);
		newgo.transform.SetParent(canvas_transform);

		Text text = newgo.GetComponent<Text>();
		text.text = text_to_print;
		text.color = text_color;
		//text.fontSize = font_size;

		RectTransform rectTransform = newgo.GetComponent<RectTransform>();
		rectTransform.position = new Vector3 (x, y,rectTransform.position.z);


		return newgo;
	}

	public static Text AddTextToCanvas(string textString)
	{
		GameObject[] canvas = GameObject.FindGameObjectsWithTag ("CANVA");
		GameObject canvasGameObject = canvas [0];
		Text text = canvasGameObject.AddComponent<Text>();
		text.text = textString;

		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		text.font = ArialFont;
		text.material = ArialFont.material;

		return text;
	}

	public void updateScore(int addValue,GameObject origin){
		spawn2 spawnobj = spawn.GetComponent<spawn2> ();
		if (!spawnobj.running) {
			return;
		}
		//AddTextToCanvas ("hello");
		Color c = addValue<0?Color.red:Color.green;

		float myX = origin.transform.position.x;
		float myY = origin.transform.position.y;
		float delta = 9.5f;
		if( myX > delta) {
			myX = delta;
		}
		if (myX < -delta) {
			myX = -delta;
		}
		if( myY > delta) {
			myY = delta;
		}
		if (myY < -delta) {
			myY = -delta;
		}

		if (addValue != 1 && addValue != -1) {
			scoreToRemove.Add (CreateText (myX, myY, "" + addValue, 48, c));
		}
		score += addValue;
		if(score<0){
			score = 0;
		}
		scoreText.text = "" + score;
	}
		

	public void registerWave(GameObject obj)
	{
		Wave curWave = obj.GetComponent<Wave>();
		
		curWave.targetPlayer = this.gameObject;
		if (!curWave.isMega)
			this.normalWaves.Enqueue (obj);
		else {
			this.megaWaves.Add (obj);
		}
	}

	public void notifyCollision(GameObject obj)
	{
		this.removeWave (obj);
	}

	public void removeWave(GameObject waveGameObject)
	{
		Wave waveObj = waveGameObject.GetComponent<Wave>();

		if( waveObj.isMega )
		{
			if (this.megaWaves.Count > 0)
			{
				/*List<GameObject> toRemove = new List<GameObject>();
				foreach (GameObject wave in this.megaWaves) {
					Wave testObj = wave.GetComponent<Wave>();
					if (testObj.creationTime < waveObj.creationTime) {	
						GameObject.DestroyImmediate(wave);
						toRemove.Add (wave);
					}
				}

				foreach (GameObject toDelete in toRemove) {
					this.megaWaves.Remove (toDelete);
				}*/

				GameObject.DestroyImmediate (waveGameObject);
				this.megaWaves.Remove (waveGameObject);

				/*do 
				{	
					curObj = this.megaWaves.Peek();
					if( curObj != null)
					{
						
						if( testObj.creationTime <= waveObj.creationTime )
						{	
							GameObject.DestroyImmediate(curObj);
							//this.megaWaves.TryDequeue(out curObj);
							curObj = this.megaWaves.Dequeue();
						}
						else
							break;
					}
				} while(curObj != null && curObj != waveGameObject);*/
			}
		}
		else
		{
			GameObject curObj = null;
			if (this.normalWaves.Count > 0) {
				do {
					//this.normalWaves.TryPeek(out curObj);
					curObj = this.normalWaves.Peek ();
					if (curObj != null) {
						Wave testObj = curObj.GetComponent<Wave> ();
						if (testObj.creationTime <= waveObj.creationTime) {
							GameObject.DestroyImmediate (curObj);
							//this.normalWaves.TryDequeue(out curObj);
							curObj = this.normalWaves.Dequeue ();
						} else
							break;
					}
				} while(curObj != null && curObj != waveGameObject);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		spawn2 spawnobj = spawn.GetComponent<spawn2> ();
		if (score >= spawnobj.win_score) {
			haswin = true;
			spawnobj.running = false;
		}
		if (Time.time - lastTimeRandom > intervalRandom) {
			lastTimeRandom = Time.time;
			if (this.scoreToRemove.Count > 0)
			{
				for (var iii = 0; iii < this.scoreToRemove.Count; iii++) {
					GameObject.DestroyImmediate (scoreToRemove[0]);
					scoreToRemove.Remove(scoreToRemove[0]);
				}
			}
		}
		if (!running) {
			return;
		}
		axis = Input.GetAxis (id + "_Move") + Input.GetAxis (id + "_Move_alt");
		debug_axis = Input.GetAxis (id + "_Move");
		debug_axis_alt = Input.GetAxis (id + "_Move_alt");
		float inc = axis * speed * Time.deltaTime;

		if (id.Equals ("P1") || id.Equals ("P2")) {
			if (
				((this.transform.position.y + inc) > -bound)
				&&
				((this.transform.position.y + inc) < bound)) {
				this.transform.Translate (new Vector3 (0, inc, 0));
			}
		} else {
			if (
				((this.transform.position.x + inc) > -bound)
				&&
				((this.transform.position.x + inc) < bound)) {
				this.transform.Translate (new Vector3 (0, inc, 0));
			}
		}
		int buttonIndex=-1;
		for(int i=0; i < this.actionButtonsList.Length; i++)
		{
			if( Input.GetButtonDown (this.actionButtonsList[i]) )
			{
				buttonIndex = i;
				break;
			}
		}

		if( buttonIndex != -1 ) {
			string nameOfAction = this.actionButtonsList [buttonIndex];
			//Debug.logger.Log (LogType.Log, "[" + id + "] input");
			GameObject[] _players = GameObject.FindGameObjectsWithTag ("Player");
			List<GameObject> listPlayers = new List<GameObject>();
			listPlayers.AddRange(_players);
			// remove curent player from list
			listPlayers.Remove(this.gameObject);

			// manage the button press
			if( this.megaWaves.Count > 0)
			
			{
				GameObject megaFoundObject = null;
				bool badColor = true;
				
				//Debug.logger.Log (LogType.Log, "fire");
				foreach(GameObject curWaveGameObject in this.megaWaves)
				{
					Wave entry = curWaveGameObject.GetComponent<Wave> ();
					if (entry.type == nameOfAction.Substring (nameOfAction.Length - 1) [0]/*entry.inInputAllowedArea ()*/) {
						megaFoundObject = curWaveGameObject;
						badColor = false;
					}
					else if (entry.type == 'N')
					{
						megaFoundObject = curWaveGameObject;
					}
					break; // debug 
				}


				if(megaFoundObject != null)
				{
					AudioSource audio = GetComponent<AudioSource>();
					Wave thisWave = megaFoundObject.GetComponent<Wave> ();
					int multiplier = 1;
					if(  thisWave.type == 'N')
					{
						if (nameOfAction.Substring (nameOfAction.Length - 1) [0] == thisWave.targetPlayer.GetComponent<Player> ().type) {
							audio.Play ();

							byte[] randoms = new byte[1];
							myrandomgenerator.GetBytes (randoms);
							// create new wave directed to the other players
							GameObject newWave = GameObject.Instantiate (Utils.getSpriteForPlayerId (thisWave.targetPlayer.GetComponent<Player> ().id, gameobjectList));
							int newTarget = (int)(((float)randoms [0] / 256.0f) * listPlayers.Count);

							float spawninc = GameObject.FindGameObjectsWithTag ("SPAWN") [0].GetComponent<spawn2> ().inc;
							newWave.GetComponent<Wave> ().init (Utils.getRotationAngleForPlayerId (listPlayers [newTarget].GetComponent<Player> ().id) + spawninc, true);
							listPlayers [newTarget].GetComponent<Player> ().registerWave (newWave);
							updateScore (10 * multiplier, megaFoundObject);
							this.removeWave (megaFoundObject);
						} else {
							if (badColor == true)
							{
								// We have to make the player pays for the bad moves
								updateScore (-5,megaFoundObject);
							}
						}
					}
					else
					{
						Debug.logger.Log (LogType.Log, nameOfAction.Substring(nameOfAction.Length - 1)[0] + " - " + thisWave.type);
						// custom type created by a previous attack by another player 
						if(nameOfAction.Substring(nameOfAction.Length - 1)[0] == thisWave.type )
						{
							Debug.logger.Log (LogType.Log, "plop");
							audio.Play();

							// player typed the right color :)
							updateScore (10 * multiplier,megaFoundObject);
							this.removeWave(megaFoundObject);
						}

						// score to update with penalties
						// nothing to do, let collision handle do the job
					}
				}

				
			}
		}
	}
}
