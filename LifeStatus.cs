using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStatus : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float getHealth()
    {
        return health;
    }

    public void setHealth(float newHealth)
    {
        health += newHealth;
    }

}
