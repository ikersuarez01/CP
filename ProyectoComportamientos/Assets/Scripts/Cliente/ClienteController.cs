using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClienteController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    [SerializeField] public WorldController worldController;
    [SerializeField] public GameObject bocadilloEsperando;
    [SerializeField] public GameObject bocadilloBebiendo;
    [SerializeField] public GameObject bocadilloRechazarPropina;
    [SerializeField] public GameObject bocadilloAceptarPropina;



    public int state = 0;
    public Vector3 destination;
    public Vector3 initPos;
    public Mesa mesa;
    public Bebida bebida;
    public bool pausaBailarina = false;
    private bool objectSpawned = false;
    GameObject bocAux = null;

    private bool mesaLibreFirstTime = true;

    private bool oneTime = false;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        initPos = new Vector3(transform.position.x-3, transform.position.y, transform.position.z);
        bebida = GetComponent<Bebida>();

    }
    private void Update()
    {
        if (pausaBailarina)
        {
            //Se para pa mirar a la bailarina
            //si la bailarina va hacia él, para de hacer todo
            
        }
        else
        {
            switch(state){
                case 0:
                    //Esperando mesa libre
                    Mesa aux = worldController.mesaLibre();
                    if (aux != null)
                    {
                        mesa = aux;
                        destination = mesa.transform.position;
                        destination = new Vector3(destination.x, destination.y, destination.z + 3);
                        navMeshAgent.destination = destination;
                        state = 1;
                    }
                    break;
                case 1:
                    //Movimiento hacia mesa
                    if (comprobatePos())
                    {
                        state = 2;
                    }
                    break;
                case 2:
                    //Esperando camarero libre
                    CamareroController camarero = worldController.camareroLibre();
                    //bocadillo esperando
                    if (!objectSpawned)
                    {
                        bocAux = Instantiate(bocadilloEsperando, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        objectSpawned = true;
                    }

                    if (camarero != null)
                    {
                        camarero.destination = new Vector3(destination.x, destination.y, destination.z - 7);
                        bebida.posicionCliente = new Vector3(destination.x, destination.y, destination.z - 7);
                        bebida.cliente = this;
                        camarero.bebida = bebida;
                        camarero.state = 1;
                        if (objectSpawned) {
                            Destroy(bocAux.transform.GetChild(0).gameObject);
                            Destroy(bocAux.gameObject);
                            objectSpawned = false;
                        }
                        state = 3;
                    }
                    break;
                case 3:
                    //Esperando
                    break;
                case 4:
                    //Animacion beber (si vuelve a pedir state = 2)
                    //bocadillo beber
                    if (!objectSpawned)
                    {
                        bocAux = Instantiate(bocadilloBebiendo, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        objectSpawned = true;
                    }
                    if (!oneTime)
                    {
                        oneTime = true;
                        StartCoroutine(WaitSeconds());
                    }
                    break;
                case 5:
                    oneTime = false;
                    navMeshAgent.destination = initPos; // solo si se va del bar
                    if (mesaLibreFirstTime)
                    {
                        mesaLibreFirstTime = false;
                        mesa.ocupado = false;
                    }
                    state = 6;
                    break;
                case 6:
                    //Se va del bar
                    if (comprobatePos())
                    {
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }
    private bool comprobatePos()
    {
        return (navMeshAgent.remainingDistance == 0);
    }
    public bool recibirPropina()
    {
        int aux = Random.Range(0, 2);
        //borro el bocadillo que haya estado pintado antes
        BorrarBocadillos();
        if (aux < 1)
        {
            //bocadillo rechazar propina
            if (!objectSpawned)
            {
                bocAux = Instantiate(bocadilloRechazarPropina, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                objectSpawned = true;
            }
            StartCoroutine(WaitSeconds2());
            return false;
        }
        else
        {
            //bocadillo dar propina
            if (!objectSpawned)
            {
                bocAux = Instantiate(bocadilloAceptarPropina, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                objectSpawned = true;
            }
            StartCoroutine(WaitSeconds2());
            return true;
        }
    }
    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(5);
        BorrarBocadillos();
        state = 5;
    }
    IEnumerator WaitSeconds2()
    {
        yield return new WaitForSeconds(3);
        BorrarBocadillos();
    }
    public void BorrarBocadillos()
    {
        if (objectSpawned && bocAux != null)
        {
            Destroy(bocAux.transform.GetChild(0).gameObject);
            Destroy(bocAux.gameObject);
            objectSpawned = false;
        }
    }
}
