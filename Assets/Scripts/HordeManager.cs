using UnityEngine;
using System.Collections;

public class HordeManager : MonoBehaviour {
	public static HordeManager instance;
	public float lateralSpeed = 5f;
	public float mysteryShipSpeed = 5f;
	public float goDownSpeed = 25f;
	public int enemiesPerRow = 11;
	public GameObject enemyPrefab;
	public GameObject mysteryShipPrefab;
	public float enemiesLateralDistance = 2;
	private int enemiesRows = 5;
	private Vector3 lateralMovement;
	private Vector3 mysteryShipMovement;
	private Vector3 goDownMovement;
	private int lateralDirection;
	private GameObject[,] horde;
	private GameObject[] hordeFirstLine;
	private GameObject mysteryShip;
	private int enemiesAlive;

	void Awake() {
		instance = this;
		lateralDirection = 1;
		lateralMovement = new Vector3 (lateralSpeed, 0, 0);
		mysteryShipMovement = new Vector3 (mysteryShipSpeed, 0, 0);
		goDownMovement = new Vector3 (0, -goDownSpeed, 0);
		horde = new GameObject[enemiesRows, enemiesPerRow];
		enemiesAlive = enemiesRows * enemiesPerRow;
		hordeFirstLine = new GameObject[enemiesPerRow];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.IsGameFinished()) {
			Move ();
			if ((mysteryShip == null) && (Random.Range (0, 100) < 1)) {
				mysteryShip = Instantiate (mysteryShipPrefab);
			}
			if (mysteryShip != null) {
				MoveMysteryShip ();
			}
		}
	}

	private void Move() {
		gameObject.transform.Translate (lateralMovement * Time.deltaTime * lateralDirection, Space.World);
	}

	private void MoveMysteryShip() {
		mysteryShip.transform.Translate (mysteryShipMovement * Time.deltaTime, Space.World);
	}

	public void MysteryShipDisappeared() {
		mysteryShip = null;
	}

	public int EnemiesAlive() {
		return enemiesAlive;
	}

	public void Generate() {
		Vector3 rowPosition;
		Vector3 enemyPosition;
		EnemyInfo enemyInfo;
		GameObject enemy;
		int row;
		int col;

		// Lowest row index -> lowest enemy in game
		row = 0;
		foreach (Transform rowTransorm in gameObject.transform) {
			for (col = 0; col < enemiesPerRow; col++) {
				rowPosition = rowTransorm.position;
				enemyPosition = new Vector3 (rowPosition.x + enemiesLateralDistance * col, rowPosition.y, rowPosition.z);
				enemy = Instantiate (enemyPrefab, enemyPosition, Quaternion.identity, rowTransorm) as GameObject;
				enemyInfo = enemy.GetComponent<EnemyInfo> ();
				enemyInfo.row = row;
				enemyInfo.col = col;
				enemyInfo.score = 10 * row;
				horde [row, col] = enemy;
				if (row == 0) {
					hordeFirstLine [col] = enemy;
				}
			}
			row++;
		}
	}

	public GameObject ChooseShooter() {
		int col;
		int lastCol;

		col = Random.Range (0, enemiesPerRow);
		lastCol = (col == 0 ? enemiesPerRow - 1 : col - 1);
		do {
			col = (col + 1) % enemiesPerRow;
		} while((col != lastCol) /* Anti infinite looping condition */ && (hordeFirstLine [col] == null));
		return hordeFirstLine [col];
	}

	public void EnemyKilled(EnemyInfo enemyInfo) {
		horde [enemyInfo.row, enemyInfo.col] = null;
		hordeFirstLine [enemyInfo.col] = (enemyInfo.row < enemiesRows - 1 ? horde [enemyInfo.row + 1, enemyInfo.col] : null);
		enemiesAlive--;
	}

	public void GoDown() {
		gameObject.transform.Translate (goDownMovement * Time.deltaTime, Space.World);
	}

	// Inverts enemy moviment direction
	public void InvertDirection() {
		lateralDirection *= -1;
	}
}
