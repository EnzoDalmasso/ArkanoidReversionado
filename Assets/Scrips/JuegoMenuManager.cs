using UnityEngine;
using UnityEngine.SceneManagement;

public class JuegoMenuManager : MonoBehaviour
{

    [Header("Paneles del juego")]
    public GameObject panelPausa;
    public GameObject panelControles;
    public GameObject panelVictoria;
    public GameObject panelDerrota;

    private bool juegoPausado = false;

    //PUNTAJE
    public GameObject txtPuntaje;
   

    //VIDA
    public GameObject txtVida;
    public GameObject ImageVida;

    private Ball ball;


    //Guarda el padre original del puntaje
    private Transform puntajePadreOriginal;
    //Posiciones destino:
    public Transform puntajePosVictoria;
    public Transform puntajePosDerrota;


    private void Start()
    {
        OcultarTodosLosPaneles();
        puntajePadreOriginal = txtPuntaje.transform.parent;

        txtPuntaje.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.puntajeTotal.ToString();
        txtVida.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.vidaActual.ToString();
    }   


    //VERIFICACION SI EL PLAYER ESTA VIVO O MUERTO
    private void OnEnable()
    {

        ball = FindFirstObjectByType<Ball>();

        if (ball != null)
        {
            ball.deadPlayer += OnMuerteJugador;
        }
           
    }

    
    private void OnDisable()
    {

        ball = FindFirstObjectByType<Ball>();

        if (ball != null) 
        {
            ball.deadPlayer -= OnMuerteJugador;
            
        }
           

    }

    //AL PRECIONAR ESCAPE ACTIVA O DESACTIVA LA PAUSA
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panelPausa.activeSelf && !panelControles.activeSelf &&
                !panelVictoria.activeSelf && !panelDerrota.activeSelf)
            {
                PausarJuego();
            }
            else if (panelPausa.activeSelf)
            {
                ReanudarJuego();
            }
        }
        //Recarga el ultimo checkpoint al presionar la R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    
//OCULTA LOS PANELES
private void OcultarTodosLosPaneles()
    {
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        panelVictoria.SetActive(false);
        panelDerrota.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;


    }

    // PAUSA
    public void PausarJuego()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
        
        txtPuntaje.SetActive(false);
        txtVida.SetActive(false);
        ImageVida.SetActive(false);


        //Cambio cursor al de menu
       // CursorController cambioCursor = FindFirstObjectByType<CursorController>();
        //if (cambioCursor != null)
       // {
        //    cambioCursor.MostrarCursorMenu();
       //}

    }

    //REANUDA EL JUEGO
    public void ReanudarJuego()
    {
        panelPausa.SetActive(false);
        panelControles.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        txtPuntaje.SetActive(true);
        txtVida.SetActive(true);
        ImageVida.SetActive(true);

        // CursorController cambioCursor = FindFirstObjectByType<CursorController>();
        // if (cambioCursor != null)
        // {
        //     cambioCursor.MostrarCursorJuego();
        //}

    }

    //PANEL DE CONTROLES
    public void MostrarControles()
    {
        panelPausa.SetActive(false);
        panelControles.SetActive(true);
  

        //CursorController cambioCursor = FindFirstObjectByType<CursorController>();
        //if (cambioCursor != null)
        //{
          //  cambioCursor.MostrarCursorMenu();
        //}

    }

    
    public void VolverDesdeControles()
    {
        panelControles.SetActive(false);
        panelPausa.SetActive(true);
   
    }

    //MUESTRA PANEL DE VICTORIA
    public void MostrarVictoria()
    {
        OcultarTodosLosPaneles();
        panelVictoria.SetActive(true);
        Time.timeScale = 0f;

        ImageVida.SetActive(false);
        txtVida.SetActive(false);

        // Mover puntaje al panel de victoria
        txtPuntaje.transform.SetParent(puntajePosVictoria);
        txtPuntaje.transform.localPosition = Vector3.zero; // centrado

        //Mostramos cursor de menu
        //CursorController cambioCursor = FindFirstObjectByType<CursorController>();
        //if (cambioCursor != null)
        //{ cambioCursor.MostrarCursorMenu(); }
    }

    //MUESTRA PANEL DE DERROTA
    public void MostrarDerrota()
    {
        OcultarTodosLosPaneles();
        panelDerrota.SetActive(true);
        Time.timeScale = 0f;

        ImageVida.SetActive(false);
        txtVida.SetActive(false);
        // Mover puntaje al panel de derrota
        txtPuntaje.transform.SetParent(puntajePosDerrota);
        txtPuntaje.transform.localPosition = Vector3.zero;

        // CursorController cambioCursor = FindFirstObjectByType<CursorController>();
        // if (cambioCursor != null)
        //{ cambioCursor.MostrarCursorMenu(); }
    }

    //RECIBE EVENTO DE MUERTE DEL PLAYER
    public void OnMuerteJugador(object sender, System.EventArgs e)
    {
        MostrarDerrota();
    }

    //
    public void ResetLevel()
    {
        Ball ball = FindFirstObjectByType<Ball>();
        Player player = FindFirstObjectByType<Player>();

        ball.resetBall();
        player.resetPlayer();
    }

    //PUNTAJE
    public void SumarPuntos(int puntos)
    {
        GameManager.instance.SumarPuntos(puntos);
        txtPuntaje.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.puntajeTotal.ToString();
    }

    public void RestarVida(int vida)
    {
        txtVida.GetComponent<TMPro.TextMeshProUGUI>().text = vida.ToString();
    }
    //Siguiente Nivel
    public void SiguienteNivel(string nombreSiguienteEscena)
    {
        //Reactivar el tiempo por si estaba pausado
        Time.timeScale = 1f;

        //Cargar siguiente nivel
        SceneManager.LoadScene(nombreSiguienteEscena);
        
    }

    
    //REINICIAMOS NIVEL
    public void ReiniciarNivel()
    {

        Time.timeScale = 1f;

        // devolver puntaje a su HUD original
        txtPuntaje.transform.SetParent(puntajePadreOriginal);
        txtPuntaje.transform.localPosition = Vector3.zero;

        GameManager.instance.ResetearPuntaje();
        GameManager.instance.ResetearVida();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //VOLVEMOS AL MENU INICIAL
    public void VolverAlMenuInicial(string nombreEscenaMenu)
    {
        Time.timeScale = 1f;
       
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}