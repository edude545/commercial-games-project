using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testagent : MonoBehaviour
{

    public Transform goal;

    public float GoalRefreshFrequency = 10f;
    private float goalRefreshTimer = 0f;

    private void Start() {
        refreshGoal();
    }

    private void Update() {
        goalRefreshTimer += Time.deltaTime;
        if (goalRefreshTimer >= GoalRefreshFrequency) {
            goalRefreshTimer -= GoalRefreshFrequency;
            refreshGoal();
        }
    }

    private void refreshGoal() {
        GetComponent<NavMeshAgent>().destination = goal.position;
    }

}
