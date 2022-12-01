using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClienteController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    [SerializeField] public WorldController worldController;

    public int state;
    public Vector3 destination;
    public Vector3 initPos;
    public Mesa mesa;
    public Bebida bebida;
    private void Start()
    {
        state = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        initPos = transform.position;
        bebida = GetComponent<Bebida>();
        

    }
    private void Update()
    {
        switch (state)
        {
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
                if(camarero != null)
                {
                    camarero.destination = new Vector3(destination.x, destination.y, destination.z - 7);
                    bebida.posicionCliente = new Vector3(destination.x, destination.y, destination.z - 7);
                    bebida.cliente = this;
                    camarero.bebida = bebida;
                    camarero.state = 1;
                    state = 3;
                }
                break;
            case 3:
                //Esperando
                break;
            case 4:
                //Animacion beber (si vuelve a pedir state = 2)
                state = 5;
                mesa.ocupado = false;
                navMeshAgent.destination = initPos; // solo si se va del bar
                break;
            case 5:
                //Se va del bar
                if (comprobatePos())
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    private bool comprobatePos()
    {
        return (navMeshAgent.remainingDistance == 0);
    }
}
