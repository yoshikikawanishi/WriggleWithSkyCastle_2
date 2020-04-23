using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchFairyBattleMovie : MonoBehaviour {

    [SerializeField] private float battle_Camera_Pos;
    [SerializeField] private Vector2 player_Initial_Pos;
    [SerializeField] private Vector2 enemy_Initial_Pos;

    private GameObject battle_Enemy;


    public void Start_Battle_Movie(GameObject battle_Enemy) {
        this.battle_Enemy = battle_Enemy;
        StartCoroutine("Battle_Movie_Cor");
    }


    private IEnumerator Battle_Movie_Cor() {
        //自機の動きを止める

        //フェードアウト

        yield return null;
        
        //画面内のアクティブな敵を消す
        List<GameObject> erased_Enemis = Erase_Visible_Enemy();

        //フェードイン

        //戦闘開始

        
    }


    //画面内のアクティブな敵を消す, 戦闘する敵は除く
    //消した敵を返す
    private List<GameObject> Erase_Visible_Enemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTag");
        List<GameObject> active_Enemies = new List<GameObject>();
        Renderer renderer;

        foreach (GameObject enemy in enemies) {
            if (!enemy.activeSelf || enemy == battle_Enemy)
                continue;
            renderer = enemy.GetComponent<Renderer>();
            if (renderer == null)
                continue;

            if (renderer.isVisible) {
                active_Enemies.Add(enemy);
                enemy.SetActive(false);
            }
        }

        return active_Enemies;
    }
}
