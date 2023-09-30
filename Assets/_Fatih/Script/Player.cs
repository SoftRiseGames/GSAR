using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
   
    public int maxHealth = 100;
    public int health;

    public HealthBar healthBar;

    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
       if( Input.GetKeyDown(KeyCode.Space)){
        TakeDamage(20);
       }

       if(health <= 0) {
         Destroy(gameObject);
       }
    }
     
     void TakeDamage (int damage){
        health -= damage;
        healthBar.SetHealth(health);
     }
     
}
