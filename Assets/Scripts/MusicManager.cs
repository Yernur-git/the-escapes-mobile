using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] AudioSource a, b;
    [SerializeField] SfxPlayer uiSounds;
    [SerializeField] AudioClip click;
    AudioSource current;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        current = a;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip, float fade = 1f)
    {
        if (current.clip == clip && current.isPlaying) return;
        StopAllCoroutines();
        AudioSource next = current == a ? b : a;
        if (next.clip != clip || !next.isPlaying)
        {
            next.clip = clip;
            next.volume = 0f;
            next.Play();
        }
        StartCoroutine(Crossfade(current, next, fade));
        current = next;
    }

    public void PlayClick()
    {
        uiSounds.Play(click, .55f);
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to, float time)
    {
        float fromVolume = from.volume;
        float toVolume = to.volume;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / time)
        {
            from.volume = Mathf.Lerp(fromVolume, 0f, t);
            to.volume = Mathf.Lerp(toVolume, 1f, t);
            yield return null;
        }
        from.Stop();
        to.volume = 1f;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
