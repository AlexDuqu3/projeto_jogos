using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour

{
    [SerializeField] private bool _toogleMusic;
    [SerializeField] private bool _toggleFX;

    public void Toogle() { 
        if(_toogleMusic) SoundManager.Instance.ToggleMusic();

    }

    public void ToogleFX()
    {
        if (_toggleFX) SoundManager.Instance.ToggleFX();

    }
}
