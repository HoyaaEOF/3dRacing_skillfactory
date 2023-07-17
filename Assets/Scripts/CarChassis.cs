using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] wheelAxles;
    [SerializeField] private float wheelBaseLength;

    [SerializeField] private Transform centerOfMass;

    [Header("DownForce")]
    [SerializeField] private float downForceMin;
    [SerializeField] private float downForceMax;
    [SerializeField] private float downForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float angularDragMin;
    [SerializeField] private float angularDragMax;
    [SerializeField] private float angularDragFactor;

    public float motorTorque;
    public float brakeTorque;
    public float steerAngle;

    public float linearVelocity => rigidBody.velocity.magnitude * 3.6f; 

    private new Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        if(centerOfMass != null)
        {
            rigidBody.centerOfMass = centerOfMass.localPosition;
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].ConfigureVehicleSubSteps(50, 50, 50);
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();

        UpdateDownForce();

        UpdateWheelAxles();
    }

    public float GetAvarageRpm()
    {
        float sum = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            sum += wheelAxles[i].GetAvarageRpm();
        }

        return sum / wheelAxles.Length;
    }

    public float GetWheelSpeed()
    {
        return GetAvarageRpm() * wheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(downForceFactor * linearVelocity, downForceMin, downForceMax);
        rigidBody.AddForce(-transform.up * downForce);
    }

    private void UpdateAngularDrag()
    {
        rigidBody.angularDrag = Mathf.Clamp(angularDragFactor * linearVelocity, angularDragMin, angularDragMax);
    }

    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            if (wheelAxles[i].IsMotor == true)
            {
                amountMotorWheel += 2;
            }
        }

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            wheelAxles[i].Update();

            wheelAxles[i].ApplyMotorTorque(motorTorque / amountMotorWheel);
            wheelAxles[i].ApplySteerAngle(steerAngle, wheelBaseLength);
            wheelAxles[i].ApplyBrakeTorque(brakeTorque);
        }
    }
}
