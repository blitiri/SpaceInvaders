using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {
	private static string lastWall;
	private EnemyInfo enemyInfo;

	void Awake() {
		enemyInfo = gameObject.GetComponent<EnemyInfo> ();
	}

	// Use this for initialization
	void Start () {
		lastWall = "";
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.EndsWith ("Wall")) {
			if (!other.gameObject.tag.Equals (lastWall)) {
				GameManager.instance.EnemiesAtTheEndOfLine ();
				lastWall = other.gameObject.tag;
			}
		}
		else if (other.gameObject.tag.Equals ("Land") && !GameManager.instance.IsGameFinished()) {
			GameManager.instance.EnemiesLanded ();
		}
		else if (other.gameObject.tag.Equals ("PlayerBullet")) {
			GameManager.instance.EnemyDestroyed (enemyInfo.score);
			Destroy (gameObject);
		}
	}
}
