using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class SimpleSkateController : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce = 10;

    int temp;
    string str;
    private SerialPort sp;

    private float m_steeringAngle;
    public float maxSpeed = 10f;

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

    public bool lastTimeBrakeTrue = false;

    public bool isJump = false;
    private void Start()
    {
        sp = new SerialPort("COM4", 9600);
        sp.ReadTimeout = 10;
        if (!sp.IsOpen)
        {
            sp.Open();

        }

        rb = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        m_rightHorizontalInput = (temp > 4) ? 1 : 0;
        m_leftHorizontalInput = (temp < -4) ? 1 : 0;
        m_upVerticalInput = Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0;
        m_downVerticalInput = Input.GetKeyDown(KeyCode.DownArrow) ? +1 : 0;
    }

    private void Steer()
    {

        if (temp > 4)
            m_steeringAngle = maxSteerAngle * m_rightHorizontalInput;

        else if (temp < -4)
            m_steeringAngle = maxSteerAngle * m_leftHorizontalInput;

        else
        {
            m_steeringAngle = 0;
        }
        FrontLeftC.steerAngle = m_steeringAngle;
        FrontRightC.steerAngle = m_steeringAngle;
    }

    /*private void Deaccelerate(int i)
    {
        if (i==1)
        {
            FrontLeftC.motorTorque = (m_downVerticalInput) * motorForce;
            FrontRightC.motorTorque = (m_downVerticalInput) * motorForce;

        }

        if (i==2)
        {
            FrontLeftC.motorTorque = 0;
            FrontRightC.motorTorque = 0;
        }
        
    }*/

    private void Accelerate()
    {
        if (rb.velocity.magnitude<maxSpeed)
        {
            FrontLeftC.motorTorque = -motorForce/10;
            FrontRightC.motorTorque = -motorForce/10;
        }

        else
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

    private void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            rb.AddForce(Vector3.up * jumpForce * Time.deltaTime * 50);
            StartCoroutine(MultipleJump());
        }
    }

    /*
    private void AirControl()
    {
        if (isJump)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(Vector3.left * 35 * 10000 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(Vector3.right * 35 * 10000 * Time.deltaTime);
            }


        }
    }
    */

    private void FixedUpdate()
    {
        // print(1.0f / Time.deltaTime);
        if (sp.IsOpen)
        {
            try
            {

                if (sp.ReadLine() == "j")
                {
                    Jump();
                    temp = 0;
                    print(temp);
                    maxSteerAngle = temp;
                }

                else if(sp.ReadLine()== "b" && lastTimeBrakeTrue)
                 {
                     //Deaccelerate(1);
                 }

                else if (sp.ReadLine() == "b")
                {
                    lastTimeBrakeTrue = true;
                }

                else
                {
                    lastTimeBrakeTrue = false;
                    //Deaccelerate(2);

                    if (!isJump)
                    {
                        temp = int.Parse(sp.ReadLine());
                        temp = temp * 2;
                        print(temp);
                        maxSteerAngle = temp;

                    }
                }
                GetInput();
                Steer();
                Accelerate();
                UpdateWheelPoses();
//                AirControl();



            }
            catch (System.Exception)
            {

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
