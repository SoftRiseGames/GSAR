using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRaycastSystem : MonoBehaviour
{
    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(transform.position, transform.position + transform.up * -50, Color.green);
        RaycastHit dedect;
        
        if(Physics.Raycast(this.transform.position,transform.TransformDirection(Vector3.up)*-1,out dedect,Mathf.Infinity,groundLayer))
        {
            Debug.Log(dedect.collider.name);
            Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.up)*-1 * dedect.distance, Color.green);
        }
        
    }
}
