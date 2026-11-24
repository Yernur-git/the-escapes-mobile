using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string param;
    [SerializeField] Slider slider;
    [SerializeField] Text percentage;
    [SerializeField] float defaultVolume;

    void OnEnable()
    {
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(param, defaultVolume));
        ShowValue(slider.value);
    }

    void Start()
    {
        slider.onValueChanged.AddListener(Set);
        Set(slider.value);
    }

    void Set(float value)
    {
        mixer.SetFloat(param, Mathf.Log10(Mathf.Max(value, .0001f)) * 20f);
        PlayerPrefs.SetFloat(param, value);
        ShowValue(value);
    }

    void ShowValue(float value)
    {
        percentage.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
