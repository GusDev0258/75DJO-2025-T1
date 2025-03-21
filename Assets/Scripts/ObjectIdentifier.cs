using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectIdentifier : MonoBehaviour
{
    private float targetDistance;

    private GameObject dragObject, takeObject;
    private GameObject targetObject;
    public Text keyText, textMessage;

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
            keyText.text = "";
            textMessage.text = "";

            int ignoreLayer = 7;
            ignoreLayer = 1 << ignoreLayer;
            ignoreLayer = ~ignoreLayer;

            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5,
                    ignoreLayer))
            {
                
                targetDistance = hit.distance;
                if (targetObject != null && hit.transform.gameObject != targetObject)
                {
                    targetObject.GetComponent<Outline>().OutlineWidth = 0f;
                    targetObject = null;
                    
                    HideText();
                }
                if (hit.transform.gameObject.tag == "Drag")
                {
                    dragObject  = hit.transform.gameObject;
                    targetObject = dragObject;

                    keyText.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                    textMessage.color = keyText.color;
                    keyText.text = "[F]";
                    textMessage.text = "Arrastar/Soltar";
                }
                if (hit.transform.gameObject.tag == "Take")
                {
                    takeObject = hit.transform.gameObject;
                    targetObject = takeObject;

                    keyText.color = new Color(51 / 255f, 1, 0);
                    textMessage.color = keyText.color;
                    keyText.text = "[F]";
                    textMessage.text = "Pegar";
                }

                if (targetObject != null)
                {
                    targetObject.GetComponent<Outline>().OutlineWidth = 5f;
                }
            }
            else
            {
                if (targetObject != null)
                {
                    targetObject.GetComponent<Outline>().OutlineWidth = 0f;
                    targetObject = null;
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

    public void HideText()
    {
        keyText.text = "";
        textMessage.text = "";
    }
}
