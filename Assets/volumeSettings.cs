using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider AudioManager;

    public void SetStaticVolume()
    {
        float volume = AudioManager.value;
        myMixer.SetFloat("static", Mathf.Log10(volume)*20);
    }
}
