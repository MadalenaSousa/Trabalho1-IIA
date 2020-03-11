using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resourceAngle;
    public float largura, centro;
    public float resource_minX, resource_maxX, resource_minY, resource_maxY;
    public float block_minX, block_maxX, block_minY, block_maxY;

    // Update is called once per frame
    void Update()
    {

        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetGaussianOutput(centro, largura, resource_minX, resource_maxX, resource_minY, resource_maxY);

        print(resourceValue);

        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards, slow first, then fast, then slow again

    }
}
