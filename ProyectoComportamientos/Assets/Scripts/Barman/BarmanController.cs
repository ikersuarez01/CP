using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BarmanController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public int state = 0;
    [SerializeField] public WorldController worldController;
    [SerializeField] public GameObject bocadillo1;
    GameObject bocAux = null;
    private bool objectSpawned = false;

    public Vector3 initRot;
    public Vector3 destinationRot;

    public Bebida bebida;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        initRot = transform.position;
        destinationRot = new Vector3(initRot.x, initRot.y + 180, initRot.z);
        bebida = GetComponent<Bebida>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                //Esperando a que haya bebidas para preparar
                Bebida aux = worldController.bebidaParaPreparar();
                if (aux != null)
                {
                    bebida = aux;
                    bebida.barman = this;
                    state = 1;
                }
                break;
            case 1:
                //Giro hacia preparar bebida
                state = 2;
                break;
            case 2:
                //Preparando bebida
                if (prepararBebida())
                {
                    //giro hacia el bar
                    timer = 0;
                    state = 3;

                }
                break;
            case 3:
                //bocadillo bebida preparada
                FindObjectOfType<AudioManager>().Play("Campanita");
                if (!objectSpawned)
                {
                    bocAux = Instantiate(bocadillo1, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                    objectSpawned = true;
                }
                state = 4;
                break;
                                
            case 4:
                //Espera camarero libre
                CamareroController camarero = worldController.camareroLibre();
                if (camarero != null)
                {
                    camarero.state = 7;
                    camarero.navMeshAgent.destination = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+7);
                    camarero.bebida = bebida;
                    state = 5;
                }
                break;
            case 5:
                //Espera a que el camarero llegue
                break;
            case 6:
                //El camarero llega
                if (objectSpawned && bocAux != null)
                {
                    Destroy(bocAux.transform.GetChild(0).gameObject);
                    Destroy(bocAux.gameObject);
                    objectSpawned = false;
                    state = 0;
                }
                //Espera hasta que el camarero llegue para entregarle la bebida
                BorrarBocadillos();
                break;
        }
    }

    private float waitTime = 3f;
    private float timer = 0;
    private bool prepararBebida()
    {
        timer += Time.deltaTime;
        if (timer > waitTime)
        {
            return true;
        }
        else
        {
            return false;
        }
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
