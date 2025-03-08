using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float mouseSens = 100f;

    public float minAngle = -90f;

    public float maxAngle = 90f;

    public Transform transformPlayer;

    private float rotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
       float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

       rotation -= mouseY;

       rotation = Mathf.Clamp(rotation, minAngle, maxAngle);
       transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
    }
}
