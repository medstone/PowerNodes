using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour {
    public enum tileState {unpowered = 0, powered = 1 };
    public tileState currState;
    public Material powerOn;
    public Material powerOff;
    public int tileX;
    public int tileY;
    public bool isVictory = false;
    public int owner;

	// Use this for initialization
	void Start () {
        currState = tileState.unpowered;
	}
	
	// Update is called once per frame
	void Update () {
		if(currState == tileState.unpowered)
        {
            this.GetComponent<Renderer>().material = powerOff;
        }
        else
        {
            this.GetComponent<Renderer>().material = powerOn;
            if (isVictory)
            {
                fieldManager.single.Win(owner);
            }
        }
	}

    void OnMouseEnter()
    {
        if (fieldManager.single.currSelected != null)
        {
            fieldManager.single.currSelected.transform.parent = this.transform;
            Debug.Log("enter");
        }
    }
}
