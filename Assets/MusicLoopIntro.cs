using UnityEngine;

public class MusicLoopIntro : MonoBehaviour 
{
	public AudioSource audioSource;
	public AudioClip musicIntro;
	
	void Start() 
	{
		audioSource.PlayOneShot(musicIntro);
		audioSource.PlayScheduled(AudioSettings.dspTime + musicIntro.length);
	}
}
