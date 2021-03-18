using System; //biblioteka potrzebna do uzycia funkcji Convert.ToInt32() i Convert.ToBoolean()
using UnityEngine;
using UnityEngine.Audio;

public class SettingsApply : MonoBehaviour
{
    //pobieranie potrzebnych uchwytow obiektow
    public AudioMixer musicAudioMixer;
    public AudioMixer soundsAudioMixer;

    //domyslne ustawienia gry
    public float defaulMusicVolume = -20f;
    public float defaulSoundsVolume = -20f;
    public bool defaulIsFullScreen = true;
    public bool cursorVisible = true;

    void Start()
    {
        //ustawienie glosnosci muzyki zgodnie z zapisanymi ustawieniami gracza przy wykorzystaniu "PlayerPrefs"
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", defaulMusicVolume);
        musicAudioMixer.SetFloat("volume", musicVolume);

        //ustawienie paska poziomu glosnosci dzwiekow w grze
        float soundsVolume = PlayerPrefs.GetFloat("soundsVolume", defaulSoundsVolume);
        soundsAudioMixer.SetFloat("volume", soundsVolume);

        //ustawienie pelnego ekranu gry zgodnie z wczesniejszymi ustawieniami gracza
        Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("isFullScreen", Convert.ToInt32(defaulIsFullScreen)));

        Cursor.visible = cursorVisible;
    }
}
