using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogMovie : SingletonMonoBehaviour<BigFrogMovie> {

    [SerializeField] private BigFrog big_Frog;
    [SerializeField] private GameObject boss_Canvas;
    

	// Use this for initialization
	void Start () {
		
	}
   

    public void Start_Battle_Movie() {
        StartCoroutine("Battle_Movie_Cor");
    }

    private IEnumerator Battle_Movie_Cor() {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        MoveTwoPoints camera_Move = camera.GetComponent<MoveTwoPoints>();
        CameraController camera_Controller = camera.GetComponent<CameraController>();

        //カメラの移動、固定        
        camera_Controller.enabled = false;
        camera_Move.Start_Move(new Vector3(3312f, 0, -10));
        yield return new WaitUntil(camera_Move.End_Move);

        //戦闘開始
        big_Frog.Start_Battle();
        boss_Canvas.SetActive(true);
    }


    public void Start_Clear_Movie() {
        StartCoroutine("Clear_Movie_Cor");
    }

    private IEnumerator Clear_Movie_Cor() {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        CameraController camera_Controller = camera.GetComponent<CameraController>();

        yield return new WaitForSeconds(1.0f);

        //カメラの固定解除
        camera_Controller.enabled = true;        
    }
}
