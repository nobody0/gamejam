using UnityEngine;
using System.Collections;

public class chatEntry {
    GameModel.Characters sender;
    float receivedTime;
    string text;

    public chatEntry(GameModel.Characters sender, float receivedTime, string text)
    {
        this.sender = sender;
        this.receivedTime = receivedTime;
        this.text = text;
    }

    public string message()
    {
        return senderName() + ": " + text;
    }

    private string senderName()
    {
        if (sender == GameModel.Characters.Summer)
        {
            return "Irene";
        }
        else if (sender == GameModel.Characters.Winter)
        {
            return "Zack";
        }
        else
        {
            return "?";
        }
    }
}
