using UnityEngine;
using System.Collections;

public class ballscript : MonoBehaviour
{

    /** 発射元の砲台 */
    public Transform cannon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cannon.position = transform.position;
        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        MainScript.Log("ball collision", "ball collision: " + collision.gameObject.name + "," + collision.contacts[0].point);
        // 地形なら破棄
        if ("Terrain" == collision.gameObject.name)
        {
            Destroy(gameObject, 3); // 3秒後に破棄
        }
    }
    void OnDestroy()
    {
        // 破棄されたら砲台側の砲弾生存フラグをクリア
        if (null != cannon)
        {
            cannon.hasChanged = false; // 砲弾存在
        }
    }
}
