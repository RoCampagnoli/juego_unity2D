using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaAnimator : MonoBehaviour {

    private PersonajeVida jugador;
    private Animator animator;
    private AudioSource aSource;
    private bool puertaAbierta = false;
    void Start() {
        animator = GetComponent<Animator>();   
        aSource = GetComponent<AudioSource>();
    }

    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (puertaAbierta) return;
        jugador = collision.GetComponent<PersonajeVida>();

        if (jugador != null && jugador.TengoTodasLasLlaves()) {
            animator.enabled = true;
            aSource.Play();
            puertaAbierta = true;
        }
    }


}
