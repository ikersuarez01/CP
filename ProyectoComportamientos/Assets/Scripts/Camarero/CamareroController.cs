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

    public int state;
    public Vector3 initPos;
    public Vector3 destination;

    
    private void Start()
    {
        state = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        initPos = this.transform.position;
        bebida = GetComponent<Bebida>();
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                //Esperando
                break;
            case 1:
                //Avanzar
                navMeshAgent.destination = destination;
                state = 2;
                break;
            case 2:
                //Movimiento hacia cliente 
                //navMeshAgent.destination = destination;
                if (comprobatePos())
                {
                    if (!objectSpawned)
                    {
                        bocAux = Instantiate(bocadilloPregunta, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
                        objectSpawned = true;
                    }

                    StartCoroutine(WaitSeconds());
                }
                break;
            case 3:
                //Cogiendo comanda al cliente
                state = 4;
                destination = barPos; //destino del bar
                navMeshAgent.destination = destination;
                break;
            case 4:
                //Movimiento hacia bar
                if (comprobatePos())
                {
                    state = 5;
                }
                break;
            case 5:
                // Pidiendo la comanda al barman
                worldController.addBebidaParaPreparar(bebida);
                state = 6;
                break;
            case 6:
                //Movimiento hacia initPos
                navMeshAgent.destination = initPos;
                if (comprobatePos())
                {
                    state = 0;
                }
                break;
            case 7:
                //Movimiento hacia bar
                if (comprobatePos())
                {
                    state = 8;
                }
                break;
            case 8:
                //Cogiendo comanda al barman
                bebida.barman.state = 0;
                state = 9;
                navMeshAgent.destination = bebida.posicionCliente;
                break;
            case 9:
                //Movimiento hacia cliente
                if (comprobatePos())
                {
                    state = 10;
                }
                break;
            case 10:
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
        yield return new WaitForSeconds(3);
        BorrarBocadillos();
        state = 3;
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

