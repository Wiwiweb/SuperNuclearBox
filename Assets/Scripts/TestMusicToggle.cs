using UnityEngine;
using UnityEngine.InputSystem;

public class TestMusicToggle : MonoBehaviour
{
  public void TogglePause(InputAction.CallbackContext context)
  {
    AudioListener.pause = !AudioListener.pause;
    Debug.Log($"Toggled at dspTime {AudioSettings.dspTime}");
    Debug.Log($"audioSourceLoop time at {GameObject.Find("AudioSource").GetComponent<AudioSource>().time}");
  }
}
