using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
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
    public bool MouseLookLocked { get; private set; } = false;

    public bool EnableDebugHotkeys = false;

    public GameObject PauseMenu;
    public GameObject SettingsMenu;

    public AudioSource FootstepSource;
    private bool playedFootstepsLastFrame;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        //col = GetComponent<Collider>();
        startPos = transform.position;
        Instance = this;
        playedFootstepsLastFrame = false;
    }

    private void Start() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        currentFear = 0;  // Ensure currentFear starts at 0
        Anomaly.anomalyCount = 0;
        fear.SetMaxFear(maxFear);
        fear.SetFear(currentFear);
        HidePauseMenu();
    }

    public void ShowPauseMenu() {
        PauseMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        LockControls();
        LockMouseLook();
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        StopScanning();
        StopFootsteps();
    }

    public void HidePauseMenu() {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        UnlockControls();
        UnlockMouseLook();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // For now, assume this can be called when the pause menu is not active
    public void ShowSettingsMenu() {
        PauseMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        LockControls();
        LockMouseLook();
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        StopScanning();
        StopFootsteps();
    }

    public void QuitToDesktop() {
        
        Application.Quit();
    }

    public void QuitToTitleScreen() {
        SceneManager.LoadScene("Title");
    }

    public void StartScanning() {
        scanning = true;
        scanProgress = 0f;
        ScanProgressBar.gameObject.SetActive(true);
        AudioSource source = AnomalyFixer.GetComponent<AudioSource>();
        source.loop = true;
        source.clip = AnomalyFixerScanLoop;
        source.Play();
    }

    public void StopScanning() {
        scanning = false;
        scanProgress = 0f;
        ScanProgressBar.gameObject.SetActive(false);
        AnomalyFixer.GetComponent<AudioSource>().Stop();
    }

    public void StartFootsteps() {
        if (!playedFootstepsLastFrame) {
            playedFootstepsLastFrame = true;
            FootstepSource.Play();
        }
    }

    public void StopFootsteps() {
        if (playedFootstepsLastFrame) {
            playedFootstepsLastFrame = false;
            FootstepSource.Stop();
        }
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
            if (PauseMenu.activeSelf) {
                HidePauseMenu();
            } else {
                ShowPauseMenu();
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
        }
        
        if (Input.GetKeyDown(KeyCode.F12)) {
            ScreenCapture.CaptureScreenshot("screenshot");
        }
        if (!GetControlsLocked()) {
            if (Input.GetMouseButtonDown(2)) {
                Camera.fieldOfView = 60f;
            }
            if (Input.GetMouseButtonDown(0)) {
                StartScanning();
            }
            if (Input.GetMouseButtonUp(0)) {
                if (true) {
                    StopScanning();
                }
            }
            if (Input.GetKeyDown(KeyCode.F)) {
                GameObject f = Flashlight.transform.GetChild(0).gameObject;
                f.SetActive(!f.activeSelf);
                AudioSource source = Flashlight.GetComponent<AudioSource>();
                source.clip = Flashlight.activeSelf ? FlashlightOn : FlashlightOff;
                source.Play();
            }
            if (EnableDebugHotkeys) {
                if (Input.GetKeyDown(KeyCode.J)) {
                    Anomaly.anomalyCount++;
                }
                if (Input.GetKeyDown(KeyCode.H)) {
                    Speed += 1;
                }
                if (Input.GetKeyDown(KeyCode.V)) {
                    Noclip = !Noclip;
                    GetComponent<CapsuleCollider>().enabled = !Noclip;
                    StopFootsteps();
                    Debug.Log("Noclip = " + Noclip);
                }
                if (Input.GetKeyDown(KeyCode.RightBracket)) {
                    Generator.Instance.SetAllLights(false);
                }
                if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                    Generator.Instance.SetAllLights(true);
                }
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
                float ad = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
                float ws = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
                dv += Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0) * new Vector3(
                    ad * /*speedmul * */  Speed,
                    Input.GetKeyDown(KeyCode.Space) ? JumpPower : 0,
                    ws * /*speedmul * */  Speed
                );
                if (ws == 0 && ws == 0) {
                    StopFootsteps();
                } else {
                    StartFootsteps();
                }
            }

            // Only rotate camera and do raycast if the mouse has been moved

            mx += Input.GetAxis("Mouse X") * Sensitivity;
            my = Mathf.Clamp(my + Input.GetAxis("Mouse Y") * Sensitivity, -90, 90);

            if (!MouseLookLocked) {
                if (mx != pmx || my != pmy) { // When mouse is moved:
                    pmx = mx; pmy = my;
                    Camera.transform.rotation = Quaternion.Euler(-my, mx, 0);
                    if (!raycastedThisFrame) {
                        DoRaycast();
                    }
                }
            }
        }

        rb.velocity = dv;

        // Out of bounds check
        if (transform.position.y < -20) {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }

        /*if (Anomaly.activeAnomalyCount > 6)
        {
            TakeFearDamage(0.01f);
        }*/
        else if (Anomaly.activeAnomalyCount > 5)
        {
            TakeFearDamage(0.008f);
        }
        else if (Anomaly.activeAnomalyCount > 0)
        {
            TakeFearDamage(0.001f);
        }

        if (currentFear >= maxFear)
        {
            
            endGameScript.FadeToLevel(5);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
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
            //Anomaly anom = hit.collider.GetComponent<Anomaly>();
            IAnomalyFixerTarget target = hit.collider.GetComponent<IAnomalyFixerTarget>();
            if (target != null) {
                if (target.CanBeInteractedWith()) {
                    target.OnInteract();
                    HealFear(20);
                }
                else {
                    TakeFearDamage(10);
                    AudioSource source = AnomalyFixer.GetComponent<AudioSource>();
                    source.loop = false;
                    source.clip = AnomalyFixerFail;
                    source.Play();
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

    public bool GetMouseLookLocked() {
        return MouseLookLocked;
    }

    public void LockMouseLook() {
        MouseLookLocked = true;
    }

    public void UnlockMouseLook() {
        MouseLookLocked = false;
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