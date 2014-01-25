using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class Game : MonoBehaviour {

	private bool started = false;
	public DaylightWater risingDaylightWater;

	// Use this for initialization
	void Start () {
		if (Network.isServer) {
			initiatePlayer();
			GameModel.Characters client = GameModel.Characters.Summer;
			if (GameModel.PlayerId == client) {
				client = GameModel.Characters.Winter;
			}
			networkView.RPC("setClientPlayerId", RPCMode.Others, (int)client);
		}
	}

	void initiatePlayer() {
		enableLevel ();
		
		GameObject PlayerPref = (GameObject)Resources.Load("Player");
		
		GameObject player = (GameObject)Network.Instantiate(PlayerPref, new Vector3(0,0,0), Quaternion.identity, 0);
		
		Player playerScript = player.GetComponent<Player>();
		playerScript.playerId = GameModel.PlayerId;
	}
	
	// Update is called once per frame
	void Update () {

		if (Network.isClient) {
			if (!started && (int)GameModel.PlayerId != 0) {
				started = true;
				initiatePlayer();
			}
		}


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

		if (GameModel.PlayerId == GameModel.Characters.Summer) {
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

	[RPC]
	public void setClientPlayerId(int playerId) {
		GameModel.PlayerId = (GameModel.Characters) playerId;
	}

	[RPC]
	public void riseDayLightWater() {
		risingDaylightWater.rise = true;
	}

	public void onIceblockInplace(int iceblockId) {
		if (iceblockId == 1) {
			networkView.RPC("riseDayLightWater", RPCMode.Others);
		}
	}
}
