using UnityEngine;
using UnityEngine.Audio;

public class PauseAudio : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot normalSnapshot, pausedSnapshot;

    void Start()
    {
        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
    }

    public void SetPaused(bool paused)
    {
        if (paused) pausedSnapshot.TransitionTo(.3f);
        else normalSnapshot.TransitionTo(.3f);
    }
}
