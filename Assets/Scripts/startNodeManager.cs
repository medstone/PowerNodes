using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startNodeManager : nodeManager {
	
    public override void AddTiles()
    {
        if(power.Count > 0)
        {
            power.Clear();
        }
        int baseX = gameObject.transform.parent.GetComponent<tileManager>().tileX;
        int baseY = gameObject.transform.parent.GetComponent<tileManager>().tileY;
        power.Add(fieldManager.single.map[baseY][baseX].GetComponent<tileManager>());
        //power.Add(fieldManager.single.map[baseY-1][baseX].GetComponent<tileManager>());
        //power.Add(fieldManager.single.map[baseY+1][baseX].GetComponent<tileManager>());
        //power.Add(fieldManager.single.map[baseY][baseX-1].GetComponent<tileManager>());
        //power.Add(fieldManager.single.map[baseY][baseX+1].GetComponent<tileManager>());
    }

    void OnMouseDown() {

    }
}
