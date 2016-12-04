using UnityEngine;
using System.Collections;

public class fallBlock : MonoBehaviour {

    [SerializeField]
    public float fallTime,
                 resetTime;

    float fallTimer = 0;
    float resetTimer = 0;
    bool falling;
    bool count;
    Transform parent;
    Vector3 startPos;

    bool toggleScr = false;

    toggleAble switchToggle;

	// Use this for initialization
	void Start () {
        parent = transform.parent.transform;
        startPos = parent.transform.position;
        fallTimer = 0;

        switchToggle = transform.parent.GetComponent<toggleAble>();
        if (switchToggle != null)
        {
            toggleScr = true;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (toggleScr == true)
        {
            
            if (switchToggle.getState())
            {
                tick();
            }
        }
        else
        {
            tick();
        }
	}
    public void set(bool value)
    {
        count = value;
    }

    void tick()
    {
        if (fallTime * 30 < fallTimer)
            falling = true;
        if (falling == true)
        {
            parent.position -= new Vector3(0f, .2f, 0f);
            resetTimer++;
            if (resetTime * 30 < resetTimer && resetTime > 0)
            {
                resetTimer = 0;
                parent.position = startPos;
                fallTimer = 0;
                falling = false;
                count = false;
            }

        }
        if (count == true)
            fallTimer++;
    }
}
