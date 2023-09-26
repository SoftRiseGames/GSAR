using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRaycastSystem : MonoBehaviour
{
    public LayerMask groundLayer;
    public LineRenderer lineRenderer;
    RaycastHit dedect;
    public Transform pos1;
    public Transform pos2;
    private float counter;
    public float dist;
    [SerializeField] float lineDrawSpeed;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = pos1.gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        counter = 0;
        lineRenderer.SetPosition(0, pos1.position);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(transform.position, transform.position + transform.up * -50, Color.green);
        
        
        if(Physics.Raycast(this.transform.position,transform.TransformDirection(Vector3.up)*-1,out dedect,Mathf.Infinity,groundLayer))
        {
            Debug.Log(dedect.collider.name);
            Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.up)*-1 * dedect.distance, Color.green);
            
            pos2 = dedect.transform;
            dist = Vector3.Distance(pos1.position, pos2.position);
            if (Input.GetMouseButton(0))
            {
                LineRendererSystem();
            }
        }
        else
        {
            pos2 = null;
            counter = 0;
            dist = 0;
            lineRenderer.enabled = false;
        }

        if(pos2 != null)
            LineRendererAnimation();

        Debug.Log(counter);

    }
    public void LineRendererSystem()
    {
        lineRenderer.enabled = true;
    }
    public void LineRendererAnimation()
    {
        if (counter < dist)
        {
           
            counter += .1f / lineDrawSpeed;
            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = pos1.position;
            Vector3 pointB = pos2.position;

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            lineRenderer.SetPosition(1, pointAlongLine);
        }
    }
}
