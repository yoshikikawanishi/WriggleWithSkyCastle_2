using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : SingletonMonoBehaviour<FadeInOut> {

    public Sprite screen_Cover_Image;
    private SpriteRenderer screen_Cover_Sprite;
    
    public GameObject rotate_Fade_Out_Prefab;
    private GameObject rotate_Fade_Out_Object;
	
    /// <summary>
    /// フェードイン開始
    /// </summary>
    /// <param name="color">画像の色、透明度は指定しない</param>
    /// <param name="speed">透明度減少の速度</param>
    public void Start_Fade_In(Color color, float speed) {
        if(speed <= 0) {
            Debug.Log("Fade In Speed Must Positive Number");
            return;
        }
        StopAllCoroutines();
        Generate();
        Prepare(color, false);
        StartCoroutine("Fade_In_Cor", speed);
    }


    /// <summary>
    /// フェードアウト開始
    /// </summary>
    /// <param name="color">画像の色、透明度は指定しない</param>
    /// <param name="speed">透明度増加の速度、1以下の正の数</param>
    public void Start_Fade_Out(Color color, float speed) {
        if(speed <= 0) {
            Debug.Log("Fade Out Speed Must Positive Number");
            return;
        }
        StopAllCoroutines();
        Generate();
        Prepare(color, true);
        StartCoroutine("Fade_Out_Cor", speed);
    }


    /// <summary>
    /// 回転フェードアウト
    /// </summary>
    public void Start_Rotate_Fade_Out() {
        var main_Camera = GameObject.FindWithTag("MainCamera");
        rotate_Fade_Out_Object = Instantiate(rotate_Fade_Out_Prefab, main_Camera.transform);
        rotate_Fade_Out_Object.transform.position = main_Camera.transform.position + new Vector3(0, 0, 10);
    }


    /// <summary>
    /// フェードアウト用のカバーを消す
    /// </summary>
    public void Delete_Fade_Out_Obj() {        
        if (screen_Cover_Sprite != null)
            screen_Cover_Sprite.gameObject.SetActive(false);
        if (rotate_Fade_Out_Object != null)
            Destroy(rotate_Fade_Out_Object);
    }


    //画像の生成
    private void Generate() {
        if (screen_Cover_Sprite == null) {
            screen_Cover_Sprite = new GameObject("Sprite").AddComponent<SpriteRenderer>();
            screen_Cover_Sprite.sprite = screen_Cover_Image;
        }
        else {
            screen_Cover_Sprite.gameObject.SetActive(true);
        }
    }


    //画像の初期設定
    private void Prepare(Color color, bool is_Fade_Out) {
        screen_Cover_Sprite.transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        screen_Cover_Sprite.transform.localPosition = new Vector3(0, 0, 10);    //座標
        screen_Cover_Sprite.transform.localScale    = new Vector3(16, 16f, 1);  //サイズ
        screen_Cover_Sprite.color                   = color;                    //色
        screen_Cover_Sprite.sortingOrder            = 16;                       //レイヤ
        if (is_Fade_Out) 
            screen_Cover_Sprite.color              += new Color(0, 0, 0, -1);   //透明度
    }


    //フェードイン
    private IEnumerator Fade_In_Cor(float speed) {
        while(screen_Cover_Sprite.color.a >= 0) {
            screen_Cover_Sprite.color += new Color(0, 0, 0, -speed);
            yield return new WaitForSeconds(0.016f);
        }
        screen_Cover_Sprite.gameObject.SetActive(false);
    }


    //フェードアウト
    private IEnumerator Fade_Out_Cor(float speed) {
        while(screen_Cover_Sprite.color.a <= 1) {
            screen_Cover_Sprite.color += new Color(0, 0, 0, speed);
            yield return new WaitForSeconds(0.016f);
        }
    }
   
}
