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
    [Header("MesafeliDusmanlar")]
    [SerializeField] float DashRange; // mesafeli d��manlar i�in ���nlanma mesafesi
    [SerializeField] float MoveRange; // mesafeli d��manlar i�in maksimum vurma mesafesi
    [Header("RangedDashPosition")]
    [SerializeField] GameObject backTransform; // arkaya at�lacak transform noktas�
    [SerializeField] GameObject leftTransform; // sola gidilecek dash noktas�
    [SerializeField] GameObject rightTransorm; //sa�a gidilecek dash noktas�
    
    
    [SerializeField] float speed; // hareket h�z�
    bool isMove; // karakter y�r�y�p y�r�memeyi kontrol etme
    int random = 0; // y�n se�erken random bir index se�me
    bool isDash = true; // dash atarkenki bool

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("player").transform; 
        rb = animator.GetComponent<Rigidbody>();
        thisEnemy = animator.GetComponent<enemyHealthSystem>();
        isMove = true; 
        backTransform = GameObject.Find("backDashTransform");
        leftTransform = GameObject.Find("leftDashTransform");
        rightTransorm = GameObject.Find("rightDashTransform");
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(Vector3.Distance(player.position, rb.position));
        
        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z); // karakterin pozisyonu al�n�r
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime); // karakter MoveTowards�
        thisEnemy.gameObject.transform.LookAt(player);

        if (thisEnemy.isDashCooldown)
            isDash = true; 
        else if (!thisEnemy.isDashCooldown)
            isDash = false;
        //burada d��man�n cooldown verisine g�re isDash boolunu kapat�p a�ar. not: Optimize edilecek.
       
       
        if (thisEnemy.enemyType == enemyTypes.BigEnemy || thisEnemy.enemyType == enemyTypes.littleEnemy) //Karakterin kendi scriptindeki enum tipine g�re davran�� sergiler.
        {
            // bu k�s�m k���k ve b�y�k d��manlar i�in
            if (Vector3.Distance(player.position, rb.position) <= attackRange) // karakter belli bir mesafedeyse
            {
                isMove = false;
                animator.SetTrigger("Attack");
            }

            else // de�ilse
            {
                isMove = true;
            }
        }
        else if (thisEnemy.enemyType == enemyTypes.rangedEnemy) // mesafeli d��manlar i�in
        {
            if(Vector3.Distance(player.position,rb.position) > MoveRange) // belli bir mesafeden fazlaysa y�r�
            {
                Debug.Log("a");
                isMove = true; //d��man�n bize sald�rabilece�i maksimum attack mesafesi i�in
            }
            else if(Vector3.Distance(player.position, rb.position) > DashRange && Vector3.Distance(player.position, rb.position) < MoveRange) // dash mesafesi ile move mesafesi aras�ndaysa y�r�meyi kes ve sald�r.
            {
                isMove = false;
                Debug.Log("attack");
            }
            
            if(Vector3.Distance(player.position, rb.position) < DashRange) // Belli bir mesafenin alt�ndaysa dash at
            {
               
                if (isDash) 
                {
                   randomSystem(1, 4);
                   thisEnemy.isDashCooldown = false;
                   thisEnemy.DashCooldown();
                }
                
                switch (random) // randomdan ��kan de�ere g�re dash at�lacak noktay� se�
                {
                    case 1:
                        thisEnemy.gameObject.transform.DOMove(backTransform.transform.position, .1f);
                        break;
                    case 2:
                        thisEnemy.gameObject.transform.DOMove(rightTransorm.transform.position, .1f);
                        break;
                    case 3:
                        thisEnemy.gameObject.transform.DOMove(leftTransform.transform.position, .1f);
                        break;
                }
            }
        }

        if (isMove)
            rb.MovePosition(newPos); // rigidbody ile gidi�i

        // not: ismove boolunu a�mam�n sebebi d��ardan kontrol edilebilirli�i daha rahat bir hale getirmek. �u anki ilk halinde gerek olmassa da 
        // d��man i�in bir manajer a�t���m�zda �ok daha kolay olacak.
    }

    public int randomSystem(int x, int y) //randoma g�re dash at�lacak mesafeyi se�
    {
        isDash = false;
        random = Random.Range(x, y);
        return random;
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
