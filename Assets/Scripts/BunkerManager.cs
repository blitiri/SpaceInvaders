using UnityEngine;
using System.Collections;

public class BunkerManager : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		Destroy (gameObject);
	}
}
