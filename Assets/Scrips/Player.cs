using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rbPlayer;
    private float inputValue;
    [SerializeField] public float moveSpeed = 25f;

    private Vector2 direction;

    Vector2 starPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        inputValue = Input.GetAxisRaw("Horizontal");

        if (inputValue == 1)
        {
            direction = Vector2.right;
        }
        else if (inputValue == -1)
        {

            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.zero;  
        }

        rbPlayer.AddForce(direction*moveSpeed*Time.deltaTime*100);
            
    }

    public void resetPlayer()
    {
        transform.position = starPosition;
        rbPlayer.linearVelocity = Vector2.zero;
    }
}

