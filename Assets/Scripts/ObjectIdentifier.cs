using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIdentifier : MonoBehaviour
{
    private float targetDistance;

    private GameObject dragObject, takeObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            dragObject = null;
            takeObject = null;

            int ignoreLayer = 7;
            ignoreLayer = 1 << ignoreLayer;
            ignoreLayer = ~ignoreLayer;

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5,
                    ignoreLayer))
            {
                targetDistance = hit.distance;
                if (hit.transform.gameObject.tag == "Drag")
                {
                    dragObject  = hit.transform.gameObject;
                }
                if (hit.transform.gameObject.tag == "Take")
                {
                    takeObject = hit.transform.gameObject;
                }
            }
        }
    }

    public float GetTargetDistance()
    {
        return this.targetDistance;
    }

    public GameObject GetDragObject()
    {
        return this.dragObject;
    }

    public GameObject GetTakeObject()
    {
        return this.takeObject;
    }
}
