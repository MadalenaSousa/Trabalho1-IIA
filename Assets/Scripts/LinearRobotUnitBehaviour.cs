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
    public float resource_minX, resource_maxX, resource_minY, resource_maxY;
    public float block_minX, block_maxX, block_minY, block_maxY;

    void Update()
    {
        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLinearOutput(resource_minX, resource_maxX, resource_minY, resource_maxY);

        blockAngle = blockDetector.GetAngleToClosestObstacle();
        blockValue = weightBlock * blockDetector.GetLinearOutput(block_minX, block_maxX, block_minY, block_maxY);

        print(blockAngle);


        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards
        applyForce(blockAngle, blockValue); // afasta-se

    }

}






