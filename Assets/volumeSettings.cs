using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    public string GroupName;

    private void OnEnable() {
        setSliderToCurrentVolume();
    }

    private void setSliderToCurrentVolume() {
        float volume;
        myMixer.GetFloat(GroupName, out volume);
        GetComponent<Slider>().value = Mathf.Pow(10, volume / 20f);
    }

    public void SetMixerVolume() {
        float sliderValue = GetComponent<Slider>().value;
        float volume = sliderValue == 0 ? float.MinValue : Mathf.Log10(sliderValue) * 20;
        myMixer.SetFloat(GroupName, volume);
        //Debug.Log($"Slider = {sliderValue}, Volume = {volume}");
    }
}
