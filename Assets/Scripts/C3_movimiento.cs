using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C3_movimiento : MonoBehaviour {

    //variables de direccion
    private int movimientoHorizontal;//0  está quieto. 
    //private int movimientoVertical;
    private Vector2 mov;

    //para correr
    [SerializeField] private float speed=5f;//velocidad
    private float ogSpeed;
    private float multSpeed;//velocidad multiplicadas
    [SerializeField] private float valMultSpeed = 1.5f;

    //para saltar
    [SerializeField] private float fuerzaSalto = 17f;
    private bool saltoActivo = false;

    private Rigidbody2D rb;

    Animator animator;
    private bool giroIzq;

    private PersonajeVida personaje;

    private bool estaEnElSuelo = false;


    void Start() {
        
        rb = GetComponent<Rigidbody2D>();//quiero acceder a las propiedades del rigid body
        //guardamos la velocidad original
        ogSpeed = speed;
        //calculamos la velocidad aumentada
        multSpeed = speed * valMultSpeed;

        //accedo a las propiedades del animator
        animator= GetComponent<Animator>();

        personaje = GetComponent<PersonajeVida>();
    }

    private void Update()
    {
        if (personaje != null && personaje.EstoyMuerto()) return;

        Sprint(multSpeed);
        MovH(1);
        //MovV(1);
        Salto(1);
    }
    private void MovH(int a)
    {
        //MOVIMIENTO HORIZONTAL
        if (Input.GetKey(KeyCode.D))
        {
            movimientoHorizontal = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movimientoHorizontal = -1;
        }
        else
        {
            movimientoHorizontal = 0;
        }

    }

    private void Salto(int a) {
        
        if (Input.GetKeyDown(KeyCode.Space)&& saltoActivo==false) {
            saltoActivo=true;
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("suelo")) return;

        // recorremos los puntos de contacto para asegurarnos de que
        // el impacto viene desde ABAJO (aterrizaje), no desde el costado (pared)
        foreach (ContactPoint2D contacto in collision.contacts) {
            if (contacto.normal.y > 0.5f) {
                saltoActivo = false;
                estaEnElSuelo = true;
                break;
            }
        }
    }


    //PARA CORRER
    private void Sprint(float multSpeed)
    {
        //para correr
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = multSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = ogSpeed;
        }
    }

    private void FixedUpdate() {
        if (personaje != null && personaje.EstoyMuerto()) {
            rb.velocity = new Vector2(0f, rb.velocity.y); // sin movimiento horizontal, pero cae por gravedad
            return;
        }
       
        rb.velocity = new Vector2(movimientoHorizontal * speed, rb.velocity.y);
        
        AnimacionPlayer();
        GirarPlayer();
        
    }

    private void AnimacionPlayer() {
        //usamos el Mathf-- devuelve un valor absoluto, siempre positivo
        if (Mathf.Abs(rb.velocity.x) > 0) {
            animator.SetFloat("xVelocity", 1);
            //C A M I N A N D O
            
        } else {
            animator.SetFloat("xVelocity", 0);
            //Q U I E T O 
           
        }
    }

    private void GirarPlayer() {
        //si se esta moviendo a la izquierda, giro al player
        if(rb.velocity.x<0 && !giroIzq) {
            giroIzq = true;
            Vector3 ls=transform.localScale;//guardo la escala actual del objeto
            ls.x *= -1;//invierto el eje X
            transform.localScale = ls;//aplico el cambio

        } else if (rb.velocity.x>0 && giroIzq) { 
            giroIzq = false;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    public bool EstaEnElSuelo() {
        return estaEnElSuelo;
    }

}

