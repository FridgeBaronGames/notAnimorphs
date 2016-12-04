using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {

    player character;
    UIscr UI;

    Vector3 checkPoint;

    Scene currentLevel;

	// Use this for initialization
	void Awake () {
        character = GameObject.Find("Character").GetComponent<player>();
        UI = GameObject.Find("UI").GetComponent<UIscr>() ;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void iDied()
    {

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        character.moveTo(checkPoint);
        UI.reset();
    }

    public void newCheckPoint(Vector3 newPoint)
    {
        checkPoint = newPoint;
    }

}
