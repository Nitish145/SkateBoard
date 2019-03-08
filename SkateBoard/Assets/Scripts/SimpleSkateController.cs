using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSkateController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 10;

    private float m_steeringAngle;
    public float maxSpeed = 20f;

    public WheelCollider FrontLeftC, FrontRightC;
    public WheelCollider RearLeftC, RearRightC;

    public Transform FrontLeftT, FrontRightT;
    public Transform RearLeftT, RearRightT;

    public float maxSteerAngle = 30;
    public float motorForce = 50;

    public float m_rightHorizontalInput;
    public float m_leftHorizontalInput;
    public float m_upVerticalInput;
    public float m_downVerticalInput;

    public bool isJump = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
          m_rightHorizontalInput = Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0;
          m_leftHorizontalInput = Input.GetKeyDown(KeyCode.LeftArrow) ? -1 : 0;
          m_upVerticalInput = Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0;
          m_downVerticalInput = Input.GetKeyDown(KeyCode.DownArrow) ? +1 : 0;
    }

    private void Steer()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            m_steeringAngle = maxSteerAngle * m_rightHorizontalInput;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            m_steeringAngle = maxSteerAngle * m_leftHorizontalInput;

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            m_steeringAngle = 0;
        }

        FrontLeftC.steerAngle = m_steeringAngle;
        FrontRightC.steerAngle = m_steeringAngle;
    }

    private void Deaccelerate()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FrontLeftC.motorTorque = (m_downVerticalInput) * motorForce;
            FrontRightC.motorTorque = (m_downVerticalInput) * motorForce;
        }

        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            FrontLeftC.motorTorque = 0;
            FrontRightC.motorTorque = 0;
        }
    }

    private void Accelerate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FrontLeftC.motorTorque = (m_upVerticalInput) * motorForce;
            FrontRightC.motorTorque = (m_upVerticalInput) * motorForce;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            FrontLeftC.motorTorque = 0;
            FrontRightC.motorTorque = 0;
        }
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
            if (Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector3.left * 35 * 10000 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector3.right * 35 * 10000 * Time.deltaTime);
            }


        }
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Deaccelerate();
        Accelerate();
        UpdateWheelPoses();
        AirControl();

        

        if (Input.GetKeyDown("space")) {
            if (!isJump)
            {
                isJump = true;
                rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 50 ,ForceMode.Impulse);
                StartCoroutine(MultipleJump());
            }
        }
    }
    IEnumerator MultipleJump()
    {
        print(isJump);
        yield return new WaitForSeconds(3);
        isJump = false;
    }

}
