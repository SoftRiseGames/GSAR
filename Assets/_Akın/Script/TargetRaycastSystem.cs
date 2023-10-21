using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TargetRaycastSystem : MonoBehaviour
{
    public LayerMask groundLayer;
    public LineRenderer lineRenderer;
    RaycastHit dedect;
    [SerializeField] GameObject character;
    public bool characterMovement;
    public Transform pos1;
    public Transform pos2;
    private float counter;
    public float dist;
    public bool isGo;
    [SerializeField] float lineDrawSpeed;
    // Start is called before the first frame update
    void Start()
    {

        lineRenderer = pos1.gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        counter = 0;
        characterMovement = true;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(transform.position, transform.position + transform.up * -50, Color.green);
        lineRenderer.SetPosition(0, pos1.position);

        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.up) * -1, out dedect, Mathf.Infinity, groundLayer))
        {
          
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * -1 * dedect.distance, Color.green);

            pos2 = dedect.transform;
            dist = Vector3.Distance(pos1.position, pos2.position);
            if (Input.GetMouseButtonDown(0))
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
            isGo = false;
        }

        if (pos2 != null)
            LineRendererAnimation();

        //Debug.Log(counter);


    }
    public void LineRendererSystem()
    {
        lineRenderer.enabled = true;
        isGo = true;
    }

    public void LineRendererAnimation()
    {
        if (counter < dist && isGo == true)
        {


            counter += .1f / lineDrawSpeed*Time.deltaTime;
            float x = Mathf.Lerp(0, dist, counter);
            characterMovement = false;
            Vector3 pointA = pos1.position;
            Vector3 pointB = pos2.position;

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            lineRenderer.SetPosition(1, pointAlongLine);

            if (x == dist)
            {
                character.transform.DOMove(dedect.transform.position, 1).OnComplete(() => { characterMovement = true; });

                Debug.Log("touch");
                isGo = false;
            }
        }


    }

}