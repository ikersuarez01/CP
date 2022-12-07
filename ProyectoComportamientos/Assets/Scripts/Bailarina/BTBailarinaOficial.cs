using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBailarinaOficial : MonoBehaviour {

    #region variables

    private BehaviourTreeEngine BTBailarina_BT;
    

    private SelectorNode MusicaSonandoCond;
    private SequenceNode Secuencia1;
    private LeafNode MusicaSonando;
    private LeafNode PeticionCliente;
    private LeafNode AvanzarHastaCliente;
    private LeafNode PararEnCliente;
    private LeafNode PedirPropina;
    private SelectorNode RespuestaClienteCond;
    private SequenceNode Secuencia2;
    private LeafNode PropinaCliente;
    private LeafNode EstarContenta;
    private LeafNode Enfadarse;
    private LeafNode Bailar;
    private InverterDecoratorNode Inverter_MusicaSonando;
    private LoopDecoratorNode LoopN_MusicaSonandoCond;
    
    //Place your variables here

    #endregion variables

    // Start is called before the first frame update
    private void Start()
    {
        BTBailarina_BT = new BehaviourTreeEngine(false);
        

        CreateBehaviourTree();
    }
    
    
    private void CreateBehaviourTree()
    {
        // Nodes
        MusicaSonandoCond = BTBailarina_BT.CreateSelectorNode("MusicaSonandoCond");
        Secuencia1 = BTBailarina_BT.CreateSequenceNode("Secuencia1", false);
        MusicaSonando = BTBailarina_BT.CreateLeafNode("MusicaSonando", MusicaSonandoAction, MusicaSonandoSuccessCheck);
        PeticionCliente = BTBailarina_BT.CreateLeafNode("PeticionCliente", PeticionClienteAction, PeticionClienteSuccessCheck);
        AvanzarHastaCliente = BTBailarina_BT.CreateLeafNode("AvanzarHastaCliente", AvanzarHastaClienteAction, AvanzarHastaClienteSuccessCheck);
        PararEnCliente = BTBailarina_BT.CreateLeafNode("PararEnCliente", PararEnClienteAction, PararEnClienteSuccessCheck);
        PedirPropina = BTBailarina_BT.CreateLeafNode("PedirPropina", PedirPropinaAction, PedirPropinaSuccessCheck);
        RespuestaClienteCond = BTBailarina_BT.CreateSelectorNode("RespuestaClienteCond");
        Secuencia2 = BTBailarina_BT.CreateSequenceNode("Secuencia2", false);
        PropinaCliente = BTBailarina_BT.CreateLeafNode("PropinaCliente", PropinaClienteAction, PropinaClienteSuccessCheck);
        EstarContenta = BTBailarina_BT.CreateLeafNode("EstarContenta", EstarContentaAction, EstarContentaSuccessCheck);
        Enfadarse = BTBailarina_BT.CreateLeafNode("Enfadarse", EnfadarseAction, EnfadarseSuccessCheck);
        Bailar = BTBailarina_BT.CreateLeafNode("Bailar", BailarAction, BailarSuccessCheck);
        Inverter_MusicaSonando = BTBailarina_BT.CreateInverterNode("Inverter_MusicaSonando", MusicaSonando);
        LoopN_MusicaSonandoCond = BTBailarina_BT.CreateLoopNode("LoopN_MusicaSonandoCond", MusicaSonandoCond);
        
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
        BTBailarina_BT.SetRootNode(LoopN_MusicaSonandoCond);
        
        // ExitPerceptions
        
        // ExitTransitions
        
    }

    // Update is called once per frame
    private void Update()
    {
        BTBailarina_BT.Update();

    }

    // Create your desired actions
    
    private void MusicaSonandoAction()
    {
    }
    
    private ReturnValues MusicaSonandoSuccessCheck()
    {
        //Write here the code for the success check for MusicaSonando
        return ReturnValues.Failed;
    }
    
    private void PeticionClienteAction()
    {
        
    }
    
    private ReturnValues PeticionClienteSuccessCheck()
    {
        //Write here the code for the success check for PeticionCliente
        return ReturnValues.Failed;
    }
    
    private void AvanzarHastaClienteAction()
    {
        
    }
    
    private ReturnValues AvanzarHastaClienteSuccessCheck()
    {
        //Write here the code for the success check for AvanzarHastaCliente
        return ReturnValues.Failed;
    }
    
    private void PararEnClienteAction()
    {
        
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
    
    private void BailarAction()
    {
        
    }
    
    private ReturnValues BailarSuccessCheck()
    {
        //Write here the code for the success check for Bailar
        return ReturnValues.Failed;
    }
    
}