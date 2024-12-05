using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsRingSpin : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 1, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
