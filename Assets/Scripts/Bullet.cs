﻿using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<PlayerHealth>();
        if(health != null)
        {
            health.TakeDamage(10);
        }
        Destroy(gameObject);
    }

}
