using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (Network.isServer || Network.isClient) {
			
			enableLevel ();

			GameObject PlayerPref = (GameObject)Resources.Load("Player");

			GameObject player = (GameObject)Network.Instantiate(PlayerPref, new Vector3(0,0,0), Quaternion.identity, 0);
			
			Player playerScript = player.GetComponent<Player>();
			playerScript.playerId = GameModel.PlayerId;

			//player.AddComponent<ThirdPersonCamera>();
			
			//enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Network.isServer && Network.connections.Length == 0) { // i am server and no player is there
			
			Application.LoadLevel("Start");
		}
		if (Network.peerType == NetworkPeerType.Disconnected) { // no connection
			//Debug.Log ("Errrrrror");
			Application.LoadLevel("Start");
		}
	}

	void OnGUI() {
		GUI.Button(new Rect(100, 100, 100, 25), " " + GameModel.PlayerId);
	}

	void enableLevel () {
		string levelName;
		GameObject level;

		if (GameModel.PlayerId == 1) {
			levelName = "Level_Sommer";
		} else {
			levelName = "Level_Winter";
		}

		/*
		var objectsInScene = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		for (int i = 0; i < objectsInScene.Length; i++) {
			if (objectsInScene[i].name.Equals(levelName)) {
				level = (GameObject) objectsInScene[i];
				break;
			}
		}//*/
		
		level = (GameObject)Resources.Load(levelName);
		Instantiate(level);
		//level.SetActive(true);
	}
}
