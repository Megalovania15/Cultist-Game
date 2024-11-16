using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //By Rowan Reeve

    // *** INSPECTOR PARAMETERS ***

    // Player controls:
    [HideInInspector]
    public string horizAxis, vertAxis;
    [HideInInspector]
    public string useButton, throwButton;
    [HideInInspector]
    public string startButton;

    public bool playerInitiated = false;

    // Movement rate (units/second)
    public float speed;

    // ...
    public float throwForce;

    // Component on object rendering the player
    public SpriteRenderer spriteRenderer;
    // Player sprites (N, NE, E, SE, S, SW, W, NW)
    public Sprite[] sprites;

    // ...
    public GameObject holdPosition;
    public GameObject levitateParticle;

    public LayerMask boxLayer;
    public LayerMask heldLayer;

    public bool holdingBuildObj = false;

    public GameObject child = null;
    public GameObject holdable = null;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    private GameObject spriteObject;
    private GameObject particleEffect;
    private Transform playerPivot;
    
    private GameManager gameManager;
    private SoundManager soundManager;

    private Direction currentDirection = Direction.East;
    private Direction nextDirection = Direction.East;

    enum Direction 
    {
        NorthWest = 1,
        North = 2,
        NorthEast = 3,
        East = 4,
        SouthEast = 5,
        South = 6,
        SouthWest = 7,
        West= 8
    }

	// Use this for initialization
	void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        anim = GetComponent<Animator>();
        playerPivot = this.gameObject.transform.GetChild(1);

        rb = GetComponent<Rigidbody2D>();
        spriteObject = spriteRenderer.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*if (Input.GetButtonDown(startButton))
        {
            soundManager.PlaySound("Start Button");
            playerInitiated = true;
        }*/
            

        /*if (playerInitiated && !gameManager.endGame)
        {
            //HandleMovement();
            if (Input.GetButtonDown(useButton))
            {
                //ToggleHold();
                //CheckIfBuildObj();
            }

            else if (Input.GetButtonDown(throwButton))
            {
                //Throw();
                //CheckIfBuildObj();
            }
        }*/

        if (child != null)
        {
            child.transform.position = holdPosition.transform.position;
        }
            
        //holdable = null;
	}

    public void OnMove(InputAction.CallbackContext context)
    {
        var moveDir = context.ReadValue<Vector2>();

        velocity.x = moveDir.x * speed * Time.deltaTime;
        velocity.y = moveDir.y * speed * Time.deltaTime;
        rb.velocity = velocity;

        UpdateSprite(moveDir.x, moveDir.y);
        UpdateRotation(moveDir.x, moveDir.y);
    }

    void UpdateRotation(float dx, float dy)
    {
        if (velocity.sqrMagnitude > 0f)
        {
            playerPivot.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg);

            playerPivot.transform.position = transform.position;

            //transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg);
        }    
            
    }

    /*void HandleMovement()
    {
        float dx = Mathf.RoundToInt(Input.GetAxis(horizAxis)), dy = Mathf.RoundToInt(Input.GetAxis(vertAxis));

        UpdateTransform(dx, dy);
        UpdateSprite(dx, dy);
    }

    void UpdateTransform(float dx, float dy)
    {
        velocity.x = dx * speed * Time.deltaTime;
        velocity.y = dy * speed * Time.deltaTime;
        rb.velocity = velocity;

        
    }*/

    /*void UpdateSprite(float dx, float dy)
    {
        float angle = Mathf.Atan2(dy, dx);
        Direction cardinal = ToDirection(angle);

        print(cardinal);

        anim.SetInteger("Direction", (int)cardinal);
        anim.SetTrigger("DirectionChanged");

    }*/

    //// Converts an angle in radians between [-π, π] to 1 of 8 cardinal
    // directions. The direction choses - Rowan's code
    private Direction ToDirection(float angle)
    {
        Direction direction = Direction.East;
        if (angle > -Mathf.PI / 8 && angle <= Mathf.PI / 8)
        {
            direction = Direction.East;
        }
        else if (angle > Mathf.PI / 8 && angle <= (3 * Mathf.PI) / 8)
        {
            direction = Direction.NorthEast;
        }
        else if (angle > (3 * Mathf.PI) / 8 && angle <= (5 * Mathf.PI) / 8)
        {
            direction = Direction.North;
        }
        else if (angle > (5 * Mathf.PI) / 8 && angle <= (7 * Mathf.PI) / 8)
        {
            direction = Direction.NorthWest;
        }
        else if (angle > (7 * Mathf.PI) / 8 || angle <= (-7 * Mathf.PI) / 8)
        {
            direction = Direction.West;
        }
        else if (angle > (-7 * Mathf.PI) / 8 && angle <= (-5 * Mathf.PI) / 8)
        {
            direction = Direction.SouthWest;
        }
        else if (angle > (-5 * Mathf.PI) / 8 && angle <= (-3 * Mathf.PI) / 8)
        {
            direction = Direction.South;
        }
        else if (angle > (-3 * Mathf.PI) / 8 && angle <= -Mathf.PI / 8)
        {
            direction = Direction.SouthEast;
        }
        return direction;
    }

    void UpdateSprite(float dx, float dy)
    {
        if (dx > 0f)
            if (dy > 0f) // NE
            {
                spriteRenderer.sprite = sprites[1];
            }
            else if (dy < 0f) // SE
            {
                spriteRenderer.sprite = sprites[4];
            }
            else // E
                spriteRenderer.sprite = sprites[6];
        else if (dx < 0f)
            if (dy > 0f) // NW
                spriteRenderer.sprite = sprites[2];
            else if (dy < 0f) // SW
                spriteRenderer.sprite = sprites[5];
            else // W
                spriteRenderer.sprite = sprites[7];
        else if (dy > 0f) // N
            spriteRenderer.sprite = sprites[0];
        else if (dy < 0f) // S
            spriteRenderer.sprite = sprites[3];

        spriteObject.transform.position = transform.position;
    }

    public void ToggleHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (child != null)
            {
                Drop();
            }
            else if (holdable != null)
            {
                Pickup();
            }
        }
    }

    /*void ToggleHold()
    {
        if (child != null)
        {
            Drop();
        }
        else if (holdable != null)
        {
            Pickup();
        }
    }*/

    public void Drop()
    {
        if (child != null)
        {
            Destroy(particleEffect);
            //child.transform.parent = null;
            child.GetComponent<Rigidbody2D>().isKinematic = false;
            child.layer = GetLayerNumber(boxLayer.value);
            child = null;
        }
    }

    void Pickup()
    {
        //levitateParticle.SetActive(true);
        child = holdable;
        particleEffect = Instantiate(levitateParticle, child.transform);
        particleEffect.transform.position = child.transform.position + new Vector3(0, -0.5f, 0);
        Debug.Log("Child has been assigned");
        //child.transform.parent = playerPivot;
        child.transform.position = holdPosition.transform.position;
        Rigidbody2D temp = child.GetComponent<Rigidbody2D>();
        temp.velocity = Vector3.zero;
        temp.isKinematic = true;
        child.layer = GetLayerNumber(heldLayer.value);
        CheckIfBuildObj();

        if (child.CompareTag("Bomb"))
        {
            Bomb bombComponent = child.GetComponent<Bomb>();

            if (!bombComponent.IsDetonating)
            {
                bombComponent.StartCoroutine(bombComponent.DetonateBomb());
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed && child != null)
        {
            CheckIfBuildObj();
            GameObject temp = child;
            Drop();
            float theta = playerPivot.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            Debug.Log(dir);
            temp.GetComponent<Rigidbody2D>().velocity = dir * throwForce;
        }
    }

    bool CheckIfBuildObj()
    {
        if (child != null)
        {
            if (child.gameObject.tag == "Altar Piece" || child.gameObject.tag == "Bucket" || child.gameObject.tag == "Candle")
               return holdingBuildObj = true;
        }

        return holdingBuildObj = false;
    }

    static int GetLayerNumber(int layer)
    {
        int layerNumber = 0;
        while (layer > 1)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber;
    }

    
}
