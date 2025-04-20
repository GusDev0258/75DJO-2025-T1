using System;
using System.Collections;
using System.Collections.Generic;
using SunTemple;
using UnityEngine;
using UnityEngine.UI;

public class PontuacaoJogador : MonoBehaviour
{
    public int pontuacaoJogador;
    public Text pontuacaoText;

    public int inimigosMortos;
    public Door portaBoss;
    public Door portaFora;

    private AudioSource source;

    public AudioClip openBossDoorSound;

    public AudioClip openOutsideDoorSound;

    // Start is called before the first frame update
    void Start()
    {
        pontuacaoJogador = 0;
        inimigosMortos = 0;
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AdicionarPontuacao(int valor)
    {
        pontuacaoJogador += valor;
        pontuacaoText.text = pontuacaoJogador.ToString();
    }

    public void AdicionarInimigosMortos()
    {
        inimigosMortos += 1;
        if (inimigosMortos >= 10)
        {
            DestravarPortaBoss();
        }
    }

    public void DestravarPortaBoss()
    {
        if (portaBoss.IsLocked)
        {
            portaBoss.IsLocked = false;
            portaBoss.GetComponent<Outline>().OutlineWidth = 5f;
            if (!portaBoss.DoorClosed)
            {
                portaBoss.GetComponent<Outline>().OutlineWidth = 0f;
            }
            source.clip = openBossDoorSound;
            source.Play();
        }
    }

    public void DestravarPortaFora()
    {
        if (portaFora.IsLocked)
        {
            portaFora.IsLocked = false;
            portaFora.GetComponent<Outline>().OutlineWidth = 5f;
            if (!portaFora.DoorClosed)
            {
                portaFora.GetComponent<Outline>().OutlineWidth = 0f;
            }
            source.clip = openOutsideDoorSound;
            source.Play();
        }
    }
}