using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {

	public string soundNameToPlay;
	public GameModel.Characters characterToTrigger;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other) 
	{
		Player player = other.gameObject.GetComponent<Player>();
		if (player) 
		{
			if (player.playerId==characterToTrigger) 
			{
				SoundManager.PlaySound(soundNameToPlay);
			}
		}
	}

}
