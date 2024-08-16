using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;

    private void OnEnable() {
        myMixer.SetFloat("Master", -20f);
        setSliderToCurrentVolume();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log($"{Screen.fullScreen}, {Screen.fullScreenMode}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    private void setSliderToCurrentVolume() {
        float volume;
        myMixer.GetFloat("static", out volume);
        GetComponent<Slider>().value = Mathf.Pow(10, volume / 20f);
    }

    public void SetStaticVolume()
    {
        float sliderValue = GetComponent<Slider>().value;
        float volume = sliderValue == 0 ? float.MinValue : Mathf.Log10(sliderValue) * 20;
        myMixer.SetFloat("static", volume);
        Debug.Log($"Slider = {sliderValue}, Volume = {volume}");
    }
}
