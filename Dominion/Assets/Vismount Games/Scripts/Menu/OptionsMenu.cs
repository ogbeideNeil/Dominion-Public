using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider effectsSlider;

    [SerializeField]
    private Slider musicSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(MasterVolumeUpdater);
        effectsSlider.onValueChanged.AddListener(EffectsVolumeUpdater);
        musicSlider.onValueChanged.AddListener(MusicVolumeUpdater);
    }

    private void OnEnable()
    {
        masterSlider.value = SoundManager.MasterVolume;
        effectsSlider.value = SoundManager.EffectsVolume;
        musicSlider.value = SoundManager.MusicVolume;
    }

    private void MasterVolumeUpdater(float volume)
    {
        SoundManager.SetMasterVolume((int) volume);
    }

    private void EffectsVolumeUpdater(float volume)
    {
        SoundManager.SetEffectsVolume((int) volume);
    }

    private void MusicVolumeUpdater(float volume)
    {
        SoundManager.SetMusicVolume((int) volume);
    }
}