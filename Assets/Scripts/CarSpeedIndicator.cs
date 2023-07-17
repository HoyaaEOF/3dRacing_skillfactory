using UnityEngine;
using UnityEngine.UI;

public class CarSpeedIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Text text;

    void Update()
    {
        text.text = car.linearVelocity.ToString("F0");
    }
}
