using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    [SerializeField] private Animator geserAnimator;
    [SerializeField] private int comboIndex;
    void Awake()
    {
        geserAnimator = GetComponent<Animator>();
        
    }

    
    void Update()
    {
        
    }

    
}
