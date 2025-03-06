using System.Collections.Generic;
using UnityEngine;

public class CarReset : MonoBehaviour
{
    public List<Transform> respawnPoints;
    public Transform car;
    public Rigidbody carRigidbody;
    public float raycastHeight = 10f;
    public float groundOffset = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCarPosition();
        }
    }

    void ResetCarPosition()
    {
        if (respawnPoints.Count < 2) return;

        int closestIndex = 0;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < respawnPoints.Count; i++)
        {
            float distance = Vector3.Distance(car.position, respawnPoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }


        int targetIndex = Mathf.Max(closestIndex - 1, 0);
        Transform bestPoint = respawnPoints[targetIndex];


        Vector3 newPosition = bestPoint.position + Vector3.up * raycastHeight;
        if (Physics.Raycast(newPosition, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
        {
            newPosition = hit.point + Vector3.up * groundOffset;
        }


        car.position = newPosition;
        car.rotation = bestPoint.rotation;

        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }
}