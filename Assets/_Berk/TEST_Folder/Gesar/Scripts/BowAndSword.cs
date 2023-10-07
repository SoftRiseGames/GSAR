using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAndSword : MonoBehaviour
{
    public GameObject sword, bow,arrow;

    
    [SerializeField] private float arrowSpeed;

    public void SwordEnable()
    {
        bow.SetActive(false);
        sword.SetActive(true);
    }
    public void BowEnable()
    {
        bow.SetActive(true);
        sword.SetActive(false);
    }
    public void ArrowEnable()
    {
        arrow.SetActive(true);
    }
    public void ArrowDisable()
    {
        arrow.SetActive(false);
    }
    public void ArrowClone()
    {
        GameObject cloneArrow = Instantiate(arrow, transform.position, transform.rotation);
        Rigidbody rbArrow = cloneArrow.GetComponent<Rigidbody>();
        rbArrow.velocity += transform.forward * arrowSpeed;
    }
}
