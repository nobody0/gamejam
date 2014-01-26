using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {

	public float speed = 5;
	public bool inPlace = false;
	public int id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerStay(Collider other) {
		if (other.name.Equals("EisblockBlocker")) {
			if (!inPlace) inPlace = true;
			this.transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
		}
	}

	void OnTriggerExit(Collider other) {
		if (inPlace && other.name.Equals("EisblockBlocker")) {
			Game game = Camera.main.GetComponent<Game>();
			game.onIceblockInplace(id);
		}
	}

}
