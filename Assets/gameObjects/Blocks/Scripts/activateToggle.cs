using UnityEngine;
using System.Collections;

public class activateToggle : MonoBehaviour {


    bool active;

    [SerializeField]
    public float timer;
    float timed;

    [SerializeField]
    public Sprite offSpr,
           onSpr;

    [SerializeField]
    public toggleAble target;

    SpriteRenderer sprRenderer;

	// Use this for initialization
	void Start () {
        sprRenderer = GetComponent<SpriteRenderer>();
        offSpr = sprRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update () {
        if (timed > timer)
        {
            active = false;
            sprRenderer.sprite = offSpr;
            timed = 0;
            target.toggleState();
        }
        if (active == true && timer != 0)
        {
            timed++;
        }
	}

    public void toggle()
    {
        active = !active;
        target.toggleState();
        if (active == true)
            sprRenderer.sprite = onSpr;
        else
            sprRenderer.sprite = offSpr;
    }
}
