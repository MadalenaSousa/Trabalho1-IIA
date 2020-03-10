using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDetectorScript : MonoBehaviour
{

    public float angleOfSensors = 10f;
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



    public float GetLinearOuput()
    {
        return strength;
    }

    public virtual float GetGaussianOutput(float centro, float largura)
    {
        // YOUR CODE HERE

        strength = (1 / largura * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-(Mathf.Pow(strength - centro, 2) / (2 * Mathf.Pow(largura, 2))));
        
        return strength;
    }

    public virtual float GetLogaritmicOutput()
    {
        // YOUR CODE HERE
        throw new NotImplementedException();
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
