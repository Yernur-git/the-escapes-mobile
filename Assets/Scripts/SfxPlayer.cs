using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    AudioSource source;
    int last = -1;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, float volume = 1f, bool variation = true)
    {
        source.pitch = variation ? Random.Range(.92f, 1.08f) : 1f;
        source.PlayOneShot(clip, volume);
    }

    public void PlayRandom(AudioClip[] clips, float volume = 1f)
    {
        int i = Random.Range(0, clips.Length);
        if (i == last && clips.Length > 1) i = (i + 1) % clips.Length;
        last = i;
        Play(clips[i], volume);
    }
}
