using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterManager : MonoBehaviour
{
    public int health;
    public Image[] hearts;
    public Sprite fullhearts;
    public Sprite emptyhearts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i =0; i<hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = fullhearts;
            }
            else
            {
                hearts[i].sprite = emptyhearts;
            }
        }
    }
}
