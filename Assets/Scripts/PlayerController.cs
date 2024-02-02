using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    // public GameObject Player;
    public float playerSpeed = 12f;
    public float bulletSpeed = 20.0f;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;


    private void Start()
    {
         Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
        originalRotation = this.transform.localRotation;
    }


    void Update()
    { 
        if(!isLocalPlayer){return;}


        var x = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * playerSpeed;
     
          transform.Translate(x, 0, z);
        // transform.Rotate(0, x, 0);


        if (axes == RotationAxes.MouseXAndY)
        {
            // Read the mouse input axis
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
            this.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            this.transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            this.transform.localRotation = originalRotation * yQuaternion;
        }

    }


    public static float ClampAngle(float angle, float min, float max)
    {
        // if (angle & lt; -360F)
        //     angle += 360F;
        //   if (angle & gt; 360F)
        //    angle -= 360F;
        if (angle < -360f) { angle += 360f; }
        if (angle > 360f) { angle -= 360f; }
        return Mathf.Clamp(angle, min, max);
    }

 

 

    


   // public override void OnStartLocalPlayer()
   // {
      // Camera.main.GetComponent<CameraFallow>().setTaret(gameObject.transform);  //seting target to fallow in CameraFallow
        //*GetComponent<MeshRenderer>().material.color = Color.blue;
   // }

}
/* ************************To move by rigidBody*******************************************
 *     private Vector3 velocity = Vector3.zero;
 *     private Rigidbody rigidbody; dont forget to get the rigidbody (GetComplonent..etc)
 *         //***float x = Input.GetAxisRaw("Horizontal");
        //***float z = Input.GetAxisRaw("Vertical") ;
        //***Vector3 moveH = transform.right * x;
        //***Vector3 moveV = transform.forward * z;
        //***Vector3 vel = (moveH + moveV).normalized * playerSpeed;
        //***Move(vel);


            private void FixedUpdate()
    {
        PerformMovement();
    }
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }
    void PerformMovement()
    {
        if(velocity != Vector3.zero)
        {
            rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }

*
*******************************************Other move method************************************************
*    if (Input.GetKey(KeyCode.W))
            {
                // SetTransformZ((transform.position.z) + playerSpeed);
                moveForward(playerSpeed);

            }

            if (Input.GetKey(KeyCode.S))
            {
                //SetTransformZ((transform.position.z) - playerSpeed);
                moveBack(playerSpeed);

            }

            if (Input.GetKey(KeyCode.A))
            {
                // SetTransformX((transform.position.x) - playerSpeed);
                moveLeft(playerSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                // SetTransformX((transform.position.x) + playerSpeed);
                moveRight(playerSpeed);

            }

            else
            {
                this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            }
      
       private void moveForward(float speed)
    {
        transform.localPosition += transform.forward * speed * Time.deltaTime;
    }

    private void moveBack(float speed)
    {
        transform.localPosition -= transform.forward * speed * Time.deltaTime;
    }

    private void moveRight(float speed)
    {
        transform.localPosition += transform.right * speed * Time.deltaTime;
    }

    private void moveLeft(float speed)
    {
        transform.localPosition -= transform.right * speed * Time.deltaTime;
    }

    void SetTransformX(float n)
    {
        transform.position = new Vector3(n, transform.position.y, transform.position.z);
    }

    void SetTransformZ(float n)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, n);
    }


 * **********************************************************shoot by boolet***********************************
 *         // if (Input.GetKeyDown(KeyCode.Space)) { CmdFire(); }
        //  if (Input.GetMouseButtonDown(0)) { CmdFire(); }
 * [Command]
    void CmdFire()
    {           //creating bullet from mprefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;                        //adding velocity
        NetworkServer.Spawn(bullet);                                                                  //spawning bullet on clients
        Destroy(bullet, 2.0f);
    }
 * 
   
        //********* m.getCamera().transform.parent = gameObject.transform.parent;
         // gameObject.transform.parent = cameraP.transform;
       // m.GetComponent<CameraFallow>().setTaret(gameObject.transform);
      //  Camera.main.GetComponent<CameraFallow>().setCameraPosition(CameraPosition.GetComponentsInChildren<>);
      // GetComponent<PlayerCamera>().setPlayer(this.gameObject);// when using PlayerCamera script

*/
