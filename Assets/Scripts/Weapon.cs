using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    // Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    // Spread
    public float spreadIntensity;

    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    public float recoilAngle = 5f; // Maximum rotation angle on the x-axis
    public float recoilSpeed = 10f; // Speed of the recoil rotation
    public float returnSpeed = 15f; // Speed at which the gun returns to idle

    private Quaternion initialRotation; // Original rotation of the gun
    private Quaternion targetRotation; // Desired rotation after recoil

    // Reload
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 spawnScale;

    public enum WeaponModel
    {
        Pistol1911,
        AK47
    }

    public WeaponModel WeaponModels;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        // Store the initial rotation of the gun
        initialRotation = transform.localRotation;
        targetRotation = initialRotation;
        bulletsLeft = magazineSize;
    }

    void Start()
    {
        // Ensure initialRotation is correctly set at the start
        initialRotation = transform.localRotation;
        targetRotation = initialRotation;
    }

    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptymagSoundAK47.Play();
            }

            HandleRecoil();

            // Handle Shooting Modes
            if (currentShootingMode == ShootingMode.Auto)
            {
                // Hold Mouse
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // Click Mouse
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            // Manual Reload Input
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(WeaponModels) > 0)
            {
                Reload();
            }

            // Automatic Reload when magazine is empty
            if (!isReloading && bulletsLeft == 0 && WeaponManager.Instance.CheckAmmoLeftFor(WeaponModels) > 0)
            {
                Reload();
            }

            // Fire Weapon if Ready
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        SoundManager.Instance.ShootSound(WeaponModels);

        readyToShoot = false;

        ApplyRecoil();

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        // Instantiate Bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.Damage = weaponDamage;
        // Pointing bullet to face shoot direction
        bullet.transform.forward = shootingDirection;
        // Shoot the Bullet with spread direction
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        // Destroy bullet after shoot
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Check if done shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.ReloadSound(WeaponModels);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        int neededAmmo = magazineSize - bulletsLeft;
        int availableAmmo = WeaponManager.Instance.CheckAmmoLeftFor(WeaponModels);
        int ammoToLoad = Mathf.Min(neededAmmo, availableAmmo);

        bulletsLeft += ammoToLoad;
        WeaponManager.Instance.DecreaseTotalAmmo(ammoToLoad, WeaponModels);

        isReloading = false;
    }

    private void ApplyRecoil()
    {
        // Set the target rotation by rotating upward on the x-axis
        targetRotation = initialRotation * Quaternion.Euler(recoilAngle, 0f, 0f);
    }

    private void HandleRecoil()
    {
        // Smoothly interpolate towards the target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * recoilSpeed);

        // If close to target rotation, reset to initial
        if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
        {
            targetRotation = initialRotation;
        }

        // Smoothly return to initial rotation
        if (Quaternion.Angle(transform.localRotation, initialRotation) > 0.1f && targetRotation == initialRotation)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * returnSpeed);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Shoot from middle of screen to check direction
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // Hit something
            targetPoint = hit.point;
        }
        else
        {
            // shoot air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Return shoot direction and spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    public void ResetInitialRotation()
    {
        initialRotation = transform.localRotation;
        targetRotation = initialRotation;
        Debug.Log($"Weapon '{gameObject.name}' initialRotation reset to: {initialRotation.eulerAngles}");
    }
}
