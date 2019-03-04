﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSkateController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 10;

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider FrontLeftC, FrontRightC;
    public WheelCollider RearLeftC, RearRightC;

    public Transform FrontLeftT, FrontRightT;
    public Transform RearLeftT, RearRightT;

    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public bool isJump = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = -Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        FrontLeftC.steerAngle = m_steeringAngle;
        FrontRightC.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        FrontLeftC.motorTorque = m_verticalInput * motorForce;
        FrontRightC.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(FrontLeftC, FrontLeftT);
        UpdateWheelPose(FrontRightC, FrontRightT);
        UpdateWheelPose(RearLeftC, RearLeftT);
        UpdateWheelPose(RearRightC, RearRightT);
    }

    private void UpdateWheelPose( WheelCollider _collider , Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }
    private void AirControl()
    {
        if (isJump)
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.left * 35 * 10000 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.right * 35 * 10000 * Time.deltaTime);
            }


        }
    }
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        AirControl();

        if (Input.GetKeyDown("space")) {
            if (!isJump)
            {
                isJump = true;
                rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 50);
                StartCoroutine(MultipleJump());
                
            }
        }
    }
    IEnumerator MultipleJump()
    {
        print(isJump);
        yield return new WaitForSeconds(2);
        isJump = false;
    }

}
