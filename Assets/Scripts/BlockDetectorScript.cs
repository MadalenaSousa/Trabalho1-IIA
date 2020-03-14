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
        //FALTA limites no Y, testar negativos e se ser < ou <= é diferente e qual o mais correto 

        if (strength <= minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else if (strength >= maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else
        {
            return strength;
        }
    }

    public virtual float GetGaussianOutput(float largura, float centro, float minX, float maxX, float minY, float maxY) //envia o centro do gráfico, a largura, que vai fazer variar a altura, do gráfico e os limites
    {
        //FALTA testar negativos e se ser < ou <= é diferente e qual o mais correto

        if (strength < minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else if (strength >= maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else
        {

            float gaussianStrength = Mathf.Pow((float)Math.E, -(Mathf.Pow(strength - centro, 2) / (2 * Mathf.Pow(largura, 2)))); //função gaussiana que "filtra" a strength com que vai contra as caixas

            if (gaussianStrength >= maxY)
            {
                gaussianStrength = maxY;
            }
            else if (gaussianStrength < minY)
            {
                gaussianStrength = minY;
            }

            return gaussianStrength;
        }
    }

    public virtual float GetLogaritmicOutput(float logBase, float minX, float maxX, float minY, float maxY) //envia a base do logaritmo e os limites
    {
        //FALTA testar negativos e se ser < ou <= é diferente e qual o mais correto

        if (strength < minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else if (strength > maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y, que é 0 quando não definido
        }
        else
        {
            float logaritmicStrength = -Mathf.Log(strength, logBase); //função logaritmica que "filtra" a strength com que vai contra as caixas

            if (logaritmicStrength >= maxY)
            {
                logaritmicStrength = maxY;
            }
            else if (logaritmicStrength <= minY)
            {
                logaritmicStrength = minY;
            }

            return logaritmicStrength;
        }
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
