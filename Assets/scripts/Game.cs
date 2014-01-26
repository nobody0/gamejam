using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class Game : MonoBehaviour {

	private bool started = false;

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
        if (GameModel.PlayerId == GameModel.Characters.Summer) {
            Instantiate((GameObject)Resources.Load("Level_Sommer"));
            Instantiate((GameObject)Resources.Load("GUI_Sommer"));
        } else {
            Instantiate((GameObject)Resources.Load("Level_Winter"));
            Instantiate((GameObject)Resources.Load("GUI_Winter"));
		}
	}

	[RPC]
	public void setClientPlayerId(int playerId) {
		GameModel.PlayerId = (GameModel.Characters) playerId;
	}
}
