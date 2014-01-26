using UnityEngine;
using System.Collections;

public static class GameModel {
	
	public enum Characters {Summer = 1, Winter = 2};
	public static Characters PlayerId;
	
	public static int deathCounter;
	public static int score;
	
	public static Vector3 lastCheckpoint;
	
}
