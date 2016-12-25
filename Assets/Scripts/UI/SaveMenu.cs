using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour {
    public SaveButton current { get; set; }
    public CanvasGroup me { get; private set; }
    public CanvasGroup main;
    public Text header;
    public Button clearButton;

    public Selectable myStart;
    public Selectable otherStart;

    void Start() {
        me = GetComponent<CanvasGroup>();
        open = false;
    }

    private bool open {
        set {
            me.alpha = value ? 1 : 0;
            me.blocksRaycasts = value;
            me.interactable = value;

            main.alpha = value ? 0 : 1;
            main.blocksRaycasts = !value;
            main.interactable = !value;

            if (value) {
                myStart.Select();
            } else {
                otherStart.Select();
            }
        }
    }

    public void Open() {
        open = true;
        clearButton.interactable = current.exists;
        header.text = current.text.text;
    }

	public void Play() {
        current.Play();
    }

    public void Clear() {
        current.Clear();
        open = false;
    }

    public void Cancel() {
        open = false;
    }
}
