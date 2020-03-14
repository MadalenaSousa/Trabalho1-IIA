using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogaritmicRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float weightBlock;
    public float resourceValue;
    public float resourceAngle;
    public float blockValue;
    public float blockAngle;
    public float logBase;
    public float resource_minX, resource_maxX, resource_minY, resource_maxY;
    public float block_minX, block_maxX, block_minY, block_maxY;

    // Update is called once per frame
    void Update()
    {

        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput(logBase, resource_minX, resource_maxX, resource_minY, resource_maxY);

        blockAngle = blockDetector.GetAngleToClosestObstacle();
        blockValue = weightBlock * blockDetector.GetLogaritmicOutput(logBase, block_minX, block_maxX, block_minY, block_maxY);

        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards, slow first, then fast, then slow again

    }
}

