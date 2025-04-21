using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusica : MonoBehaviour
{
    public AudioClip musicaDaArea;
    private GerenciaAreaMusica gerenciaAreaMusica;

    private void Start()
    {
       gerenciaAreaMusica = GetComponent<GerenciaAreaMusica>();
         if (gerenciaAreaMusica == null)
         {
              gerenciaAreaMusica = gameObject.AddComponent<GerenciaAreaMusica>();
         }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gerenciaAreaMusica.TocarMusica(musicaDaArea);
        }
    }
}