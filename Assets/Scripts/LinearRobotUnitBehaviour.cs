using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resourceAngle;
    public float blockValue;
    public float blockAngle;

    public float blockValue;
    public float blockAngle;

    void Update()
    {
        // get sensor data

        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLinearOuput();

        blockAngle = blockDetector.GetAngleToClosestObstacle();
        blockValue = weightResource * blockDetector.GetLinearOuput();

        print(blockAngle);


        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards
        applyForce(blockAngle, -blockValue);

    }

}






