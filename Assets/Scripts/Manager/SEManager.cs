using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour {

    [System.Serializable]
	public class SE {
        public string name = "input name";
        public AudioClip clip;
        public float volume; 
    }

    public List<SE> SE_List = new List<SE>();

    private AudioSource _audio;

    private void Awake() {
        _audio = GetComponent<AudioSource>();
    }


    public void Play(string name) {
        foreach(SE se in SE_List) {
            if(se.name == name) {
                _audio.clip = se.clip;
                _audio.volume = se.volume;
                _audio.Play();
                return;
            }
        }
        Debug.Log("SE " + name + " is not exist");
    }


    public void Play(string name, float volume) {
        foreach (SE se in SE_List) {
            if (se.name == name) {
                _audio.clip = se.clip;
                _audio.volume = volume;
                _audio.Play();
                return;
            }
        }
        Debug.Log("SE " + name + " is not exist");
    }


    // ---------------- Editor用 --------------------
    public void Add_SE() {
        SE_List.Add(new SE());
    }


    public void Remove_SE(int index) {
        SE_List.RemoveAt(index);
    }
}
