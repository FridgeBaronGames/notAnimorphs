using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIscr : MonoBehaviour {

    float levelStartDelay = 2f;

    GameObject blackObj;
    Image blackOut;

	// Use this for initialization
	void Awake () {
        blackObj = GameObject.Find("blackOut");
        blackOut = blackObj.GetComponent<Image>();
	}

    void Start()
    {
        Debug.Log("fading in");
        reset();
    }

    public void reset()
    {
        closeScreen(0);
        openScreen(1);
    }
	
    void closeScreen(float timeIn)
    {
        blackOut.CrossFadeAlpha(1f, timeIn, false);
    }
    void openScreen(float timeOut)
    {
        blackOut.CrossFadeAlpha(0f, timeOut, false);
    }

    void setState(bool newState, GameObject obj)
    {
        obj.SetActive(newState);
    }


    private void onLevelWasLoaded (int index)
    {

    }
}
