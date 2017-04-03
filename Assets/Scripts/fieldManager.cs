using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fieldManager : MonoBehaviour {
    public int hSize;
    public int vSize;
    public GameObject tilePrefab;
    public float gap;
    public List<List<GameObject>> map;
    public GameObject startNode;
    public static fieldManager single;
    public int playerTurn;
    public GameObject currSelected;
    public GameObject playerContainers;
    public List<nodeManager> nodes;
    public Material victoryOff;
    public Material victoryOn;
    public Text winnText;

    void OnAwake()
    {
        nodes = new List<nodeManager>();
    }

	// Use this for initialization
	void Start () {
        currSelected = null;
        single = this;
        map = new List<List<GameObject>>();
        float vPos = tilePrefab.transform.lossyScale.y / 2;
        for (int i = 0; i < hSize; i++)
        {
            map.Add(new List<GameObject>());
            float hPos = tilePrefab.transform.lossyScale.x / 2;
            for (int j = 0; j < vSize; j++)
            {
                GameObject place = Instantiate<GameObject>(tilePrefab);
                place.transform.position = new Vector3(hPos, vPos, 0);
                hPos += gap + tilePrefab.transform.lossyScale.x;
                map[i].Add(place);
                place.GetComponent<tileManager>().tileX = j;
                place.GetComponent<tileManager>().tileY = i;
            }
            vPos += gap + tilePrefab.transform.lossyScale.y;
        }
        float camX = 0;
        float camY = 0;
        int centerX;
        int centerY;
        centerX = Mathf.CeilToInt(hSize / 2);
        centerY = Mathf.CeilToInt(vSize / 2);
        if (hSize % 2 == 1)
        {
            camX = map[0][Mathf.CeilToInt(hSize / 2)].transform.position.x;
        }
        else
        {
            camX = map[0][0].transform.position.x + ((map[0][hSize-1].transform.position.x - map[0][0].transform.position.x)/2);
        }
        if (vSize % 2 == 1)
        {
            camY = map[Mathf.CeilToInt(vSize / 2)][0].transform.position.y;
        }
        else
        {
            camY = map[0][0].transform.position.y + ((map[vSize-1][0].transform.position.y - map[0][0].transform.position.y)/2);
        }
        Camera.main.transform.position = new Vector3(camX, camY, -10f);
        Camera.main.GetComponent<Camera>().orthographicSize = (((tilePrefab.transform.lossyScale.y * vSize) + (gap * vSize)) / 2);
        GameObject start = Instantiate<GameObject>(startNode);
        start.transform.parent = map[centerY][centerX].transform;
        start.transform.localPosition = new Vector3(0, 0, 1);
        start.GetComponent<nodeManager>().AddTiles();
        playerTurn = 0;
        GameObject leftContainer = Instantiate<GameObject>(playerContainers);
        leftContainer.transform.position = new Vector3(fieldManager.single.map[0][0].transform.position.x - leftContainer.transform.lossyScale.x, Camera.main.transform.position.y, 0);
        for (int i = 0; i < leftContainer.transform.childCount; ++i)
        {
            leftContainer.transform.GetChild(i).GetComponent<nodeManager>().SetOwner(0);
        }
        GameObject rightContainer = Instantiate<GameObject>(playerContainers);
        rightContainer.transform.position = new Vector3(fieldManager.single.map[0][fieldManager.single.hSize-1].transform.position.x + rightContainer.transform.lossyScale.x, Camera.main.transform.position.y, 0);
        rightContainer.transform.Rotate(new Vector3(0, 180f, 0));
        for (int i = 0; i < rightContainer.transform.childCount; ++i)
        {
            rightContainer.transform.GetChild(i).GetComponent<nodeManager>().SetOwner(1);
        }
        tileManager victoryTile = map[vSize - 1][0].GetComponent<tileManager>();
        victoryTile.isVictory = true;
        victoryTile.owner = 0;
        victoryTile.powerOn = victoryOn;
        victoryTile.powerOff = victoryOff;
        victoryTile = map[0][hSize-1].GetComponent<tileManager>();
        victoryTile.isVictory = true;
        victoryTile.owner = 0;
        victoryTile.powerOn = victoryOn;
        victoryTile.powerOff = victoryOff;

    }

    void Update()
    {
        if (currSelected != null)
        {
            currSelected.transform.localPosition = new Vector3(0, 0, -1f);
            if (currSelected.transform.parent == null)
            {
                currSelected.SetActive(false);
            }
            else
            {
                currSelected.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currSelected.GetComponent<nodeManager>().SetRotation(nodeManager.facing.left);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                currSelected.GetComponent<nodeManager>().SetRotation(nodeManager.facing.up);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                currSelected.GetComponent<nodeManager>().SetRotation(nodeManager.facing.down);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                currSelected.GetComponent<nodeManager>().SetRotation(nodeManager.facing.right);
            }
        }
    }
	
    public void Win(int player) {
        switch (player)
        {
            case 0:
                winnText.text = "Left Wins!!!";
                winnText.gameObject.transform.parent.gameObject.SetActive(true);
                break;
            case 1:
                winnText.text = "Right Wins!!!!";
                winnText.gameObject.transform.parent.gameObject.SetActive(true);
                break;
        }
    }

    void EndTurn()
    {
        switch (playerTurn)
        {
            case 0:
                playerTurn = 1;
                break;
            case 1:
                playerTurn = 0;
                break;
        }
    }

    public void SelectPiece(GameObject selected)
    {
        if(currSelected != null)
        {
            if(currSelected == selected)
            {
                //currSelected.GetComponent<nodeManager>().AddTiles();
                nodes.Add(currSelected.GetComponent<nodeManager>());
                ReloadNodes();
                currSelected = null;
                return;
            }
            Destroy(currSelected);
            currSelected = null;
        }
        currSelected = Instantiate<GameObject>(selected);
        currSelected.SetActive(false);
    }

    public void ReloadNodes()
    {
        foreach (nodeManager node in nodes)
        {
            node.AddTiles();
        }
        EndTurn();
    }
}
