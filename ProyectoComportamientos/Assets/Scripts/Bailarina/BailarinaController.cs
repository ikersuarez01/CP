using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BailarinaController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public ClienteController cliente;
    public WorldController worldController;

    bool trabajandoConCliente;


    void Update()
    {
        if (!musicaSonando())
        {
            if (clienteLibre())
            {

            }
        }
        
    }
    private bool musicaSonando()
    {
        return worldController.musicaSonando;
    }
    private bool clienteLibre()
    {
        bool clienteDisponible = false;
        if (trabajandoConCliente)
        {
            clienteDisponible = trabajandoConCliente;
            return clienteDisponible;
        }
        return clienteDisponible;
    }
}
