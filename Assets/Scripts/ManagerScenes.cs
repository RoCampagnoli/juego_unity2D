using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScenes : MonoBehaviour{


    public void CambiarEscenas(string a) {
        SceneManager.LoadScene(a);
    }

    public void Salir() {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }

    public void VolverAJugar() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        if (FindObjectsOfType<ManagerScenes>().Length > 1) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    void Update()
    {
        
    }
}
