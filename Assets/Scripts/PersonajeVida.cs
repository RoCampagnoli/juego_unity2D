using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PersonajeVida : Character{
    // referencia para la barra y el texto
    private bool estoyMuerto = false;

    public Image healtBar;//barra de vida
    public TMP_Text textoVida;


    public TMP_Text textoEspada;

    public TMP_Text textoLlave;


    public bool tengoEspada = false;

    public int llavesRecolectadas = 0;
    [SerializeField] private int llavesNecesarias = 3;

    private Animator animator;

    [Header("Ajuste visual al morir")]
    [SerializeField] private float ajusteYAlMorir = -0.45f;
    // cuanto bajar el sprite para que apoye en el piso (probar valores negativos chicos)

    [Header("Ataque con espada")]
    [SerializeField] private float rangoEspada = 2f; // que tan lejos llega el golpe
    [SerializeField] private int danioEspada = 40;

    [Header("Colores de la barra de vida")]
    [SerializeField] private Color colorVidaNormal = Color.green;
    [SerializeField] private Color colorVidaBaja = Color.red;
    [SerializeField] private Color colorVidaExtra = new Color(1f, 0.84f, 0f); // dorado

    [Header("Sonido de Game Over")]
    [SerializeField] private AudioClip sonidoGameOver;
    private AudioSource aSource;

    [Header("Game Over")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private Button botonVolverJugar;
    [SerializeField] private Button botonSalir;

    void Start(){
        animator = GetComponent<Animator>();
        aSource = GetComponent<AudioSource>();

        ActualizarUI();
        ActualizarTextos();
    }

    void Update(){
        AtacarConEspada();
    }

    public override void SumarVida(float pocion){
       base.SumarVida(pocion);
        ActualizarUI();

        Debug.Log("Vida restante: " + vida);
    }

    public override void PerderVida(float danioMalvado) 
    {
        if (estoyMuerto) return;// si ya esta muerto, ignoramos cualquier daño nuevo

        base.PerderVida(danioMalvado);
        ActualizarUI();

        Debug.Log("Vida restante: " + vida);
        if (vida <= 0) {
            Morir();
        }
    }
    private void Morir() {
        estoyMuerto = true;
        Debug.Log("jugador muerto");
        animator.SetTrigger("muerte");

        if (sonidoGameOver != null && aSource != null) {
            aSource.PlayOneShot(sonidoGameOver);
        }

        if (panelGameOver != null) {
            panelGameOver.SetActive(true);
            ConectarBotonesGameOver();

        }

        StartCoroutine(EsperarYFijarEnElSuelo());

    }

    private void ConectarBotonesGameOver() {
        ManagerScenes manager = FindObjectOfType<ManagerScenes>();
        if (manager == null) {
            Debug.LogWarning("No se encontro ManagerScenes. Asegurate de arrancar el juego desde la escena menu.");
            return;
        }
        botonVolverJugar.onClick.AddListener(manager.VolverAJugar);
        botonSalir.onClick.AddListener(manager.Salir);
    }

    private IEnumerator EsperarYFijarEnElSuelo() {
        C3_movimiento movimiento = GetComponent<C3_movimiento>();
        // esperamos, frame a frame, hasta que la fisica lo haya asentado en el piso
        while (movimiento != null && !movimiento.EstaEnElSuelo()) {
            yield return null;
        }
        // desactivamos el collider para que no siga ocupando el espacio vertical
        // de antes, ya que el sprite ahora se ve "caido" pero el collider no rota con el
        Collider2D miCollider = GetComponent<Collider2D>();
        if (miCollider != null) {
            miCollider.enabled = false;
        }

        // sin collider, la gravedad lo haria caer atravesando el piso,
        // asi que congelamos el rigidbody para que se quede fijo donde murio
        Rigidbody2D miRigidbody = GetComponent<Rigidbody2D>();
        if (miRigidbody != null) {
            miRigidbody.velocity = Vector2.zero;
            miRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }


        // ajustamos la posicion Y relativa a donde estaba parado, no a un valor fijo,
        // para que funcione sea cual sea la altura del piso donde murio
        Vector3 posicionActual = transform.position;
        transform.position = new Vector3(posicionActual.x, posicionActual.y + ajusteYAlMorir, posicionActual.z);


    }


    public bool EstoyMuerto() {
        return estoyMuerto;
    }

    private void AtacarConEspada() {
        if (Input.GetKeyDown(KeyCode.X) && tengoEspada ) {
            animator.SetTrigger("Ataque");//evento momentaneo
            GolpearConEspada(); 
        } 
    }
    private void GolpearConEspada() {
        float direccion = Mathf.Sign(transform.localScale.x); // 1 = mira derecha, -1 = mira izquierda
        Vector2 origen = transform.position;
        Vector2 puntoGolpe = origen + new Vector2(direccion * rangoEspada, 0f);
        
        LayerMask capaEnemigos = LayerMask.GetMask("Enemigos");

        Collider2D enemigoGolpeado = Physics2D.OverlapCircle(puntoGolpe, 0.5f,capaEnemigos);
        Debug.Log("Collider encontrado: " + (enemigoGolpeado == null ? "NINGUNO" : enemigoGolpeado.gameObject.name));


        if (enemigoGolpeado != null) {
            C4_malvados malvado = enemigoGolpeado.GetComponent<C4_malvados>();
            if (malvado != null) {
                malvado.PerderVida(danioEspada);
            }
        }
    }


    public void SumarLlave() {
        llavesRecolectadas++;
        ActualizarTextos();
    }

    public bool TengoTodasLasLlaves() {
        return llavesRecolectadas >= llavesNecesarias;
    }

    private void ActualizarUI() {
        // la barra no puede pasar de 1, asi que la calculamos aparte clampeada
        healtBar.fillAmount = Mathf.Clamp01(vida / vidaMaxima);
        // el texto en cambio muestra el valor real, sin clampear
        if (textoVida != null) {
            textoVida.text = vida + "/" + vidaMaxima;
        }
        ActualizarColorBarra();


    }
    public void ActualizarTextos() {
        if (tengoEspada) {
            textoEspada.text = "1/1";
        } else {
            textoEspada.text = "0/1";
        }

        textoLlave.text = llavesRecolectadas + "/" + llavesNecesarias;

    }
    private void ActualizarColorBarra() {
        if (vida > 100) {
            healtBar.color = colorVidaExtra; // dorado
        } else if (vida < 50) {
            healtBar.color = colorVidaBaja; // rojo
        } else {
            healtBar.color = colorVidaNormal; // el color de siempre
        }
    }

   
}
