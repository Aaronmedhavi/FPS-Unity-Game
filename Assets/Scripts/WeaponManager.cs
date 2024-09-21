using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [Header("Weapon Slots")]
    public List<GameObject> weaponSlots;

    [Header("UI Elements")]
    public Image weaponIconImage; // Assign the WeaponIcon Image in the Inspector

    public GameObject activeWeaponSlot;

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
            UpdateWeaponUI();
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
        // Example: Switching weapons with number keys 1-9
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        // Add more key bindings if you have more weapon slots
        // For example:
        // if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchActiveSlot(2); }
    }

    public void PickupWeapon(GameObject pickedUpWeapon)
    {
        AddWeaponIntoActiveSlot(pickedUpWeapon);
        UpdateWeaponUI(); // Update UI after picking up a new weapon
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

        UpdateWeaponUI(); // Update UI after switching weapons
    }

    private void UpdateWeaponUI()
    {
        if (weaponIconImage == null)
        {
            Debug.LogWarning("WeaponManager: WeaponIcon Image is not assigned.");
            return;
        }

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon activeWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            if (activeWeapon != null && activeWeapon.weaponIcon != null)
            {
                weaponIconImage.sprite = activeWeapon.weaponIcon;
                weaponIconImage.enabled = true; // Ensure the icon is visible
            }
            else
            {
                Debug.LogWarning("Active weapon or its icon is missing.");
                weaponIconImage.enabled = false; // Hide the icon if missing
            }
        }
        else
        {
            // No weapon equipped
            weaponIconImage.sprite = null;
            weaponIconImage.enabled = false; // Hide the icon
        }
    }
}
