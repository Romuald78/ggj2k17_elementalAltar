using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wave : MonoBehaviour {

	public GameObject targetPlayer;
	public float speed = 1.0f;
	public float collisionSpace = 1.0f;
	public static float collisionDistance = 0.5f;
	public static float inputAllowedDistance = 1.0f;
	public bool isMega;
	public float creationTime;
	public bool running = true;
	public float currentDistance;
	public char type; // values : N (neutral), then colors, R, G, B, Y

	public Color waveColor()
	{
		return Color.grey;
	}

	// Use this for initialization
	void Start () {
		Vector3 targetPosition = targetPlayer.transform.position;
		Vector3 currentPosition = this.transform.position;
		//float distance = Vector3.Distance(currentPosition, targetPosition);
		//this.transform.Rotate (0.0f, 90.0f,0.0f);		
		// normalize collision Space
		//this.collisionDistance = distance * this.collisionAreaRatio;
		//this.inputAllowedDistance = distance * this.inputAllowedAreaRatio;
	}

	public void init(float angleDeg,bool isMega){
		this.isMega = isMega;
		this.transform.Rotate(new Vector3(0.0f, 0.0f, angleDeg));
		this.creationTime = Time.fixedTime;
	}

	private void moveTowardsTarget()
	{
		Vector3 targetPosition = targetPlayer.transform.position;
		Vector3 currentPosition = this.transform.position;
		this.currentDistance = Vector3.Distance(targetPosition, currentPosition);

		if (!this.inCollisionArea()) {
			this.transform.Translate (
				(this.speed * Time.deltaTime),
				0,
				0,
				Space.Self);
				float bound = 10f;
				if (currentPosition.x > bound || currentPosition.x < -bound || currentPosition.y > bound || currentPosition.y < -bound) {
					//out of bound
					if (isMega) {
						int multiplier = 1;
					targetPlayer.GetComponent<Player> ().updateScore (-10*multiplier,this.gameObject); //RGR
					} else {
					targetPlayer.GetComponent<Player> ().updateScore (-1,this.gameObject); //RGR
				}
				// 1: notify collision (collided wave will be removed from the player in this call, and the object will be destroyed)
				targetPlayer.GetComponent<Player>().notifyCollision(this.gameObject);
				}
		} else {
			// collision !!!!
			// 1: notify collision (collided wave will be removed from the player in this call, and the object will be destroyed)
			if (isMega) {
				int multiplier = 1;
				targetPlayer.GetComponent<Player> ().updateScore (-10*multiplier,this.gameObject);
			} else {
				targetPlayer.GetComponent<Player> ().updateScore (1,this.gameObject);
			}
			targetPlayer.GetComponent<Player>().notifyCollision(this.gameObject);
		}
	}

	public bool inCollisionArea()
	{
		Vector3 targetPosition = targetPlayer.transform.position;
		Vector3 currentPosition = this.transform.position;
		return Vector2.Distance(currentPosition, targetPosition) <= collisionDistance;
	}

	public bool inInputAllowedArea()
	{
		
		/*Vector3 targetPosition = targetPlayer.transform.position;
		Vector3 currentPosition = this.transform.position;
		return Vector3.Distance (currentPosition, targetPosition) <= Wave.inputAllowedDistance;
*/
		return true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!running) {
			return;
		}
		moveTowardsTarget ();
	}
}
