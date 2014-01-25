using UnityEngine;
using System.Collections;

public class changeSkyboxAndFog : MonoBehaviour {

    public Material skyboxSummer;
    public Material skyboxWinter;

    public Color fogColorSummer;
    public float fogDensitySummer;
    public float fogEndDistanceSummer;
    public float fogStartDistanceSummer;
    public FogMode fogModeSummer;

    public Color ambientLightSummer;

    public Color fogColorWinter;
    public float fogDensityWinter;
    public float fogEndDistanceWinter;
    public float fogStartDistanceWinter;
    public FogMode fogModeWinter;

    public Color ambientLightWinter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (GameModel.PlayerId == GameModel.Characters.Summer)
        {
            RenderSettings.skybox = skyboxSummer;

            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColorSummer;
            RenderSettings.fogDensity = fogDensitySummer;
            RenderSettings.fogEndDistance = fogEndDistanceSummer;
            RenderSettings.fogStartDistance = fogStartDistanceSummer;
            RenderSettings.fogMode = fogModeSummer;

            RenderSettings.ambientLight = ambientLightSummer;

            enabled = false;
        }

        if (GameModel.PlayerId == GameModel.Characters.Winter)
        {
            RenderSettings.skybox = skyboxWinter;

            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColorWinter;
            RenderSettings.fogDensity = fogDensityWinter;
            RenderSettings.fogEndDistance = fogEndDistanceWinter;
            RenderSettings.fogStartDistance = fogStartDistanceWinter;
            RenderSettings.fogMode = fogModeWinter;

            RenderSettings.ambientLight = ambientLightWinter;

            enabled = false;
        }
	}
}
