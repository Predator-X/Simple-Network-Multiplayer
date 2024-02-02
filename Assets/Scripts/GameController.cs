
using System.Collections.Generic;//****************************************PlayerSetup
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(PlayerManager))]
public class GameController : NetworkBehaviour{
  //  [SerializeField]
   // GameObject playerUIPrefab;
   // private GameObject playerUIinstance;
    [SerializeField]
    Behaviour[] oncilentoff;

    //Camera sceneCamera;

    [SerializeField]
    string layerName = "ClientPlayer";

    private void Start()
    {
       
        if (!isLocalPlayer)
        {
            AssigningLayer();
            DisableComponents();
        }
        else
        {
            GetComponent<PlayerManager>().SetupPlayer();
        }
  
        //Create PlayerUI
    // playerUIinstance   = Instantiate(playerUIPrefab);
        //playerUIinstance.name = playerUIPrefab.name;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager player = GetComponent<PlayerManager>();
        GameManager.RegisterPlayer(netID,player);
    }

    void DisableComponents()
    {
        for (int i = 0; i < oncilentoff.Length; i++)
        {
            oncilentoff[i].enabled = false;
        }
    }

    private void OnDisable()
    {
       // Destroy(playerUIinstance);
       if(isLocalPlayer)
        GameManager.instance.SetingSceneCameraActive(true);

        GameManager.DisRegisterPlayer(transform.name);
    }



    void AssigningLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
/*
 *  RegisterPlayer();
     void RegisterPlayer()
    {
        string ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = ID;
    }
    **********************************************************************************************
    In strat
    sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
          
    In void OnDisable
      if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

 */
