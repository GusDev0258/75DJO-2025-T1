using System;
using UnityEngine;

namespace Weapons
{
    public class MagazineGlock : MonoBehaviour, IPegavel
    {
        private AudioSource source;
        public AudioClip clip;
        void Start()
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }
        public void Take()
        {
            source.clip = clip;
            source.Play();
            Glock glock = GameObject.FindWithTag("Weapon").GetComponent<Glock>();
            glock.addMagazine();
        }
    }
}