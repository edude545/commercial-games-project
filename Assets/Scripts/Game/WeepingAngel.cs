using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// code used with combination of chatGPT and www.youtube.com/watch?v=_e57zSZSOS8&t=312s
public class WeepingAngel : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    Vector3 dest;
    public Camera playerCam, jumpscareCam;
    public float aiSpeed;
    public float catchDistance;
    public float jumpscareTime;
    public string sceneAfterDeath;

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        float distance = Vector3.Distance(transform.position, player.position);

        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds))
        {
            ai.speed = 0;
            ai.SetDestination(transform.position);
        }

        if (!GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds))
        {
            ai.speed = aiSpeed;
            dest = player.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (dest - transform.position).normalized, out hit, Mathf.Infinity))
            {
                if (hit.transform != player)
                {
                    return; // There is an obstacle between AI and player, don't move
                }
            }

            ai.destination = dest;

            if (distance <= catchDistance)
            {
                player.gameObject.SetActive(false);
                jumpscareCam.gameObject.SetActive(true);
                StartCoroutine(killPlayer());
            }
        }
    }

    IEnumerator killPlayer()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(sceneAfterDeath);
    }
}