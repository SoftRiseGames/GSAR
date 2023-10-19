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
    public bool enemybig;
    public bool enemylittle;
    public bool enemydodge;

    void Start()
    {
        character = GameObject.Find("player").GetComponent<CharacterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int DamageSys(int dmglittle, int dmgbig)
    {
        if (enemybig)
            character.Health = character.Health - dmgbig;
        else if (enemylittle)
            character.Health = character.Health - dmglittle;
        return character.Health;
    }
    public void Damage() => DamageSys(littleEnemyDamage, bigEnemyDamage);
}
