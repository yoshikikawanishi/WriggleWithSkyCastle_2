using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : MonoBehaviour {

    [SerializeField] private MovieSystem first_Opening_Movie;
    [SerializeField] private float right_Side;
    [SerializeField] private float scroll_Speed;

    private GameObject main_Camera;
    private CameraController camera_Controller;


    void Start() {
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Controller = main_Camera.GetComponent<CameraController>();

        Start_Opening();        
    }



    public void Start_Opening() {
        StartCoroutine("Opening_Cor");
    }


    private IEnumerator Opening_Cor() {
        camera_Controller.enabled = false;

        first_Opening_Movie.Start_Movie();
        yield return new WaitUntil(first_Opening_Movie.End_Movie);

        //強制スクロール開始
        StartCoroutine("Scroll_Camera");
    }


    public void Shake_Camera() {
        CameraShake shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        shake.Shake(0.3f, new Vector2(3, 3), true);
    }


    private IEnumerator Scroll_Camera() {
        while(main_Camera.transform.position.x < right_Side) {
            main_Camera.transform.position += new Vector3(scroll_Speed, 0, 0);
            yield return new WaitForSeconds(0.016f);
        }
    }



    
    
}
