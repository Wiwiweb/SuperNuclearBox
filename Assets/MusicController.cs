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
