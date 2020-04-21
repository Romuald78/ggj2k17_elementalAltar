using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class spawn2 : MonoBehaviour {
	public GameObject wave;
	public GameObject waveMega;

	public float interval = 1f;
	private float internalInterval = 1f;
	public float intervalVariance = 0.3f;
	private float lastTime = 0.0f;
	public float inc = 0.0f;
	float sens = 1.0f;
	public float intervalRandom = 0.5f;
	public float lastTimeRandom = 0.0f;
	public float angularSpeed = 0.8f;
	public int probaMegaValue = 50;
	public int win_score = 200;
	public bool running = true;
	//private Color[] colorsList = new Color[]{ Color.grey, Color.red, Color.green, Color.blue, Color.yellow };
	// Use this for initialization
	void Start () {
		this.internalInterval = this.interval;
	}
	
	// Update is called once per frame
	void FixedUpdate () {	
		if (!running) {

			GameObject[] listPlayers = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject totot in listPlayers) {
				Player tootPlayer = totot.GetComponent<Player> ();
				tootPlayer.running = false;
				if (tootPlayer.haswin == true) {
					totot.transform.position = new Vector3 (0, 0, -2);
					totot.transform.localScale = new Vector3 (5, 5, 5);
					//totot.transform.Translate (new Vector3 (0, 0, -2));
				}
			}
			return;
		}
		inc += angularSpeed*sens;
		float DELTAMAX = 30f;
		if (Time.fixedTime - lastTimeRandom > intervalRandom) {
			lastTimeRandom = Time.fixedTime;
			intervalRandom = Random.Range (2f, 10f);
			sens = -sens;
		}
			
		if (inc > DELTAMAX) {
			inc = DELTAMAX;
			sens = -1;
		} else if (inc < -DELTAMAX) {
			inc = -DELTAMAX;
			sens = +1;
		}

		if (Time.fixedTime - lastTime > internalInterval) {
			
			lastTime = Time.fixedTime;
			internalInterval = Random.Range (interval - intervalVariance, interval + intervalVariance);
			GameObject[] listPlayers = GameObject.FindGameObjectsWithTag ("Player");

			int probaMega = Random.Range (0, 100);
			bool probaMegaBool = probaMega > probaMegaValue;

			foreach (GameObject player in listPlayers)
			{
				Player playerObj = player.GetComponent<Player> ();
				int initialAngle = Utils.getRotationAngleForPlayerId(playerObj.id);
				GameObject newInstance;

				if (probaMegaBool) {
					newInstance = GameObject.Instantiate (this.waveMega);
					//Debug.logger.Log(LogType.Log,"New MEGA");
				} else {
					newInstance = GameObject.Instantiate (this.wave);
					//Debug.logger.Log(LogType.Log,"New normal");
				}
				//.logger.Log(LogType.Log,probaMegaBool);
				newInstance.GetComponent<Wave>().type = 'N';
				newInstance.GetComponent<Wave> ().init (initialAngle+inc,probaMegaBool);

				playerObj.registerWave(newInstance);
			}
				
		}

	}
}
