using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    [SerializeField]
    public float speed = 15f,
                 maxVel = 4f,
                 jumpForce = 30f,
                 jumpDecay = .95f,
                 swimSpeed = 2,
                 swimMax = 4,
                 sinkSpeed = 1,
                 sinkMax = 3,
                 waterJumpMult = .5f;

    [SerializeField]
    public float gravity = 20f,
                 gravityAccel = 1.02f,
                 jumpCuttoff = .3f;

    bool jump;
    bool tryFlip;
    //bool isFalling;
    bool canFlip;

    bool isSliding;
    bool isSwimming;
    bool waterJump;

    GameObject Slime;
    GameObject RockMonster;
    GameObject Hawk;
    GameObject Human;
    GameObject Salmon;
    GameObject currentForm;
    formStats[] formRay;
    int formInt;

    doors flipType;
    [SerializeField]
    Morph formNum,
          newForm;

    float fallCount;
    float jumpCount;

    bool zAxis;
    //Salmon Stuff
    bool rightFacing = true;
    
    //bool moving;

    Vector3 velocity;
    Vector3 moveInput;
    [SerializeField]
    Vector3 moveSpeed;


    //Block Links
    Vector3 teleLoc;
    activateToggle toggleLink;

    CharacterController myCharacter;
    Animator anim;
    ParticleSystem morphEmmiter;

    
    void Awake()
    {
        myCharacter = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        morphEmmiter = GetComponent<ParticleSystem>();


        Slime = transform.Find("Slime").gameObject;
        RockMonster = transform.Find("RockMonster").gameObject;
        Hawk = transform.Find("Hawk").gameObject;
        Human = transform.Find("Human").gameObject;
        Salmon = transform.Find("Salmon").gameObject;
        formRay = new formStats[5];
        formRay[0] = new formStats(2f, 4f, 10f, .9f, 1, 3, 1, 4, 0.5f); //Slime
        formRay[1] = new formStats(1.5f, 3f, 10f, .9f, 1, 3, 1, 4, 0.5f); //RockMonster
        formRay[2] = new formStats(2f, 15f, 10f, .9f, 1, 3, 1, 0, 0.5f); //Hawk
        formRay[3] = new formStats(2f, 4f, 10f, .9f, 1, 3, 1, 4, 0.5f); //Human
        formRay[4] = new formStats(1f, 100f, 1f, .95f, 3, 9, .4f, 0, 25f); //Salmon
        //For the test
        formInt = 1;
        formNum = Morph.Rock;
        newForm = Morph.Slime;
        currentForm = Slime;
        activateNew(Morph.Slime);
       

        moveSpeed = new Vector3(0f, 0f, 0f);
        jumpCount = 1;
        canFlip = false;
        zAxis = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
    void Update()
    {
        checkKeys();
        generalAction();
        morphEmmiter.Emit(0);
    }
	// Update is called once per frame
	void FixedUpdate () {
        velocity = myCharacter.velocity;
        if (formNum != newForm)
        {
            runMorph();
        }
        switch (formNum)
        {
            case Morph.Slime:
                if (!isSwimming)
                    BasicMove();
                else
                    basicSwim();
                break;
            case Morph.Rock:
                BasicMove();
                break;
            case Morph.Hawk:
                if (!isSwimming)
                    hawkMove();
                else
                    basicSwim();
                break;
            case Morph.Human:
                BasicMove();
                break;
            case Morph.Salmon:
                if (!isSwimming)
                    salmonMove();
                else
                {
                    salmonSwim();
                }
                break;
        }

        
    }

    void runMorph()
    {
        morphEmmiter.Emit(45);
        switch (newForm)
        {
            case Morph.Slime:
                formNum = Morph.Slime;
                loadSpeeds(formRay[0]);
            break;
            case Morph.Rock:
                formNum = Morph.Rock;
                loadSpeeds(formRay[1]);
                break;
            case Morph.Hawk:
                formNum = Morph.Hawk;
                loadSpeeds(formRay[2]);
                break;
            case Morph.Human:
                formNum = Morph.Human;
                loadSpeeds(formRay[3]);
                break;
            case Morph.Salmon:
                formNum = Morph.Salmon;
                loadSpeeds(formRay[4]);
                break;
        }
        activateNew(newForm);
    }

    void checkKeys()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Jump
        if (Input.GetKeyDown("space"))
            jump = true;
        if (Input.GetKeyUp("space"))
            jump = false;

        //Flip
        if (Input.GetKeyDown(KeyCode.F))
            tryFlip = true;
        if (Input.GetKeyUp(KeyCode.F))
            tryFlip = false;

        //Morph
        if (Input.GetKeyDown(KeyCode.Alpha1))
            newForm = Morph.Slime;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            newForm = Morph.Rock;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            newForm = Morph.Hawk;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            newForm = Morph.Human;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            newForm = Morph.Salmon;

    }

