using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breathing : MonoBehaviour
{
    private bool isInhaling = true;

    public float minHeight = -0.035f;
    public float maxHeight = 0.035f;

    [Range(0f, 5f)] public float breathStrength = 1f;

    private float movement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInhaling)
        {
            movement = Mathf.Lerp(movement, maxHeight, Time.deltaTime * breathStrength);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);

            if( movement >= maxHeight - 0.001f)
            {
                isInhaling = false;
            }

        }
        else
        {
            movement = Mathf.Lerp(movement, minHeight, Time.deltaTime * breathStrength);
            transform.localPosition = new Vector3(transform.localPosition.x, movement, transform.localPosition.z);

            if (movement <= minHeight + 0.01f)
            {
                isInhaling = true;
            }
        }

        if (breathStrength > 1)
        {
            breathStrength = Mathf.Lerp(breathStrength, 1f, Time.deltaTime * 0.2f);
        }
    }
}
