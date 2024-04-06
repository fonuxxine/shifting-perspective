using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Texture volumeHighSprite;     // VolumeHigh.png
    public Texture volumeMediumSprite;   // VolumeMedium.png
    public Texture volumeLowSprite;   // VolumeMedium.png
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
            >= 0.9f => volumeHighSprite,    // >= 90% will be the full volume sprite
            > 0.45f => volumeMediumSprite,   // between 45% and 90% is medium volume sprite
            > 0f => volumeLowSprite,        // between 0% and 60% will be the low volume sprite
            _ => volumeMuteSprite           // 0% will be the muted sprite
        };
    }
}