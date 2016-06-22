using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    bool playMode;
    List<Transform> blocks = new List<Transform>();

    public Transform baseBlock;
    public Button playButton;

    BlockBase editorBlockPrefab;
    BlockBase editorBlockInstance;

	// Use this for initialization
	void Start () {
        blocks.Add(baseBlock);
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
                            editorBlockInstance.Attach(socket, hitBlock);
                            blocks.Add(editorBlockInstance.transform);

                            InstantiateEditorBlockPrefab();
                        }
                    }
                }
            }
        }	
	}

    public void OnPlay() {

        for (int i = 0; i < blocks.Count; i++) {
            blocks[i].GetComponent<BlockBase>().OnPlay();
        }

        playMode = !playMode;

        playButton.GetComponentInChildren<Text>().text = playMode ? "Edit" : "Play";
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
