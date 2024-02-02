using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(weaponManager))]
public class PlayerShoot : NetworkBehaviour {
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask Lmask;

    private const string PlayerTag = "Player";
    
    private PlayerWeapon currentWeapon;
    private weaponManager weaponMan;
  //  [SerializeField]
  //  private GameObject weaponGFX;


    private void Start()
    {
        if(cam == null)
        {
            Debug.Log("PlayerShoot:No camera referenced");
            this.enabled = false;
        }
        weaponMan = GetComponent<weaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponMan.GetCurrentWeapon();
        if (currentWeapon.fireRate <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                InvokeRepeating("Shoot", 0.0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CancelInvoke("Shoot");
            }
        }
     
    }
    //Is Called on server when a player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }
    //is called on all clients when we need to do shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponMan.GetCurrentGraphics().muzzleFlash.Play();
    }
    //is called on server when we hit something. Takes in hit point and the normal of the surface
    [Command]
    void CmdOnHit(Vector3 _pos,Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }
    //is called on all clients . (Here we can spawn in cool effects!!!!!)
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos,Vector3 _normal)
    {
        GameObject _hitEffectRefference = Instantiate(weaponMan.GetCurrentGraphics().hitEffectPrefab,_pos,Quaternion.LookRotation(_normal));
        Destroy(_hitEffectRefference, 1.0f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer) { return; }
        CmdOnShoot();//we are shooting call the OnShoot method on server
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit , currentWeapon.range, Lmask))
        {
            if (hit.collider.tag == PlayerTag)
            {
                CmdPlayerGotShot(hit.collider.name,currentWeapon.damage);
            }
            CmdOnHit(hit.point, hit.normal);//if we hit something call OnHit method on server
        }
    }

    [Command]
    void CmdPlayerGotShot(string playerID , int _damage)
    {
        Debug.Log(playerID + "Got Shot");
        //Destroy(GameObject.Find(ID));
        PlayerManager player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(_damage);
    }
}
