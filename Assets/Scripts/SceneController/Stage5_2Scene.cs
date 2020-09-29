using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_2Scene : MonoBehaviour {

    [SerializeField] private GeneratorSystem soul_Enemy_Generator;
    [SerializeField] private GeneratorSystem big_Fairy_Generator;
    [SerializeField] private GeneratorSystem yin_Ball_Generator;
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
    private const float FINISH_GENERATE_ENEMY_LINE  = 3800f;    

    //生成の種類
    public enum GenKind {
        soul_Enemy,
        big_Fairy,
        yin_Ball,
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
        yield return new WaitForSeconds(1.0f);
        int list_Size = gen_List.Count;
        //特定の生成を順番に実行していく
        for(int i = 0; i < list_Size; i++) {
            switch (gen_List[i]) {
                //亡霊編隊生成
                case GenKind.soul_Enemy:
                    soul_Enemy_Generator.Start_Generate();
                    yield return new WaitForSeconds(Span(GenKind.soul_Enemy));
                    break;
                //ひまわり妖精生成
                case GenKind.big_Fairy:
                    big_Fairy_Generator.Start_Generate();
                    yield return new WaitForSeconds(Span(GenKind.big_Fairy));
                    break;
                //陰陽玉生成
                case GenKind.yin_Ball:
                    yin_Ball_Generator.Start_Generate();
                    yield return new WaitForSeconds(Span(GenKind.yin_Ball));
                    break;
            }
        }

        StartCoroutine("Generating_Flow_Cor");
    }


    //生成後、次の生成までの待ち時間
    public float Span(GenKind kind) {
        switch (kind) {
            case GenKind.soul_Enemy: return 7.0f;
            case GenKind.big_Fairy: return 22.0f;
            case GenKind.yin_Ball: return 15.0f;
        }
        return 0;
    }
}
