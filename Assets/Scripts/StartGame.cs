using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(1);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}