using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] spawnBotPoints;
    [SerializeField] Transform[] spawnTargetPoints;
    [SerializeField] LayerMask deathWall;


    [SerializeField] float speed = 2f;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(spawnBotPoints[0].localPosition.x, spawnBotPoints[1].localPosition.x), 1,
            Random.Range(spawnBotPoints[0].localPosition.z, spawnBotPoints[1].localPosition.z));
        target.localPosition = new Vector3(Random.Range(spawnTargetPoints[0].localPosition.x, spawnTargetPoints[1].localPosition.x), 1,
            Random.Range(spawnTargetPoints[0].localPosition.z, spawnTargetPoints[1].localPosition.z));
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(CheckWallClose(transform.position + Vector3.left *0.5f));
        //sensor.AddObservation(CheckWallClose(transform.position + Vector3.left * -0.5f));
    }

    private float CheckWallClose(Vector3 startPos)
    {
        bool isTouching = Physics.Raycast(startPos, transform.forward, 1f, deathWall);
        Debug.Log(isTouching);
        Debug.DrawRay(startPos, transform.forward, Color.red);
        float result = isTouching ? 1f : 0f;
        return result;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveX = vectorAction[0];
        float moveZ = vectorAction[1];

        transform.position += new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[0] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathWall"))
        {
            SetReward(-0.5f);
            EndEpisode();
        }
        else if (other.CompareTag("Target"))
        {
            SetReward(2f);
            EndEpisode();
        }
    }

    public void AddReward(float reward)
    {
        SetReward(reward);
    }
}
