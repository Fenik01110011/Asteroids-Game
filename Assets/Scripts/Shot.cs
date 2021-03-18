using UnityEngine;

public class Shot : MonoBehaviour
{
    //pobieranie potrzebnych uchwytow obiektow
    public GameObject bulletPrefab;
    public Rigidbody playerRigidbody;

    //deklaracja zmiennych mozliwych do edycji w Unity
    public float bulletSpeed = 250f;
    public bool bulletSpeedScale = true;
    public bool playSound = true;

    private void Awake() //funkcja wywolywana jeszcze przed funckcja Start()
    {
        //ustawianie glosnosci wystrzalu w zaleznosci od wielkosci broni
        GetComponent<AudioSource>().volume = Mathf.Clamp(Mathf.Abs(transform.localScale.x), 0, 2) / 2;
    }

    //funckja wystrzeliwujaca pocisk
    public void Shoot()
    {
        //utworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2.4f * Mathf.Abs(transform.localScale.x), transform.rotation);
        bullet.transform.localScale *= Mathf.Abs(transform.localScale.x); //dostosowanie wielkosci pocisku
        bullet.GetComponent<Rigidbody>().mass *= Mathf.Pow(Mathf.Abs(transform.localScale.x), 3); //dostosowanie masy pocisku

        //jesli "bulletSpeedScale" ustawione jest na "true", to zmienia predkosc pocisku zaleznie od jego wielkosci
        //w przypadku gdy ustawione jest na "false", wszystkie pociski leca z ta sama okreslona predkoscia
        if (bulletSpeedScale)
            bullet.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity.magnitude * transform.forward + transform.forward * bulletSpeed * 1 / Mathf.Pow(Mathf.Abs(transform.localScale.x), 3);
        else
            bullet.GetComponent<Rigidbody>().velocity = playerRigidbody.velocity + transform.forward * bulletSpeed;

        if(playSound)
            GetComponent<AudioSource>().Play(); //jesli dzwiek jest ustawiony na wlaczony, to odgrywa dzwiek wystrzalu
    }
}
