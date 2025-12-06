using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int puntajeTotal = 0;
    public int vidaMax = 3;
    public int vidaActual = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //NO se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Metodo para sumar puntaje
    public void SumarPuntos(int puntos)
    {
        puntajeTotal += puntos;
    }

    public void ResetearVida()
    {
        vidaActual = vidaMax;
    }
    public void ResetearPuntaje()
    {
        puntajeTotal = 0;
    }
}
