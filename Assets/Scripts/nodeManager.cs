using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class nodeManager : MonoBehaviour {
    public List<tileManager> power;
    public int owner;
    public enum facing { right = 0, down, left, up };
    public facing currFace;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	public void Update () {
        if ((gameObject.transform.parent != null && gameObject.transform.parent.GetComponent<tileManager>() != null) && (gameObject.transform.parent.GetComponent<tileManager>().currState == tileManager.tileState.powered || power.Contains(gameObject.transform.parent.GetComponent<tileManager>())))
        {
            for (int i = 0; i < power.Count; i++)
            {
                power[i].currState = tileManager.tileState.powered;
            }
        }
	}

    public void SetOwner(int newOwner)
    {
        owner = newOwner;
    }

    public abstract void AddTiles();

    public virtual void SetRotation(facing newFacing)
    {

    }

    void OnDestroy()
    {
        for (int i = 0; i < power.Count; i++)
        {
            power[i].currState = tileManager.tileState.unpowered;
        }
    }

    void OnMouseDown()
    {
        if (owner != fieldManager.single.playerTurn) return;
        fieldManager.single.SelectPiece(gameObject);
    }
}
