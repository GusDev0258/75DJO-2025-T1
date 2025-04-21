using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciaAreaMusica : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void TocarMusica(AudioClip novaMusica)
    {
        if (audioSource.clip == novaMusica) return;

        audioSource.clip = novaMusica;
        audioSource.Play();
    }
}