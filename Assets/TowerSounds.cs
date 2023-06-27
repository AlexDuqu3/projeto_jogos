using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSounds : MonoBehaviour
{
    //[SerializeField] private bool _sellTowerSound;
    //[SerializeField] private bool _upgradeTowerSound;

    [SerializeField] private AudioSource _sellTowerclip;
    [SerializeField] private AudioSource _upgradeTowerclip;

    public void UpgradeTowerSound()
    {
        SoundManager.Instance.PlaySound(_upgradeTowerclip);
    }

    public void SellTowerSound()
    {
        SoundManager.Instance.PlaySound(_sellTowerclip);
    }




}
