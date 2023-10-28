using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LittleEnemyStates : StateMachineBehaviour
{
    Transform player; // karakter pozisyon erişimiiçin kullanılır   
    Rigidbody rb; //rigidbody erişimi
    public enemyHealthSystem thisEnemy;// düşmana ulaşıcak olan kod
    [SerializeField] float attackRange; // atak mesafesi
    [Header("MesafeliDusmanlar")]
    [SerializeField] float DashRange; // mesafeli düşmanlar için ışınlanma mesafesi
    [SerializeField] float MoveRange; // mesafeli düşmanlar için maksimum vurma mesafesi
    [Header("RangedDashPosition")]
    [SerializeField] GameObject backTransform; // arkaya atılacak transform noktası
    [SerializeField] GameObject leftTransform; // sola gidilecek dash noktası
    [SerializeField] GameObject rightTransorm; //sağa gidilecek dash noktası
    
    
    [SerializeField] float speed; // hareket hızı
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
        
        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z); // karakterin pozisyonu alınır
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime); // karakter MoveTowardsı
        thisEnemy.gameObject.transform.LookAt(player);

        if (thisEnemy.isDashCooldown)
            isDash = true; 
        else if (!thisEnemy.isDashCooldown)
            isDash = false;
        //burada düşmanın cooldown verisine göre isDash boolunu kapatıp açar. not: Optimize edilecek.
       
       
        if (thisEnemy.enemyType == enemyTypes.BigEnemy || thisEnemy.enemyType == enemyTypes.littleEnemy) //Karakterin kendi scriptindeki enum tipine göre davranış sergiler.
        {
            // bu kısım küçük ve büyük düşmanlar için
            if (Vector3.Distance(player.position, rb.position) <= attackRange) // karakter belli bir mesafedeyse
            {
                isMove = false;
                animator.SetTrigger("Attack");
            }

            else // değilse
            {
                isMove = true;
            }
        }
        else if (thisEnemy.enemyType == enemyTypes.rangedEnemy) // mesafeli düşmanlar için
        {
            if(Vector3.Distance(player.position,rb.position) > MoveRange) // belli bir mesafeden fazlaysa yürü
            {
                Debug.Log("a");
                isMove = true; //düşmanın bize saldırabileceği maksimum attack mesafesi için
            }
            else if(Vector3.Distance(player.position, rb.position) > DashRange && Vector3.Distance(player.position, rb.position) < MoveRange) // dash mesafesi ile move mesafesi arasındaysa yürümeyi kes ve saldır.
            {
                isMove = false;
                Debug.Log("attack");
            }
            
            if(Vector3.Distance(player.position, rb.position) < DashRange) // Belli bir mesafenin altındaysa dash at
            {
               
                if (isDash) 
                {
                   randomSystem(1, 4);
                   thisEnemy.isDashCooldown = false;
                   thisEnemy.DashCooldown();
                }
                
                switch (random) // randomdan çıkan değere göre dash atılacak noktayı seç
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
            rb.MovePosition(newPos); // rigidbody ile gidişi

        // not: ismove boolunu açmamın sebebi dışardan kontrol edilebilirliği daha rahat bir hale getirmek. Şu anki ilk halinde gerek olmassa da 
        // düşman için bir manajer açtığımızda çok daha kolay olacak.
    }

    public int randomSystem(int x, int y) //randoma göre dash atılacak mesafeyi seç
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
