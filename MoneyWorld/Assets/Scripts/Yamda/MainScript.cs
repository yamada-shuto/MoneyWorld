using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour
{

    /** ゲーム状態: 開始 */
    public const int STATE_START = 0;
    /** ゲーム状態: ゲーム中 */
    public const int STATE_RUNNING = 1;
    /** ゲーム状態: 勝ち */
    public const int STATE_WIN = 2;
    /** ゲーム状態: 負け */
    public const int STATE_LOSE = 3;

    /** ゲーム状態 */
    public static int state = STATE_START; // ゲーム状態: 開始
                                    /** デバッグテキスト */
    public static Hashtable debugTexts = new Hashtable();
    /** プレーヤー */
    public static Transform player;
    /** 対戦相手 */
    public Transform other;
    GUIText label;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE_START: // ゲーム状態: 開始
                label.text = "START";
                label.enabled = true;
                break;
            case STATE_RUNNING: // ゲーム状態: ゲーム中
                break;
            case STATE_WIN: // ゲーム状態: 勝ち
                label.text = "YOU WIN";
                label.enabled = true;
                break;
            case STATE_LOSE: // ゲーム状態: 負け
                label.text = "WIN LOSE";
                label.enabled = true;
                break;
        }
        // click or touch start
        if (label.enabled)
        {
            if (Input.GetMouseButtonUp(0)
                    || (Input.touchCount > 0) && (TouchPhase.Ended == Input.GetTouch(0).phase))
            {
                state = STATE_RUNNING; // ゲーム状態: ゲーム中
                label.enabled = false;
                player.GetComponent<Transform>();
                other.GetComponent<Transform>();
            }

        }
    }

    void OnGUI()
    {
        // デバッグテキスト表示
        
        for (int y = 1; y > 0;y++)
        {
            object k = debugTexts[y];
            Rect rect = new Rect(0, y * 20, 400, 20);
            GUI.Label(rect, debugTexts[k].ToString());
        }
    }
    internal static void Log(string key, string text)
    {
        //	// デバッグテキスト表示登録
        debugTexts[key] = text;
        //	// デバッグテキストログ出力
        print(text);
    }
}