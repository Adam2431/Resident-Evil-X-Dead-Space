using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private AudioMixer AudioMixer;

    [SerializeField] private TextMeshProUGUI MasterSlider = null;
    [SerializeField] private TextMeshProUGUI SFXSlider = null;
    [SerializeField] private TextMeshProUGUI MusicSlider = null;
    [SerializeField] private TextMeshProUGUI VoiceSlider = null;

    [SerializeField] private Slider MasterSliderValue = null;
    [SerializeField] private Slider SFXSliderValue = null;
    [SerializeField] private Slider MusicSliderValue = null;
    [SerializeField] private Slider VoiceSliderValue = null;

    [SerializeField] private float maxSliderAmount = 100f;

    public void MasterSliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        MasterSlider.text = localValue.ToString("0");
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SFXSliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        SFXSlider.text = localValue.ToString("0");
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void MusicSliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        MusicSlider.text = localValue.ToString("0");
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void VoiceSliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        VoiceSlider.text = localValue.ToString("0");
        AudioMixer.SetFloat("VoiceVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }
    private void Start()
    {

        if (PlayerPrefs.GetFloat("MasterVolume", -1) != -1) {
            AudioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
            MasterSlider.text = (PlayerPrefs.GetFloat("MasterVolume") * maxSliderAmount).ToString("0");
            MasterSliderValue.value = PlayerPrefs.GetFloat("MasterVolume");
        }
        else { 
            AudioMixer.SetFloat("MasterVolume", Mathf.Log10(1) * 20);
            MasterSlider.text = (1 * maxSliderAmount).ToString("0");
            MasterSliderValue.value = 1;
        }

        if (PlayerPrefs.GetFloat("SFXVolume", -1) != -1) {
            AudioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
            SFXSlider.text = (PlayerPrefs.GetFloat("SFXVolume") * maxSliderAmount).ToString("0");
            SFXSliderValue.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else { 
            AudioMixer.SetFloat("SFXVolume", Mathf.Log10(1) * 20);
            SFXSlider.text = (1 * maxSliderAmount).ToString("0");
            SFXSliderValue.value = 1;
        }

        if (PlayerPrefs.GetFloat("MusicVolume", -1) != -1) {
            AudioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            MusicSlider.text = (PlayerPrefs.GetFloat("MusicVolume") * maxSliderAmount).ToString("0");
            MusicSliderValue.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else { 
            AudioMixer.SetFloat("MusicVolume", Mathf.Log10(1) * 20);
            MusicSlider.text = (1 * maxSliderAmount).ToString("0");
            MusicSliderValue.value = 1;
        }

        if (PlayerPrefs.GetFloat("VoiceVolume", -1) != -1)
        {
            AudioMixer.SetFloat("VoiceVolume", Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume")) * 20);
            VoiceSlider.text = (PlayerPrefs.GetFloat("VoiceVolume") * maxSliderAmount).ToString("0");
            VoiceSliderValue.value = PlayerPrefs.GetFloat("VoiceVolume");
        }
        else
        {
            AudioMixer.SetFloat("VoiceVolume", Mathf.Log10(1) * 20);
            VoiceSlider.text = (1 * maxSliderAmount).ToString("0");
            VoiceSliderValue.value = 1;
        }
        
    }
}
