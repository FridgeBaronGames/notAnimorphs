using UnityEngine;
using System.Collections;

public class teleportBlock : MonoBehaviour {


    [SerializeField]
    public teleportBlock nextLoc;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 teleport()
    {
        Vector3 retVec = nextLoc.transform.position;
        retVec += new Vector3(.5f, -.5f);
        return retVec;
    }
}
