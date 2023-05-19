using UnityEngine;

public class MusicController : MonoBehaviour
{
  public AudioSource audioSourceIntro;
  public AudioSource audioSourceLoop;

  public static MusicController instance;

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
}
