using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



class PlacedBlock {
    public BlockBase block;
    public BlockBase parent;
    public Transform socket;
}

public class GameManager : MonoBehaviour {

    bool playMode;
    List<BlockBase> blocks = new List<BlockBase>();
    List<PlacedBlock> placedBlocks = new List<PlacedBlock>();

    public BlockBase baseBlock;
    public Transform originTransform;
    public Button playButton;

    BlockBase editorBlockPrefab;
    BlockBase editorBlockInstance;

    public static GameManager manager;

    void Awake() {
        if (manager == null) {
            manager = this;
            DontDestroyOnLoad(gameObject);
        } else if (manager == this) {
            Destroy(this);
        }
    }
	// Use this for initialization
	void Start () {

        
        blocks.Add(baseBlock);

        PlacedBlock placedBlock = new PlacedBlock();
        placedBlock.block = baseBlock;
        placedBlock.parent = null;
        placedBlock.socket = originTransform;
        placedBlocks.Add(placedBlock);

        playMode = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!playMode && editorBlockInstance) {
            editorBlockInstance.transform.position = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                BlockBase hitBlock = hit.transform.GetComponent<BlockBase>();

                if (hitBlock) {
                    Transform socket = hitBlock.GetClosestSocket(hit);                  

                    if (socket) {
                        editorBlockInstance.Place(socket);

                        if (Input.GetMouseButtonUp(0)) {
                            editorBlockInstance.GetComponentInChildren<Collider>().enabled = true;
                            hitBlock.sockets.Remove(socket);
                            editorBlockInstance.Attach(hitBlock);
                            blocks.Add(editorBlockInstance);

                            PlacedBlock placedBlock = new PlacedBlock();
                            placedBlock.block = editorBlockInstance;
                            placedBlock.parent = hitBlock;
                            placedBlock.socket = socket;
                            placedBlocks.Add(placedBlock);

                            InstantiateEditorBlockPrefab();
                        }
                    }
                }
            }
        }	
	}

    public void OnPlay() {

        playMode = !playMode;

        for (int i = 0; i < blocks.Count; i++) {
            blocks[i].OnPlay(playMode);
        }

        if (!playMode) {
            for (int i = 0; i < blocks.Count; i++) {
                placedBlocks[i].block.Place(placedBlocks[i].socket);

                if (placedBlocks[i].parent) {
                    placedBlocks[i].block.Attach(placedBlocks[i].parent);
                }     
            }
        }

        playButton.GetComponentInChildren<Text>().text = playMode ? "Edit" : "Play";
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.dat");

        float f = 7;
        bf.Serialize(file, f);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/saveGame.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.dat", FileMode.Open);

            float f = (float)bf.Deserialize(file);
            Debug.Log(f);
            file.Close();
        }
    }

    public void SetEditorBlock(BlockBase prefab) {
        if (editorBlockPrefab != prefab) {
            editorBlockPrefab = prefab;
            if (editorBlockInstance) {
                Destroy(editorBlockInstance.gameObject);
            }
            InstantiateEditorBlockPrefab();
        }
    }

    void InstantiateEditorBlockPrefab() {    
        editorBlockInstance = Instantiate(editorBlockPrefab, Vector3.zero, Quaternion.identity) as BlockBase;
    }
}
