using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotUnitBehaviourScript : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resourceAngle;
    public float weightBlock;
    public float blockValue;
    public float blockAngle;
    public float largura, centro, altura, logBase;
    public float resource_minX, resource_maxX, resource_minY, resource_maxY;
    public float block_minX, block_maxX, block_minY, block_maxY;
    public string resourceActivationType, blockActivationType;

    // Update is called once per frame
    void Update()
    {
        // get sensor data
        if(resourceActivationType == "linear")
        {
            resourceAngle = resourcesDetector.GetAngleToClosestResource();
            resourceValue = weightResource * resourcesDetector.GetLinearOutput(resource_minX, resource_maxX, resource_minY, resource_maxY);
        } 
        else if(resourceActivationType == "logaritmic")
        {
            resourceAngle = resourcesDetector.GetAngleToClosestResource();
            resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput(logBase, resource_minX, resource_maxX, resource_minY, resource_maxY);
        } 
        else if(resourceActivationType == "gaussian")
        {
            resourceAngle = resourcesDetector.GetAngleToClosestResource();
            resourceValue = weightResource * resourcesDetector.GetGaussianOutput(centro, largura, resource_minX, resource_maxX, resource_minY, resource_maxY);
        }

        if (blockActivationType == "linear")
        {
            blockAngle = blockDetector.GetAngleToClosestObstacle();
            blockValue = weightBlock * blockDetector.GetLinearOutput(block_minX, block_maxX, block_minY, block_maxY);
        }
        else if (blockActivationType == "logaritmic")
        {
            blockAngle = blockDetector.GetAngleToClosestObstacle();
            blockValue = weightBlock * blockDetector.GetLogaritmicOutput(logBase, block_minX, block_maxX, block_minY, block_maxY);
        }
        else if (blockActivationType == "gaussian")
        {
            blockAngle = blockDetector.GetAngleToClosestObstacle();
            blockValue = weightBlock * blockDetector.GetGaussianOutput(altura, centro, largura, block_minX, block_maxX, block_minY, block_maxY);
        }

        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards, slow first, then fast, then slow again
        applyForce(blockAngle, blockValue); // go towards, slow first, then fast, then slow again
    }
}
