using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CarGearboxIndicator : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private Text text;
    public event UnityAction IndicatorChanged;

    private string prevGearName = "1";

    private void Start()
    {
        car.GearChanged += OnGearChanged;
    }

    private void OnDestroy()
    {
        car.GearChanged -= OnGearChanged;
    }

    private void OnGearChanged(string gearName)
    {
        text.text = gearName;
        if (prevGearName != gearName)
        {
            IndicatorChanged?.Invoke();
            prevGearName = gearName;
        }

    }
}
