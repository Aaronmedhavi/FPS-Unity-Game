using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioSource ReloadingChannel;

    public AudioClip reloadingSoundAK47;
    public AudioSource emptymagSoundAK47;

    public AudioClip P1911Shot;
    public AudioClip AK47Shot;

    public AudioClip reloadingSound1911;

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

    public void ShootSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911Shot); 
                break;
            case WeaponModel.AK47:
                ShootingChannel.PlayOneShot(AK47Shot); 
                break;
        }
    }

    public void ReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1911:
                ReloadingChannel.PlayOneShot(reloadingSound1911);
                break;
            case WeaponModel.AK47:
                ReloadingChannel.PlayOneShot(reloadingSoundAK47);
                break;
        }
    }
}
