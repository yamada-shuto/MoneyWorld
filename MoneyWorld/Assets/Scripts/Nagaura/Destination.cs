using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {

    // 現在地
    private Vector3 m_CurrentPosition;
    // 目的地
    private Vector3 m_Destination;
    // Use this for initialization
    void Start ()
    {
        // 現在地を設定する
        m_CurrentPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // 目的地を設定する
    public void SetDestination()
    {
        // 現在地を設定する
        m_CurrentPosition = transform.position;
        // ランダムなVector2の値を取得
        Vector2 randDestination = Random.insideUnitCircle * 8;
        // 現在地から計算して目的地を設定する
        m_Destination = m_CurrentPosition + new Vector3(randDestination.x, 0.0f, randDestination.y);
    }

    // 目的地を取得する
    public Vector3 GetDestination(){ return m_Destination; }
}
