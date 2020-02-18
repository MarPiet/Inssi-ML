using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    Rigidbody rBody;


    public Transform Target;
    public Transform[] Floors;
    public Transform[] Obstacles;
    public Transform[] Spikes;
    RayPerception rayPer;

    private Vector3 startpos;
    private int floor = 0;
    private bool dead = false;
    public bool grounded = false;
    private bool hitobstacle = false;
    private bool elevated = false;
    private bool bubblehit = false;
    private bool goal1 = false;
    private bool goal2 = false;
    private bool goal3 = false;
    private bool ramphit = false;




    RollerAcademy academy;


    public override void InitializeAgent()
    {
        base.InitializeAgent();
        rayPer = GetComponent<RayPerception>();
        rBody = GetComponent<Rigidbody>();
        startpos = this.transform.position;

        academy = FindObjectOfType<RollerAcademy>();
    }

    public override void AgentReset()
    {

     
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = startpos;
        bubblehit = false;
        goal1 = false;
        goal2 = false;


        // Move the target to a new spot
        /*  Target.position = new Vector3(Random.value * 8 - 4,
                                        0.5f,*/


        /*  if(academy.resetParameters["map_difficulty"] == 0f)
          {
             foreach(Transform obstacle in Obstacles)
              {
                  obstacle.gameObject.SetActive(false);
              }
              foreach (Transform spike in Spikes)
              {
                  spike.gameObject.SetActive(false);
              }
          }*/

    }

    public override void CollectObservations()
    {
        Vector3 relativePosition = Target.position - this.transform.position;

        AddVectorObs(relativePosition);
        AddVectorObs(transform.rotation.eulerAngles.y);
        Transform closestFloor = Floors[0];

     /*   foreach (Transform floor in Floors)
        {
            if (Vector3.Distance(this.transform.position, floor.position) < Vector3.Distance(this.transform.position, closestFloor.position))
                closestFloor = floor;
        }
        AddVectorObs(this.transform.position - closestFloor.position);


        foreach (Transform obstacle in Obstacles)
        {
            AddVectorObs(obstacle.position);
        }*/
        AddVectorObs(rBody.velocity);
        AddVectorObs(gameObject.transform.rotation);

        foreach(Transform spike in Spikes)
        {
            AddVectorObs(spike.GetComponent<Rigidbody>().velocity);
        }


        float rayDistance = 15f;
        float[] rayAngles = {70f, 90f, 110f};
        string[] detectableObjects;

        detectableObjects = new[] {"obstacle", "platform", "spike", "bubble", "target"};
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1f, -10f));

        /* if((this.transform.position - floors[0].position).magnitude < (this.transform.position - floors[1].position).magnitude)
             AddVectorObs(this.transform.position - floors[0].position);
         else
             AddVectorObs(this.transform.position - floors[1].position);*/

    }

    public float speed = 20f;
    private float previousDistance = float.MaxValue;
    private float rotationamount = 0f;


    public override void AgentAction(float[] vectorAction, string textAction)
    {
      /* if(GetCumulativeReward() > 0f)
        {
            foreach (Transform obstacle in Obstacles)
            {
                obstacle.gameObject.SetActive(true);
            }
            academy.resetParameters["map_difficulty"] = 1f;
        }*/
        // Actions, size = 3
        Vector3 controlSignal = Vector3.zero;
        rotationamount = vectorAction[0];
        controlSignal.z = vectorAction[1];
        float jump = vectorAction[2];

        if (jump > 0f && grounded)
        {
            grounded = false;
            rBody.AddForce(new Vector3(0, 75f, 0));
        }
        if (controlSignal.z > 0f)
        {
            rBody.AddForce(transform.forward * speed);

        }
        transform.Rotate(new Vector3(0f, rotationamount, 0f) * 1.5f);



        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        previousDistance = distanceToTarget;

        // Reached target
        if (distanceToTarget < 1.42f)
         {
             SetReward(1.0f);
             Done();
         }
        if (distanceToTarget < 10f && !goal2)
        {
            goal2 = true;
            SetReward(0.5f);
        }
        if (distanceToTarget < 25f && !goal1)
        {
            goal1 = true;
            SetReward(0.20f);
        }
        if (this.transform.position.z >= 55f && !goal3 && grounded)
        {
            goal3 = true;
            SetReward(0.50f);
        }

        if (distanceToTarget < previousDistance && !hitobstacle)
        {
             AddReward(0.01f);
           // AddReward((40.5f / distanceToTarget / 66) * 0.01f);
        }
  
      /*  if(transform.rotation.eulerAngles.y < 90 || transform.rotation.eulerAngles.y > 270)
        {
            AddReward(0.005f);
        }*/
      

        // Fell off platform
        if (this.transform.position.y < 0)
        {
            AddReward(-1.0f);
            Done();
        }
        if (dead)
        {
            dead = false;
            AddReward(-1.0f);
            Done();
        }
        if (bubblehit)
        {
            AddReward(0.25f);
        }
        if (hitobstacle)
        {
            AddReward(-0.1f);
        }

         AddReward(-0.005f);


    }
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "spike")
        {
            dead = true;
        }
        if (c.gameObject.tag == "bubble")
        {
            if(!bubblehit)
                bubblehit = true;
        }

    }

    void OnCollision(Collision c)
    {
        if (c.gameObject.tag == "platform")
            grounded = true;
        if (c.gameObject.tag == "obstacle")
            hitobstacle = true; 
       /* if(c.gameObject.tag == "elevatedplatform" && !elevated)
        {
            elevated = true;
        }*/

    }
    void OnCollisionStay(Collision c)
    {
        if (c.gameObject.tag == "platform")
            grounded = true;
        if (c.gameObject.tag == "obstacle")
            hitobstacle = true;


    }
    void OnCollisionExit(Collision c)
    {
       // if (c.gameObject.tag == "platform")
          //  grounded = false;
        if (c.gameObject.tag == "obstacle")
            hitobstacle = false;
    }

    

}