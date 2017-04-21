using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {

float BALL_FORCE = 1800;

    /** 砲弾 */
Transform ballPrefab;
    /** 爆発 */
 ParticleSystem particlePrefab;
    /** 爆発 */
    ParticleSystem particle = null;
    /** 地形 */
    Terrain terrain;

    /** 砲弾存在 */
    bool ballExists = false;
    /** 砲弾位置 */
    Vector3 ballPosition;

    /** マウスボタン */
    bool mouseDown = false;
    /** マウス位置 */
    Vector3 mousePos;
    /** タッチ */
    bool touchDown = false;
    /** タッチ位置 */
    Vector3 touchPos;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (MainScript.STATE_RUNNING != MainScript.state)
        { // not ゲーム状態: ゲーム中
            return;
        }
        float angleResolution = 180.0f / Mathf.Min(Screen.width, Screen.height); // 操作回転量: 画面短辺 -> 180度
        Vector3 distance = Vector3.zero; // 入力移動量
        bool fire = false; // 発射
        if (gameObject.CompareTag("Player"))
        {
            // 自分
            // mouse drag: 移動量を求める
            Vector3 mousePosPrev = mousePos;
            mousePos = Input.mousePosition;
            if (mouseDown)
            {
                distance = mousePos - mousePosPrev;
            }
            // mouse down
            if (Input.GetMouseButtonDown(0))
            {
                mouseDown = true;
            }
            // mouse up: 発射
            if (Input.GetMouseButtonUp(0))
            {
                fire = mouseDown;
                mouseDown = false;
            }
            // touch swipe
            Vector3 touchPosPrev = touchPos;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    // touch down
                    case TouchPhase.Began:
                        touchDown = true;
                        touchPos = touch.position;
                        break;
                    // touch swipe: 移動量を求める
                    case TouchPhase.Moved:
                        touchPos = touch.position;
                        distance = touchPos - touchPosPrev;
                        break;
                    // touch up: 発射
                    case TouchPhase.Ended:
                        fire = touchDown;
                        touchDown = false;
                        touchPos = touch.position;
                        distance = touchPos - touchPosPrev;
                        break;
                }
            }
            // mouse or touch 移動量 -> 回転
            // 縦方向: x軸回転
            // 横方向: y軸回転
            if (Vector3.zero != distance)
            {
                Vector3 v = new Vector3(distance.y, distance.x, 0);
                Vector3 angle = transform.eulerAngles - v * angleResolution;
                angle.x = Mathf.Max(0, Mathf.Min(80, angle.x));
                angle.y = Mathf.Max(80, Mathf.Min(190, (angle.y + 180) % 360)); // -100..10
                angle.y = (angle.y + 180) % 360;
                transform.eulerAngles = angle;
                MainScript.Log("eulerAngles", "eulerAngles=" + angle + "," + transform.eulerAngles);
            }
        }
        else
        {
            // 相手
            if (!ballExists)
            { // not 砲弾存在
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                // 横回転量 = (プレイヤーとの角度 - 砲弾との角度) / 2 + ランダム
                float playerAngle = Mathf.Atan2(
                    player.transform.position.z - transform.position.z,
                    player.transform.position.x - transform.position.x) * 180 / Mathf.PI;
                float ballAngle = Mathf.Atan2(
                    ballPosition.z - transform.position.z,
                    ballPosition.x - transform.position.x) * 180 / Mathf.PI;
                distance.y -= (playerAngle - ballAngle) / 2 + Random.value * 2 - 1;
                // 縦回転量 = (プレイヤーとの距離 - 砲弾との距離) / 10 + ランダム
                float magnitude= (player.transform.position - transform.position).magnitude
                    - (ballPosition - transform.position).magnitude;
                distance.x += magnitude / 10 + Random.value * 2 - 1;
                // 回転
                Vector3 v = new Vector3(distance.x, distance.y, 0);
                transform.eulerAngles += v;
                // 発射。
                fire = true;
            }
        }
        // 発射。
        if (fire && !ballExists)
        {
            // 発射角度
            float y = Mathf.Cos(2 * Mathf.PI * transform.eulerAngles.x / 360);
            float r = Mathf.Sin(2 * Mathf.PI * transform.eulerAngles.x / 360);
            float x = r * Mathf.Sin(2 * Mathf.PI * transform.eulerAngles.y / 360);
            float z = r * Mathf.Cos(2 * Mathf.PI * transform.eulerAngles.y / 360);
            Vector3 angle = new Vector3(x, y, z);
            // インスタンスの生成
            Rigidbody ball = (Rigidbody)Instantiate(ballPrefab, transform.FindChild("Cylinder").position + angle * 5, Quaternion.identity);
            ballExists = true;
            // 力を加える
            ball.AddForce(angle * BALL_FORCE);
            MainScript.Log("AddForce", "AddForce=" + angle * BALL_FORCE + "," + (angle * BALL_FORCE).magnitude);
            // 発射元の砲台
            ball.GetComponent(ballscript).cannon = transform;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        MainScript.Log("cannon collision", "cannon collision: " + collision.gameObject.name + "," + collision.contacts[0].point);
        if ("ball(Clone)" == collision.gameObject.name)
        {
            // 砲台非表示
            gameObject.SetActive(false);
            // 破壊エフェクト。
            // インスタンスの生成
            particle = (ParticleSystem)Instantiate(particlePrefab, transform.position, Quaternion.identity);
            // 勝ち負け
            if (MainScript.STATE_RUNNING == MainScript.state)
            {
                MainScript.state = (gameObject.CompareTag("Player") ? MainScript.STATE_LOSE : MainScript.STATE_WIN);
            }
        }
    }

    /**
     * 初期化。
     */
    void Init()
    {
        ballExists = false;
        mouseDown = false;
        touchDown = false;
        // 破壊エフェクト破棄
        if (null != particle)
        {
            Destroy(particle);
            particle = null;
        }
        // 砲台角度・位置
        if (gameObject.CompareTag("Player"))
        {
            // プレイヤー
            transform.eulerAngles = new Vector3(45, 315, 0);
            Vector3 v = new Vector3(100 - 20 * Random.value, transform.position.y, 20 * Random.value);
            transform.position = v;
        }
        else
        {
            // 対戦相手
            transform.eulerAngles = new Vector3(45, 135, 0);
            ballPosition = new Vector3(100 * Random.value, 0, 100 * Random.value);
            Vector3 v = new Vector3(20 * Random.value, transform.position.y, 100 - 20 * Random.value);
            transform.position = v;
        }
        // 砲台の地形の高さ
        
        MainScript.Log("height", "height=" + transform.position.y);
        // 砲台表示
        gameObject.SetActive(true);
    }

}
