using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] MoveToTargetAgent agent;
    [SerializeField] float periodOfAttack = 3f;
    bool isAgentInTrigger;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(periodOfAttack);
        if (isAgentInTrigger)
        {

            agent.AddReward(-1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isAgentInTrigger = other.CompareTag("Player");
    }
}
