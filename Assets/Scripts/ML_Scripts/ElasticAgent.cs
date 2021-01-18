using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using UnityEngine.UI;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class ElasticAgent : Agent
{
    GrasshopperInUnity gh;
    bool firstRun = true;

    const float increment = 0.1f;
    float num1, num2;
    float temp1, temp2;
    float act1, act2;
    float area;
    float previousArea;
    float receivedArea;

    float targetArea;
    float tolerance = 0.3f; // 0.000052f; // this corresponds to 0.01 in our original domain //we can make the tolerance smaler and smaller in the curriculum

    public Text currentText;
    public Text targetText;
    public Text rewardText;
    public Text stepText;
    public Text numText;
    public override void Initialize()
    {
        // Debug.Log("Initialize");
        Rhino.Runtime.HostUtils.RegisterNamedCallback("Unity:GetNumber", GetNumber);


        
        gh = new GrasshopperInUnity();
        
    }


    //An agent is an entity that can observe its environment, decide on the best course of action using those observations, 
    //and execute those actions within its environment.

    public override void OnEpisodeBegin() //Called at the beginning of an Agent's episode, including at the beginning of the simulation.
    {
        
        //here we randomize the starting values  and target values for each episode begin

        //Debug.Log("Episode Begin");

        targetArea = UnityEngine.Random.Range(0f, 1f);
        targetText.text = "Target Normalized Area: " + targetArea.ToString();

        

        num1 = UnityEngine.Random.Range(0f, 1f); //we are starting from normalized values otherwise we need to normalize later
        num2 = UnityEngine.Random.Range(0f, 1f);
        num1= (float)Math.Round((double)num1, 1);
        num2= (float)Math.Round((double)num2, 1);
        

        gh.SendNumberToUnity((double)num1, (double)num2);



       
    }
    

    public override void CollectObservations(VectorSensor sensor) // Called every step that the Agent requests a decision. This is one possible way for collecting the Agent's observations of the environment
    {
        //get all the elements that need to keep track of in order to makde decisions

        sensor.AddObservation(num1);
        sensor.AddObservation(num2); // this are normalized from definition

        sensor.AddObservation(targetArea); // target is generated normalized

        //Debug.Log("Observasions Collected");

    }

    public override void OnActionReceived(float[] vectorAction) // Called every time the Agent receives an action to take. Receives the action chosen by the Agent. It is also common to assign a reward in this method.
    {
       
        /// get the values form the action[] and apply them to the mechanism

        // Get the action index for slider1
        int sl1 = Mathf.FloorToInt(vectorAction[0]);
        // Get the action index for slider2
        int sl2 = Mathf.FloorToInt(vectorAction[1]);

        if (sl1 == 0) { num1 += 0; }
        if (sl1 == 1) { num1 += increment; }
        if (sl1 == 2) { num1 -= increment; }

        if (sl2 == 0) { num1 += 0; }
        if (sl2 == 1) { num1 += increment; }
        if (sl2 == 2) { num1 -= increment; }

        numText.text = num1.ToString() + "   " + num2.ToString();
        stepText.text = "Step: " + StepCount.ToString();

        

        if (num1 < 0 || num2 < 0)
        {
            num1 = 0;
            num2 = 0;

            SetReward(-1f);
            EndEpisode();
        }
        if (num1 > 1 || num2 > 1)
        {
            num1 = 1;
            num2 = 1;

            SetReward(-1f);
            EndEpisode();
        }

        gh.SendNumberToUnity((double)num1, (double)num2);

        

        




    }

    public override void Heuristic(float[] actionsOut) // hard code the actions[] for testing before we start learning
    {
        //Debug.Log("Heuristic");

       
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //Debug.Log("sl1-right");
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 2;
        }

        actionsOut[1] = 0;
        if (Input.GetKey(KeyCode.D))
        {
           // Debug.Log("sl2-right");
            actionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[1] = 2;
        }


    }

    void GetNumber(object sender, Rhino.Runtime.NamedParametersEventArgs args)
    {
        double values;
        // Debug.Log("Run");
        if (args.TryGetDouble("num", out values))
        {
            // Debug.Log("Received from Rhino");
            //Get results
            area = (float)values;


            currentText.text = "Current Normalized Area: " + area.ToString();            


            //Set Rewards

           
            AddReward(-1f / MaxStep);

            if (Mathf.Abs(area - targetArea) <= tolerance)
            {
                AddReward(+1f);
                EndEpisode();
            }

            rewardText.text = "Reward: " + GetCumulativeReward().ToString();          


        }

    }
}
