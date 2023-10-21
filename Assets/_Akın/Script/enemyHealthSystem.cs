using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthSystem : MonoBehaviour
{
    public CharacterManager character;
    public GameObject player;
    [Header("healthVeriables")]
    [SerializeField] int littleEnemyDamage;
    [SerializeField] int bigEnemyDamage;
    [Header("HealthTypes")]
    public enemyTypes enemyType;
   
    void Start()
    {
        character = GameObject.Find("player").GetComponent<CharacterManager>();
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
    public void Damage() => DamageSys(littleEnemyDamage, bigEnemyDamage); // eventsystemde aktif olarak gözükmesi için bir voide atanmasý gerek. Çaðýrýrken damage üzerinden çaðýrýlacak.
}
public enum enemyTypes
{
    Defult = 0,
    littleEnemy,
    BigEnemy,
    rangedEnemy
}
