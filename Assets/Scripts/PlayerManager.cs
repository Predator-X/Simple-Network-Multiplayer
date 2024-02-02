using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerManager : NetworkBehaviour {

    // public RectTransform localPlayerCanvas;
    // public Text localPlayerCanvasText;
    [SerializeField]
    public const int maxHealth = 100;
  
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SerializeField]
    private GameObject[] OnDeathDisableGameObjects;
    [SerializeField]
    private GameObject deathEffect;


    [SyncVar]
    public int currentHealth = maxHealth;
    public Canvas HealthBarCanvass;
    public Text LocalPlayerCanvasText;
    public RectTransform LocalPlayerCanvas;


  //  PlayerHealth m;

    //  [SyncVar]
    //  private int currentHealth;

    private bool _isDead = false;
    private bool firstSetup = true;

    private void Start()
    {
            if (isLocalPlayer)
          {
            HealthBarCanvass.GetComponent<Canvas>().enabled = false;
          }
        //HealthBarCanvas = gameObject.GetComponentInChildren<Canvas>();
        if (!isLocalPlayer) { HealthBarCanvass.GetComponent<Canvas>().enabled = true; }

    }

    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }





    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {      
            //switch camera
            GameManager.instance.SetingSceneCameraActive(false);
          
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }

        SetDefaults();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(10);
        }
    }

   [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isDead) { return; }
        currentHealth -= amount;
       GetComponent<PlayerHealth>().OnChangeHealth(currentHealth);
        //  OnChangeHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }



   

    private void Die()
    {
        isDead = true; 
        for (int i = 0; i <disableOnDeath.Length; i++)//Disable components
        {
            disableOnDeath[i].enabled = false;
        }
        for (int i = 0; i < OnDeathDisableGameObjects.Length; i++)//Disable GameObjects
        {
            OnDeathDisableGameObjects[i].SetActive(false);
        }

        Collider _col = GetComponent<Collider>();//Disable collider
        if (_col != null)
        {
            _col.enabled = false;
        }
        //Spawn death effect
        GameObject _graficInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_graficInstance, 1f);
        //switch camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetingSceneCameraActive(true);
        }
        Debug.Log(transform.name + "is Dead!!!");
        StartCoroutine(Respawn());
    }

  
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        
            Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
            transform.position = _spawnPoint.position;
            transform.rotation = _spawnPoint.rotation;
        
      //  yield return new WaitForSeconds(0.1f);

        SetupPlayer();
        Debug.Log(transform.name +" Respawned");
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)//enable components active
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < OnDeathDisableGameObjects.Length; i++)//enable GameObjects
        {
            OnDeathDisableGameObjects[i].SetActive(true);
        }
        //enable collider
        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
            Debug.Log("Got Collider");
        }

    }
}
/*
 *   public void OnChangeHealth(int currentHealt)
    {
       // healthbar.sizeDelta = new Vector2(currentHealth, healthbar.sizeDelta.y);
         LocalPlayerCanvas.sizeDelta = new Vector2(currentHealt + currentHealt, LocalPlayerCanvas.sizeDelta.y);
        // LocalPlayerCanvasText.text = currentHealth + "%";
        if (isLocalPlayer)
        {
            if (LocalPlayerCanvas != null)
            {
            LocalPlayerCanvas.sizeDelta = new Vector3(currentHealt + currentHealt, LocalPlayerCanvas.sizeDelta.y);
            LocalPlayerCanvasText.text = currentHealt + "%";
             }
        }
    }
 
 */
