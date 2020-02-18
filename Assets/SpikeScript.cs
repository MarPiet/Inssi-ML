using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private bool leftSpike = true;
    public float Speed = 80f;

    void Start()
    {
        if (gameObject.transform.localPosition.x > 0)       
            leftSpike = false;       
    }

    void Update()
    {
        if (leftSpike && gameObject.transform.localPosition.x <= -4)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * Speed);
        }
        if (leftSpike && gameObject.transform.localPosition.x >= -1)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * Speed);
        }

        if (!leftSpike && gameObject.transform.localPosition.x >= 4)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * Speed);
        }
        if (!leftSpike && gameObject.transform.localPosition.x <= 1)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * Speed);
        }

    }
}
