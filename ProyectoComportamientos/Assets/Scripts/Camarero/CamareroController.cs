using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CamareroController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    [SerializeField] public WorldController worldController;
    public Bebida bebida;
    public int index;

    [SerializeField] public GameObject bocadilloPregunta;

    private bool objectSpawned = false;
    GameObject bocAux = null;

    [SerializeField] public Vector3 barPos;

    public int state = 0;
    public Vector3 initPos;
    public Vector3 destination;

    public bool oneTime = false;

    [SerializeField] public GameObject bocadilloBebidaAmarillo;
    [SerializeField] public GameObject bocadilloBebidaAzul;
    [SerializeField] public GameObject bocadilloBebidaRojo;
    [SerializeField] public GameObject bocadilloBebidaVerde;


    [SerializeField] public GameObject[] modeloBasos;
    public Animator anim;
    private void Start()
    {
        modeloBasos[0].SetActive(false);
        modeloBasos[1].SetActive(false);
        modeloBasos[2].SetActive(false);
        modeloBasos[3].SetActive(false);
        anim = GetComponentInChildren<Animator>();
        initPos = this.transform.position;
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                anim.Play("Idle");
                //Esperando
                break;
            case 1:
                //Avanzar
                oneTime = false;
                navMeshAgent.destination = destination;
                state = 2;
                break;
            case 2:
                //Movimiento hacia cliente 
                //navMeshAgent.destination = destination;
                anim.Play("WalkSinBebida");
                if (comprobatePos())
                {
                    if (!oneTime)
                    {
                        if (!objectSpawned)
                        {
                            bocAux = Instantiate(bocadilloPregunta, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                            objectSpawned = true;
                        }
                        oneTime = true;
                        StartCoroutine(WaitSeconds());
                        bebida.cliente.BocadillosBebida();
                    }
                }
                break;
            case 3:
                //Cogiendo comanda al cliente
                anim.Play("HablaSinBebida");
                state = 4;
                oneTime = false;
                destination = barPos; //destino del bar
                navMeshAgent.destination = destination;
                break;
            case 4:
                anim.Play("WalkSinBebida");
                //Movimiento hacia bar
                if (comprobatePos())
                {
                    state = 5;
                }
                break;
            case 5:
                // Pidiendo la comanda al barman
                anim.Play("HablaSinBebida");
                if (!oneTime)
                {
                    oneTime = true;
                    worldController.addBebidaParaPreparar(bebida);
                    StartCoroutine(AskBebida());
                }
                break;
            case 6:
                anim.Play("WalkSinBebida");
                //Movimiento hacia initPos
                navMeshAgent.destination = initPos;
                oneTime = false;
                if (comprobatePos())
                {
                    state = 0;
                }
                break;
            case 7:
                anim.Play("WalkSinBebida");
                //Movimiento hacia bar
                if (comprobatePos())
                {
                    state = 8;
                }
                break;
            case 8:
                anim.Play("CogerBebida");
                //Cogiendo comanda al barman
                bebida.barman.state = 6;
                state = 9;
                navMeshAgent.destination = bebida.posicionCliente;
                break;
            case 9:
                anim.Play("WalkBebida");
                //Movimiento hacia cliente
                if (comprobatePos())
                {
                    state = 10;
                }
                break;
            case 10:
                anim.Play("DejarBebida");
                //Entregando la comanda al cliente
                bebida.cliente.state = 4;
                state = 6;
                break;
            default:
                break;
        }


    }
    private bool comprobatePos()
    {
        return (navMeshAgent.remainingDistance == 0);
    }
    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(2);
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
    IEnumerator AskBebida()
    {
        switch (bebida.tipo)
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
        yield return new WaitForSeconds(0.5f);
        BorrarBocadillos();
        state = 6;
    }
}

