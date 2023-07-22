using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]
public class CarGearboxSound : MonoBehaviour
{
    [SerializeField] private CarGearboxIndicator indicator;
    [SerializeField] private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        indicator.IndicatorChanged += OnIndicatorChanged;
    }

    private void OnDestroy()
    {
        indicator.IndicatorChanged -= OnIndicatorChanged;
    }

    private void OnIndicatorChanged()
    {
        audio.Play();
    }
}
