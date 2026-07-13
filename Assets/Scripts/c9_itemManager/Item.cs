using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public int myCount = 1;
    [SerializeField] private string objId;

    private bool picked = false;
    [SerializeField] private AudioClip clip;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public string GetObjId() {
        return objId;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (picked) return;

        if (collision.CompareTag("Player")) {
            picked = true;
            ItemManager inv = collision.GetComponent<ItemManager>();

            // primero buscamos la clave SIN modificar el diccionario dentro del foreach
            GameObject claveEncontrada = null;
            foreach (KeyValuePair<GameObject, int> items in inv.inventory) {
                Item invId = items.Key.GetComponent<Item>();
                if (invId.objId == objId) {
                    claveEncontrada = items.Key;
                    break;
                }
            }

            // recien aca, ya afuera del foreach, modificamos el diccionario
            if (claveEncontrada != null) {
                inv.inventory[claveEncontrada] += myCount;
                print("Tenes " + inv.inventory[claveEncontrada] + " " + claveEncontrada.name);

                // Efectos especiales por tipo de objeto
                if (objId == "espada") {
                    collision.GetComponent<PersonajeVida>().tengoEspada = true;
                    collision.GetComponent<PersonajeVida>().ActualizarTextos();

                }
                if (objId == "llave") {
                    collision.GetComponent<PersonajeVida>().SumarLlave();

                }
               

                if (clip != null)
                   AudioSource.PlayClipAtPoint(clip, transform.position);

               Destroy(gameObject);
                
            }
        }
    }
}
    

