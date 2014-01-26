using UnityEngine;
using System.Collections;

public static class SoundManager {
	public static GameObject dialogAudioSourceHolder;

	public static void PlaySound (string soundName)
	{
		AudioClip clip = Resources.Load<AudioClip>(soundName);
		
		if (SoundManager.dialogAudioSourceHolder == null) {
			SoundManager.dialogAudioSourceHolder = GameObject.Find("Main Camera/DialogAudioSourceHolder");
		}

		SoundManager.dialogAudioSourceHolder.audio.Stop();
		SoundManager.dialogAudioSourceHolder.audio.clip = clip;
		SoundManager.dialogAudioSourceHolder.audio.loop = false;
		SoundManager.dialogAudioSourceHolder.audio.Play();
	}
}
