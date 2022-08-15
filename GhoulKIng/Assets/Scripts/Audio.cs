using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Audio : MonoBehaviour
{
    [SerializeField] Slider Masterslider;
    [SerializeField] Slider SFXslider;
    [SerializeField] Slider Musicslider;
    [SerializeField] AudioMixer mixer;
    private void Awake()
    {
       // slider.onValueChanged.AddListener(master);
    }

    public void masterVol()
    {
        mixer.SetFloat("Master", Mathf.Log10(Masterslider.value) * 20);

        
    }
    public void SFXVol()
    {
        mixer.SetFloat("SFX", Mathf.Log10(SFXslider.value) * 20);

    }
    public void musicVol()
    {

        mixer.SetFloat("Music", Mathf.Log10(Musicslider.value) * 20);
    }

}
