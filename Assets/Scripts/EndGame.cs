using UnityEngine;
using UnityEngine.SceneManagement; //biblioteka potrzebna do obslugi scen

public class EndGame : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true; //pokazuje kursor
    }

    void Update()
    {
        //Zaczyna nowa gre po wcisnieciu kalwisza "Enter"
        if (Input.GetKeyDown(KeyCode.Return))
            NewGame();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1); //laduje ponownie scene gry, dzieki czemu gra zaczyna sie na nowo
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu"); //laduje scene z glownym menu
    }

    public void QuitGame()
    {
        Application.Quit(); //zamyka gre
    }
}
