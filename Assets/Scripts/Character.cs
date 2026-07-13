using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float vida = 100f;
    [SerializeField] protected float vidaMaxima = 100f;
    protected SpriteRenderer miSprite;



    public virtual void SumarVida(float cantidad) {
        vida += cantidad;
       
    }

    public virtual void PerderVida(float danio) {
        vida -= danio;
        if (vida < 0) {
            vida = 0;
        }
        StartCoroutine(ParpadearDanio());

    }
    protected virtual void Awake() {
        miSprite = GetComponent<SpriteRenderer>();
    }
    protected IEnumerator ParpadearDanio() {
        if (miSprite == null) yield break;
        miSprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        miSprite.color = Color.white;
    }
}
