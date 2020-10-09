using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage4_BossMovie : MonoBehaviour {

    [SerializeField] private MasterSpark master_Spark_Prefab;


    public void Generate_Master_Spark() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        var spark = Instantiate(master_Spark_Prefab.gameObject);
        spark.transform.position = new Vector3(main_Camera.transform.position.x + 240f, player.transform.position.y);        
    }


    public void Change_Scene_To_Stage5() {
        SceneManager.LoadScene("Stage5_1Scene");
    }


    public void Fade_In() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }


    public void Fade_Out() {
        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.02f);
        BGMManager.Instance.Fade_Out();
    }


    public void Camera_Shake() {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>().Shake(0.3f, new Vector2(5f, 5f), true);
        GetComponent<AudioSource>().Play();
    }
}
