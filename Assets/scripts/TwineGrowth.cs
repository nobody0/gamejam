using UnityEngine;
using System.Collections;

public class TwineGrowth : MonoBehaviour {
	public int RankeId;
	public Vector3 startTransform;
	public Vector3 endPosition;
	public float startScale; // endscales are always 1

	public float growTick = 1.8f;
	public bool growing = false;

	public float growingSpeed = 1f;

	// Use this for initialization
	void Start () {
		if (RankeId == 5)
			Camera.main.GetComponent<Game>().ranke_5 = this;
		endPosition = transform.position;
		transform.position = transform.position - startTransform;
		transform.localScale = startScale*Vector3.one;
		Debug.Log(transform.position + " " + endPosition);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (transform.position);
		if (growing) {
			
			appear ();
			Camera.main.GetComponent<Game>().onRankeGrow(RankeId);
			/*
			if (transform.localScale.x >= 1) {
				appear ();
			} else {
				grow ();
			}//*/
		}
	}

	void grow () {
		Vector3 curPosition = transform.position;
		transform.position = curPosition + (endPosition - curPosition).normalized * growingSpeed * Time.deltaTime;
		transform.localScale = transform.localScale + growingSpeed * Time.deltaTime * Vector3.one;
	}

	public void appear () {
		
		transform.localScale = Vector3.one;
		transform.position = endPosition;
		growing = false;
	}
}
