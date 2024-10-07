using System;
using UnityEngine;

namespace OSK
{
    public class UIAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip _openingSound;
        [SerializeField] private AudioClip _closingSound;

        public void PlayOpeningSound(bool playAudio)
        {
            if (playAudio && _openingSound != null)
            {
                AudioSource.PlayClipAtPoint(_openingSound, Camera.main.transform.position);
            }
        }

        public void PlayClosingSound(bool playAudio)
        {
            if (playAudio && _closingSound != null)
            {
                AudioSource.PlayClipAtPoint(_closingSound, Camera.main.transform.position);
            }
        }
    }
}