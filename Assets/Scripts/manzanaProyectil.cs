using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manzanaProyectil : MonoBehaviour { 
     [SerializeField] private int danio = 10; // cuanto resta a la vida del enemigo al impactar

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        ImpactarSiEsEnemigo(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ImpactarSiEsEnemigo(collision.gameObject);
    }

    private void ImpactarSiEsEnemigo(GameObject malito) {
        C4_malvados enemigo = malito.GetComponent<C4_malvados>();
        if (enemigo != null) {
            enemigo.PerderVida(danio);
            Destroy(gameObject); // la manzana se destruye al impactar, no sigue de largo
        }
    }
}
