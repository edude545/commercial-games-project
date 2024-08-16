using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{

    public TMP_Text Text;

    public string ButtonText;

    public bool AddEqualSignsOnHighlight;
    public bool MouseoverSound;

    public UnityEvent OnClick;

    enum HighlightStyles {
        EqualsSigns,
        None
    }

    private void OnValidate() {
        Text.text = ButtonText;
    }

    private void Start() {
        ButtonText = Text.text;
    }

    private void OnEnable() {
        if (AddEqualSignsOnHighlight) {
            Text.text = ButtonText;
        }
    }

    public void OnPointerEnter() {
        if (AddEqualSignsOnHighlight) {
            Text.text = $"== {ButtonText} ==";
        }
        if (MouseoverSound) {
            GetComponent<AudioSource>().Play();
        }
    }

    public void OnPointerExit() {
        if (AddEqualSignsOnHighlight) {
            Text.text = ButtonText;
        }   
    }

    public void RunOnClickEvent() {
        OnClick.Invoke();
    }

}
