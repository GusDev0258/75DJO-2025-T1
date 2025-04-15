using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActions : MonoBehaviour
{
    private ObjectIdentifier identifier;

    private bool took = false;
    // Start is called before the first frame update
    void Start()
    {
        identifier = GetComponent<ObjectIdentifier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && identifier.GetTakeObject() != null)
        {
            Take();
        }

        if (!Input.GetKeyDown(KeyCode.F) || identifier.GetDragObject() == null) return;
        if (!took)
        {
            Drag();
        }
        else
        {
            Drop();
        }

        took = !took;
    }

    private void Take()
    {
        IPegavel takeObject = identifier.GetTakeObject().GetComponent<IPegavel>();
        takeObject.Take();
        StartCoroutine(DestroyAfterDelay(identifier.GetTakeObject(), 0.2f));
        identifier.HideText();
    }

    private IEnumerator DestroyAfterDelay(GameObject takeObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(takeObject);
    }

    private void Drag()
    {
        GameObject obj = identifier.GetDragObject();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Init();
        identifier.enabled = false;
    }

    private void Drop()
    {
        GameObject obj = identifier.GetDragObject();
        Destroy(obj.GetComponent<DragDrop>());
        identifier.enabled = true;

    }
}
