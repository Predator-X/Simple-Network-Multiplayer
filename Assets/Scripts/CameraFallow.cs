
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CameraFallow : NetworkBehaviour {
    public const int PlayerMaxHealth = 100;

    // [SyncVar(hook ="OnPlayerHealthChange")]
    // [SyncVar(hook ="RpcOnPlayerHealthChange")]

    public int PlayerCurrentHealth;
  
    public RectTransform PlayerHealthBarRectTranslate;
    public Transform PlayerTransform;
    public int depth = -5;
    public int height = 3;
    public int width = 0;

    int playerHealth;
    int playerMaxHealth;

    Vector3 offset;
    Transform cameraPosition;
       public GameObject[] Player;
    private void Start()
    {
        //if (isLocalPlayer) { return; }
    }

    private void Update()
    {
        if(PlayerTransform != null)
        {
           
            gameObject.transform.parent = PlayerTransform;
            gameObject.transform.localPosition = new Vector3(width, height, depth);
            //PlayerTransform.GetComponent<PlayerHealth>().getPlayerHealth()+"%";
          //  if(isClient)
          //  OnPlayerHealthChange();
        }
        //  Player = GameObject.FindGameObjectsWithTag("Player");

        //   for (int i=0;i< Player.Length; i++) 
        //  {
        //  if (Player[i]==isLocalPlayer)
        //  {

        //    PlayerCurrentHealth = Player[i].GetComponent<PlayerHealth>().getPlayerHealth();
        //  }

        // }


        //   gameObject.GetComponentInChildren<Text>().text = PlayerCurrentHealth + "%";
        //  PlayerCurrentHealth = PlayerTransform.GetComponent<PlayerHealth>().getPlayerHealth();
        //  PlayerHealthBarRectTranslate.sizeDelta = new Vector2(100 + PlayerCurrentHealth, PlayerHealthBarRectTranslate.sizeDelta.y);

        // PlayerHealthBarRectTranslate.sizeDelta = new Vector2(100 + PlayerCurrentHealth, PlayerHealthBarRectTranslate.sizeDelta.y);
        // PlayerCurrentHealth = PlayerTransform.GetComponent<PlayerHealth>().getPlayerHealth();
        //  gameObject.GetComponentInChildren<Text>().text = PlayerCurrentHealth + "%";
          PlayerCurrentHealth = gameObject.GetComponentInParent<PlayerHealth>().getPlayerHealth();
 
        gameObject.GetComponentInChildren<Text>().text = PlayerCurrentHealth + "%";
        PlayerHealthBarRectTranslate.sizeDelta = new Vector2(100 + PlayerCurrentHealth, PlayerHealthBarRectTranslate.sizeDelta.y);
    }


    

    public void setTaret(Transform target)
    {
        PlayerTransform = target;
    }
    public RectTransform getRectTransform()
    {
        return PlayerHealthBarRectTranslate;
    }



      // playerHealth= playerH.getPlayerHealth();
      //  playerMaxHealth=playerH.getPlayerMaxHealth();
        
  
   
}
/*
     public float smooth = 2.0f;
    public float tiltAngle = 20.0f;
    public float tiltAngleHorizontal = 2.0f;
    public float damping = 1;

void start()
{
    // gameObject.transform.localPosition = PlayerTransform.position + new Vector3(width, height, depth);


    // currentPlayer = PlayerController.getPlayer();
    // currentPlayer.transform.parent = transform.parent;
    // PlayerController.setCamera(gameObject);
    //  transform.localPosition= PlayerTransform.position + new Vector3(width, height, depth);
    // offset = PlayerTransform.position - transform.position;
    //  gameObject.transform.localPosition = new Vector3(width, height, depth);

}

//cameraPosition.position = PlayerTransform.transform.position ;
float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngleHorizontal;
float tiltArounX = Input.GetAxis("Vertical") * tiltAngle;
Quaternion Target = Quaternion.Euler(tiltArounX, 0, tiltAroundZ);

        public void setCameraPosition(Transform cam)
    {
        cameraPosition = cam;
    }

        public GameObject getCamera()
    {
        return this.gameObject;
    }

*/
// transform.position = PlayerTransform.position + offset;
// offset = transform.position - PlayerTransform.position;
///////// transform.position = PlayerTransform.transform.position ;
//* transform.rotation = PlayerTransform.rotation;
// offset = PlayerTransform.position - transform.position + new Vector3(width, height, depth);
//transform.position = PlayerTransform.position + offset;

//  transform.rotation = Quaternion.Slerp(transform.rotation, Target, Time.deltaTime * smooth);
// Quaternion Target = Quaternion.Euler(PlayerTransform.rotation.x, PlayerTransform.rotation.y, PlayerTransform.rotation.z);


// transform.eulerAngles = new Vector3(0, PlayerTransform.eulerAngles.y, 0);
//transform.rotation = PlayerTransform.rotation;

//**************************************************************************************************************************************************************************************
//new Vector3(PlayerTransform.position.x+width,PlayerTransform.position.y+height,PlayerTransform.position.z+depth);
//  transform.rotation = PlayerTransform.rotation;
//  gameObject.transform.localRotation = Quaternion.Slerp(PlayerTransform.rotation, Target, Time.deltaTime * smooth);