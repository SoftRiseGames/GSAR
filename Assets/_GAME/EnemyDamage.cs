using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public enemyHealthSystem thisEnemy;
    private void Start()
    {
        if (thisEnemy == null)
            thisEnemy = this.gameObject.transform.parent.GetComponent<enemyHealthSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "GESAR")
        {
            thisEnemy.Damage();
        }
    }
}
