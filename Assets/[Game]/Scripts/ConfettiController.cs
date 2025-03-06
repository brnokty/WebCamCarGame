using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> confetties = new List<ParticleSystem>();

    private void Start()
    {
        EventManager.Instance.Register(EventTypes.RaceFinish, PlayConfetti);
    }

    //play confetties
    public void PlayConfetti(EventArgs args)
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Audience);
        foreach (var confetti in confetties)
        {
            confetti.Play();
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unregister(EventTypes.RaceFinish, PlayConfetti);
    }
}