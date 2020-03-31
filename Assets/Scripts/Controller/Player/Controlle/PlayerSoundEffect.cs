using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour {

    [SerializeField] private AudioSource jump_Sound;
    [SerializeField] private AudioSource land_Sound;
    [SerializeField] private AudioSource kick_Sound;
    [SerializeField] private AudioSource attack_Sound;
    [SerializeField] private AudioSource graze_Sound;
    [SerializeField] private AudioSource shoot_Sound;
    [SerializeField] private AudioSource charge_Shoot_Sound;
    [SerializeField] private AudioSource charge_Sound;
    [SerializeField] private AudioSource damaged_Sound;
    [SerializeField] private AudioSource hit_Attack_Sound;
    [SerializeField] private AudioSource alert_Sound;
    

    public void Play_Jump_Sound() {
        jump_Sound.Play();
    }

    public void Play_Land_Sound() {
        land_Sound.Play();
    }

    public void Play_Kick_Sound() {
        kick_Sound.Play();
    }

    public void Play_Attack_Sound() {
        attack_Sound.Play();
    }

    public void Play_Graze_Sound() {
        graze_Sound.Play();
    }

    public void Play_Shoot_Sound() {
        shoot_Sound.Play();
    }

    public void Play_Charge_Shoot_Sound() {
        charge_Shoot_Sound.Play();
    }

    //チャージ音開始
    public void Start_Charge_Sound() {
        charge_Sound.pitch = 1;
        charge_Sound.Play();
    }

    //チャージ音中止
    public void Stop_Charge_Sound() {
        charge_Sound.Stop();
    }

    //チャージ音ピッチ変更
    public void Change_Charge_Sound_Pitch(float pitch) {
        charge_Sound.pitch = pitch;
    }

    public void Play_Damaged_Sound() {
        damaged_Sound.Play();
    }

    public void Play_Hit_Attack_Sound() {
        hit_Attack_Sound.Play();
    }

    public void Play_Alert_Sound() {
        alert_Sound.Play();
    }
}
