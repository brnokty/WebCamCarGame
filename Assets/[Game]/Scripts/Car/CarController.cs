using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float maxTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 3000f;

    public WheelCollider frontLeftWheel, frontRightWheel, rearLeftWheel, rearRightWheel;
    public Transform frontLeftMesh, frontRightMesh, rearLeftMesh, rearRightMesh;

    private float accelerationInput;
    private float brakeInput;
    private float steerInput;
    private int gear = 1; // 1: İleri, -1: Geri


    private bool isFlipped = false;
    private float flipTimer = 0f;
    private float flipThreshold = 3f;

    private bool canControlCar = false;
    private Rigidbody _rigidbody;
    private SoundManager _soundManager;

    private void Start()
    {
        _soundManager = SoundManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        EventManager.Instance.Register(EventTypes.StartRace, RaceStarted);
        EventManager.Instance.Register(EventTypes.SavePhotos, RaceFinished);
        UIManager.Instance.inGamePanel.SetGear(gear);
        _rigidbody.centerOfMass = new Vector3(0, 0.21f, 0); //Arabanın ağırlık merkesini ayarladım

        AdjustWheelFriction(frontLeftWheel);
        AdjustWheelFriction(frontRightWheel);
        AdjustWheelFriction(rearLeftWheel);
        AdjustWheelFriction(rearRightWheel);

        AdjustSuspension(frontLeftWheel);
        AdjustSuspension(frontRightWheel);
        AdjustSuspension(rearLeftWheel);
        AdjustSuspension(rearRightWheel);
        EventRunner.Instance.GameStart();
        _soundManager.PlaySound(SoundManager.SoundType.CarEngine, true);
    }

    void AdjustWheelFriction(WheelCollider wheel)
    {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        forwardFriction.stiffness = 2.0f; // 1.0'dan büyük olmalı (2.0 iyi bir başlangıç)
        wheel.forwardFriction = forwardFriction;

        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
        sidewaysFriction.stiffness = 2.5f; // Daha büyük değerler daha az kayma sağlar
        wheel.sidewaysFriction = sidewaysFriction;
    }

    void AdjustSuspension(WheelCollider wheel)
    {
        JointSpring suspension = wheel.suspensionSpring;
        suspension.spring = 35000f; // Yay sertliği (Daha küçükse süspansiyon yumuşar)
        suspension.damper = 5000f; // Amortisör sönümleme
        suspension.targetPosition = 0.5f; // Süspansiyon başlangıç noktası
        wheel.suspensionSpring = suspension;
    }

    private void Update()
    {
        if (!canControlCar)
            return;

        GetInput();
        UpdateWheels();
        CheckIfFlipped();
    }

    private void FixedUpdate()
    {
        if (!canControlCar)
            return;

        MoveCar();
        SteerCar();
        BrakeCar();
    }

    private void GetInput()
    {
        accelerationInput = Input.GetAxis("Vertical") * gear;
        steerInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        if (Gamepad.current != null)
        {
            accelerationInput = Gamepad.current.rightTrigger.ReadValue() * gear;
            brakeInput = Gamepad.current.leftTrigger.ReadValue();
            steerInput = Gamepad.current.leftStick.x.ReadValue();

            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                ChangeGear();
            }
        }
    }

    private void MoveCar()
    {
        float torque = accelerationInput * maxTorque;
        rearLeftWheel.motorTorque = torque;
        rearRightWheel.motorTorque = torque;

        float speed = GetSpeedInKMH();
        UIManager.Instance.inGamePanel.SetSpeed(speed);

        float pitch = Mathf.Lerp(0.3f, 1.8f, speed / 150f);
        float volume = Mathf.Lerp(0.3f, 1, speed / 150f);

        _soundManager.SetSoundPitch(SoundManager.SoundType.CarEngine, pitch);
        _soundManager.SetSoundVolume(SoundManager.SoundType.CarEngine, volume);
    }

    private void SteerCar()
    {
        float steer = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steer;
        frontRightWheel.steerAngle = steer;
    }

    private void BrakeCar()
    {
        float brake = brakeInput * brakeForce;
        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    private void ChangeGear()
    {
        if (rearLeftWheel.rpm < 5 && rearRightWheel.rpm < 5)
        {
            gear *= -1;
            UIManager.Instance.inGamePanel.SetGear(gear);
        }
    }

    private void UpdateWheels()
    {
        UpdateWheelPosition(frontLeftWheel, frontLeftMesh);
        UpdateWheelPosition(frontRightWheel, frontRightMesh);
        UpdateWheelPosition(rearLeftWheel, rearLeftMesh);
        UpdateWheelPosition(rearRightWheel, rearRightMesh);
    }

    private void UpdateWheelPosition(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }

    private void CheckIfFlipped()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0.3f) // Araç ters mi?
        {
            if (!isFlipped)
            {
                isFlipped = true;
                flipTimer = 0f;
            }

            flipTimer += Time.deltaTime;

            if (flipTimer >= flipThreshold)
            {
                FlipCar();
            }

            //print("Ters");
        }
        else
        {
            isFlipped = false;
            flipTimer = 0f;
            //print("Ters Değil");
        }
    }

    private void FlipCar()
    {
        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        StartCoroutine(SmoothFlip(targetRotation, 1.5f));
    }

    private IEnumerator SmoothFlip(Quaternion targetRotation, float duration)
    {
        StopCar();
        float elapsed = 0f;
        Quaternion initialRotation = transform.rotation;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }


    private void StopCar()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        frontLeftWheel.motorTorque = 0f;
        frontRightWheel.motorTorque = 0f;
        rearLeftWheel.motorTorque = 0f;
        rearRightWheel.motorTorque = 0f;

        frontLeftWheel.brakeTorque = 10000f;
        frontRightWheel.brakeTorque = 10000f;
        rearLeftWheel.brakeTorque = 10000f;
        rearRightWheel.brakeTorque = 10000f;

        StartCoroutine(ResetWheelRPM());
    }

    private IEnumerator ResetWheelRPM()
    {
        frontLeftWheel.enabled = false;
        frontRightWheel.enabled = false;
        rearLeftWheel.enabled = false;
        rearRightWheel.enabled = false;

        yield return null;

        frontLeftWheel.enabled = true;
        frontRightWheel.enabled = true;
        rearLeftWheel.enabled = true;
        rearRightWheel.enabled = true;

        frontLeftWheel.brakeTorque = 0f;
        frontRightWheel.brakeTorque = 0f;
        rearLeftWheel.brakeTorque = 0f;
        rearRightWheel.brakeTorque = 0f;
    }

    private void SoftStop()
    {
        frontLeftWheel.motorTorque = 0f;
        frontRightWheel.motorTorque = 0f;
        rearLeftWheel.motorTorque = 0f;
        rearRightWheel.motorTorque = 0f;

        frontLeftWheel.brakeTorque = 10000f;
        frontRightWheel.brakeTorque = 10000f;
        rearLeftWheel.brakeTorque = 10000f;
        rearRightWheel.brakeTorque = 10000f;
    }

    private float GetSpeedInKMH()
    {
        return _rigidbody.velocity.magnitude * 3.6f;
    }

    private void RaceStarted(EventArgs args)
    {
        canControlCar = true;
    }

    private void RaceFinished(EventArgs args)
    {
        canControlCar = false;
        SoftStop();
        _soundManager.StopSound(SoundManager.SoundType.CarEngine);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unregister(EventTypes.StartRace, RaceStarted);
        EventManager.Instance.Unregister(EventTypes.SavePhotos, RaceFinished);
    }
}