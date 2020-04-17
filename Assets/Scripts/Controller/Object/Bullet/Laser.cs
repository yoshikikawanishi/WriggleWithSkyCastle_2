using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(CircleColliderPool))]
public class Laser : MonoBehaviour {

    private List<Vector2> points;
    List<CircleCollider2D> colliders = new List<CircleCollider2D>();    

    private struct section {
        public Vector2 direction;   // 方向ベクトル.
        public Vector2 left;        // セクションの左端.
        public Vector2 right;       // セクションの右側.
    }
    private section[] sections;    

    [SerializeField] Material laserMat;
    [SerializeField, Range(0, 30)] public float laserWidth = 5;
    [SerializeField, Range(0, 5f)] float quality = 3;    
    [SerializeField] int length = 200;
    [SerializeField] int sortingOrder = 5;
    [SerializeField] private bool play_On_Awake = true;
    
    private bool enable = false;

    private float appendDistance;
    private float appendSqrDistance;
    private int maxPointCount;

    private Vector3 start_Pos;
    private float start_Angle;


    void Awake() {
        appendDistance = 8f - quality;
        appendSqrDistance = Mathf.Pow(appendDistance, 2);
        maxPointCount = (int)(length / appendDistance);       
    }

    void OnEnable() {        
        if (play_On_Awake) {
            Launch();            
        }        
    }

    void Start() {
        if(transform.parent == null) {
            Debug.Log("Laser Need Parent");
            Stop();
        }
    }

    void Update() {
        if (enable) {
            //メッシュの生成
            setPoints();
            setVectors();
            createMesh();            
        }
    }

    void LateUpdate() {
        //座標を固定する
        Fix_Body();
    }


    void setPoints() {
        var point = transform.parent.position;

        if (points == null) {
            points = new List<Vector2>();
            points.Add(point);
        }
        // ポイントの追加.
        addPoint(point);        

        while (points.Count > maxPointCount) {
            points.RemoveAt(0);
            colliders[0].enabled = false;
            colliders.RemoveAt(0);
        }

    }


    void setVectors() {
        // 2つ以上セクションを用意できない状態の場合処理を抜ける.
        if (points == null || points.Count <= 1) return;

        sections = new section[points.Count];

        for (int i = 0; i < points.Count; i++) {
            // ----- 方向ベクトルの計算 -----
            if (i == 0) {
                // 始点の場合.
                sections[i].direction = points[i + 1] - points[i];
            }
            else if (i == points.Count - 1) {
                // 終点の場合.
                sections[i].direction = points[i] - points[i - 1];
            }
            else {
                // 途中の場合.
                sections[i].direction = points[i + 1] - points[i - 1];
            }

            sections[i].direction.Normalize();

            // ----- 方向ベクトルに直交するベクトルの計算 -----
            Vector2 side = Quaternion.AngleAxis(90f, -Vector3.forward) * sections[i].direction;
            side.Normalize();

            sections[i].left = points[i] - (Vector2)start_Pos - side * laserWidth / 2f;
            sections[i].right = points[i] - (Vector2)start_Pos + side * laserWidth / 2f;
        }
    }
    

    void createMesh() {
        if (points == null || points.Count <= 1) return;

        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Mesh mesh = mf.mesh = new Mesh();

        mesh.name = "CurveLaserMesh";

        int meshCount = points.Count - 1;                   // 四角メッシュ生成数はセクション - 1.

        Vector3[] vertices = new Vector3[(meshCount) * 4];  // 四角なので頂点数は1つのメッシュに付き4つ.
        Vector2[] uvs = new Vector2[vertices.Length];       //マテリアルのuv座標
        int[] triangles = new int[(meshCount) * 2 * 3];     // 1つの四角メッシュには2つ三角メッシュが必要. 三角メッシュには3つの頂点インデックスが必要.

        // ----- 頂点座標の割り当て -----
        for (int i = 0; i < meshCount; i++) {
            vertices[i * 4 + 0] = sections[i].left;
            vertices[i * 4 + 1] = sections[i].right;
            vertices[i * 4 + 2] = sections[i + 1].left;
            vertices[i * 4 + 3] = sections[i + 1].right;

            var step = (float)1 / meshCount;
            
            uvs[i * 4 + 0] = new Vector2(0f, i * step);
            uvs[i * 4 + 1] = new Vector2(1f, i * step);
            uvs[i * 4 + 2] = new Vector2(0f, (i + 1) * step);
            uvs[i * 4 + 3] = new Vector2(1f, (i + 1) * step);
            
        }

        // ----- 頂点インデックスの割り当て -----
        int positionIndex = 0;

        for (int i = 0; i < meshCount; i++) {
            triangles[positionIndex++] = (i * 4) + 1;
            triangles[positionIndex++] = (i * 4) + 0;
            triangles[positionIndex++] = (i * 4) + 2;

            triangles[positionIndex++] = (i * 4) + 2;
            triangles[positionIndex++] = (i * 4) + 3;
            triangles[positionIndex++] = (i * 4) + 1;
        }
        
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mr.material = laserMat;
        mr.sortingOrder = sortingOrder;        
    }    


    void addPoint(Vector2 point) {
        while (true) {
            var distance = (point - points[points.Count - 1]);
            if (distance.sqrMagnitude < appendSqrDistance) break;

            distance *= appendDistance / distance.magnitude;
            points.Add(points[points.Count - 1] + distance);
            
            addCollider(points[points.Count - 1]);
        }
    }


    void addCollider(Vector2 point) {
        CircleCollider2D cc = GetComponent<CircleColliderPool>().Get_Collider();
        colliders.Add(cc);
        cc.offset = point - (Vector2)start_Pos;
    }


    void Fix_Body() {
        transform.position = start_Pos;
        transform.rotation = Quaternion.Euler(0, 0, start_Angle);
    }


    public void Launch() {        
        enable = true;
        start_Pos = transform.position;
        start_Angle = transform.rotation.z;
        sections = null;
        points = null;
        colliders.Clear();
        GetComponent<CircleColliderPool>().Set_Inactive_All();
    }


    public void Stop() {
        enable = false;
    }    
}