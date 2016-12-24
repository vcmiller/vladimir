using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Controller : MonoBehaviour {
    public static Controller inst { get; private set; }

    public string saveName = "save.vtcr";
    public float layerPadding = 0.1f;

    public SaveState currentSave { get; private set; }
    public Layer[] layers;
    public Player player { get; private set; }

    public bool[] activeLayers { get; private set; }
    
	// Use this for initialization
	void Start () {
        inst = this;
        activeLayers = new bool[layers.Length];
        player = FindObjectOfType<Player>();
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
            currentSave.upgrades = new bool[16];
            currentSave.keys = new bool[8];
            currentSave.foundUpgradePoints = new bool[16];
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

        player.Damage(-player.actualMaxHealth);
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.persistentDataPath + "/" + currentSave.file);
        bf.Serialize(fs, currentSave);
        fs.Close();
    }
	
	// Update is called once per frame
	void Update () {
        if (player) {
            int index = 0;
            foreach (Layer layer in layers) {
                float y = player.transform.position.y;
                bool b = y >= layer.ymin + layerPadding && y <= layer.ymax - layerPadding;
                layer.active = b;
                activeLayers[index++] = b;
            }
        }
        
	}
}
