using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect_Obj;
    [SerializeField] private GameObject power_Charge_Effect_Blue_Obj;
    [SerializeField] private ParticleSystem small_Power_Charge_Effect;
    [SerializeField] private ParticleSystem burst_Effect_Green;
    [SerializeField] private ParticleSystem burst_Effect_Red;
    [SerializeField] private ParticleSystem burst_Effect_Blue;
    [SerializeField] private GameObject ban_Player_Flying_Effect_Obj;
    [SerializeField] private Animator disable_Flying_Screen_Effect;
    [SerializeField] private GameObject blue_Fire_Pillar_Pre_Effect;
    [SerializeField] private GameObject back_Door_Effect;

    //================================大チャージ====================================
    public void Play_Power_Charge_Effect() {        
        power_Charge_Effect_Obj.SetActive(true);
    }

    public void Play_Power_Charge_Effect(float span) {
        power_Charge_Effect_Obj.SetActive(true);
        Invoke("Stop_Power_Charge_Effect", span);
    }

    public void Play_Power_Charge_Effect(Vector3 position, float span) {
        var effect = Instantiate(power_Charge_Effect_Obj);
        effect.transform.position = position;
        effect.SetActive(true);
        Destroy(effect, span);
    }

    public void Stop_Power_Charge_Effect() {
        power_Charge_Effect_Obj.SetActive(false);
    }

    //================================大チャージ青====================================
    public void Play_Power_Charge_Effect_Blue() {
        power_Charge_Effect_Blue_Obj.SetActive(true);
    }

    public void Stop_Power_Charge_Effect_Blue() {
        power_Charge_Effect_Blue_Obj.SetActive(false);
    }

    //================================小チャージ====================================
    public void Play_Small_Power_Charge_Effect() {
        small_Power_Charge_Effect.Play();
    }

    //================================バーストエフェクト====================================
    public void Play_Burst_Effect_Green() {
        burst_Effect_Green.Play();
    }

    public void Play_Burst_Effect_Green(Vector2 pos) {
        var obj = Instantiate(burst_Effect_Green.gameObject);
        obj.transform.position = pos;
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 3.0f);
    }

    public void Play_Burst_Effect_Red() {
        burst_Effect_Red.Play();
    }

    public void Play_Burst_Effect_Blue() {
        burst_Effect_Blue.Play();
    }
    //================================飛行不可エフェクト====================================
    public void Play_Ban_Flying_Effect() {
        ban_Player_Flying_Effect_Obj.GetComponent<AudioSource>().Play();
        ban_Player_Flying_Effect_Obj.GetComponent<ParticleSystem>().Play();
        disable_Flying_Screen_Effect.gameObject.SetActive(true);
        disable_Flying_Screen_Effect.SetTrigger("AppearTrigger");
    }


    public void Release_Ban_Flying_Effect() {
        disable_Flying_Screen_Effect.SetTrigger("DisappearTrigger");
        disable_Flying_Screen_Effect.gameObject.SetActive(false);
    }

    //================================火柱予測線====================================
    public void Play_Pre_Blue_Fire_Pillar_Effect(float pos_X) {
        GameObject obj = Instantiate(blue_Fire_Pillar_Pre_Effect);
        obj.transform.position = new Vector3(pos_X, 0, 0);
        Destroy(obj, 5.0f);
    }

    //================================後ろ戸===================================
    private List<GameObject> back_Doors = new List<GameObject>();

    public void Play_Back_Door_Effect(Vector2 pos) {
        GameObject door = Instantiate(back_Door_Effect);
        door.transform.position = pos;
        back_Doors.Add(door);
        door.GetComponent<Animator>().SetTrigger("AppearTrigger");
    }    

    public void Delete_Back_Door_Effect() {
        foreach(var door in back_Doors) {
            door.GetComponent<Animator>().SetTrigger("DisappearTrigger");
            Destroy(door, 2.0f);
        }
        back_Doors.Clear();
    }
}
