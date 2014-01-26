using UnityEngine;
using System.Collections;

public class WaterRising : MonoBehaviour {
	public bool rise = false;
	public float originY = -10;
	public float targetY = -5;
	public float risingSpeed = 7;
	// Use this for initialization
	void Start () {
		Camera.main.GetComponent<Game>().risingDaylightWater = this;
		transform.position = new Vector3(transform.position.x, originY, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (rise) {
			transform.Translate(new Vector3(0, risingSpeed*Time.deltaTime, 0),Space.World);
			if (transform.position.y >= targetY) {

				enabled = false;
			}
		}
	}
}
