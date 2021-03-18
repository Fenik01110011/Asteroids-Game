using UnityEngine;
using UnityEngine.SceneManagement; //biblioteka potrzebna do obslugi scen

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        //zaladowuje kolejna scene zgodnie z kolejnosci w menadzerze scen, w tym przypadku scene gry
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit(); //zamyka gre
    }
}
