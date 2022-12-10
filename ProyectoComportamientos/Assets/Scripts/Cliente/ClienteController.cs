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
    [SerializeField] public GameObject bocadilloBebidaAmarillo;
    [SerializeField] public GameObject bocadilloBebidaAzul;
    [SerializeField] public GameObject bocadilloBebidaRojo;
    [SerializeField] public GameObject bocadilloBebidaVerde;
    [SerializeField] public GameObject bocadilloEnfado;

    CamareroController camarero;


    public bool borracho = false;
    public bool exigente = false;

    public int state = 0;
    public Vector3 destination;
    public Vector3 initPos;
    public Vector3 puertaPos;
    public Mesa mesa;
    public Bebida bebida;
    public int tipoBebida;// 0 = amarillo / 1 = azul / 2 = rojo / 3 = Verde
    public bool pausaBailarina = false;
    private bool objectSpawned = false;
    GameObject bocAux = null;
    GameObject modeloBebida;

    private bool mesaLibreFirstTime = true;

    private bool oneTime = false;
    private float timePuerta;
    private float timeMesa;
    private float t = 0f;
    private void Start()
    {
        t = 0f;
        timePuerta = 8f;
        timeMesa = 16f;
        if (exigente)
        {
            timePuerta = timePuerta / 2f;
            timeMesa = timeMesa / 2f;
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
        initPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
                    t += Time.deltaTime / timePuerta;
                    if (t > 1)
                    {
                        t = 0;
                        bocAux = Instantiate(bocadilloEnfado, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        bocAux.GetComponent<BocadilloCamara>().client = this;
                        bocAux.GetComponent<BocadilloCamara>().movement = true;
                        worldController.numEPuerta++;
                        state = 5;
                        break;
                    }
                    Mesa aux = worldController.mesaLibre();
                    if (aux != null)
                    {
                        mesa = aux;
                        destination = mesa.transform.position;
                        destination = new Vector3(destination.x+1, destination.y, destination.z);
                        navMeshAgent.destination = destination;
                        t = 0;
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
                    if (!comprobatePos())
                    {
                        state = 1;
                        break;
                    }
                    oneTime = false;
                    t += Time.deltaTime / timeMesa;
                    if (t > 1)
                    {
                        //t = 0;
                        BorrarBocadillos();
                        bocAux = Instantiate(bocadilloEnfado, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        bocAux.GetComponent<BocadilloCamara>().client = this;
                        bocAux.GetComponent<BocadilloCamara>().movement = true;
                        navMeshAgent.destination = puertaPos;
                        worldController.numEMesa++;
                        state = 5;
                        break;
                    }
                    camarero = worldController.camareroLibre();
                    //bocadillo esperando
                    if (!objectSpawned)
                    {
                        bocAux = Instantiate(bocadilloEsperando, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        bocAux.GetComponent<BocadilloCamara>().client = this;
                        objectSpawned = true;
                    }

                    if (camarero != null)
                    {
                        BorrarBocadillos();
                        tipoBebida = Random.Range(0, 4);
                        bebida.tipo = tipoBebida;
                        camarero.destination = new Vector3(destination.x-1, destination.y, destination.z+1);
                        bebida.posicionCliente = new Vector3(destination.x-1, destination.y, destination.z+1);
                        bebida.cliente = this;
                        camarero.bebida = bebida;
                        camarero.state = 1;
                        state = 3;
                    }
                    break;
                case 3:
                    t = 0;
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
                        modeloBebida = Instantiate(bebida.modeloBebidas[tipoBebida], new Vector3(mesa.transform.position.x+0.3f, mesa.transform.position.y+1.5f, mesa.transform.position.z), Quaternion.identity);
                        StartCoroutine(WaitSeconds());
                    }
                    break;
                case 5:
                    oneTime = false;
                    navMeshAgent.destination = puertaPos; // solo si se va del bar
                    if (mesaLibreFirstTime)
                    {
                        mesaLibreFirstTime = false;
                        if(mesa != null)
                        {
                            mesa.ocupado = false;
                        }
                    }
                    state = 6;
                    break;
                case 6:
                    //Se va del bar
                    if (comprobatePos())
                    {
                        worldController.removeConcreteClient(this);
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
        if (exigente)
        {
            aux = 0;
        }
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
        for (var i = modeloBebida.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(modeloBebida.transform.GetChild(i).gameObject);
        }
        Destroy(modeloBebida);
        if (borracho)
        {
            int p = Random.Range(0, 100);
            if (p <= 33.3f)
            {
                worldController.numCContento++;
                state = 5;
            }
            else
            {
                worldController.numCContento++;
                state = 2;
            }
        }
        else
        {
            worldController.numCContento++;
            state = 5;
        }
    }
    IEnumerator WaitSeconds2()
    {
        yield return new WaitForSeconds(2f);
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
    public void BocadillosBebida()
    {
        StartCoroutine(ShowBebidaChosen());
    }
    IEnumerator ShowBebidaChosen()
    {
        yield return new WaitForSeconds(2); 
        switch (tipoBebida)
        {
            case 0:
                if (!objectSpawned)
                {
                    bocAux = Instantiate(bocadilloBebidaAmarillo, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                    objectSpawned = true;
                }
                break;
            case 1:
                if (!objectSpawned)
                {
                    bocAux = Instantiate(bocadilloBebidaAzul, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                    objectSpawned = true;
                }
                break;
            case 2:
                if (!objectSpawned)
                {
                    bocAux = Instantiate(bocadilloBebidaRojo, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                    objectSpawned = true;
                }
                break;
            case 3:
                if (!objectSpawned)
                {
                    bocAux = Instantiate(bocadilloBebidaVerde, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                    objectSpawned = true;
                }
                break;
        }
        camarero.BorrarBocadillos();
        yield return new WaitForSeconds(2);
        BorrarBocadillos();

        camarero.state = 3;
    }
    
}
