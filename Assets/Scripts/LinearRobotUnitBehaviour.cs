using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float weightBlock; //valor negativo <-> D3I afasta-se do obstáculo
    public float resourceValue;
    public float resourceAngle;
    public float blockValue;
    public float blockAngle;

    void Update()
    {
        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetGaussianOutput(5, 10);

        blockAngle = blockDetector.GetAngleToClosestObstacle();
        blockValue = weightBlock * blockDetector.GetLinearOuput();

        print(blockAngle);


        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards
        applyForce(blockAngle, blockValue); // afasta-se

    }

}






