using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    // REFERENCIAS
    [Header("Referencias")]
    public Rigidbody2D rigidBall;

    // MOVIMIENTO BALL
    [SerializeField] public float speed = 300;
    [SerializeField] private float launchSpeed = 6f;
    private Vector2 velocity;

    // Par�metros para evitar movimiento perfectamente horizontal/vertical
    [Header("Anti-stuck")]
    [SerializeField] private float stuckEpsilon = 0.2f;
    [SerializeField] private float minSpeed = 5f;

    //POSICIONAMIENTO BALL
    Vector2 startPosition;

    // VIDA, MANA Y ESTADO
    [Header("Stats")]
    [SerializeField] private int LifeMax = 3;
    public int currentLife;
    public bool IsAlive;

    // SOUND
    public AudioSource audioSource;
    public AudioClip playerSound, brickSound, deadZoneSound, wallSound;

    // EVENTOS
    JuegoMenuManager menuManager;
    public event EventHandler deadPlayer;

    void Start()
    {
        startPosition = transform.position;

  

        resetBall();

        menuManager = FindFirstObjectByType<JuegoMenuManager>();

        GameManager.instance.ResetearVida();
        currentLife = GameManager.instance.vidaActual;
        if (menuManager != null) menuManager.RestarVida(currentLife);

        IsAlive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            currentLife--;
            GameManager.instance.vidaActual = currentLife;
            if (menuManager != null) menuManager.RestarVida(currentLife);

            if (audioSource != null)
            {
                audioSource.clip = deadZoneSound;
                audioSource.Play();
            }

            Dead();
        }
        else if (collision.gameObject.GetComponent<Player>())
        {
            if (audioSource != null)
            {
                audioSource.clip = playerSound;
                audioSource.Play();
            }
        }
        else if (collision.gameObject.GetComponent<Brick>())
        {
            if (audioSource != null)
            {
                audioSource.clip = brickSound;
                audioSource.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (audioSource != null)
            {
                audioSource.clip = wallSound;
                audioSource.Play();
            }
        }
    }

    public void resetBall()
    {
        transform.position = startPosition;

        if (rigidBall == null) return;

        rigidBall.linearVelocity = Vector2.zero;

        //Genero direccion aleatoria con componente Y positiva para que suba al empezar
        Vector2 dir = new Vector2(UnityEngine.Random.Range(-0.7f, 0.7f), 1f).normalized;

        //Asigno velocidad directa 
        rigidBall.linearVelocity = dir * launchSpeed;
    }

    public void Dead()
    {
        if (!IsAlive) return;

        // Si la vida es 0, muere
        if (currentLife <= 0)
        {
            IsAlive = false;
            deadPlayer?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject, 1f); // Se destruye 
        }
        else
        {
            if (menuManager != null) menuManager.ResetLevel();
        }
    }


    void FixedUpdate()
    {
        if (rigidBall == null) return;

        Vector2 v = rigidBall.linearVelocity;

        
        if (v.sqrMagnitude < 0.0001f) return;

        bool nearHorizontal = Mathf.Abs(v.y) < stuckEpsilon; 
        bool nearVertical = Mathf.Abs(v.x) < stuckEpsilon;   

        if (nearHorizontal || nearVertical)
        {
            Vector2 newDir = v;

            if (nearHorizontal)
            {
                //le metemos un pequeño angulo vertical
                float signY = (v.y >= 0f) ? 1f : -1f; //intentamos respetar sentido vertical si existiera, si no, elegimos positivo
                newDir.y = UnityEngine.Random.Range(0.3f, 0.7f) * signY;
                //damos una componente x distinta de cero
                newDir.x = (Mathf.Abs(v.x) > 0.01f) ? v.x : UnityEngine.Random.Range(0.3f, 0.7f) * (UnityEngine.Random.value < 0.5f ? -1f : 1f);
            }

            if (nearVertical)
            {
                float signX = (v.x >= 0f) ? 1f : -1f;
                newDir.x = UnityEngine.Random.Range(0.3f, 0.7f) * signX;
                newDir.y = (Mathf.Abs(v.y) > 0.01f) ? v.y : UnityEngine.Random.Range(0.3f, 0.7f) * (UnityEngine.Random.value < 0.5f ? -1f : 1f);
            }

            if (newDir == Vector2.zero) newDir = new Vector2(0.5f, 0.5f);

            float currentSpeed = v.magnitude;
            if (currentSpeed < minSpeed) currentSpeed = minSpeed;

            rigidBall.linearVelocity = newDir.normalized * currentSpeed;
        }
    }
}
