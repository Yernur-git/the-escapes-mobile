using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip track;

    void Start()
    {
        MusicManager.Instance.SetPaused(false);
        MusicManager.Instance.Play(track);
    }
}
