using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	
	public string IP = "127.0.0.1";
	//public string IP = "10.10.71.25";
	public int Port = 25001;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			//*
			if (GUI.Button(new Rect(100, 100, 100, 25), "Start Client")) {
				Network.Connect(IP, Port);
				GameModel.PlayerId = 1;
			}
			if (GUI.Button(new Rect(100, 125, 100, 25), "Start Server")) {
				Network.InitializeServer(2, Port, true);
				GameModel.PlayerId = 2;
			}//*/
		}
		if (Network.isServer) {
			GUI.Button(new Rect(100, 100, 100, 25), "waiting for client");
		}
	}

	void OnPlayerConnected(NetworkPlayer player) {
		startGame();
	}

	void OnConnectedToServer() {
		startGame();
	}

	void startGame () { // load level, reset scores
		Application.LoadLevel ("Level1"); 

	}
	
	void OnDisconnectedFromServer() {

	}
}
