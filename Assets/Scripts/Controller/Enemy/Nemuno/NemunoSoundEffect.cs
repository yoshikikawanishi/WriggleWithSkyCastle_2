using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoSoundEffect : MonoBehaviour {

    [SerializeField] private AudioSource before_Slash_Sound;
    [SerializeField] private AudioSource slash_Sound;
    [SerializeField] private AudioSource jump_Sound;
    [SerializeField] private AudioSource land_Sound;


    public void Play_Before_Slash_Sound() {
        before_Slash_Sound.Play();
    }

    public void Play_Slash_Sound() {
        slash_Sound.Play();
    }

    public void Play_Jump_Sound(float volume) {
        jump_Sound.volume = volume;
        jump_Sound.Play();
    }

    public void Play_Land_Sound() {
        land_Sound.Play();
    }
}
