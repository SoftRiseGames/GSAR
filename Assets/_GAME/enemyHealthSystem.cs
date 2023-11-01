using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthSystem : MonoBehaviour
{
    public CharacterManager character;
    public GameObject player;
    public littleEnemyStates enemyStates;
    public enemyHealthSystem instance;
   
    [Header("healthVeriables")]
    [SerializeField] int littleEnemyDamage;
    [SerializeField] int bigEnemyDamage;
    [SerializeField] float health;
    [Header("HealthTypes")]
    public enemyTypes enemyType;
    public bool isDashCooldown;
    public bool isCaptured;
    public float Distance;
    public bool isGroundCollide;
    public bool isConnect;
    void Start()
    {
        character = GameObject.Find("GESAR").GetComponent<CharacterManager>();
        if (instance == null)
            instance = this;

        BaseStartDistance();
    }

    private void Update()
    {
        
       
    }
    public int DamageSys(int dmglittle, int dmgbig)
    {
       if (enemyType == enemyTypes.BigEnemy)
           character.health = character.health - dmgbig;
       else if (enemyType == enemyTypes.littleEnemy)
            character.health = character.health - dmglittle;
       
        return character.health;

        //eventsystemde karakterin nerede vurduðunda ne kadar can götüreceðine dair veri baþlýðý
    }
    public void BaseStartDistance()
    {
        if (enemyType == enemyTypes.BigEnemy)
            Distance = 3;
        else if (enemyType == enemyTypes.littleEnemy)
            Distance = 3;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
        isDashCooldown = true;

    }

    public void Damage() => DamageSys(littleEnemyDamage, bigEnemyDamage); // eventsystemde aktif olarak gözükmesi için bir voide atanmasý gerek. Çaðýrýrken damage üzerinden çaðýrýlacak.
    public void DashCooldown() => StartCoroutine(wait());

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Cevre")
        {
            isGroundCollide = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Cevre")
        {
            isGroundCollide = true;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Cevre")
        {
            isGroundCollide = false;
        }
    }

}


public enum enemyTypes
{
    Defult = 0,
    littleEnemy,
    BigEnemy,
    rangedEnemy
}
