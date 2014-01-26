using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chat : MonoBehaviour {

    private bool showChat = true;

    private List<chatEntry> entries = new List<chatEntry>();

    private Vector2 scrollPosition = Vector2.zero;

    private Rect position;

    private Rect positionInput;

    private string currentMessage = "";

    private string chatInputName = "ChatInput";

    private string looseFocus = "looseFocusDood";

    private bool sendMessage = false;

    private bool checkEnter = true;

	// Use this for initialization
    void Start()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
        //gui measumrents:

        //w 1920
        //wo 70
        //ww 525

        //h 1080
        //ho 810
        //hh 220

        //input ho 1030
        //input hh 32
        //input wo 70
        //input ww 525

        position = new Rect(Screen.width * 70 / 1920, Screen.height * 810 / 1080, Screen.width * 525 / 1920 + 15, Screen.height * 220 / 1080);

        positionInput = new Rect(Screen.width * 70 / 1920, Screen.height * 1030 / 1080, Screen.width * 525 / 1920, Screen.height * 32 / 1080);

        GUI.FocusControl(chatInputName);

        if (sendMessage)
        {
            sendMessage = false;

            networkView.RPC("addMessage", RPCMode.All, (int)GameModel.PlayerId, currentMessage);
            currentMessage = "";
        }

        checkEnter = true;
	}

    void OnGUI()
    {

        GUILayout.BeginArea(position);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        for (int i=0; i<entries.Count; i++)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(position.width-30));

            GUILayout.Label(entries[i].message());
            GUILayout.FlexibleSpace ();
 
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();

        GUI.SetNextControlName(chatInputName);

        currentMessage = GUI.TextField(
            positionInput,
            currentMessage
        );

        GUI.SetNextControlName(looseFocus);
        GUI.Label(new Rect(-100, -100, 1, 1), "");

        Event e = Event.current;
        if (e.keyCode == KeyCode.Return && e.type == EventType.keyUp)
        {
            if (checkEnter)
            {
                checkEnter = false;

                if (GUI.GetNameOfFocusedControl() == chatInputName)
                {
                    if (currentMessage != "")
                    {
                        sendMessage = true;
                    }

                    GUI.FocusControl(looseFocus);
                }
                else
                {
                    GUI.FocusControl(chatInputName);
                }
            }
        }
    }

    [RPC]
    void addMessage(int sender, string text)
    {
        addMessage((GameModel.Characters)sender, text);
    }

    void addMessage(GameModel.Characters sender, string text)
    {
        entries.Add(
            new chatEntry(sender, Time.time, text)
        );

        scrollPosition.y = 9999999;
    }
}
