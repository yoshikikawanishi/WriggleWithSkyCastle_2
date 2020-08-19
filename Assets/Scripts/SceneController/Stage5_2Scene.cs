using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_2Scene : MonoBehaviour {

    [SerializeField] private MovieSystem soul_Enemy_Generator;
    [SerializeField] private MovieSystem big_Fairy_Generator;
    [Space]
    [SerializeField] private List<GenKind> gen_List = new List<GenKind>();

    //敵生成開始するかどうかのフラグ用
    private enum GenEnemyState {
        entry,
        active,
        finished
    }
    private GenEnemyState stage_State = GenEnemyState.entry;
    
    //敵生成開始・終了するカメラのx座標
    private const float START_GENERATE_ENEMY_LINE   = 120f;
    private const float FINISH_GENERATE_ENEMY_LINE  = 3000f;    

    //生成の種類
    public enum GenKind {
        soul_Enemy,
        big_Fairy,
    }           
      
    //その他オブジェクト
    private GameObject main_Camera;


    void Start () {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");       
    }


    void Update () {        
        if (stage_State == GenEnemyState.entry) {
            //敵生成開始
            if (main_Camera.transform.position.x > START_GENERATE_ENEMY_LINE) {
                StartCoroutine("Generating_Flow_Cor");
                stage_State = GenEnemyState.active;
            }
        }        
        else if(stage_State == GenEnemyState.active) {            
            //敵生成終了
            if (main_Camera.transform.position.x > FINISH_GENERATE_ENEMY_LINE) {
                StopAllCoroutines();
                stage_State = GenEnemyState.finished;
            }
        }
	}


    //大元の敵生成コルーチン
    private IEnumerator Generating_Flow_Cor() {
        int list_Size = gen_List.Count;
        //特定の生成を順番に実行していく
        for(int i = 0; i < list_Size; i++) {
            switch (gen_List[i]) {
                //亡霊編隊生成
                case GenKind.soul_Enemy:
                    soul_Enemy_Generator.Start_Movie();
                    yield return new WaitForSeconds(Span(GenKind.soul_Enemy));
                    break;
            }
        }
    }


    //生成後、次の生成までの待ち時間
    public float Span(GenKind kind) {
        switch (kind) {
            case GenKind.soul_Enemy: return 10.0f;
            case GenKind.big_Fairy: return 4.0f;
        }
        return 0;
    }
}
