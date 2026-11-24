using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip track;

    void Start()
    {
        MusicManager.Instance.Play(track);
    }
}
