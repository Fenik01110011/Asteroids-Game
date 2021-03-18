using UnityEngine;
using UnityEngine.SceneManagement; //biblioteka potrzebna do obslugi scen

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject player;

    void Update()
    {
        //nasluchuje wcisniecia klawisza "Esc" lub "p" w celu zatrzymania gry, oprocz przypadku gdzie zycie gracza jest mniejsze lub rowne 0
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("p")) && player.GetComponent<Player>().health > 0)
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    //wznawia zatrzymana gre
    public void Resume ()
    {
        pauseMenuUI.SetActive(false); //ukrywa menu pauzy
        Time.timeScale = 1f; //uruchamia ponownie czas w grze
        gameIsPaused = false; //ustawia pomocnicza zmienna
        Cursor.visible = false; //ukrywa kursor
    }

    public void Pause()
    {
        //jesli obecny wynik gracza jest wiekszy od najlepszego poprzedniego wyniku
        //to wtedy na wszelki wypadek zapisuje sie jego wartosc popzez wykorzystanie "PlayerPrefs"
        if (PlayerPrefs.GetFloat("topScore", 0) < player.transform.position.z)
            PlayerPrefs.SetFloat("topScore", player.transform.position.z);

        pauseMenuUI.SetActive(true); //pokazanie menu pauzy
        Time.timeScale = 0f; //zatrzymanie czasu gry
        gameIsPaused = true; //ustawienie pomocniczej zmiennej
        Cursor.visible = true; //pokazuje kursor
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; //uruchamia ponownie czas w grze
        SceneManager.LoadScene("Menu"); //laduje scene z menu glownym
    }

    public void QuitGame()
    {
        Application.Quit(); //zamyka gre
    }
}
