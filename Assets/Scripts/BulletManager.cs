using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {
	public float movementSpeed = 5;
	private Vector3 movement;

	void Awake() {
		movement = new Vector3(0, movementSpeed, 0);
	}
			
	void Update() {
		int direction;

		direction = 0;
		if(gameObject.tag.Equals ("PlayerBullet")) {
			direction = 1;
		}
		else if(gameObject.tag.Equals ("EnemyBullet")) {
			direction = -1;
		}
		gameObject.transform.Translate (movement * direction * Time.deltaTime, Space.World);
		if ((gameObject.transform.position.y < 0) || (gameObject.transform.position.y > 20)) {
			if(gameObject.tag.Equals ("PlayerBullet")) {
				GameManager.instance.PlayerBulletDestroyed ();
			}
			else if(gameObject.tag.Equals ("EnemyBullet")) {
				GameManager.instance.EnemyBulletDestroyed ();
			}
			Destroy (gameObject);
		}
	}

	// Detects trigger with any game object
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Player")) {
			GameManager.instance.PlayerKilled ();
		}
		if(gameObject.tag.Equals ("PlayerBullet")) {
			GameManager.instance.PlayerBulletDestroyed ();
		}
		else if(gameObject.tag.Equals ("EnemyBullet")) {
			GameManager.instance.EnemyBulletDestroyed ();
		}
		Destroy (gameObject);
	}
}
