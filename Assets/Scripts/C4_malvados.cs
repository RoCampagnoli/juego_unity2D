using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4_malvados : Character {

    public enum EnumMalvado { m_verde, m_violeta };
    public EnumMalvado malvado;
    private int danio;


    //[SerializeField] private Timer timer;// cada tanto tiempo va a aparecer el malo

    //desplazamiento del malvado
    [SerializeField] private float velocidadMov;
    [SerializeField] private float distanciaMov;

    [Header("Persecucion")]
    [SerializeField] private Transform player;
    // arrastrar el Player aca en el inspector
    [SerializeField] private float detectionRange = 2f;
    // que tan lejos detecta al jugador

    [Header("Daño continuo")]
    [SerializeField] private float tiempoEntreGolpes = 2f; // cooldown entre golpes mientras te toca
    private float proximoGolpe = 0f;


    private Vector3 posicionInicial;

    private bool moviendoDerecha = true;

    void Start() {
        posicionInicial = transform.position;
        TipoMalvado();

    }

    void Update() {
        float distanciaJugador = Vector3.Distance(transform.position, player.position);

        if (distanciaJugador < detectionRange) {
            // si el jugador esta cerca, lo persigue en vez de patrullar
            PerseguirJugador();
        } else {
            desplazamientoMalvado();
        }
        /* if(!malitoOn && timer.tiempoRestante <= 0)
         {
             malitoOn = true;
             Debug.Log("Modo Salvaje");
             danio *= 2;
         }*/
    }

    private void TipoMalvado() {
        switch (malvado) {
            case EnumMalvado.m_verde:
                vida = 50f;
                vidaMaxima = 50f;
                danio = 5;
                velocidadMov = 1f;
                distanciaMov = 2f;
                break;
            case EnumMalvado.m_violeta:
                vida = 100f;
                vidaMaxima = 100f;
                danio = 10;
                velocidadMov = 3f;
                distanciaMov = 4f;
                break;
        }
    }

    public int getDanio() {
        return danio;
    }

    public bool PuedeGolpear() {
        // controla el cooldown: si ya pasó el tiempo necesario, puede volver a hacer daño
        if (Time.time >= proximoGolpe) {
            proximoGolpe = Time.time + tiempoEntreGolpes;
            return true;
        }
        return false;
    }

    void PerseguirJugador() {

        // limite de cuanto se puede alejar de su posicion inicial (mismo rango que su patrullaje)
        float limiteIzquierdo = posicionInicial.x - distanciaMov;
        float limiteDerecho = posicionInicial.x + distanciaMov;

        // direccion solo en X, ignorando la altura del jugador (para que no "vuele" si salta)
        float direccionX = player.position.x - transform.position.x;


        // si ya esta en el limite y el jugador esta mas alla, se queda quieto en vez de salir del rango
        if (transform.position.x <= limiteIzquierdo && direccionX < 0) return;
        if (transform.position.x >= limiteDerecho && direccionX > 0) return;

        direccionX = Mathf.Sign(direccionX); // nos quedamos solo con -1 o 1

        // si el jugador esta a la derecha y el enemigo mira a la izquierda (o viceversa), lo giramos
        if ((direccionX > 0 && transform.localScale.x < 0) || (direccionX < 0 && transform.localScale.x > 0)) {
            GirarMalvado();
        }

        Vector3 movimiento = new Vector3(direccionX, 0f, 0f); // sin componente vertical
        transform.Translate(movimiento * velocidadMov * Time.deltaTime);
    }

    void GirarMalvado() {
        Vector3 ls = transform.localScale;//guardo la escala actual del objeto
        ls.x *= -1;//cambio el signo de la escala en x para girar el objeto
        transform.localScale = ls;//aplico el cambio
    }

    void desplazamientoMalvado() {

        if (moviendoDerecha) {
            transform.Translate(Vector2.right * velocidadMov * Time.deltaTime);
            if (transform.position.x >= posicionInicial.x + distanciaMov) {
                moviendoDerecha = false;
                GirarMalvado();
            }
        } else {
            transform.Translate(Vector2.left * velocidadMov * Time.deltaTime);
            if (transform.position.x <= posicionInicial.x - distanciaMov) {
                moviendoDerecha = true;
                GirarMalvado();
            }
        }
    }

    public override void PerderVida(float cantidad) {
        base.PerderVida(cantidad);
        Debug.Log(gameObject.name + " recibio " + cantidad + " de daño, vida restante: " + vida);

        if (vida <= 0) {
            Destroy(gameObject);
        }
    }
   
}
