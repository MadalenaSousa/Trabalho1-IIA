using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resourceAngle;
    public float largura;
    public float centro;

    // Update is called once per frame
    void Update()
    {

        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetGaussianOutput(centro, largura);

        print(resourceValue);

        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards, slow first, then fast, then slow again

    }
}
