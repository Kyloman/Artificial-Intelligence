using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    public Vector3 initialAngle;
    public float initialVelocity;


    public float fitness;

    public GameObject goal;

    public bool isDead = false;

    public bool inHole = false;

    Rigidbody body;

    public float lifeTime = 0;

  

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);
        body = GetComponent<Rigidbody>();
        goal = GameObject.Find("GOAL");
    }

    // Update is called once per frame
    void Update()
    {
        checkAlive();

        if (!inHole && !isDead)
        {
            lifeTime += Time.deltaTime;
        }

        //gravity check
        GetComponent<Rigidbody>().AddForce(Vector3.down * 2000 * 9.8f *Time.deltaTime,ForceMode.Force);
    }



    public void randomizeMovement()
    {
        Vector3 randomAngle = new Vector3(Random.Range(-180f, 180f), 0, Random.Range(-180f, 180f));
        initialAngle = randomAngle;


        float randomVelocity = Random.Range(5f, 70f);
        initialVelocity = randomVelocity;

        Quaternion test = Quaternion.identity;
        test.eulerAngles = randomAngle;

        //this.GetComponent<Rigidbody>().AddForce(randomAngle * randomVelocity, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().rotation = test;
        this.GetComponent<Rigidbody>().AddForce(randomAngle * randomVelocity, ForceMode.Impulse);

    }

    public void moveBall()
    {
        Quaternion test = Quaternion.identity;
        test.eulerAngles = initialAngle;

        //this.GetComponent<Rigidbody>().AddForce(randomAngle * randomVelocity, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().rotation = test;
        this.GetComponent<Rigidbody>().AddForce(initialAngle * initialVelocity, ForceMode.Impulse);
    }

    public void calculateFitness()
    {
        if (inHole == true)
        {
            fitness = (4.0f/lifeTime) + 1.0f / (transform.position - goal.transform.position).magnitude;
        }
        else
        {
            fitness = 1.0f / (transform.position - goal.transform.position).magnitude;
            if(transform.position.y < -5)
            {
                fitness /= 4;
            }
        }
    }

    private void checkAlive()
    {
        if (this.GetComponent<Rigidbody>().velocity.magnitude < 1.4f || transform.position.y < -5)
        {
            if (lifeTime > 1.6f)
            {
                if (!isDead)
                    Debug.Log("I died");
                isDead = true;
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "GOAL")
        {
            Debug.Log("Made it in the hole!");
            inHole = true;
        }
    }

    public void resetValues()
    {
        fitness = 0;
        inHole = false;
        isDead = false;
        lifeTime = 0;
    }




}
