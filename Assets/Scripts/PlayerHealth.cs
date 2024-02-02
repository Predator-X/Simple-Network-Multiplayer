using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerHealth : NetworkBehaviour {
    public const int maxHealth = 100;
    // [SyncVar hook] is link a function to the SyncVar and all clients when value of SyncVar changes (link to a server)
    //([SyncVAr] Network synchronization , THIS MAKES current health system network aware and working under Server Authority)
    [SyncVar(hook = "OnChangeHealth")]  
    public int currentHealth = maxHealth;
    public RectTransform healthbar;
   public Canvas HealthBarCanvas;
    public bool destroyOnDeath;
    public RectTransform localPlayerCanvas;
    public Text localPlayerCanvasText;

    private NetworkStartPosition[] spawnPoints;

    private void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            HealthBarCanvas.GetComponent<Canvas>().enabled = false;
        }

        //HealthBarCanvas = gameObject.GetComponentInChildren<Canvas>();
         if (!isLocalPlayer) { HealthBarCanvas.GetComponent<Canvas>().enabled = true; }

      
    }

    public void TakeDamage(int amount)
    {
        if (!isServer) { return; }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {  // it take damage check destroy on death when current health reaches 0 before respawning 
            if (destroyOnDeath) 
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
    }
  
   public void OnChangeHealth(int currentHealth)
    {
        healthbar.sizeDelta = new Vector2(currentHealth, healthbar.sizeDelta.y);

        if (isLocalPlayer)
        {
            if (localPlayerCanvas != null)
            {
                localPlayerCanvas.sizeDelta = new Vector3(currentHealth + currentHealth, localPlayerCanvas.sizeDelta.y);
               localPlayerCanvasText.text = currentHealth + "%";
            }
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
           Vector3 spawnPoint = Vector3.zero; // set spawnPoint to orginal default value
            if(spawnPoints != null && spawnPoints.Length>0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            transform.position = spawnPoint;

        }
    }

    public int getPlayerHealth()
    {
        return currentHealth;
    }
    public void setPlayerHealth(int hIN)
    {
        currentHealth = hIN;
    }
    public int getPlayerMaxHealth()
    {
        return maxHealth;
    }
}
