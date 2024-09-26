using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Weapon Slots")]
    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 90; // Example initial ammo
    public int totalPistolAmmo = 45; // Example initial ammo

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (weaponSlots.Count > 0)
        {
            activeWeaponSlot = weaponSlots[0];
        }
        else
        {
            Debug.LogError("WeaponManager: No weapon slots assigned.");
        }
    }

    private void Update()
    {
        HandleWeaponActivation();
        HandleWeaponSwitchInput();
    }

    private void HandleWeaponActivation()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
    }

    private void HandleWeaponSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
        // Add more slots if needed
    }

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedUpWeapon)
    {
        DropCurrentWeapon();

        pickedUpWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedUpWeapon.GetComponent<Weapon>();
        if (weapon == null)
        {
            Debug.LogError("Picked up weapon does not have a Weapon component.");
            return;
        }

        // Set Position
        pickedUpWeapon.transform.localPosition = weapon.spawnPosition;
        // Set Rotation
        Quaternion desiredRotation = Quaternion.Euler(weapon.spawnRotation);
        pickedUpWeapon.transform.localRotation = desiredRotation;
        // Set Scale
        pickedUpWeapon.transform.localScale = weapon.spawnScale;
        // Reset initialRotation and targetRotation in Weapon script
        weapon.ResetInitialRotation();

        weapon.isActiveWeapon = true;
        if (weapon.animator != null)
        {
            weapon.animator.enabled = true;
        }
        else
        {
            Debug.LogWarning("Weapon Animator is not assigned.");
        }
    }

    private void DropCurrentWeapon()
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            Weapon weapon = weaponToDrop.GetComponent<Weapon>();
            if (weapon == null)
            {
                Debug.LogError("Dropped weapon does not have a Weapon component.");
                return;
            }

            weapon.isActiveWeapon = false;
            if (weapon.animator != null)
            {
                weapon.animator.enabled = false;
            }

            // Detach the weapon from the active slot
            weaponToDrop.transform.SetParent(null);
            // Optionally, set the drop position and rotation
            // Here, we keep it at the player's position
            weaponToDrop.transform.position = transform.position; // Adjust as needed
            weaponToDrop.transform.rotation = Quaternion.identity; // Adjust as needed
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (slotNumber < 0 || slotNumber >= weaponSlots.Count)
        {
            Debug.LogWarning("SwitchActiveSlot: Slot number out of range.");
            return;
        }

        // Deactivate current weapon
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            if (currentWeapon != null)
            {
                currentWeapon.isActiveWeapon = false;
            }
            else
            {
                Debug.LogWarning("Current weapon does not have a Weapon component.");
            }
        }

        // Switch active slot
        activeWeaponSlot = weaponSlots[slotNumber];

        // Activate new weapon
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            if (newWeapon != null)
            {
                newWeapon.isActiveWeapon = true;
            }
            else
            {
                Debug.LogWarning("New weapon does not have a Weapon component.");
            }
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.Pistol1911:
                totalPistolAmmo -= bulletsToDecrease;
                totalPistolAmmo = Mathf.Max(totalPistolAmmo, 0); // Prevent negative ammo
                break;
            case Weapon.WeaponModel.AK47:
                totalRifleAmmo -= bulletsToDecrease;
                totalRifleAmmo = Mathf.Max(totalRifleAmmo, 0); // Prevent negative ammo
                break;
            // Add cases for other weapon types as needed
            default:
                Debug.LogWarning("DecreaseTotalAmmo: WeaponModel not recognized.");
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case Weapon.WeaponModel.Pistol1911:
                return totalPistolAmmo;
            case Weapon.WeaponModel.AK47:
                return totalRifleAmmo;
            // Add cases for other weapon types as needed
            default:
                Debug.LogWarning("CheckAmmoLeftFor: WeaponModel not recognized.");
                return 0;
        }
    }

    public void AddTotalAmmo(int bulletsToAdd, WeaponModel weaponModel)
    {
        switch (weaponModel)
        {
            case WeaponModel.Pistol1911:
                totalPistolAmmo += bulletsToAdd;
                break;
            case WeaponModel.AK47:
                totalRifleAmmo += bulletsToAdd;
                break;
            // Add cases for other weapon types as needed
            default:
                Debug.LogWarning("AddTotalAmmo: WeaponModel not recognized.");
                break;
        }
    }
}
