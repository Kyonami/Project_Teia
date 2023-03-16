using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    
    [Header("BGM Audio Source")]
    [SerializeField] AudioSource mainAudio;

    [Header("Audio Clip - BGM")]
    [SerializeField] AudioClip mainBGM;

    [Header("Audio Clip - SE")]
    [SerializeField] AudioClip mainSE;

    [Header("Options")]
    [SerializeField] Slider mainBGMSlider;
    [SerializeField] Slider mainSESlider;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void ChangeVolum_MainAudio()
    {
        mainAudio.volume = mainBGMSlider.value;
    }

    public void SESound()
    {
        PlaySoundEffect(mainSE);
    }

    private void PlaySoundEffect(AudioClip audioClip)
    {
        AudioSource se = gameObject.AddComponent<AudioSource>();
        se.clip = audioClip;
        se.volume = mainSESlider.value;
        se.loop = false;
        se.Play();
        Destroy(se, audioClip.length);
    }
}
