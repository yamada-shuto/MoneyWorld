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
    // 現在地
    private Vector3 m_CurrentPosition;
    // 目的地
    private Vector3 m_Destination;
    // Use this for initialization
    void Start ()
    {
        // エージェントを取得する
        m_Agent = GetComponent<NavMeshAgent>();
        // アニメーターを取得する
        m_Animator = GetComponent<Animator>();
        // 到着したかの判定用
        m_isArrived = false;
        // 現在地を設定する
        m_CurrentPosition = transform.position;
        // 目的地を設定する
        SetDestination();
        // エージェントの目的地を設定する
        if (m_Agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            m_Agent.SetDestination(m_Destination);
        }
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
            SetDestination();
            // エージェントの目的地を設定する
            m_Agent.SetDestination(m_Destination);
        }
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
}
