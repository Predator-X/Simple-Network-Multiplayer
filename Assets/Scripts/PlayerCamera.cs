using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
[AddComponentMenu("Camera-Control/Mouse Look")]

public class PlayerCamera : NetworkBehaviour //MonoBehaviour
{
   // public Camera cam;

   public GameObject Player;

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

    void Start()
    {
        Rigidbody rigidbody = Player.GetComponent<Rigidbody>(); 
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
        originalRotation = Player.transform.localRotation;
    }
    void Update()
    {
        if (Player != null)
        {

            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                Player.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Player.transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                Player.transform.localRotation = originalRotation * yQuaternion;
            }
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

    public void setPlayer(GameObject player)
    {
        Player = player;
        
    }

}
