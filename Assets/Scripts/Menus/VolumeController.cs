using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Texture volumeHighSprite;     // VolumeHigh.png
    public Texture volumeMediumSprite;   // VolumeMedium.png
    public Texture volumeMuteSprite;    // VolumeMute.png

    public RawImage volumeIcon;
    public Slider slider;

    void Start()
    {
        // load the volume setting from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f); // default volume is 1 (max)
        
        // prepare the volume slider
        slider.value = savedVolume;
        UpdateVolumeIcon();
        
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float volumeValue)
    {
        UpdateVolumeIcon();
        
        // set the game volume based on the slider value
        AudioListener.volume = volumeValue;

        // save the volume setting to PlayerPrefs
        PlayerPrefs.SetFloat("GameVolume", volumeValue);
        PlayerPrefs.Save();
    }

    void UpdateVolumeIcon()
    {
        volumeIcon.texture = slider.value switch
        {
            >= 0.75f => volumeHighSprite,   // >= 75% will be the full volume sprite
            > 0f => volumeMediumSprite,     // between 0% and 75% will be the partial-volume sprite
            _ => volumeMuteSprite           // 0% will be the muted sprite
        };
    }
}