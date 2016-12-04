using UnityEngine;
using System.Collections;

public class moveBlock : MonoBehaviour {


    [SerializeField]
    public int moveSteps,
        endDelayA,
        endDelayB;
    [SerializeField]
    public Vector2 moveSpeed;

    int movedSteps = 1;
    int delaySteps = 0;
    int delayedSteps = 0;
    bool movingForward = true;
    bool delaying;
    bool delayA = true;

    bool active = true;





	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (active == true)
        {
            if (delaying == false)
            {
                delaying = move();
                if (delaying == true)
                {
                    if (delayA == true)
                        delaySteps = endDelayA;
                    else
                        delaySteps = endDelayB;
                    delayA = !delayA;
                }
            }
            else
            {
                delayedSteps += 1;
                if (delayedSteps >= delaySteps)
                    delaying = false;
            }
        }
        
	}


    bool move()
    {

        if (movedSteps < moveSteps*30)
        {
            if (movingForward == true)
                transform.position += new Vector3(moveSpeed.x/100, moveSpeed.y/100, 0);
            else
                transform.position -= new Vector3(moveSpeed.x/100, moveSpeed.y/100, 0);
            movedSteps += 1;
        }
        else
        {
            movingForward = !movingForward;
            movedSteps = 0;
            return true;
        }
        return false;
    }

    void swap()
    {
        active = !active;
    }
}
