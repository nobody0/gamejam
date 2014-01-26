using UnityEngine;
using System.Collections;

public class BudTrigger : MonoBehaviour {

	public GameObject Twine;
	public bool isActivated = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isActivated) {
			Twine.GetComponent<TwineGrowth>().growing = true;
			isActivated = false;
		}
	}

}
