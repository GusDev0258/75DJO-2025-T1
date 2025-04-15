using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMovement : MonoBehaviour
{
    public float value = 0.1f;

    public float maxValue = 0.6f;

    public float smoothValue = 6;

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * value;
        float movementY = -Input.GetAxis("Mouse Y") * value;
        
        movementX = Mathf.Clamp(movementX, -maxValue, maxValue);
        movementY = Mathf.Clamp(movementY, -maxValue, maxValue);

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition,
            Time.deltaTime * smoothValue);
    }
}
