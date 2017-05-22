using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // エージェント
    private NavMeshAgent m_Agent;
    // アニメーター
    private Animator m_Animator;
    // 到着したか判定用
    private bool m_isArrived;
    // 目的地
    private Vector3 m_Destination;
    // 目的地リスト
    public Vector3[] m_DestinationList;
    // 
    public int m_Iterator;
    // Use this for initialization
    void Start ()
    {
        //
        m_Iterator = 0;
        m_DestinationList = new Vector3[4];
        m_DestinationList[0] = new Vector3(0.0f, 0.0f, 2.0f);
        m_DestinationList[1] = new Vector3(4.0f, 0.0f, 2.0f);
        m_DestinationList[2] = new Vector3(4.0f, 0.0f, 6.0f);
        m_DestinationList[3] = new Vector3(7.0f, 0.0f, 6.0f);
        // エージェントを取得する
        m_Agent = GetComponent<NavMeshAgent>();
        // アニメーターを取得する
        m_Animator = GetComponent<Animator>();
        // 到着したかの判定用
        m_isArrived = false;
        // 目的地を設定する
        m_Destination = m_DestinationList[m_Iterator++];
        // エージェントの目的地を設定する
        if (m_Agent.pathStatus != NavMeshPathStatus.PathInvalid){ m_Agent.SetDestination(m_Destination); }
    }

    // Update is called once per frame
    void Update()
    {
        // 到着していたらなにもしない
        if (m_isArrived)
            return;
        // アニメーターのSpeedの値を設定する
        m_Animator.SetFloat("Speed", m_Agent.speed);
        // 目的地から現在の位置までの距離が1未満か判定する
        if (Vector3.Distance(m_Destination, transform.position) < 0.8f)
        {
            // アニメーターのSpeedの値に0を設定する
            m_Animator.SetFloat("Speed", 0.0f);
            // 目的地を設定する
            m_Destination = m_DestinationList[m_Iterator++];
            // エージェントの目的地を設定する
            m_Agent.SetDestination(m_Destination);
            if (m_Iterator >= m_DestinationList.Length){ m_isArrived = true; }
        }
    }
}
