using UnityEngine;
using System.Collections;

public class WaterSwimming : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other) {

		
	}
	void OnTriggerExit(Collider other) {
		Player playerScript = other.GetComponent<Player>();
		playerScript.isInWater = false;

	}
	void OnTriggerEnter(Collider other) {
		Player playerScript = other.GetComponent<Player>();
		playerScript.isInWater = true;
	}
}
