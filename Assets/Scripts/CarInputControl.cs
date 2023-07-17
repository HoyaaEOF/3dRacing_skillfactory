using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve brakeCurve;
    [SerializeField] private AnimationCurve steerCurve;

    [SerializeField][Range(0.0f, 1.0f)] private float autoBrakeStrength = 0.5f;

    private float wheelSpeed;
    private float verticalAxis;
    private float horizontalAxis;
    private float handbrakeAxis;

    private void Update()
    {
        wheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottle();
        UpdateSteer();

        UpdateAutoBrake();

        if (Input.GetKeyDown(KeyCode.E))
        {
            car.UpGear();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            car.DownGear();
        }
    }

    private void UpdateSteer()
    {
        car.steerControll = steerCurve.Evaluate(wheelSpeed / car.MaxSpeed) * horizontalAxis;
    }

    private void UpdateThrottle()
    {
        if (Mathf.Sign(verticalAxis) == Mathf.Sign(wheelSpeed) || Mathf.Abs(wheelSpeed) < 0.5f)
        {
            car.throttleControll = Mathf.Abs(verticalAxis);
            car.brakeControll = 0;
        }

        else
        {
            car.throttleControll = 0;
            car.brakeControll = brakeCurve.Evaluate(wheelSpeed / car.MaxSpeed);
        }

        if (verticalAxis < 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f)
        {
            car.ShiftToReverseGear();
        }

        if (verticalAxis > 0 && wheelSpeed > -0.5f && wheelSpeed <= 0.5f)
        {
            car.ShiftToFirstGear();
        }
    }
    private void UpdateAutoBrake()
    {
        if (verticalAxis == 0)
        {
            car.brakeControll = brakeCurve.Evaluate(car.WheelSpeed / car.MaxSpeed) * autoBrakeStrength;
        }
    }

    private void UpdateAxis()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");
        handbrakeAxis = Input.GetAxis("Jump");
    }
}
