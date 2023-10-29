using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthSystem : MonoBehaviour
{
    public CharacterManager character;
    public GameObject player;
    public littleEnemyStates enemyStates;
    public enemyHealthSystem instance;
    public bool isDeath;
    [Header("healthVeriables")]
    [SerializeField] int littleEnemyDamage;
    [SerializeField] int bigEnemyDamage;
    [SerializeField] float health;
    [Header("HealthTypes")]
    public enemyTypes enemyType;
    public bool isDashCooldown;
    public bool isCaptured;
    void Start()
    {
        character = GameObject.Find("GESAR").GetComponent<CharacterManager>();
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        
        if (isDeath)
            Destroy(instance.gameObject);
    }
    public int DamageSys(int dmglittle, int dmgbig)
    {
        if (enemyType == enemyTypes.BigEnemy)
            character.Health = character.Health - dmgbig;
        else if (enemyType == enemyTypes.littleEnemy)
            character.Health = character.Health - dmglittle;
        
        return character.Health;

        //eventsystemde karakterin nerede vurduðunda ne kadar can götüreceðine dair veri baþlýðý
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
        isDashCooldown = true;

    }

    public void Damage() => DamageSys(littleEnemyDamage, bigEnemyDamage); // eventsystemde aktif olarak gözükmesi için bir voide atanmasý gerek. Çaðýrýrken damage üzerinden çaðýrýlacak.
    public void DashCooldown() => StartCoroutine(wait());

    public void DestroySystem()
    {
       
    }
}


public enum enemyTypes
{
    Defult = 0,
    littleEnemy,
    BigEnemy,
    rangedEnemy
}
