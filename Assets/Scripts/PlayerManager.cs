using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	public float movementSpeed = 5f;
	public float leftLimit = -15f;
	public float rightLimit = 15f;
	private Vector3 movement;

	// Use this for initialization
	void Start () {
		movement = new Vector3 (movementSpeed, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.IsGameFinished ()) {
			Move ();
		}
	}

	private void Move() {
		if (Input.GetKey (KeyCode.D)) {
			if (gameObject.transform.position.x < rightLimit) {
				gameObject.transform.Translate (movement * Time.deltaTime, Space.World);
			}
		} else if (Input.GetKey (KeyCode.A)) {
			if (gameObject.transform.position.x > leftLimit) {
				gameObject.transform.Translate (-movement * Time.deltaTime, Space.World);
			}
		}
		if (Input.GetKey (KeyCode.Space)) {
			GameManager.instance.PlayerFires ();
		}
	}

	// Detects trigger with any game object
	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals ("EnemyBullet")) {
			GameManager.instance.PlayerKilled ();
		}
	}
}
