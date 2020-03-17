using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10;
    public float rangeOfSensors = 0.1f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angle;
    public int numObjects;
    public bool debug_mode;
    // Start is called before the first frame update
    void Start()
    {

        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // FixedUpdate is called at fixed intervals of time
    void FixedUpdate()
    {
        ObjectInfo pickup; //crio objeto pickup, que vai ter um angulo e uma distancia
        
        pickup = GetClosestPickup(); //esse objeto vai ser o pickup mais proximo
        
        if (pickup != null) //se existe
        {
            angle = pickup.angle; //guardo o angulo
            strength = 1.0f / (pickup.distance + 1.0f); //formula está no enunciado do projeto, formula da energia
        }
        else
        { // no object detected
            strength = 0;
            angle = 0;
        }
        
    }

    public float GetAngleToClosestResource()
    {
        return angle;
    }



    public float GetLinearOutput(float declive, float outputO, float minX, float maxX, float minY, float maxY) //envia os limites, declive e valor na origem
    {

        if (strength <= minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y
        }
        else if (strength >= maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y
        } 
        else if(strength <= minY)
        {
            return minY;
        } 
        else if(strength >= maxY)
        {
            return maxY;
        } 
        else
        {
            return declive * strength + outputO;
        }
      
    }

    public virtual float GetGaussianOutput(float centro, float largura, float minX, float maxX, float minY, float maxY) //envia o centro do gráfico, a largura, que vai fazer variar a altura, do gráfico e os limites
    {
        
        if (strength < minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y
        }
        else if (strength >= maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y
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
        if (strength <= minX) //se a força é menor que o minimo em X
        {
            return minY; //força fica igual ao minimo em Y
        }
        else if (strength >= maxX) //se a força é maior que o maximo em X
        {
            return minY; //força fica igual ao minimo em Y
        }
        else
        {
            float logaritmicStrength = - Mathf.Log(strength, logBase); //função logaritmica que "filtra" a strength com que vai contra as caixas

            if(logaritmicStrength >= maxY)
            {
                logaritmicStrength = maxY;
            } 
            else if(logaritmicStrength <= minY)
            {
                logaritmicStrength = minY;
            }

            return logaritmicStrength;
        }
    }


    
    public List<ObjectInfo> GetVisibleObjects(string objectTag)
    {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>(); //lista de objetos

        for (int i = 0; i * angleOfSensors <= 360f; i++)
        {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors))
            {

                if (hit.transform.gameObject.CompareTag(objectTag))
                {
                    if (debug_mode)
                    {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }

    public ObjectInfo[] GetVisiblePickups() //retorna array de objetos pickup visiveis
    {
        return (ObjectInfo[]) GetVisibleObjects("Pickup").ToArray();
    }

    public ObjectInfo GetClosestPickup() //retorna objeto pickup visivel mais proximo
    {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Pickup").ToArray();
        if(a.Length == 0)
        {
            return null;
        }
        return a[a.Length-1];
    }


    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);

    }
}
