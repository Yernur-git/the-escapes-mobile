using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    [SerializeField] GameObject touchControls;

    void Awake()
    {
        touchControls.SetActive(PlayerPrefs.GetInt("touchControls", 1) == 1);
    }
}
