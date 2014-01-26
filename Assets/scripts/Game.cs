﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NetworkView))]
public class Game : MonoBehaviour {

	private bool started = false;
	public WaterRising risingDaylightWater;
	
	public TwineGrowth ranke_5;
	public TwineGrowth ranke_6;

	public string chat = "hallo welt";

	public string message;
	public bool showMessage = false;
	public bool messageReturn;

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
		Vector3 startPosition = new Vector3(145,0,9);//Vector3.zero;
		if (GameModel.PlayerId == GameModel.Characters.Summer) {
			startPosition.z += 2;
		}
		GameObject player = (GameObject)Network.Instantiate(PlayerPref, startPosition, Quaternion.identity, 0);
		
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
		GUI.Button(new Rect(100, 125, 100, 25), " " + chat);

		GUI.Label(new Rect(50, 700, 700, 400), chat);
		
		
		if (Input.GetKey(KeyCode.KeypadEnter)) {
			Debug.Log("chat!!");
			string chatinput = GUI.TextField(new Rect(50, 1100, 25, 400), "");
			networkView.RPC("updateChat", RPCMode.All, chatinput, (int)GameModel.PlayerId);
		}

		if (showMessage) {
			if (GUI.Button(new Rect(200, 100, 200, 50), message)) {
				messageReturn = true;
			}
		}
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

	[RPC]
	public void riseDayLightWater() {
		risingDaylightWater.rise = true;
	}

	public void onIceblockInplace(int iceblockId) {
		if (iceblockId == 1) {
			networkView.RPC("riseDayLightWater", RPCMode.Others);
		}
	}

	
	[RPC]
	public void ranke5Grow() {
		ranke_5.appear();
	}
	
	public void onRankeGrow(int rankeId) {
		if (rankeId == 5) {
			networkView.RPC("ranke5Grow", RPCMode.Others);
		}
		if (rankeId == 6) {
			networkView.RPC("ranke5Grow", RPCMode.Others);
		}
	}

	[RPC]
	public void updateChat(string chatinput, int playerId) {
		string name;
		GameModel.Characters pId = (GameModel.Characters) playerId;
		if (pId == GameModel.PlayerId) {
			name = "me";
		} else {
			if (pId == GameModel.Characters.Summer) {
				name = "Irene";
			} else {
				name = "Zack";
			}
		}
		chat = name + ": " + chatinput;
	}
}
