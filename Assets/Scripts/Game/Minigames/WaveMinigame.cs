using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WaveMinigame : MonoBehaviour
{

    public Color BGColor = Color.black;
    public Color TargetWaveColor = Color.green;
    public Color PlayerWaveColor = Color.yellow;

    private Texture2D displayTex;

    public int displayWidth = 400;
    public int displayHeight = 300;

    public float TargetFrequency = 4f; // range: 5 to 30
    public float TargetAmplitude = 1f; // range: 0 to 1
    public float TargetPhase = 0f; // modulo'd by 2*pi

    public float SelectedFrequency = 3f;
    public float SelectedAmplitude = 2.5f;
    public float SelectedPhase = 0.5f;

    private int selectedKnob = 0;

    public float FrequencyChangeRate = 4f;
    public float AmplitudeChangeRate = 0.5f;
    public float PhaseChangeRate = 1f;

    public float MinFrequency = 5f;
    public float MaxFrequency = 30f;
    public float MinAmplitude = 0.1f;
    public float MaxAmplitude = 0.9f;
    private float tau = 2 * Mathf.PI;

    public float FrequencyTolerance = 0.5f;
    public float AmplitudeTolerance = 0.1f;
    public float PhaseTolerance = 0.5f;

    private bool frequencyInRange;
    private bool amplitudeInRange;
    private bool phaseInRange;

    public int CurveSamples = 200;

    public TMP_Text DebugText;

    private void Start() {
        displayTex = new Texture2D(displayWidth, displayHeight);
        displayTex.filterMode = FilterMode.Point;
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(displayTex, new Rect(0, 0, displayWidth, displayHeight), new Vector2(0.5f,0.5f));
        startMinigame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            selectedKnob = Mathf.Min(selectedKnob + 1, 2);
        } else if (Input.GetKeyDown(KeyCode.W)) {
            selectedKnob = Mathf.Max(selectedKnob - 1, 0);
        }
        float mod = (Input.GetKey(KeyCode.A) ? -1 : 1) + (Input.GetKey(KeyCode.D) ? 1 : -1);
        mod *= Time.deltaTime;
        switch (selectedKnob) {
            case 0: SelectedFrequency = Mathf.Clamp(SelectedFrequency + FrequencyChangeRate * mod, MinFrequency, MaxFrequency); break;
            case 1: SelectedAmplitude = Mathf.Clamp(SelectedAmplitude + AmplitudeChangeRate * mod, MinAmplitude, MaxAmplitude); break;
            case 2: SelectedPhase = SelectedPhase + PhaseChangeRate * mod; break;
            default: break;
        }
        DebugText.SetText($"Selected knob: {selectedKnob}\ns. freq: {SelectedFrequency}\ns. amp: {SelectedAmplitude}\ns. phase: {SelectedPhase}\nt. freq{TargetFrequency}\nt. amp{TargetAmplitude}\nt. phase{TargetPhase}\nfreq in range: {frequencyInRange}\namp in range: {amplitudeInRange}\nphase in range: {phaseInRange}");
        drawBG(BGColor);
        drawWave(TargetFrequency, TargetAmplitude, TargetPhase, TargetWaveColor);
        drawWave(SelectedFrequency, SelectedAmplitude, SelectedPhase, PlayerWaveColor);
        displayTex.Apply();

        if (Input.GetKeyDown(KeyCode.R)) {
            startMinigame();
        }

        checkIfCorrect();
    }

    private void startMinigame() {
        TargetFrequency = randomRange(MinFrequency, MaxFrequency);
        SelectedFrequency = randomRange(MinFrequency, MaxFrequency);
        TargetAmplitude = randomRange(MinAmplitude, MaxAmplitude);
        SelectedAmplitude = randomRange(MinAmplitude, MaxAmplitude);
        TargetPhase = randomRange(0f, tau);
        SelectedPhase = randomRange(0f, tau);
    }

    private float randomRange(float v0, float v1) {
        return Random.value * (v1 - v0) + v0;
    }

    private void checkIfCorrect() {
        frequencyInRange = Mathf.Abs(TargetFrequency - SelectedFrequency) < FrequencyTolerance;
        amplitudeInRange = Mathf.Abs(TargetAmplitude - SelectedAmplitude) < AmplitudeTolerance;
        float d = (Mathf.Abs(TargetPhase - SelectedPhase) % tau);
        phaseInRange = d < PhaseTolerance || (tau-d) < PhaseTolerance;
    }

    private void drawBG(Color color) {
        Color[] pixels = new Color[displayTex.width * displayTex.height];
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = color;
        }
        displayTex.SetPixels(pixels);
    }

    private void drawWave(float frequency, float amplitude, float phase, Color color) {
        Vector2[] points = new Vector2[CurveSamples];
        for (int i = 0; i < points.Length; i++) {
            float t = (float)i / points.Length;
            points[i] = new Vector2(t * displayTex.width, ((Mathf.Sin(t * frequency + phase + Time.time) * 0.5f) * amplitude + 0.5f) * displayTex.height);
        }
        for (int i = 0; i < points.Length-1; i++) {
            drawLine(Mathf.RoundToInt(points[i].x), Mathf.RoundToInt(points[i + 1].x), Mathf.RoundToInt(points[i].y), Mathf.RoundToInt(points[i + 1].y), color);
        }
    }

    private void draw(int x, int y, Color color) {
        displayTex.SetPixel(x, displayTex.height - y - 1, color);
    }

    // From https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    private void drawLine(int x0, int x1, int y0, int y1, Color color) {
        int dx = Mathf.Abs(x1 - x0);
        int sx = x0 < x1 ? 1 : -1;
        int dy = -Mathf.Abs(y1 - y0);
        int sy = y0 < y1 ? 1 : -1;
        int error = dx + dy;
        while (true) {
            draw(x0, y0, color);
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * error;
            if (e2 >= dy) {
                if (x0 == x1) break;
                error += dy;
                x0 += sx;
            }
            if (e2 <= dx) {
                if (y0 == y1) break;
                error += dx;
                y0 += sy;
            }
        }
    }

}
