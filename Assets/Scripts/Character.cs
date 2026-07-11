using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float vida = 100f;
    [SerializeField] protected float vidaMaxima = 100f;


    public virtual void SumarVida(float cantidad) {
        vida += cantidad;
        if (vida > vidaMaxima) {
            vida= vidaMaxima;
        }
    }

    public virtual void PerderVida(float danio) {
        vida -= danio;
        if (vida < 0) {
            vida = 0;
        }
    }
   
}
