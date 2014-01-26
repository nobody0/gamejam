using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameModel.PlayerId == GameModel.Characters.Summer)
		{
			Camera.main.audio.clip = Resources.Load<AudioClip>("summer");
			Camera.main.audio.loop = true;
			Camera.main.audio.Play();
			enabled = false;
		}
		
		if (GameModel.PlayerId == GameModel.Characters.Winter)
		{
			Camera.main.audio.clip = Resources.Load<AudioClip>("winter-normal");
			Camera.main.audio.loop = true;
			Camera.main.audio.Play();
			enabled = false;
		}
	}
}
