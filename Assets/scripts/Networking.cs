using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour
{

    private string IP = "127.0.0.1";

    private string gameTypeName = "GGJ14: since idea";
    private int Port = 25723;

	private bool showCharacterSelection = false;


    void Awake()
    {
        MasterServer.ClearHostList();
        MasterServer.RequestHostList(gameTypeName);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void connectToMaster()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //poll servers and try to connect to one
            HostData[] hostData = MasterServer.PollHostList();
            for (int i = 0; i < hostData.Length; i++)
            {
                Debug.Log(hostData[i].ip[0]);
                Debug.Log(hostData[i].port);

                if (hostData[i].connectedPlayers == 1)
                {
                    Network.Connect(hostData[i]);
                    return;
                }
            }
            MasterServer.ClearHostList();
        }
    }

    void registerToMaster()
    {
        MasterServer.RegisterHost(gameTypeName, "MyGame", "");
    }

	void startServer() {
		showCharacterSelection = false;
		Network.InitializeServer(1, Port, !Network.HavePublicAddress());
	}

    void OnGUI()
    {
		if (showCharacterSelection) {
			GUI.Label(new Rect(50, 50, 150, 25), "Select your Character");
			if (GUI.Button(new Rect(50, 100, 70, 25), "Irene")) {
				GameModel.PlayerId = GameModel.Characters.Summer; //sommer
				startServer();
			}
			if (GUI.Button(new Rect(150, 100, 70, 25), "Zack")) {
				GameModel.PlayerId = GameModel.Characters.Winter; //winter
				startServer();
			}
			if (GUI.Button(new Rect(150, 200, 70, 25), "cancel")) {
				showCharacterSelection = false;
			}
		} else {
	        if (Network.peerType == NetworkPeerType.Disconnected)
	        {
	            //IP input and conenct to server
	            GUI.Label(new Rect(250, 50, 150, 25), "Input server IP");
	            IP = GUI.TextField(new Rect(400, 50, 100, 25), IP);

				if (GUI.Button(new Rect(50, 50, 150, 25), "Connect to Server")) {
					Network.Connect(IP, Port);
				}

	            //own ip and start server

				if (GUI.Button(new Rect(50, 100, 100, 25), "Start Server")) {
					showCharacterSelection = true;
				}
			}
		}

        if (Network.peerType == NetworkPeerType.Connecting)
        {
            GUI.Label(new Rect(50, 50, 150, 25), "Connecting to Server");
        }

        if (Network.isServer)
        {
            GUI.Label(new Rect(50, 100, 200, 25), "Waiting for client to Connect");

			
			GUI.Label(new Rect(50, 50, 150, 25), "Your local network IP");
			GUI.TextField(new Rect(200, 50, 100, 25), Network.player.ipAddress);

            if (GUI.Button(new Rect(50, 125, 100, 25), "Stop Server")) {
                Network.Disconnect();
            }
        }
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        startGame();
    }

    void OnConnectedToServer()
    {
        startGame();
    }

    void startGame()
    { // load level, reset scores
        Application.LoadLevel("Level1");
    }

    void OnDisconnectedFromServer()
    {

    }
}
