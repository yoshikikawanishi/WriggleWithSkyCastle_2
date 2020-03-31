using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlowerFairy1 : MonoBehaviour {

    [SerializeField] private GameObject bullet;

	
	void OnEnable () {
        StartCoroutine("Shoot_Cor");
	}

    private IEnumerator Shoot_Cor() {
        ShootFunction _shoot = GetComponent<ShootFunction>();
        ObjectPool pool = ObjectPoolManager.Instance.Get_Pool(bullet);
        _shoot.Set_Bullet_Pool(pool, null);
        while (true) {
            yield return new WaitForSeconds(1.5f);

            if (GetComponent<Renderer>().isVisible) {
                UsualSoundManager.Instance.Play_Shoot_Sound();
                _shoot.Diffusion_Bullet(8, 60f, 0, 10f);
                _shoot.Diffusion_Bullet(8, 55f, 4f, 10f);
                _shoot.Diffusion_Bullet(8, 55f, -4f, 10f);
            }

            yield return new WaitForSeconds(2.7f);
        }
    }
}
