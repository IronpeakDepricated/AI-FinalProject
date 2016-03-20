using UnityEngine;
using System.Collections;

public class BlindZombie : MonoBehaviour {
	
	// Update is called once per frame
	bool forward = true;
	float total = 0;
	void Update () {
		if (forward) {
			transform.position += (new Vector3 (1, 0, 0)) * Time.deltaTime*5;
		} else {
			transform.position -= (new Vector3 (1, 0, 0)) * Time.deltaTime*5;
		}
		total += Time.deltaTime*5;
		if (total >= 10) {
			forward = !forward;
			total = 0;
		}
	}
}