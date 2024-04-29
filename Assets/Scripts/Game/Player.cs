using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Written by Lyra

public class Player : MonoBehaviour {

    public Camera Camera;
    [HideInInspector] public GameObject LookTarget;
    public GameObject Flashlight;

    public float Speed = 0.1f;
    public float JumpPower = 60f;
    public float Sensitivity = 3f;
    public float Gravity = -1f;
    public bool Noclip = false;
    public float ScrollSensitivity = 1f;
    public float InteractionDistance = 2.2f;

    public float maxFear = 100;
    public float currentFear;
    public HealthBar fear;

    float pmx = 0f;
    float pmy = 0f;
    float mx = 0f;
    float my = 0f;

    Vector3 startPos;
    Rigidbody rb;
    private Collider coll;
    bool raycastedThisFrame = false;

    public bool ControlsLocked { get; private set; } = false;

    public static Player Instance;
  

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        //col = GetComponent<Collider>();
        startPos = transform.position;
        Instance = this;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        currentFear = 0;
        fear.SetMaxFear(maxFear);
    }

    private void Update() {
        raycastedThisFrame = false;
        Vector3 dv = new Vector3(0, Noclip ? 0 : Gravity + rb.velocity.y, 0);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
        }
        if (Input.GetMouseButtonDown(2)) {
            Camera.fieldOfView = 60f;
        }
        if (Input.GetKeyDown(KeyCode.F12)) {
            ScreenCapture.CaptureScreenshot("screenshot");
        }
        if (Input.GetMouseButtonDown(0)) {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            Flashlight.SetActive(!Flashlight.activeSelf);
        }
        if (!GetControlsLocked()) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.V)) {
                Noclip = !Noclip;
                GetComponent<CapsuleCollider>().enabled = !Noclip;
                Debug.Log("Noclip = " + Noclip);
            }

            //float speedmul;
           // if (Input.GetKey(KeyCode.LeftShift)) { speedmul = 3f; } else if (Input.GetKey(KeyCode.LeftControl)) { speedmul = 0.2f; } else { speedmul = 1f; }

            if (Noclip) {
                dv = Camera.transform.rotation * new Vector3(
                    Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
                    Input.GetKey(KeyCode.E) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0,
                    Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
                ) * /*speedmul * */ Speed;
            } else {
                dv += Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0) * new Vector3(
                    (Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0) * /*speedmul * */  Speed,
                    Input.GetKeyDown(KeyCode.Space) ? JumpPower : 0,
                    (Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0) * /*speedmul * */  Speed
                );
            }

            // Only rotate camera and do raycast if the mouse has been moved

            mx += Input.GetAxis("Mouse X") * Sensitivity;
            my = Mathf.Clamp(my + Input.GetAxis("Mouse Y") * Sensitivity, -90, 90);

            if (mx != pmx || my != pmy) { // When mouse is moved:
                pmx = mx; pmy = my;
                Camera.transform.rotation = Quaternion.Euler(-my, mx, 0);
                if (!raycastedThisFrame) {
                    DoRaycast();
                }
            }
        }

        rb.velocity = dv;

        // Out of bounds check
        if (transform.position.y < -20) {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }

        TakeFearDamage(0.001f);

    }

    // Try interaction raycast
    public void Interact() {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Anomaly", "House");
        //int layerMask = (1 << 9) & (1 << 10); // anomaly layer and house layer
        if (Physics.Raycast(transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit, InteractionDistance, layerMask)) {
            Debug.Log($"Hit object {hit.collider.name}");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Anomaly anom = hit.collider.GetComponent<Anomaly>();
            if (anom != null) {
                anom.OnInteract();
                HealFear(10);
            }
        } else {
            //Debug.Log("Did not hit anything");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            TakeFearDamage(20);
        }
    }

    public bool GetControlsLocked() {
        return ControlsLocked;
    }

    public void LockControls() {
        ControlsLocked = true;
    }

    public void UnlockControls() {
        ControlsLocked = false;
    }

    protected void DoRaycast() {
        raycastedThisFrame = true;
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        LookTarget = null;
        if (Physics.Raycast(ray, out rayHit)) {
            LookTarget = rayHit.transform.gameObject;
        }
    }

    void TakeFearDamage(float damage)
    {
        currentFear += damage;
        fear.SetFear(currentFear);
    }

    void HealFear(float heal)
    {
        currentFear -= heal;
        fear.SetFear(currentFear);
    }

}