using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public float rotationSpeed = 90.0f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the coin around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
