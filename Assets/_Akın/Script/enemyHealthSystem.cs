using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealthSystem : MonoBehaviour
{
    public CharacterManager character;
    public GameObject player;
    public littleEnemyStates enemyStates;
    [Header("healthVeriables")]
    [SerializeField] int littleEnemyDamage;
    [SerializeField] int bigEnemyDamage;
    
    [Header("HealthTypes")]
    public enemyTypes enemyType;
    public bool isDashCooldown;
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

        //eventsystemde karakterin nerede vurdu�unda ne kadar can g�t�rece�ine dair veri ba�l���
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
        isDashCooldown = true;

    }
    public void Damage() => DamageSys(littleEnemyDamage, bigEnemyDamage); // eventsystemde aktif olarak g�z�kmesi i�in bir voide atanmas� gerek. �a��r�rken damage �zerinden �a��r�lacak.
    public void DashCooldown() => StartCoroutine(wait());
}


public enum enemyTypes
{
    Defult = 0,
    littleEnemy,
    BigEnemy,
    rangedEnemy
}
