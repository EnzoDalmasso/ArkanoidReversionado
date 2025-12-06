using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private int bricksRemaining;
    private JuegoMenuManager menuManager;

    private void Start()
    {
        //Referencia al menu
        menuManager = FindFirstObjectByType<JuegoMenuManager>();

        //Contamos SOLO los ladrillos hijos de este objeto
        bricksRemaining = GetComponentsInChildren<Brick>().Length;
 
    }

    public void BrickDestroyed()
    {
        bricksRemaining--;

        
        if (bricksRemaining <= 0)
        {
            menuManager.MostrarVictoria();
        }
        
    }
}
