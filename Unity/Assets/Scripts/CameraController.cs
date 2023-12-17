using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothness;
    public float sensitivity;
    public float minPitch;
    public float maxPitch;

    public float zoomSensitivity;
    public float minDistance;
    public float maxDistance;
    float dist;

    float yaw;
    float pitch;

    float syaw;
    float spitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        syaw = Mathf.Lerp(syaw, yaw, smoothness);
        spitch = Mathf.Lerp(spitch, pitch, smoothness);

        transform.rotation = Quaternion.Euler(spitch, syaw, 0);

		transform.position = target.position + transform.rotation * Vector3.forward * dist;
        
    }
}
