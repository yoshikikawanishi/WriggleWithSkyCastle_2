using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour {

    public string next_Scene;

    public enum Change_Efect {
        non,
        fade_Out,
        fade_Out_With_BGM
    }
    public Change_Efect change_Effect_Type;

    [Space]
    [SerializeField] private Color fade_Out_Color;

    private bool is_Changing = false;

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" && !is_Changing) {
            StartCoroutine("Change_Scene_Cor");
        }
    }

    
    //シーン遷移用コルーチン
    private IEnumerator Change_Scene_Cor() {
        //遷移前のエフェクト
        is_Changing = true;

        switch (change_Effect_Type) {            
            case Change_Efect.fade_Out:
                FadeInOut.Instance.Start_Fade_Out(fade_Out_Color, 0.05f);
                yield return new WaitForSeconds(1.0f);
                break;
            case Change_Efect.fade_Out_With_BGM:
                FadeInOut.Instance.Start_Fade_Out(fade_Out_Color, 0.05f);
                BGMManager.Instance.Fade_Out();
                yield return new WaitForSeconds(1.0f);
                break;
        }

        is_Changing = false;
        SceneManager.LoadScene(next_Scene);
    }

}
