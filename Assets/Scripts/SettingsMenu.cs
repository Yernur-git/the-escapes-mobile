using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Toggle guardLabels;
    [SerializeField] Toggle touchControls;
    [SerializeField] Button openButton;

    void OnEnable()
    {
        guardLabels.SetIsOnWithoutNotify(PlayerPrefs.GetInt("guardLabels", 0) == 1);
        touchControls.SetIsOnWithoutNotify(PlayerPrefs.GetInt("touchControls", 1) == 1);
        EventSystem.current.SetSelectedGameObject(guardLabels.gameObject);
    }

    public void SetGuardLabels(bool enabled)
    {
        PlayerPrefs.SetInt("guardLabels", enabled ? 1 : 0);
    }

    public void SetTouchControls(bool enabled)
    {
        PlayerPrefs.SetInt("touchControls", enabled ? 1 : 0);
    }

    public void Close()
    {
        PlayerPrefs.Save();
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(openButton.gameObject);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) PlayerPrefs.Save();
    }
}
