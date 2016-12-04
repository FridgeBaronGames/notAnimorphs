using UnityEngine;
using System.Collections;

public class toggleAble : MonoBehaviour {

    [SerializeField]
    bool active;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool getState()
    {
        return active;
        
    }

    public void toggleState()
    {
        active = !active;
    }
}
