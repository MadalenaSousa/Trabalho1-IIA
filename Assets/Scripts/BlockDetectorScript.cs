using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
    public float rangeOfSensors = 10f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angleToClosestObj;
    public int numObjects;
    public bool debugMode;
    // Start is called before the first frame update
    void Start()
    {
        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // YOUR CODE HERE
        ObjectInfo wall;
        wall = GetClosestWall();
        if (wall != null)
        {
            angleToClosestObj = wall.angle;
            strength = 1.0f / (wall.distance + 1.0f);
        }
        else
        { // no object detected
            strength = 0;
            angleToClosestObj = 0;
        }

        //FIM DO CÓDIGO NOVO


    }

    public float GetAngleToClosestObstacle()
    {
        return angleToClosestObj;
    }

    public float GetLinearOuput()
    {
        return strength;
    }

    public virtual float GetGaussianOutput()
    {
        // YOUR CODE HERE

        throw new NotImplementedException();
    }

    public virtual float GetLogaritmicOutput()
    {
        // YOUR CODE HERE

        throw new NotImplementedException();
    }


    public List<ObjectInfo> GetVisibleObstacles(string objectTag)
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>(); //lista de objetos


        for (int i = 0; i * angleOfSensors <= 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debugMode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.blue);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }

    public ObjectInfo[] GetVisibleWalls()
    {
        return (ObjectInfo[])GetVisibleObstacles("Wall").ToArray();
    }

    public ObjectInfo GetClosestWall()
    {
        ObjectInfo[] a = (ObjectInfo[])GetVisibleObstacles("Wall").ToArray();
        if (a.Length == 0)
        {
            return null;
        }
        return a[a.Length - 1];
    }
}
