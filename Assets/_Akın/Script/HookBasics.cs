using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBasics : MonoBehaviour
{
    [SerializeField] Transform raycastPointer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, raycastPointer.transform.position, Color.green);
    }
}
