using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineNodeManager : nodeManager {
    public bool locked;
    public bool track;
    bool skipFrame;

    void Start() {
        locked = false;
        track = false;
    }

    public void Update()
    {
        base.Update();
        if (track)
        {
            float horizDiff = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x;
            float vertDiff = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y;
            if (Mathf.Abs(vertDiff) > Mathf.Abs(horizDiff))
            {
                if (vertDiff < 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    currFace = facing.down;
                }
                else
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    currFace = facing.up;
                }
            }
            else {
                if (horizDiff < 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    currFace = facing.left;
                }
                else
                {
                    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    currFace = facing.right;
                }
            }
            if (!skipFrame && Input.GetMouseButtonDown(0))
            {
                OnMouseDown();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Destroy(gameObject);
                fieldManager.single.ReloadNodes();
            }
        }
        skipFrame = false;
    }

    public override void SetRotation(facing newFacing)
    {
        currFace = newFacing;
        switch (currFace)
        {
            case facing.left:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case facing.right:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case facing.up:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case facing.down:
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                break;
        }
    }

    public override void AddTiles()
    {
        locked = true;
        if(power.Count > 0)
        {
            foreach(tileManager man in power)
            {
                man.currState = tileManager.tileState.unpowered;
            }
            power.Clear();
        }
        int hPos = gameObject.transform.parent.GetComponent<tileManager>().tileX;
        int vPos = gameObject.transform.parent.GetComponent<tileManager>().tileY;
        switch (currFace)
        {
            case facing.right:
                for(int i = hPos+1; i < fieldManager.single.hSize; ++i)
                {
                    power.Add(fieldManager.single.map[vPos][i].GetComponent<tileManager>());
                    if(fieldManager.single.map[vPos][i].transform.childCount > 0)
                    {
                        return;
                    }
                }
                break;
            case facing.down:
                for (int i = vPos - 1; i >= 0; --i)
                {
                    power.Add(fieldManager.single.map[i][hPos].GetComponent<tileManager>());
                    if (fieldManager.single.map[i][hPos].transform.childCount > 0)
                    {
                        return;
                    }
                }
                break;
            case facing.left:
                for (int i = hPos - 1; i >= 0; --i)
                {
                    power.Add(fieldManager.single.map[vPos][i].GetComponent<tileManager>());
                    if (fieldManager.single.map[vPos][i].transform.childCount > 0)
                    {
                        return;
                    }
                }
                break;
            case facing.up:
                for (int i = vPos + 1; i < fieldManager.single.vSize; ++i)
                {
                    power.Add(fieldManager.single.map[i][hPos].GetComponent<tileManager>());
                    if (fieldManager.single.map[i][hPos].transform.childCount > 0)
                    {
                        return;
                    }
                }
                break;
        }
    }

    // Update is called once per frame
    void OnMouseDown () {
        if (owner != fieldManager.single.playerTurn) return;
        if (!locked)
        {
            fieldManager.single.SelectPiece(gameObject);
        }
        else
        {
            skipFrame = true;
            track = !track;
            if (!track)
            {
                fieldManager.single.ReloadNodes();
            }
        }
	}
}
