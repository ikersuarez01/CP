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
    [SerializeField] public GameObject[] modeloBasos;
    GameObject bocAux = null;
    private bool objectSpawned = false;

    public Vector3 initRot;
    public Vector3 destinationRot;

    public Bebida bebida;
    public GameObject modeloBebida;

    public Animator anim;
    void Start()
    {
        modeloBasos[0].SetActive(false);
        modeloBasos[1].SetActive(false);
        modeloBasos[2].SetActive(false);
        modeloBasos[3].SetActive(false);
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        initRot = transform.position;
        destinationRot = new Vector3(initRot.x, initRot.y + 180, initRot.z);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                anim.Play("Idle");
                //Esperando a que haya bebidas para preparar
                Bebida aux = worldController.bebidaParaPreparar();
                if (aux != null)
                {
                    bebida = aux;
                    bebida.barman = this;
                    modeloBasos[0].SetActive(false);
                    modeloBasos[1].SetActive(false);
                    modeloBasos[2].SetActive(false);
                    modeloBasos[3].SetActive(false);
                    modeloBasos[bebida.tipo].SetActive(true);
                    state = 1;
                }
                break;
            case 1:
                //Giro hacia preparar bebida
                transform.Rotate(new Vector3(0, 180, 0), Space.Self);
                anim.Play("PrepararBebida");
                timer = 0f;
                oneTimeAnim = false;
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
                modeloBebida = Instantiate(bebida.modeloBebidas[bebida.tipo], new Vector3(this.transform.position.x, this.transform.position.y+0.88f, this.transform.position.z-1.5f), Quaternion.identity);
                state = 4;
                break;
                                
            case 4:
                //Espera camarero libre
                CamareroController camarero = worldController.camareroLibre();
                if (camarero != null)
                {
                    camarero.state = 7;
                    camarero.navMeshAgent.destination = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-2.5f);
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
                }
                for (var i = modeloBebida.transform.childCount - 1; i >= 0; i--)
                {
                    Object.Destroy(modeloBebida.transform.GetChild(i).gameObject);
                }
                Destroy(modeloBebida);
                state = 0;
                //Espera hasta que el camarero llegue para entregarle la bebida
                BorrarBocadillos();
                break;
        }
    }

    private float waitTime = 6.8f;
    private float timer = 0;
    private bool oneTimeAnim = false;
    private bool prepararBebida()
    {
        timer += Time.deltaTime;
        if (timer > 4.2f){
            if (!oneTimeAnim)
            {
                oneTimeAnim = true;
                transform.Rotate(new Vector3(0, 180, 0), Space.World);
                anim.Play("ServirBebida");
            }
        }
        
        
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
