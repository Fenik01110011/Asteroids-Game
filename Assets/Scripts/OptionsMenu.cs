using System; //biblioteka potrzebna do uzycia funkcji Convert.ToInt32()
using UnityEngine;
using UnityEngine.Audio; //biblioteka odpowiadajaca za obsluge audio
using UnityEngine.UI; //biblioteka potrzebna do obslugi elementow interfejsu uzytkownika takich jak "Slider" i "Toggle"

public class OptionsMenu : MonoBehaviour
{
    //pobranie potrzebnych uchwytow do obiektow
    public AudioMixer musicAudioMixer;
    public AudioMixer soundsAudioMixer;
    public GameObject sliderMusicVolume;
    public GameObject sliderSoundsVolume;
    public GameObject fullscreenToggle;
    public GameObject SettingsApply;

    private void Start()
    {
        //ustawienie paska poziomu glosnosci muzyki zgodnie z zapisanymi ustawieniami gracza przy wykorzystaniu "PlayerPrefs"
        //w przypadku nieustawionej wartosci przyjmuje ustawiona domyslna wartosc
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", SettingsApply.GetComponent<SettingsApply>().defaulMusicVolume);
        sliderMusicVolume.GetComponent<Slider>().value = musicVolume;

        //ustawienie paska poziomu glosnosci dzwiekow w grze
        float soundsVolume = PlayerPrefs.GetFloat("soundsVolume", SettingsApply.GetComponent<SettingsApply>().defaulSoundsVolume);
        sliderSoundsVolume.GetComponent<Slider>().value = soundsVolume;

        //ustawienie zaznaczenia przelacznika zgodnie z tym czy gra zajmuje obecnie pelny ekran
        fullscreenToggle.GetComponent<Toggle>().isOn = Screen.fullScreen;
    }
    
    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("volume", volume); //ustawia glosnosc muzyki
        PlayerPrefs.SetFloat("musicVolume", volume); //zapisuje ustawienia glosnosc muzyki za pomoca "PlayerPrefs"
    }

    public void SetSoundsVolume(float volume)
    {
        soundsAudioMixer.SetFloat("volume", volume); //ustawia glosnosc dzwiekow w grze
        PlayerPrefs.SetFloat("soundsVolume", volume); //zapisuje ustawienia glosnosc dzwiekow w grze za pomoca "PlayerPrefs"
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen; //wlacza lub wylacza pelny ekran
        PlayerPrefs.SetInt("isFullScreen", Convert.ToInt32(isFullScreen)); //zapis wyboru pelnego ekranu
    }
}
