using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Controller : MonoBehaviour {
    public static Controller inst { get; private set; }

    public string saveName = "save.vtcr";

    public SaveState currentSave { get; private set; }

	// Use this for initialization
	void Start () {
        inst = this;
        Load();
	}

    void Load() {
        if (File.Exists(Application.persistentDataPath + "/" + saveName)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.OpenRead(Application.persistentDataPath + "/" + saveName);
            currentSave = (SaveState)bf.Deserialize(fs);
            fs.Close();

            UpdateProgress();
        } else {
            currentSave = new SaveState();
            currentSave.checkpoint = -1;
            currentSave.file = saveName;
        }
    }

    void UpdateProgress() {
        if (currentSave.checkpoint >= 0) {
            foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>()) {
                if (checkpoint.index == currentSave.checkpoint) {
                    FindObjectOfType<Player>().transform.position = checkpoint.transform.position;
                    break;
                }
            }
        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(Application.persistentDataPath + "/" + currentSave.file);
        bf.Serialize(fs, currentSave);
        fs.Close();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
