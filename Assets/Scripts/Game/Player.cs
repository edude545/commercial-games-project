using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Written by Lyra

public class Player : MonoBehaviour {

    public static Player Instance;

    public Camera Camera;
    [HideInInspector] public GameObject LookTarget;
    public GameObject Tools;
    public GameObject Flashlight;
    public GameObject AnomalyFixer;
    public float ToolsTurnSpeed = 0.9f;
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
    public EndGame endGameScript;

    public AudioClip FlashlightOn;
    public AudioClip FlashlightOff;

    float pmx = 0f;
    float pmy = 0f;
    float mx = 0f;
    float my = 0f;

    bool scanning = false;
    float scanProgress = 0f;
    public float ScanTime = 20f;
    public UnityEngine.UI.Image ScanProgressBar;
    public AudioClip AnomalyFixerScanLoop;
    public AudioClip AnomalyFixerFail;

    Vector3 startPos;
    Rigidbody rb;
    private Collider coll;
    bool raycastedThisFrame = false;

    public bool ControlsLocked { get; private set; } = false;



    private void Awake() {
        rb = GetComponent<Rigidbody>();
        //col = GetComponent<Collider>();
        startPos = transform.position;
        Instance = this;
      
    }

    private void Start() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        currentFear = 0;  // Ensure currentFear starts at 0
        fear.SetMaxFear(maxFear);
        fear.SetFear(currentFear);
    }



    private void Update() {
        raycastedThisFrame = false;
        Vector3 dv = new Vector3(0, Noclip ? 0 : Gravity + rb.velocity.y, 0);
        if (scanning) {
            scanProgress += Time.deltaTime;
            ScanProgressBar.fillAmount = scanProgress / ScanTime;
            if (scanProgress >= ScanTime) {
                AnomalyFixer.GetComponent<AudioSource>().Stop();
                scanning = false;
                scanProgress = 0f;
                Interact();
            }
        }

        Tools.transform.rotation = Quaternion.Slerp(Tools.transform.rotation, Camera.transform.rotation, ToolsTurnSpeed);

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
            scanning = true;
            scanProgress = 0f;
            ScanProgressBar.gameObject.SetActive(true);
            AudioSource source = AnomalyFixer.GetComponent<AudioSource>();
            source.loop = true;
            source.clip = AnomalyFixerScanLoop;
            source.Play();
        }
        if (Input.GetMouseButtonUp(0)) {
            if (true) {
                scanning = false;
                scanProgress = 0f;
                ScanProgressBar.gameObject.SetActive(false);
                AnomalyFixer.GetComponent<AudioSource>().Stop();
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            GameObject f = Flashlight.transform.GetChild(0).gameObject;
            f.SetActive(!f.activeSelf);
            AudioSource source = Flashlight.GetComponent<AudioSource>();
            source.clip = Flashlight.activeSelf ? FlashlightOn : FlashlightOff;
            source.Play();
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

        if (Anomaly.activeAnomalyCount > 6)
        {
            TakeFearDamage(0.01f);
        }
        else if (Anomaly.activeAnomalyCount > 3)
        {
            TakeFearDamage(0.005f);
        }
        else if (Anomaly.activeAnomalyCount > 0)
        {
            TakeFearDamage(0.001f);
        }

        if (currentFear >= maxFear)
        {

            endGameScript.FadeToLevel(0);
            Anomaly.activeAnomalyCount = 0;
        }

        if (currentFear <= 0)
        {
            currentFear = 0;
        }
    }

    public void OnBlackoutEnd() {
    }

    // Try interaction raycast
    public void Interact() {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Anomaly", "House");
        ScanProgressBar.gameObject.SetActive(false);
        if (Physics.Raycast(transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit, InteractionDistance, layerMask)) {
            Debug.Log($"Hit object {hit.collider.name}");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Anomaly anom = hit.collider.GetComponent<Anomaly>();
            if (anom != null) {
                if (anom.IsTriggered) {
                    anom.OnInteract();
                    HealFear(20);
                }
                else {
                    TakeFearDamage(10);
                }
            }
        } else {
            AudioSource source = AnomalyFixer.GetComponent<AudioSource>();
            source.loop = false;
            source.clip = AnomalyFixerFail;
            source.Play();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            TakeFearDamage(10);
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

    void TakeFearDamage(float damage) {
        currentFear += damage;
        fear.SetFear(currentFear);
    }

    void HealFear(float heal) {
        currentFear -= heal;
        fear.SetFear(currentFear);
    }


    
}