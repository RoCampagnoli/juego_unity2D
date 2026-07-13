using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C5_colisiones : MonoBehaviour{
    private PersonajeVida jugador;
    private C4_malvados malvado; 
    private C4_malvados malvadoTocando;


    void Start(){
        jugador= GetComponent<PersonajeVida>();
    }

    void Update(){
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
         malvado = collision.gameObject.GetComponent<C4_malvados>();

         if (malvado != null &&  (jugador != null && !jugador.EstoyMuerto())){

             jugador.PerderVida(malvado.getDanio());

         }
    }
    private void OnCollisionStay2D(Collision2D collision) {
        malvadoTocando = collision.gameObject.GetComponent<C4_malvados>();
        // mientras el enemigo me sigue tocando, sigue restando vida,
        // respetando el cooldown del enemigo

        if (malvadoTocando != null && (jugador != null && !jugador.EstoyMuerto())) {
            if (malvadoTocando.PuedeGolpear()) {
                jugador.PerderVida(malvadoTocando.getDanio());
                Debug.Log("tocando");
            }
        }
    }

       
    

}
