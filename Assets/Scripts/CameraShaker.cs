using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField][Range(0f, 1f)] private float normalizeSpeedShake;
    [SerializeField] private float shakeAmount;
    private void Update()
    {
        if(car.NormalizeLinearVelocity >= normalizeSpeedShake) transform.position += Random.insideUnitSphere * shakeAmount * Time.deltaTime;
    }
}
