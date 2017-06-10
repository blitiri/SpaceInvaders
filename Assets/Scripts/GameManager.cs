using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
	// GameManager singleton
	public static GameManager instance;
	public int maxActivePlayerBullets = 1;
	public int maxActiveEnemyBullets = 3;
	public GameObject player; 
	public GameObject bulletPrefab;
	private int activePlayerBullets;
	private int activeEnemyBullets;
	private bool gameFinished;
	private int score;
	public int lives = 5;

	// Script initialization
	void Awake() {
		instance = this;
		activePlayerBullets = 0;
		activeEnemyBullets = 0;
		gameFinished = false;
		score = 0;
	}

	// Script start
	void Start () {
		HordeManager.instance.Generate ();
	}
	
	// Script update
	void Update () {
		if (lives == 0) {
			gameFinished = true;
		}
		if (!gameFinished) {
			if (HordeManager.instance.EnemiesAlive () <= 0) {
				gameFinished = true;
			} else {
				EnemyFires ();
			}
		}
		else {
			if (HordeManager.instance.EnemiesAlive () <= 0) {
				Debug.Log ("E.T. go home!");
			} else {
				Debug.Log ("E.T. party time!");
				Destroy (player);
			}
		}
	}

	public bool IsGameFinished() {
		return gameFinished;
	}

	// I nemici sono arrivati alla fine della riga e devono scendere invertendo la direzione
	public void EnemiesAtTheEndOfLine() {
		HordeManager.instance.GoDown ();
		HordeManager.instance.InvertDirection ();
	}

	public void PlayerKilled() {
		lives--;
	}

	public void EnemiesLanded() {
		gameFinished = true;
	}

	public void MysteryShipDestroyed() {
		score += 300;
		Debug.Log ("Score: " + score);
	}

	public void EnemyDestroyed(int enemyScore) {
		score += enemyScore;
		Debug.Log ("Score: " + score);
	}

	// An enemy fires if he can (see maxActiveEnemyBullet)
	public void EnemyFires() {
		GameObject shooter;
		GameObject bullet;
		Vector3 bulletPosition;

		if(activeEnemyBullets < maxActiveEnemyBullets) {
			shooter = HordeManager.instance.ChooseShooter ();
			if (shooter != null) {
				activeEnemyBullets++;
				bullet = Instantiate (bulletPrefab) as GameObject;
				bulletPosition = new Vector3(shooter.transform.position.x, shooter.transform.position.y - 1f, shooter.transform.position.z);
				bullet.transform.position = bulletPosition;
				bullet.tag = "EnemyBullet";
			}
		}
	}

	// Detects the enemy bullet destroiment
	public void EnemyBulletDestroyed() {
		activeEnemyBullets--;
	}

	// Player fires if he can (see maxActivePlayerBullet)
	public void PlayerFires() {
		GameObject bullet;
		Vector3 bulletPosition;

		if(activePlayerBullets < maxActivePlayerBullets) {
			activePlayerBullets++;
			bullet = Instantiate (bulletPrefab) as GameObject;
			bulletPosition = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
			bullet.transform.position = bulletPosition;
			bullet.tag = "PlayerBullet";
		}
	}

	// Detects the player bullet destroiment
	public void PlayerBulletDestroyed() {
		activePlayerBullets--;
	}
}
