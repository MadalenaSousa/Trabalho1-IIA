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

        ObjectInfo wall;
        wall = GetClosestWall();
        
        if (wall != null) //se detectar alguma wall
        {
            angleToClosestObj = wall.angle;
            strength = 1.0f / (wall.distance + 1.0f);
        }
        else
        { // no object detected
            strength = 0;
            angleToClosestObj = 0;
        }

    }

    public float GetAngleToClosestObstacle()  //obtém o ângulo ao bloco mais próximo
    {
        return angleToClosestObj;
    }

    public float GetLinearOutput(float minX, float maxX, float minY, float maxY) //envia os limites
    {
        
        return strength;
    }

    public virtual float GetGaussianOutput(float largura, float centro, float minX, float maxX, float minY, float maxY) //envia o centro do gráfico, a largura, que vai fazer variar a altura, do gráfico e os limites
    {
        //falta por os limites

        float gaussianStrength = (1 / largura * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-(Mathf.Pow(strength - centro, 2) / (2 * Mathf.Pow(largura, 2)))); //função gaussiana que "filtra" a strength com que vai contra as paredes

        return gaussianStrength;
    }

    public virtual float GetLogaritmicOutput(float logBase, float minX, float maxX, float minY, float maxY) //envia a base do logaritmo e os limites
    {
        //falta por os limites

        float logaritmicStrength = -Mathf.Log(strength, logBase); //função logaritmica que "filtra" a strength com que vai contra as paredes

        return logaritmicStrength;
    }


    public List<ObjectInfo> GetVisibleObstacles(string objectTag) //retorna os objetos visíveis
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

    public ObjectInfo[] GetVisibleWalls() //retorna os objetos visíveis com a tag “wall”
    {
        return (ObjectInfo[])GetVisibleObstacles("Wall").ToArray();
    }


    public ObjectInfo GetClosestWall() //retorna o objeto visível com a tag “wall” mais próximo
    {
        ObjectInfo[] a = (ObjectInfo[])GetVisibleObstacles("Wall").ToArray();
        if (a.Length == 0)
        {
            return null;
        }
        return a[a.Length - 1];
    }
}
