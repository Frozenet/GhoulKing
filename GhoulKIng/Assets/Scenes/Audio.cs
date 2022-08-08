using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Audio : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer mixer;
    private void Awake()
    {
        slider.onValueChanged.AddListener(master);
    }

    public void master(float num)
    {
        mixer.SetFloat("SFX", Mathf.Log10(num) * 20);
    }

}
