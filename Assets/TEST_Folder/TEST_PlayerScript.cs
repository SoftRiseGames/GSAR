using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;               // Karakter rigidbody
    [SerializeField] private Animator animator;          // Karakter animator
    [SerializeField] private float moveSpeed = 5f;       // Hareket h�z�
    [SerializeField] private float rotationSpeed = 10f;  // D�n�� h�z�

    

    private enum Behaviors {Waiting,Walking,BowUp,BowDown,Roll,Attack}    // Davran��lar� temsil eden enum 

    [SerializeField] private Behaviors behaviors= Behaviors.Waiting;        // Karakterin mevcut davran���n� temsil eden enum de�i�ken

    private void Update()
    {
        Move();                      // Karakteri hareket ettiren fonksiyon.
        Rotate();                    // Karakterin d�n���n� kontrol eden fonksiyon.
        AnimControl();               // Karakterin animasyon kontrol�n� sa�layan fonksiyon.
        
        
    }
    
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");          // Yatay (sol-sa�) giri� de�eri.
        float verticalInput = Input.GetAxis("Vertical");              // Dikey (ileri-geri) giri� de�eri.

        
        // Hareket vekt�r� ile karakteri ileri, geri, sola veya sa�a hareket ettirme.
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized * moveSpeed * Time.deltaTime; 
        rb.MovePosition(rb.position + movement);

        // E�er hareket var ise , karakter Walking durumuna ge�er.
        if (horizontalInput != 0 || verticalInput != 0)          
        {
            
            behaviors = Behaviors.Walking;      

        }
        // E�er hareket yok ise ne durumda oldu�unu kontrol etmek i�in fonksiyona bakar.
        else
        {
            BowControl();
            SwordControl();
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            behaviors = Behaviors.Roll;
        }

    
        
        
    }
    private void Rotate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");        // Yatay (sol-sa�) giri� de�eri.
        float verticalInput = Input.GetAxis("Vertical");            // Dikey (ileri-geri) giri� de�eri.

        if (horizontalInput != 0 || verticalInput != 0) 
        {
            // Karakterin yeni d�n�� rotasyonunu hesaplar ve bu rotasyona d�nd�r�r.
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0f, verticalInput)); 
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);  
        }
    }
    
    private void BowControl()
    {
        

        if (Input.GetMouseButton(1))            //E�er mouse sa� t�k bas�l� ise BowUp duruma ge�er.
        {
            behaviors = Behaviors.BowUp;
        }
        else if (Input.GetMouseButtonUp(1))     //Mouse bas�l� tutulmay� b�rakt���nda BowDown duruma ge�er.
        {

            behaviors = Behaviors.BowDown;
        }
        else                                    //Hi�bir hareket yok ise Waiting duruma ge�er.
        {
            behaviors = Behaviors.Waiting;
        }


    }
    
    private void SwordControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            behaviors = Behaviors.Attack;

        }
        
    }
    private void AttackComboSystem()
    {
        if (Input.GetMouseButtonDown(0) && behaviors==Behaviors.Attack && animator.GetInteger("Attack")==0)
        {
            animator.SetInteger("Attack", 1);
        }
        else if (Input.GetMouseButtonDown(0) && behaviors == Behaviors.Attack && animator.GetInteger("Attack") == 1)
        {
            animator.SetInteger("Attack", 2);
        }
        else if (Input.GetMouseButtonDown(0) && behaviors == Behaviors.Attack && animator.GetInteger("Attack") == 2)
        {
            animator.SetInteger("Attack", 3);
        }
       
        

    }
    private void AttackFinished()
    {
        animator.SetInteger("Attack", 0);
    }
    private void AnimControl()
    {
        
        switch (behaviors)    // Karakterin davran���na g�re animasyonlar� kontrol eder.
        {
            case Behaviors.Walking:         
                animator.SetBool("Walking", true);       // Walking durumundayken "Walking" animasyonunu �al��t�r�r.
                animator.SetBool("Waiting", false);      
                animator.SetBool("BowUp", false);
                animator.SetBool("BowDown", false);
                break;
            
            case Behaviors.Waiting:
                animator.SetBool("Walking", false);      // Waiting durumundayken "Walking" animasyonunu durdurur.
                animator.SetBool("Waiting", true);
                animator.SetBool("BowUp", false);
                animator.SetBool("Roll", false);
                animator.SetBool("BowDown", false);
                break;
            case Behaviors.BowUp:
                animator.SetBool("BowUp", true);           // BowUp durumundayken "Bow" animasyonunu �al��t�r�r.
                animator.SetBool("Waiting", false);
                animator.SetInteger("Attack", 0);
                break;
            case Behaviors.BowDown:
                animator.SetBool("BowDown", true);       // BowDown durumundayken "BowDown" animasyonunu �al��t�r�r.
                animator.SetBool("BowUp", false);
                break;
            case Behaviors.Roll:
                animator.SetBool("Roll", true);
                animator.SetBool("Waiting", false);
                break;
            case Behaviors.Attack:
                AttackComboSystem();
                animator.SetBool("BowUp", false);
                break;
        }

    }
   
}
