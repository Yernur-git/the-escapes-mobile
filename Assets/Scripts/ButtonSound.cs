using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void Play()
    {
        MusicManager.Instance.PlayClick();
    }
}
