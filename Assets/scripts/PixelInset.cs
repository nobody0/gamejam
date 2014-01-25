using UnityEngine;
using System.Collections;

public class PixelInset : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rect rect = guiTexture.pixelInset;
		rect.height = Screen.height;
		rect.width = Screen.width;
		guiTexture.pixelInset = rect;
	}
}
