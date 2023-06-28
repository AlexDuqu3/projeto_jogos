using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _slider.value = AudioListener.volume;
    }

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
