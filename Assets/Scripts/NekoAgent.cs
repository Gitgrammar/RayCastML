using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
public class NekoAgent : Agent
{
    public Transform target;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    //エピソード開始時に呼ばれる
    public override void OnEpisodeBegin()
    {
        //agentの落下時
        if (transform.localPosition.y <0)
        {
            //リセット
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            transform.localPosition = Vector3.zero;

        }
        //targetの位置もリセット
        target.localPosition = new Vector3(
            Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    // 行動実行時に呼ばれるメソッド
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        int action = actions.DiscreteActions[0];

        if (action == 1) dirToGo = transform.forward;
        if (action == 2) dirToGo = transform.forward *-1.0f;
        if (action == 3) rotateDir = transform.up *-1.0f;
        if (action == 4) rotateDir = transform.up;
        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        rb.AddForce(dirToGo * 0.4f, ForceMode.VelocityChange);



        //RaycastAgentがtargetの位置に到着
        float distanceToTarget = Vector3.Distance(
            transform.localPosition, target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            EndEpisode();
        }
        //RaycastAgent が落下
        if (this.transform.localPosition.y < -0.1f)
        {
            EndEpisode();
        }
    }
    //ヒューリスティクモードの行動決定時に呼ばれる
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow)) actions[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow)) actions[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) actions[0] = 3;
        if (Input.GetKey(KeyCode.RightArrow)) actions[0] = 4;
    }
}
