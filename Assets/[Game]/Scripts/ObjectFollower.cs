using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private Transform followTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private bool followXRotation;
    [SerializeField] private bool followYRotation;
    [SerializeField] private bool followZRotation;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followTransform.position, Time.deltaTime * followSpeed);

        //transform.position = followTransform.position;
        // Follow the target object's rotation with lerp
        Vector3 targetRotation = followTransform.rotation.eulerAngles;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float x = followXRotation ? targetRotation.x : currentRotation.x;
        float y = followYRotation ? targetRotation.y : currentRotation.y;
        float z = followZRotation ? targetRotation.z : currentRotation.z;
        transform.rotation = Quaternion.Euler(x, y, z);
    }
}