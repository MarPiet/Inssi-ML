using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    private bool goingUp = false;
    private Transform otherTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* if (goingUp && transform.position.y < 22f)       
         {
             transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z + 0.02f);
             otherTransform.position = Vector3.MoveTowards(otherTransform.position, transform.position, Mathf.Infinity);
         }
         if (transform.position.y >= 22f && transform.position.z <= 35f) 
         {
             transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
             otherTransform.position = Vector3.MoveTowards(otherTransform.position, transform.position, Mathf.Infinity);

         }
         if (transform.position.z >= 35f)
         {
             gameObject.SetActive(false);
             otherTransform.gameObject.GetComponent<RollerAgent>().SetBubble();
             goingUp = false;
             gameObject.transform.localPosition = new Vector3(0f, 0f, 23f);
             gameObject.SetActive(true);
         }
      */
    }

    void OnTriggerEnter(Collider other)
    {
        /*  if (other.gameObject.tag == "agent")
          {
              other.GetComponent<RollerAgent>().SetBubble();
              goingUp = true;
              otherTransform = other.gameObject.transform;
          }*/
        if (other.gameObject.tag == "agent")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0, 300f, 0));

        }

    }
      
}
