using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ChaseMannequin : MonoBehaviour
{

    public bool Chasing = false;
    public bool Posing = true;
    public float KillDistance;

    private NavMeshAgent ai;
    private float moveSpeed;
    private Vector3 dest;

    public float JumpscareTime;
    private bool inKillSequence;

    private bool inView = false;

    private Vector3 playerSpawnPoint;
    private Vector3 spawnPoint;

    public int PoseCount;
    public Animator Animator;
    public Transform Head;

    public void StartChaseSequence() {
        playerSpawnPoint = Player.Instance.transform.position;
        spawnPoint = transform.position;
        Chasing = true;
        Posing = true;
        inKillSequence = false;
        ai.isStopped = false;
    }

    public void Start() {
        ai = GetComponent<NavMeshAgent>();
        moveSpeed = ai.speed;
    }

    void Update() {

        /*if (Input.GetKeyDown(KeyCode.RightBracket)) {
            Generator.Instance.SetAllLights(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket)) {
            Generator.Instance.SetAllLights(true);
        }
        */
        if (Input.GetKeyDown(KeyCode.K)) {
            StartChaseSequence();
        }

        if (inKillSequence) {
            return;
        }

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Player.Instance.Camera);
        float distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
        
        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds)) {
            inView = true;
            ai.speed = 0f;
            ai.SetDestination(transform.position);
        } else {
            if (inView) {
                int i = Random.Range(1, PoseCount);
                Animator.SetInteger("PoseID", i);
            }
            inView = false;
            ai.speed = Chasing ? moveSpeed : 0f;

            /*RaycastHit hit;
            if (Physics.Raycast(transform.position, (dest - transform.position).normalized, out hit, Mathf.Infinity)) {
                if (hit.transform != Player.Instance.transform) {
                    return; // There is an obstacle between AI and player, don't move
                }
            }*/

            ai.SetDestination(Player.Instance.transform.position);

            if (Chasing && distance <= KillDistance) {
                Debug.Log("Mannequin reached player, starting kill sequence");
                inKillSequence = true;
                ai.isStopped = true;
                Player.Instance.LockControls();
                Player.Instance.LockMouseLook();
                GetComponent<AudioSource>().Play();
                StartCoroutine(pointPlayerAtThis());
            }
        }

    }

    public float PlayerCameraTurnTime = 1f;
    IEnumerator pointPlayerAtThis() {
        float progress = 0f;
        Quaternion start = Player.Instance.Camera.transform.rotation;
        Quaternion end = Quaternion.LookRotation(Head.position - Player.Instance.Camera.transform.position);
        while (progress < PlayerCameraTurnTime) {
            progress += Time.deltaTime;
            Player.Instance.Camera.transform.rotation = Quaternion.Slerp(start, end, progress/PlayerCameraTurnTime);
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(killPlayer());
    }

    IEnumerator killPlayer() {
        yield return new WaitForSeconds(JumpscareTime);
        Player.Instance.transform.position = playerSpawnPoint;
        transform.position = spawnPoint;
        Player.Instance.UnlockControls();
        Player.Instance.UnlockMouseLook();
        StartChaseSequence();
        //SceneManager.LoadScene(sceneAfterDeath);
    }

}
