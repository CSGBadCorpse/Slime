using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioListener audioListener;//SFX 
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        audioSource.Play();
    }


}