/**********************************  Character Actions *******************************************/
    void generalAction()
    {
        if (tryFlip == true && canFlip == true)
        {
            if (zAxis == false)
            {
                transform.Rotate(new Vector3(0, -90, 0));
                tryFlip = false;
                canFlip = false;
                zAxis = true;
                Debug.Log("Rotating boss");
            }
            else
            {
                transform.Rotate(new Vector3(0, 90, 0));
                tryFlip = false;
                canFlip = false;
                zAxis = false;
                Debug.Log("Rotating boss");

            }
        }
        if (moveInput.x == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveInput.x == 1)
            transform.localScale = new Vector3(1, 1, 1);
        if (moveSpeed.x != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);

        if (teleLoc != Vector3.zero && tryFlip == true)
        {
            transform.position = teleLoc;
            tryFlip = false;
        }

        if (toggleLink != null && tryFlip == true && formNum == Morph.Human)
        {
            toggleLink.toggle();
            tryFlip = false;
        }

        
    }
    void BasicMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        //Set 

        float xMove;
        if (zAxis == true)
            xMove = moveSpeed.z;
        else
            xMove = moveSpeed.x;
        if (isSliding == true)
        {
            if (moveInput.x == 0)
                xMove = xMove * .95f;
            else
                xMove = (xMove * speed) * 0.65f;
        }
        else
            xMove = xMove * 0.8f;

        xMove += moveInput.x * speed;

        if (xMove > maxVel)
            xMove = maxVel;
        if (xMove < -maxVel)
            xMove = -maxVel;

        if (zAxis == true)
            moveSpeed.z = xMove;
        else
            moveSpeed.x = xMove;

        

        moveSpeed.y = 0f;

        if (!myCharacter.isGrounded)
        {
            if (jump && jumpCount > jumpCuttoff)
            {
                if (waterJump)
                {
                    moveSpeed.y += (jumpForce*waterJumpMult) * jumpCount;
                    jumpCount *= jumpDecay;
                }
                else
                {
                    moveSpeed.y += jumpForce * jumpCount;
                    jumpCount *= jumpDecay;
                }

            }
            else
            {
                moveSpeed.y -= Mathf.Pow(gravity, fallCount);
                fallCount = fallCount * gravityAccel;
            }
        }
        else
        {
            fallCount = 1;
            jumpCount = 1;
            waterJump = false;
            if (jump)
            {
                moveSpeed.y += jumpForce;
            }
        }
    }
    void basicSwim()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        float xMove;
        if (zAxis == true)
            xMove = moveSpeed.z;
        else
            xMove = moveSpeed.x;

        xMove = xMove * .7f + moveInput.x * swimSpeed;


        if (!myCharacter.isGrounded)
        {
            moveSpeed.y = (moveSpeed.y * .7f) - sinkSpeed;
            if (moveSpeed.y < -sinkMax)
                moveSpeed.y = -sinkMax;
        }
        if (jump)
        {
            moveSpeed.y += swimSpeed + sinkSpeed;
        }

        if (zAxis)
            moveSpeed.z = xMove;
        else
            moveSpeed.x = xMove;

    }

    void slimeMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        if (isSliding == true)
        {
            if (zAxis == false)
                moveSpeed = new Vector3(moveSpeed.x * .9f, 0f, 0f);
            else
                moveSpeed = new Vector3(0f, 0f, moveSpeed.x * .9f);
        }
            
        else
            moveSpeed = new Vector3(0f, 0f, 0f);


        if (zAxis == false)
        {
            moveSpeed.x += moveInput.x * speed;
            if (moveSpeed.x > maxVel)
                moveSpeed.x = maxVel;
            if (moveSpeed.x < -maxVel)
                moveSpeed.x = -maxVel;
        }
        else
        {
            moveSpeed.z += moveInput.x * speed;
            if (moveSpeed.z > maxVel)
                moveSpeed.z = maxVel;
            if (moveSpeed.z < -maxVel)
                moveSpeed.z = -maxVel;
        }

        if (!myCharacter.isGrounded)
        {
            if (jump && jumpCount > jumpCuttoff)
            {
                moveSpeed.y += jumpForce * jumpCount;
                jumpCount *= jumpDecay;

            }
            else
            {
                moveSpeed.y -= Mathf.Pow(gravity, fallCount);
                fallCount = fallCount * gravityAccel;
            }
        }
        else
        {
            fallCount = 1;
            jumpCount = 1;
            if (jump)
            {
                moveSpeed.y += jumpForce;
            }
        }
    }
    void rockMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        moveSpeed = new Vector3(0f, 0f, 0f);
        if (zAxis == false)
            moveSpeed.x = moveInput.x * speed;
        else
            moveSpeed.z = moveInput.x * speed;

        if (!myCharacter.isGrounded)
        {
            if (jump && jumpCount > jumpCuttoff)
            {
                moveSpeed.y += jumpForce * jumpCount;
                jumpCount *= jumpDecay;

            }
            else
            {
                moveSpeed.y -= Mathf.Pow(gravity, fallCount);
                fallCount = fallCount * gravityAccel;
            }
        }
        else
        {
            fallCount = 1;
            jumpCount = 1;
            if (jump)
            {
                moveSpeed.y += jumpForce;
            }
        }
    }
    void hawkMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        float xMove = 0;

        if (zAxis)
            xMove = moveSpeed.z * .95f;
        else
            xMove = moveSpeed.x * .95f;

        xMove += moveInput.x * speed;

        if (xMove > maxVel)
            xMove = maxVel;
        if (xMove < -maxVel)
            xMove = -maxVel;

        if (zAxis)
            moveSpeed.z = xMove;
        else
            moveSpeed.x = xMove;

        moveSpeed.y = (moveInput.y * speed) + (moveSpeed.y * .7f);

        float moveRatio = Mathf.Abs(xMove) / (maxVel);

        Debug.Log(moveRatio + " :" + xMove);
        moveSpeed.y -= (gravity*.5f) * (1-moveRatio);





    }
    void hawkSwim()
    {

    }
    void humanMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        moveSpeed = new Vector3(0f, 0f, 0f);
        if (zAxis == false)
            moveSpeed.x = moveInput.x * speed;
        else
            moveSpeed.z = moveInput.x * speed;

        if (!myCharacter.isGrounded)
        {
            if (jump && jumpCount > jumpCuttoff)
            {
                moveSpeed.y += jumpForce * jumpCount;
                jumpCount *= jumpDecay;

            }
            else
            {
                moveSpeed.y -= Mathf.Pow(gravity, fallCount);
                fallCount = fallCount * gravityAccel;
            }
        }
        else
        {
            fallCount = 1;
            jumpCount = 1;
            if (jump)
            {
                moveSpeed.y += jumpForce;
            }
        }
    }
    void salmonMove()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        float xMove = 0;

        if (moveInput.x == 1)
            rightFacing = true;
        if (moveInput.x == -1)
            rightFacing = false;

        if (!waterJump)
        {
            if (rightFacing)
                xMove = 1;
            else
                xMove = -1;
            if (zAxis)
                moveSpeed.z = xMove;
            else
                moveSpeed.x = xMove;
        }


        moveSpeed.y = 0;

        if (!myCharacter.isGrounded)
        {
            if (jump && jumpCount > jumpCuttoff)
            {
                if (waterJump)
                {
                    moveSpeed.y += (jumpForce * waterJumpMult) * jumpCount;
                    jumpCount *= jumpDecay;
                }
                else
                {
                    moveSpeed.y += jumpForce * jumpCount;
                    jumpCount *= jumpDecay;
                }

            }
            else
            {
                moveSpeed.y -= Mathf.Pow(gravity, fallCount);
                fallCount = fallCount * gravityAccel;
            }
        }
        else
        {
            fallCount = 1;
            jumpCount = 1;
            waterJump = false;


        }
    }
    void salmonSwim()
    {
        myCharacter.Move(moveSpeed * Time.deltaTime);

        float xMove;
        if (zAxis == true)
            xMove = moveSpeed.z;
        else
            xMove = moveSpeed.x;

        xMove = xMove * .7f + moveInput.x * swimSpeed;

        moveSpeed.y = (moveSpeed.y * .7f) + (moveInput.y * swimSpeed);


        if (zAxis)
            moveSpeed.z = xMove;
        else
            moveSpeed.x = xMove;

    }


    //Triggers
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            canFlip = true;
        }
        if (other.tag.Contains("'Spike'"))
            Debug.Log("You got Spiked");
        if (other.tag.Contains("'Ice'"))
            isSliding = true;
        if (other.tag.Contains("'Fall'"))
            other.GetComponent<fallBlock>().set(true);

        if (other.tag.Contains("'Tele'"))
            teleLoc = other.GetComponent<teleportBlock>().teleport();

        if (other.tag.Contains("'Switch'"))
            toggleLink = other.GetComponent<activateToggle>();

        if (other.tag.Contains("'Water'"))
        {
            isSwimming = true;
            fallCount = 1;
            jumpCount = 1;
            waterJump = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("'Door'"))
            canFlip = false;
        if (other.tag.Contains("'Back'"))
            flipType = doors.Back;
        if (other.tag.Contains("'Ice'"))
            isSliding = false;

        if (other.tag.Contains("'Tele"))
            teleLoc =  Vector3.zero;
        if (other.tag.Contains("'Switch'"))
            toggleLink = null;
        if (other.tag.Contains("'Water'"))
            isSwimming = false;
    }



    //
    void loadSpeeds(formStats newStats)
    {
        speed = newStats.speed;
        maxVel = newStats.maxVel;
        jumpForce = newStats.jumpForce;
        jumpDecay = newStats.jumpDecay;
        swimSpeed = newStats.swimSpeed;
        swimMax = newStats.swimMax;
        sinkSpeed = newStats.sinkSpeed;
        sinkMax = newStats.sinkMax;
        waterJumpMult = newStats.waterJumpMult;
    }
    void activateNew(Morph turnOn)
    {
        Slime.SetActive(false);
        RockMonster.SetActive(false);
        Hawk.SetActive(false);
        Human.SetActive(false);
        Salmon.SetActive(false);
        switch (turnOn)
        {
            case Morph.Slime:
                Slime.SetActive(true);
                break;
            case Morph.Rock:
                RockMonster.SetActive(true);
                break;
            case Morph.Hawk:
                Hawk.SetActive(true);
                break;
            case Morph.Human:
                Human.SetActive(true);
                break;
            case Morph.Salmon:
                Salmon.SetActive(true);
                break;
        }
    }
}





enum doors
{
    Back,
    Front
}

enum Morph
{
    Slime = 1,
    Rock = 2,
    Hawk = 3,
    Human = 4,
    Salmon = 5
}



public class formStats
{
    public float speed = 8f,
                 maxVel = 4f,
                 jumpForce = 10f,
                 jumpDecay = .90f,
                 swimSpeed = 2,
                 swimMax = 4,
                 sinkSpeed = 1,
                 sinkMax = 2,
                 waterJumpMult = .5f;

    public formStats(float newSpeed, float newMaxVel, float newJumpForce, float newJumpDecay,float newSwimSpeed,float newSwimMax, float newSinkSpeed, float newSinkMax, float newWaterJumpMult)
    {
        speed = newSpeed;
        maxVel = newMaxVel;
        jumpForce = newJumpForce;
        jumpDecay = newJumpDecay;
        swimSpeed = newSwimSpeed;
        swimMax = newSwimMax;
        sinkSpeed = newSinkSpeed;
        sinkMax = newSinkMax;
        waterJumpMult = newWaterJumpMult;
    }
}