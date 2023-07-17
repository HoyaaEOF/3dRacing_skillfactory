using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{
    public event UnityAction<string> GearChanged;

    private CarChassis chassis;

    
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float maxBrakeTorque;

    [Header("Engine")]
    [SerializeField] private AnimationCurve engineTorqueCurve;
    [SerializeField] private float engineTorque;
    [SerializeField] private float engineMaxTorque;
    [SerializeField] private float engineRpm;
    [SerializeField] private float engineMinRpm;
    [SerializeField] private float engineMaxRpm;

    [Header("Gearbox")]
    [SerializeField] private float[] gears;
    [SerializeField] private float finalDriveRatio;

    [SerializeField] private float selectedGear;
    [SerializeField] private float rearGear;
    [SerializeField] private float upShiftEngineRpm;
    [SerializeField] private float downShiftEngineRpm;

    [SerializeField] private int maxSpeed;

    [SerializeField] private int selectedGearIndex;
    public float linearVelocity => chassis.linearVelocity;
    public float WheelSpeed => chassis.GetWheelSpeed();
    public float MaxSpeed => maxSpeed;
    public float EngineRPM => engineRpm;
    public float EngineMaxRpm => engineMaxRpm;

    [SerializeField] private float LinearVelocity;
    public float throttleControll;
    public float steerControll;
    public float brakeControll;
    public float handBrakeControll;

    private void Start()
    {
        chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        LinearVelocity = linearVelocity;

        UpdateEngineTorquie();

        AutoGearShift();

        if (linearVelocity >= maxSpeed)
        {
            engineTorque = 0;
        }

        chassis.motorTorque = throttleControll * engineTorque;
        chassis.brakeTorque = brakeControll * maxBrakeTorque;
        chassis.steerAngle = steerControll * maxSteerAngle;
    }

    public string GetSelectedGearName()
    {
        if (selectedGear == rearGear) return "R";

        if (selectedGear == 0) return "N";

        return (selectedGearIndex + 1).ToString();
    }

    private void AutoGearShift()
    {
        if (selectedGear < 0) return;

        if (engineRpm >= upShiftEngineRpm)
        {
            UpGear();
        }

        if (engineRpm < downShiftEngineRpm)
        {
            DownGear();
        }
    }

    public void UpGear()
    {
        ShiftGear(selectedGearIndex + 1);
    }

    public void DownGear()
    {
        ShiftGear(selectedGearIndex - 1);
    }

    public void ShiftToReverseGear()
    {
        selectedGear = rearGear;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    public void ShiftToFirstGear()
    {
        ShiftGear(0);
    }

    public void ShiftToNeutralGear()
    {
        selectedGear = 0;
        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void ShiftGear(int gearIndex)
    {
        gearIndex = Mathf.Clamp(gearIndex, 0, gears.Length - 1);
        selectedGear = gears[gearIndex];

        selectedGearIndex = gearIndex;

        GearChanged?.Invoke(GetSelectedGearName());
    }

    private void UpdateEngineTorquie()
    {
        engineRpm = engineMinRpm + Mathf.Abs(chassis.GetAvarageRpm() * selectedGear * finalDriveRatio);
        engineRpm = Mathf.Clamp(engineRpm, engineMinRpm, engineMaxRpm);

        engineTorque = engineTorqueCurve.Evaluate(engineRpm/engineMaxRpm) * engineMaxTorque * finalDriveRatio * Mathf.Sign(selectedGear) * gears[0];
    }
}
