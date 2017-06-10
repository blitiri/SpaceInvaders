using UnityEngine;
using System.Collections;

public class MysteryShipManager : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.EndsWith ("Wall")) {
			Destroy (gameObject);
			HordeManager.instance.MysteryShipDisappeared ();
		} else if (other.gameObject.tag.EndsWith ("PlayerBullet")) {
			GameManager.instance.MysteryShipDestroyed ();
		}
	}
}
