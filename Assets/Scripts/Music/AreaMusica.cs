using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusica : MonoBehaviour
{
    public AudioClip musicaDaArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GerenciaAreaMusica.Instance != null)
            {
                GerenciaAreaMusica.Instance.TocarMusica(musicaDaArea);
            }
        }
    }
}