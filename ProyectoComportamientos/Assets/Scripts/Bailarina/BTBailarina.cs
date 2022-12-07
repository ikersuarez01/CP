using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBailarina : MonoBehaviour {

    #region variables

    private BehaviourTreeEngine BTBailarina_BT;
    

    private SelectorNode MusicaSonandoCond;
    private SequenceNode Secuencia1;
    private LeafNode Bailar;
    private LeafNode PeticionCliente;
    private LeafNode AvanzarHastaCliente;
    private LeafNode PararEnCliente;
    private LeafNode PedirPropina;
    private SelectorNode RespuestaClienteCond;
    private SequenceNode Secuencia2;
    private LeafNode PropinaCliente;
    private LeafNode EstarContenta;
    private LeafNode Enfadarse;
    private LeafNode MusicaSonando;
    private InverterDecoratorNode Inverter_MusicaSonando;
    private LoopDecoratorNode Loop;

    //Place your variables here

    #endregion variables

    public NavMeshAgent navMeshAgent;
    public ClienteController cliente;
    public WorldController worldController;

    bool trabajandoConCliente;
    // Start is called before the first frame update
    private void Start()
    {
        BTBailarina_BT = new BehaviourTreeEngine(false);
        navMeshAgent = GetComponent<NavMeshAgent>();

        CreateBehaviourTree();
    }
    
    
    private void CreateBehaviourTree()
    {
        // Nodes
        MusicaSonandoCond = BTBailarina_BT.CreateSelectorNode("MusicaSonandoCond");
        Secuencia1 = BTBailarina_BT.CreateSequenceNode("Secuencia1", false);
        Bailar = BTBailarina_BT.CreateLeafNode("Bailar", BailarAction, BailarSuccessCheck);
        PeticionCliente = BTBailarina_BT.CreateLeafNode("PeticionCliente", PeticionClienteAction, PeticionClienteSuccessCheck);
        AvanzarHastaCliente = BTBailarina_BT.CreateLeafNode("AvanzarHastaCliente", AvanzarHastaClienteAction, AvanzarHastaClienteSuccessCheck);
        PararEnCliente = BTBailarina_BT.CreateLeafNode("PararEnCliente", PararEnClienteAction, PararEnClienteSuccessCheck);
        PedirPropina = BTBailarina_BT.CreateLeafNode("PedirPropina", PedirPropinaAction, PedirPropinaSuccessCheck);
        RespuestaClienteCond = BTBailarina_BT.CreateSelectorNode("RespuestaClienteCond");
        Secuencia2 = BTBailarina_BT.CreateSequenceNode("Secuencia2", false);
        PropinaCliente = BTBailarina_BT.CreateLeafNode("PropinaCliente", PropinaClienteAction, PropinaClienteSuccessCheck);
        EstarContenta = BTBailarina_BT.CreateLeafNode("EstarContenta", EstarContentaAction, EstarContentaSuccessCheck);
        Enfadarse = BTBailarina_BT.CreateLeafNode("Enfadarse", EnfadarseAction, EnfadarseSuccessCheck);
        MusicaSonando = BTBailarina_BT.CreateLeafNode("MusicaSonando", MusicaSonandoAction, MusicaSonandoSuccessCheck);
        Inverter_MusicaSonando = BTBailarina_BT.CreateInverterNode("Inverter_MusicaSonando", MusicaSonando);
        Loop = BTBailarina_BT.CreateLoopNode("Loop", MusicaSonandoCond);

        // Child adding
        MusicaSonandoCond.AddChild(Secuencia1);
        MusicaSonandoCond.AddChild(Bailar);

        Secuencia1.AddChild(Inverter_MusicaSonando);
        Secuencia1.AddChild(PeticionCliente);
        Secuencia1.AddChild(AvanzarHastaCliente);
        Secuencia1.AddChild(PararEnCliente);
        Secuencia1.AddChild(PedirPropina);
        Secuencia1.AddChild(RespuestaClienteCond);
        
        RespuestaClienteCond.AddChild(Secuencia2);
        RespuestaClienteCond.AddChild(Enfadarse);
        
        Secuencia2.AddChild(PropinaCliente);
        Secuencia2.AddChild(EstarContenta);

        // SetRoot
        BTBailarina_BT.SetRootNode(Loop);
        // ExitPerceptions

        // ExitTransitions

    }

    // Update is called once per frame
    private void Update()
    {
        BTBailarina_BT.Update();
    }

    // Create your desired actions
    
    private void BailarAction()
    {
        //animacionBailar
        print("Bailando");
    }
    
    private ReturnValues BailarSuccessCheck()
    {
        //Write here the code for the success check for Bailar
        print("acaba de bailar");
        
        return ReturnValues.Succeed;
    }
    
    private void PeticionClienteAction()
    {
        //Animacion espera
        print("buscando cliente");
    }
    
    private ReturnValues PeticionClienteSuccessCheck()
    {
        //Write here the code for the success check for PeticionCliente
        ClienteController aux = worldController.clienteLibre();
        if (aux == null)
        {
            print("no hay clientes");
            return ReturnValues.Failed;
        }
        else
        {
            print("cliente recibido");
            cliente = aux;
            return ReturnValues.Succeed;
        }
    }
    
    private void AvanzarHastaClienteAction()
    {
        print("empiezo a caminar");
        Vector3 pos = new Vector3(cliente.gameObject.transform.position.x + 2, cliente.gameObject.transform.position.y + cliente.gameObject.transform.position.z);
        navMeshAgent.destination = pos;
    }
    
    private ReturnValues AvanzarHastaClienteSuccessCheck()
    {
        if (navMeshAgent.remainingDistance > 0)
        {
            print("todavia no he llegado al cliente");
            return ReturnValues.Running;
        }
        if(navMeshAgent.remainingDistance == 0)
        {
            print("he llegado al cliente");
            return ReturnValues.Succeed;
        }
        //Write here the code for the success check for AvanzarHastaCliente
        return ReturnValues.Failed;
    }
    
    private void PararEnClienteAction()
    {
        //animacion pedir
    }
    
    private ReturnValues PararEnClienteSuccessCheck()
    {
        //Write here the code for the success check for PararEnCliente
        return ReturnValues.Failed;
    }
    
    private void PedirPropinaAction()
    {
        
    }
    
    private ReturnValues PedirPropinaSuccessCheck()
    {
        //Write here the code for the success check for PedirPropina
        return ReturnValues.Failed;
    }
    
    private void PropinaClienteAction()
    {
        
    }
    
    private ReturnValues PropinaClienteSuccessCheck()
    {
        //Write here the code for the success check for PropinaCliente
        return ReturnValues.Failed;
    }
    
    private void EstarContentaAction()
    {
        
    }
    
    private ReturnValues EstarContentaSuccessCheck()
    {
        //Write here the code for the success check for EstarContenta
        return ReturnValues.Failed;
    }
    
    private void EnfadarseAction()
    {
        
    }
    
    private ReturnValues EnfadarseSuccessCheck()
    {
        //Write here the code for the success check for Enfadarse
        return ReturnValues.Failed;
    }
    
    private void MusicaSonandoAction()
    {
        
    }
    
    private ReturnValues MusicaSonandoSuccessCheck()
    {
        if (worldController.musicaSonando)
        {
            return ReturnValues.Succeed;
        }
        else{
            return ReturnValues.Failed;
        }
        //Write here the code for the success check for MusicaSonando
        return ReturnValues.Failed;
    }
    
}