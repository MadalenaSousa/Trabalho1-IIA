﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogaritmicRobotUnitBehaviour : RobotUnit
{
    public float weightResource;
    public float resourceValue;
    public float resourceAngle;
    public float logBase;

    // Update is called once per frame
    void Update()
    {

        // get sensor data
        resourceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput(logBase);

        print(resourceValue);

        // apply to the ball
        applyForce(resourceAngle, resourceValue); // go towards, slow first, then fast, then slow again

    }
}

