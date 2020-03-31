using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour {

    [SerializeField] private float start_Time = 1.0f;
    [Space]
    [SerializeField] private int loop_Count = 1;    //0未満で永続ループ
    [SerializeField] private float span = 2.0f;


    private void OnEnable() {
        StartCoroutine("Shoot_Cor");
    }

    private IEnumerator Shoot_Cor() {
        ShootSystem _shoot = GetComponent<ShootSystem>();
        Renderer _renderer = GetComponent<Renderer>();

        if (_shoot == null)
            yield break;

        yield return new WaitForSeconds(start_Time);        
        if (loop_Count < 0)
            loop_Count = 100;
        for (int i = 0; i < loop_Count; i++) {
            if(_renderer == null) {
                _shoot.Shoot();
                UsualSoundManager.Instance.Play_Shoot_Sound();
            }
            else if (_renderer.isVisible) {
                _shoot.Shoot();
                UsualSoundManager.Instance.Play_Shoot_Sound();
            }
            yield return new WaitForSeconds(span);
        }
    }
}
