using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BarmanController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public int state;
    [SerializeField] public WorldController worldController;

    public Vector3 initRot;
    public Vector3 destinationRot;

    public Bebida bebida;
    void Start()
    {
        state = 0;
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
                //Espera camarero para entregar bebida
                CamareroController camarero = worldController.camareroLibre();
                if (camarero != null)
                {
                    camarero.state = 7;
                    camarero.navMeshAgent.destination = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+8);
                    camarero.bebida = bebida;
                    state = 4;
                }
                break;
            case 4:
                //Espera hasta que el camarero llegue para entregarle la bebida
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
}
