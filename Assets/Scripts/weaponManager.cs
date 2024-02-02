using UnityEngine;
using UnityEngine.Networking;
public class weaponManager : NetworkBehaviour {
    [SerializeField]
    private string weaponLayerName = "weapon";
    [SerializeField]
    private PlayerWeapon primaryWeapon;
    [SerializeField]
    private Transform weaponHolder;

    private PlayerWeapon currentWeapon;
    private weaponGraphics currentGraphics;

    private void Start()
    {
        EquipWeapon(primaryWeapon);
    }
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIn= (GameObject)Instantiate(_weapon.weaponGraphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIn.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIn.GetComponent<weaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.Log("No weaponGraphics component on the weapon object :"+ _weaponIn.name);
        }

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(_weaponIn, LayerMask.NameToLayer(weaponLayerName));
            //_weaponIn.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public  weaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }
}
