using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(AudioSource))]
public class WindSound : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioSource windAudio;
    [SerializeField] private float baseVolume;
    [SerializeField] private float volumeModifier;
    [SerializeField] private float backgroungVolumeModifier;
    [SerializeField] private float minBackgroungVolume;

    [SerializeField] private float neededVelocity;

    private float startBackgroungVolume;

    bool isPlaying = false;

    private void Start()
    {
        windAudio = GetComponent<AudioSource>();
        startBackgroungVolume = backgroundAudio.volume;
    }

    private void Update()
    {
        if (car.linearVelocity >= neededVelocity)
        {
            
            if (isPlaying == false) 
            {
                windAudio.Play();
                isPlaying = true;
            }

            backgroundAudio.volume -= backgroungVolumeModifier * Time.deltaTime;
            if (backgroundAudio.volume < minBackgroungVolume)
            {
                backgroundAudio.volume = minBackgroungVolume;
            }

            windAudio.volume = baseVolume + (volumeModifier * car.NormalizeLinearVelocity);

        }
        else
        {
            if (isPlaying == true)
            {                
                windAudio.Stop();
                isPlaying = false;
            }
            if (backgroundAudio.volume != startBackgroungVolume) backgroundAudio.volume += backgroungVolumeModifier * Time.deltaTime;
            if (backgroundAudio.volume > startBackgroungVolume)
            {
                backgroundAudio.volume = startBackgroungVolume;
            }
        }
    }

}
