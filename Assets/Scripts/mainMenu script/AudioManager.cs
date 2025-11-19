using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        masterSlider.value = volume;
        SetMasterVolume(volume);
    }

    public void SetMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
}
