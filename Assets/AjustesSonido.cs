using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AjustesSonido : MonoBehaviour
{
    [SerializeField] private AudioMixer miAudioMixer;
    [SerializeField] private Slider SliderMusica;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
        
        
    }

    public void SetMusicVolume()
    {
        float volume = SliderMusica.value;
        miAudioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        SliderMusica.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
}
