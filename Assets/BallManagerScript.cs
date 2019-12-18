using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManagerScript : MonoBehaviour
{

    public GameObject ball;

    public GameObject ballsLeft;

    public GameObject textGen;

   /*HUGE PROBLEM. WHEN WE ADD IN  GOLF BALL, WE HAVE TO MAKE IT A NEW ONE*/
   //We tried a quick fix last night, idk if works, see line 121..

    List<GameObject> golfBalls;

    List<GameObject> nextGen;

    public int ballsPerGen = 50;
    public int currentGen = 0;
    float totalFitnessSum = 0f;
    int bestBallIndex = 0; //the best ball from previous generation

    public bool intelligentMutation = false;

    float mutationOffset = 0.0f;

    //set up golf balls and randomize movement for first time
    void Start()
    {
        ballsLeft = GameObject.Find("NumberText");
        textGen = GameObject.Find("CurrentGenText");
    }

    public void assignValues(int newBallsPerGen, float newMutation)
    {
        golfBalls = new List<GameObject>();
        nextGen = new List<GameObject>();

        mutationOffset = newMutation;
        ballsPerGen = newBallsPerGen;

    }

    public void realStart()
    {
        for (int y = 0; y < ballsPerGen; y++)
        {

            GameObject tempball = Instantiate(ball, new Vector3(4, 0.4f, 0), Quaternion.identity);

            golfBalls.Add(tempball);

            if (currentGen == 0)
            {
                tempball.GetComponent<BallScript>().randomizeMovement();
            }
        }
    }

    //check for dead balls + control genetic algorithm 
    void Update()
    {
        int deadCounter = 0;
        foreach (GameObject obj in golfBalls)
        {
            if (obj.GetComponent<BallScript>().isDead)
            {
                deadCounter++;
            }
        }

        ballsLeft.GetComponent<Text>().text = (ballsPerGen - deadCounter).ToString();

        if (deadCounter == ballsPerGen)
        {
            Debug.Log("All balls stopped. next gen time");
            calculateFitnessSum(golfBalls);
            Debug.Log("Fitness sum calculated");
            naturalSelection();
            Debug.Log("Natural selection completed");
            applyMutation();
            Debug.Log("Mutation applied");
            resetBalls();
            Debug.Log("Balls reset");
            deadCounter = 0;
            totalFitnessSum = 0;
            currentGen++;
            textGen.GetComponent<Text>().text = currentGen.ToString();
            bestBallIndex = 0;
        }
        
    }

    void resetBalls()
    {
        Debug.Log(golfBalls.ToString());

        foreach (GameObject obj in golfBalls)
        {
            Destroy(obj);
        }
            golfBalls.Clear();

        //BUG AFRER THIS
        moveList();


        foreach(GameObject obj in golfBalls)
        {
            obj.transform.position = new Vector3(4, 0.4f, 0);
            obj.GetComponent<BallScript>().resetValues();
            obj.GetComponent<BallScript>().moveBall();
        }
    }
 

    //apply natural selection and fill up new list
    private void naturalSelection()
    {

        for(int x = 1; x < golfBalls.Count; x++)
        {
            //GameObject test = selectChild();

            //BELOW IS WRONG BUT IM GOING TO TRY A QUICK FIX, THIS IS WHAT WE ARE AT FROM LAST NIGHT
            //nextGen.Add(selectChild());
            GameObject temp = Instantiate(selectChild());
            nextGen.Add(temp);

            //May be working?? We just need to fix mutation and select best candidate...
        }

        //add best child here
        GameObject temp2 = Instantiate(golfBalls[bestBallIndex]);
        nextGen.Add(temp2);

    }

    //pick a child based on their fitness value
    //adds up all the fitness values and marks a random point in it
    //keeps going through the array until that value is exceeded, and whatever dot did it is added
    private GameObject selectChild()
    {
        float randomVal = Random.Range(0f, totalFitnessSum);

        float tempsum = 0f;

        for (int x = 0; x < golfBalls.Count; x++)
        {

            golfBalls[x].GetComponent<BallScript>().calculateFitness(); //may be removable?
            tempsum += golfBalls[x].GetComponent<BallScript>().fitness;

            if(tempsum > randomVal)
            {
                return golfBalls[x];
            }
        }

        Debug.Log("Random value was: " + randomVal.ToString() + " and the tempSum was " + tempsum.ToString() + " out of " + golfBalls.Count.ToString() + " golf balls.");
        return null; //uh oh! stinky!
    }

    //apply mutation 
    private void applyMutation()
    {
        foreach (GameObject obj in nextGen)
        {
            /*
            if (obj.GetComponent<BallScript>().inHole)
            {
                obj.GetComponent<BallScript>().initialAngle.x += Random.Range(-1, 1);
                obj.GetComponent<BallScript>().initialAngle.z += Random.Range(-1, 1);
                obj.GetComponent<BallScript>().initialVelocity += Random.Range(-0.25f, 0.25f);
            }
            else
            {
                obj.GetComponent<BallScript>().initialAngle.x += Random.Range(-10, 10);
                obj.GetComponent<BallScript>().initialAngle.z += Random.Range(-10, 10);
                obj.GetComponent<BallScript>().initialVelocity += Random.Range(-3, 3);
            }
            */
            /*
            obj.GetComponent<BallScript>().initialAngle.x += Random.Range(-5, 5);
            obj.GetComponent<BallScript>().initialAngle.z += Random.Range(-5, 5);
            obj.GetComponent<BallScript>().initialVelocity += Random.Range(-2, 2);
            */

            if(intelligentMutation == true && obj.GetComponent<BallScript>().inHole)
            {
                Debug.Log("IT WORKS");
                obj.GetComponent<BallScript>().initialAngle.x += Random.Range(-mutationOffset * 0.1f, mutationOffset * 0.1f);
                obj.GetComponent<BallScript>().initialAngle.z += Random.Range(-mutationOffset * 0.1f, mutationOffset * 0.1f);
                obj.GetComponent<BallScript>().initialVelocity += Random.Range(-mutationOffset * 0.05f, mutationOffset * 0.075f);
            }
            else
            {
                obj.GetComponent<BallScript>().initialAngle.x += Random.Range(-mutationOffset, mutationOffset);
                obj.GetComponent<BallScript>().initialAngle.z += Random.Range(-mutationOffset, mutationOffset);
                obj.GetComponent<BallScript>().initialVelocity += Random.Range(-mutationOffset / 3, mutationOffset / 3);
            }


        }
    }

    //get the total fitness values of a list of golf balls
    //also find the best scoring ball and save it
    private void calculateFitnessSum(List<GameObject> list)
    {


        int count = 0;

        float champHighscore = 0f;
        int champIndex = 0;

        foreach (GameObject obj in golfBalls)
        {
            obj.GetComponent<BallScript>().calculateFitness();
            totalFitnessSum += obj.GetComponent<BallScript>().fitness;

            if(obj.GetComponent<BallScript>().fitness > champHighscore)
            {

                //get best ball
                champHighscore = obj.GetComponent<BallScript>().fitness;
                champIndex = count;
            }
            count++;
        }
    }

    //BUG IS HERE WHEN FINISHED WITH SECOND GENERATION.
    //deep copy one list to another and clears old one
    private void moveList()
    {
        foreach (GameObject obj in nextGen)
            golfBalls.Add(obj);
        nextGen.Clear();
    }

    public void resetAll()
    {

        foreach (GameObject obj in golfBalls)
        {
            Destroy(obj);
        }
        golfBalls.Clear();

        foreach (GameObject obj in nextGen)
        {
            Destroy(obj);
        }
        nextGen.Clear();

        //ballsPerGen = 200;
        currentGen = 0;
        totalFitnessSum = 0f;
        bestBallIndex = 0; //the best ball from previous generation


}
}
