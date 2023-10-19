using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LittleEnemyStates : StateMachineBehaviour
{
    Transform player; // karakter pozisyon eri�imii�in kullan�l�r   
    Rigidbody rb; //rigidbody eri�imi
    public enemyHealthSystem thisEnemy;// d��mana ula��cak olan kod
    [SerializeField] float attackRange; // atak mesafesi
    [SerializeField] float attackraneMax; // uzun mesafeli d��manlar i�in hareket etme mesafesi
   

    [SerializeField] float speed; // hareket h�z�
    bool isMove; // bool
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player").transform; 
        rb = animator.GetComponent<Rigidbody>();
        thisEnemy = animator.GetComponent<enemyHealthSystem>();
        isMove = true;
     
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z); // karakterin pozisyonu al�n�r
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime); // karakter MoveTowards�
        
        if(isMove)
            rb.MovePosition(newPos); // rigidbody ile gidi�i
        
        // mesafe belli bir alandan k���kse 
        if(thisEnemy.enemybig || thisEnemy.enemylittle)
        {
            if (Vector3.Distance(player.position, rb.position) <= attackRange)
            {
                isMove = false;
                animator.SetTrigger("Attack");
            }
        }
        else if (thisEnemy.enemydodge)
        {

        }
        
        else // de�ilse
        {
            isMove = true;
        }
     // not: ismove boolunu a�mam�n sebebi d��ardan kontrol edilebilirli�i daha rahat bir hale getirmek. �u anki ilk halinde gerek olmassa da 
     // d��man i�in bir manajer a�t���m�zda �ok daha kolay olacak.
    }

   
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
