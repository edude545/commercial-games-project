using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : Anomaly {

    public int PoseCount;

    private Animator animator;

    private void Start() {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public override void OnAnomalyFixed() {
        animator.SetInteger("PoseID", 0);
    }

    public override void OnAnomalyTriggered() {
        animator.SetInteger("PoseID", Random.Range(1, PoseCount));
    }

}
