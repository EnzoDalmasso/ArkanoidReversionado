using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScene : MonoBehaviour
{
  
    public void resetGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
