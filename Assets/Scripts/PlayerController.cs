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
    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    private GameObject spriteObject;
    
    private GameManager gameManager;
    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();

        rb = GetComponent<Rigidbody2D>();
        spriteObject = spriteRenderer.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown(startButton))
        {
            soundManager.PlaySound("Start Button");
            playerInitiated = true;
        }
            

        if (playerInitiated && !gameManager.endGame)
        {
            //HandleMovement();
            if (Input.GetButtonDown(useButton))
            {
                //ToggleHold();
                CheckIfBuildObj();
            }

            else if (Input.GetButtonDown(throwButton))
            {
                //Throw();
                CheckIfBuildObj();
            }
        }
            
        //holdable = null;
	}

    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        //print(value.normalized);

        velocity.x = value.x * speed * Time.deltaTime;
        velocity.y = value.y * speed * Time.deltaTime;
        rb.velocity = velocity;

        UpdateSprite(value.x, value.y);
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

        if (velocity.sqrMagnitude > 0f)
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dy, dx) * Mathf.Rad2Deg);
    }*/

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
        levitateParticle.SetActive(false);
        child.transform.parent = null;
        child.GetComponent<Rigidbody2D>().isKinematic = false;
        child.layer = GetLayerNumber(boxLayer.value);
        child = null;
    }

    void Pickup()
    {
        levitateParticle.SetActive(true);
        child = holdable;
        Debug.Log("Child has been assigned");
        child.transform.parent = gameObject.transform;
        child.transform.position = holdPosition.transform.position;
        Rigidbody2D temp = child.GetComponent<Rigidbody2D>();
        temp.velocity = Vector3.zero;
        temp.isKinematic = true;
        child.layer = GetLayerNumber(heldLayer.value);
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.performed && child != null)
        {
            GameObject temp = child;
            Drop();
            float theta = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            temp.GetComponent<Rigidbody2D>().velocity = dir * throwForce;
        }
    }

    void CheckIfBuildObj()
    {
        holdingBuildObj = false;

        if (child != null)
        {
            if (child.gameObject.tag == "Altar Piece" || child.gameObject.tag == "Bucket" || child.gameObject.tag == "Candle")
                holdingBuildObj = true;
        }
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Crate" || other.gameObject.tag == "Rock" || other.gameObject.tag == "Altar Piece" ||
            other.gameObject.tag == "Bucket" || other.gameObject.tag == "Candle" || other.gameObject.tag == "Bomb")
        {
            holdable = other.gameObject;
            Debug.Log("Holdable = " + other.gameObject.name);
        }
                
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (holdable == other.gameObject)
        {
            holdable = null;
        }
    }
}
