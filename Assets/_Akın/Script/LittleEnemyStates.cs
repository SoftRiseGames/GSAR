using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LittleEnemyStates : StateMachineBehaviour
{
    Transform player; // karakter pozisyon eriþimiiçin kullanýlýr   
    Rigidbody rb; //rigidbody eriþimi
    public enemyHealthSystem thisEnemy;// düþmana ulaþýcak olan kod
    [SerializeField] float attackRange; // atak mesafesi
    [SerializeField] float attackraneMax; // uzun mesafeli düþmanlar için hareket etme mesafesi
   

    [SerializeField] float speed; // hareket hýzý
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
        
        
        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z); // karakterin pozisyonu alýnýr
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime); // karakter MoveTowardsý
        
        if(isMove)
            rb.MovePosition(newPos); // rigidbody ile gidiþi
        
        // mesafe belli bir alandan küçükse 
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
        
        else // deðilse
        {
            isMove = true;
        }
     // not: ismove boolunu açmamýn sebebi dýþardan kontrol edilebilirliði daha rahat bir hale getirmek. Þu anki ilk halinde gerek olmassa da 
     // düþman için bir manajer açtýðýmýzda çok daha kolay olacak.
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
