using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraVignete : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField][Range(0.0f, 1.0f)] private float normalizeSpeedVignette;
    [SerializeField] float vignetteAmount;
    [SerializeField] float maxVignetteAmount;

    private PostProcessVolume postProcessing;
    private Vignette vignette;

    private void Start()
    {
        postProcessing = GetComponent<PostProcessVolume>();
        postProcessing.profile.TryGetSettings(out vignette);
    }

    private void Update()
    {
        if (car.NormalizeLinearVelocity >= normalizeSpeedVignette)
        {
            vignette.intensity.value += vignetteAmount * Time.deltaTime;

            if (vignette.intensity.value > maxVignetteAmount)
            {
                vignette.intensity.value = maxVignetteAmount;
            }

        }
        else
        {
            vignette.intensity.value -= vignetteAmount * Time.deltaTime;
            if (vignette.intensity.value <= 0)
            {
                vignette.intensity.value = 0;
            }
        }
    }
}
