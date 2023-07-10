using UnityEngine;

public class MusicController : MonoBehaviour
{
  public static MusicController instance;

  public AudioSource audioSourceIntro;
  public AudioSource audioSourceLoop;

	private bool introWasPaused;

  void Awake()
  {
    if (instance == null)
    {
      DontDestroyOnLoad(gameObject);
      instance = this;
    }
    else
    {
			Destroy(gameObject);
    }
  }

  void Start()
  {
    double startTime = AudioSettings.dspTime + 0.5;
    double introLength = (double)audioSourceIntro.clip.samples / audioSourceIntro.clip.frequency;

    #if UNITY_WEBGL
      // The loop with intro doesn't work very precisely with the WebGL audio system...
      // Overlap them a bit, still sounds ok
      introLength -= 0.1;
    #endif
    
    audioSourceIntro.PlayScheduled(startTime);
    audioSourceLoop.PlayScheduled(startTime + introLength);
  }

	public void Pause()
	{
		AudioListener.pause = true;
	}
	
	public void UnPause()
	{
		AudioListener.pause = false;
	}
}
