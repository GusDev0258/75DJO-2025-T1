using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciaAreaMusica : MonoBehaviour
{
    public static GerenciaAreaMusica Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = 0.6f;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void TocarMusica(AudioClip novaMusica)
    {
        if (audioSource.clip == novaMusica) return;

        audioSource.Stop(); 
        audioSource.clip = novaMusica;
        audioSource.Play();
    }
}