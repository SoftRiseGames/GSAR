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
    [Header("MesafeliDusmanlar")]
    [SerializeField] float DashRange; // mesafeli düþmanlar için ýþýnlanma mesafesi
    [SerializeField] float MoveRange; // mesafeli düþmanlar için maksimum vurma mesafesi
    [Header("RangedDashPosition")]
    [SerializeField] GameObject backTransform; // arkaya atýlacak transform noktasý
    [SerializeField] GameObject leftTransform; // sola gidilecek dash noktasý
    [SerializeField] GameObject rightTransorm; //saða gidilecek dash noktasý
    
    
    [SerializeField] float speed; // hareket hýzý
    bool isMove; // karakter yürüyüp yürümemeyi kontrol etme
    int random = 0; // yön seçerken random bir index seçme
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
        
        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z); // karakterin pozisyonu alýnýr
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime); // karakter MoveTowardsý
        thisEnemy.gameObject.transform.LookAt(player);

        if (thisEnemy.isDashCooldown)
            isDash = true; 
        else if (!thisEnemy.isDashCooldown)
            isDash = false;
        //burada düþmanýn cooldown verisine göre isDash boolunu kapatýp açar. not: Optimize edilecek.
       
       
        if (thisEnemy.enemyType == enemyTypes.BigEnemy || thisEnemy.enemyType == enemyTypes.littleEnemy) //Karakterin kendi scriptindeki enum tipine göre davranýþ sergiler.
        {
            // bu kýsým küçük ve büyük düþmanlar için
            if (Vector3.Distance(player.position, rb.position) <= attackRange) // karakter belli bir mesafedeyse
            {
                isMove = false;
                animator.SetTrigger("Attack");
            }

            else // deðilse
            {
                isMove = true;
            }
        }
        else if (thisEnemy.enemyType == enemyTypes.rangedEnemy) // mesafeli düþmanlar için
        {
            if(Vector3.Distance(player.position,rb.position) > MoveRange) // belli bir mesafeden fazlaysa yürü
            {
                Debug.Log("a");
                isMove = true; //düþmanýn bize saldýrabileceði maksimum attack mesafesi için
            }
            else if(Vector3.Distance(player.position, rb.position) > DashRange && Vector3.Distance(player.position, rb.position) < MoveRange) // dash mesafesi ile move mesafesi arasýndaysa yürümeyi kes ve saldýr.
            {
                isMove = false;
                Debug.Log("attack");
            }
            
            if(Vector3.Distance(player.position, rb.position) < DashRange) // Belli bir mesafenin altýndaysa dash at
            {
               
                if (isDash) 
                {
                   randomSystem(1, 4);
                   thisEnemy.isDashCooldown = false;
                   thisEnemy.DashCooldown();
                }
                
                switch (random) // randomdan çýkan deðere göre dash atýlacak noktayý seç
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
            rb.MovePosition(newPos); // rigidbody ile gidiþi

        // not: ismove boolunu açmamýn sebebi dýþardan kontrol edilebilirliði daha rahat bir hale getirmek. Þu anki ilk halinde gerek olmassa da 
        // düþman için bir manajer açtýðýmýzda çok daha kolay olacak.
    }

    public int randomSystem(int x, int y) //randoma göre dash atýlacak mesafeyi seç
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
