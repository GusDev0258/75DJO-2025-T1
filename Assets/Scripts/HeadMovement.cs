using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    private float time = 0.0f;

    public float speed = 0.1f;

    public float strength = 0.2f;

    public float originPoint = 0.0f;

    private Vector3 positionSaver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float waveCutter = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        positionSaver = transform.localPosition;

        if (horizontal == 0 && vertical == 0)
        {
            time = 0.0f;
        }
        else
        {
            waveCutter = Mathf.Sin(time);
            time += speed;

            if (waveCutter > Mathf.PI * 2)
            {
                time -= (Mathf.PI * 2);
            }
        }

        if (waveCutter != 0)
        {
            float changeMovement = waveCutter * strength;
            float totalAxis = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxis  = Mathf.Clamp(totalAxis  , 0.0f, 1.0f);
            changeMovement =  totalAxis * changeMovement;
            positionSaver.y = originPoint + changeMovement;
        }
        else
        {
            positionSaver.y = originPoint;
        }

        transform.localPosition = positionSaver;
    }
}
