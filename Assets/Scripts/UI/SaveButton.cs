using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour {
    public int index;
    public Text text { get; private set; }
    public Button button { get; private set; }
    public SaveMenu saveMenu { get; private set; }

    public bool exists {
        get {
            return File.Exists(file);
        }
    }

    public string file {
        get {
            return Application.persistentDataPath + "/save-" + index + ".vtcr";
        }
    }

    public string save {
        get {
            return "save-" + index + ".vtcr";
        }
    }

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        saveMenu = FindObjectOfType<SaveMenu>();

        if (File.Exists(file)) {
            text.text = "SLOT " + index + ": " + File.GetLastWriteTime(file);
        } else {
            text.text = "[ EMPTY SLOT ]";
        }

	}

    public void Select() {
        saveMenu.current = this;
        saveMenu.Open();
    }
	
	public void Play() {
        Controller.saveName = save;
        SceneManager.LoadScene("Game");
    }

    public void Clear() {
        File.Delete(file);
        text.text = "[ EMPTY SLOT ]";
    }
}
