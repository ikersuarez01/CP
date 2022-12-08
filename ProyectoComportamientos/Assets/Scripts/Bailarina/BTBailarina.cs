using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBailarina : MonoBehaviour {

    #region variables
    private BehaviourTreeEngine behaviourTree;

    #endregion variables

    [SerializeField] WorldController worldController;
    ClienteController cliente;
    NavMeshAgent navMeshAgent;
    private bool recibePropina;
    private Vector3 initPos;

    private bool objectSpawned = false;
    GameObject bocAux = null;
    [SerializeField] public GameObject bocadilloPedirPropina;
    [SerializeField] public GameObject bocadilloEnfado;
    [SerializeField] public GameObject bocadilloContento;
    private bool successPropina = false;
    private bool showEmotions = false;
    private bool esperaUnPoco = false;



    // Start is called before the first frame update
    private void Start()
    {
        initPos = transform.position;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        CreateBehaviourTree();
    }

    public void CreateBehaviourTree()
    {
        behaviourTree = new BehaviourTreeEngine(false);

        SelectorNode MusicaSonandoCond = behaviourTree.CreateSelectorNode("MusicaSonandoCond");

        SequenceNode Secuencia1 = behaviourTree.CreateSequenceNode("Secuencia1", false);
        LeafNode MusicaSonando = behaviourTree.CreateLeafNode("MusicaSonando", MusicaSonandoAction, MusicaSonandoSuccessCheck);
        LeafNode PeticionCliente = behaviourTree.CreateLeafNode("PeticionCliente", PeticionClienteAction, PeticionClienteSuccessCheck);
        LeafNode AvanzarHastaCliente = behaviourTree.CreateLeafNode("AvanzarHastaCliente", AvanzarHastaClienteAction, AvanzarHastaClienteSuccessCheck);
        LeafNode PararEnCliente = behaviourTree.CreateLeafNode("PararEnCliente", PararEnClienteAction, PararEnClienteSuccessCheck);
        LeafNode PedirPropina = behaviourTree.CreateLeafNode("PedirPropina", PedirPropinaAction, PedirPropinaSuccessCheck);
        SelectorNode RespuestaClienteCond = behaviourTree.CreateSelectorNode("RespuestaClienteCond");

        SequenceNode Secuencia2 = behaviourTree.CreateSequenceNode("Secuencia2", false);
        LeafNode PropinaCliente = behaviourTree.CreateLeafNode("PropinaCliente", PropinaClienteAction, PropinaClienteSuccessCheck);
        LeafNode EstarContenta = behaviourTree.CreateLeafNode("EstarContenta", EstarContentaAction, EstarContentaSuccessCheck);
        LeafNode Enfadarse = behaviourTree.CreateLeafNode("Enfadarse", EnfadarseAction, EnfadarseSuccessCheck);

        LeafNode GoInitPos = behaviourTree.CreateLeafNode("GoInitPos", GoInitPosAction, GoInitPosSuccessCheck);
        LeafNode Bailar = behaviourTree.CreateLeafNode("Bailar", BailarAction, BailarSuccessCheck);
        InverterDecoratorNode Inverter_MusicaSonando = behaviourTree.CreateInverterNode("Inverter_MusicaSonando", MusicaSonando);

        LoopDecoratorNode rootNode = behaviourTree.CreateLoopNode("Root node", MusicaSonandoCond);

        MusicaSonandoCond.AddChild(Secuencia1);
        MusicaSonandoCond.AddChild(GoInitPos);
        MusicaSonandoCond.AddChild(Bailar);

        Secuencia1.AddChild(MusicaSonando);
        Secuencia1.AddChild(PeticionCliente);
        Secuencia1.AddChild(AvanzarHastaCliente);
        Secuencia1.AddChild(PararEnCliente);
        Secuencia1.AddChild(PedirPropina);
        Secuencia1.AddChild(RespuestaClienteCond);

        RespuestaClienteCond.AddChild(Secuencia2);
        RespuestaClienteCond.AddChild(Enfadarse);

        Secuencia2.AddChild(PropinaCliente);
        Secuencia2.AddChild(EstarContenta);

        behaviourTree.SetRootNode(rootNode);
    }

    // Update is called once per frame
    private void Update()
    {
        behaviourTree.Update();
    }

    private void MusicaSonandoAction()
    {
        //print("musica sonando check");
    }
    private ReturnValues MusicaSonandoSuccessCheck()
    {
        if (!worldController.musicaSonando)
        {
            return ReturnValues.Succeed;
        }
        else
        {
            return ReturnValues.Failed;
        }
        
    }

    private void PeticionClienteAction()
    {
        //Animacion espera
        //print("buscando cliente");
    }

    private ReturnValues PeticionClienteSuccessCheck()
    {
        //Write here the code for the success check for PeticionCliente
        ClienteController aux = worldController.clienteLibre();
        if (aux == null)
        {
            //print("no hay clientes");
            return ReturnValues.Failed;
        }
        else
        {
            //print("cliente recibido");
            cliente = aux;
            return ReturnValues.Succeed;
        }
    }

    private void AvanzarHastaClienteAction()
    {
        //print("empiezo a caminar");
        Vector3 pos = new Vector3(cliente.gameObject.transform.position.x, cliente.gameObject.transform.position.y, cliente.gameObject.transform.position.z+1);
        navMeshAgent.destination = pos;
    }
    private ReturnValues AvanzarHastaClienteSuccessCheck()
    {
        if (navMeshAgent.remainingDistance > 0)
        {
            //print("todavia no he llegado al cliente");
            return ReturnValues.Running;
        }
        if (navMeshAgent.remainingDistance == 0)
        {
            //print("he llegado al cliente");
            return ReturnValues.Succeed;
        }
        return ReturnValues.Succeed;
    }

    private void PararEnClienteAction()
    {
        //animacion pedir
        //print("parado en cliente");
        cliente.pausaBailarina = true;
    }

    private ReturnValues PararEnClienteSuccessCheck()
    {
        //comprobar finalizacion de animacion
        return ReturnValues.Succeed;
    }
    private void PedirPropinaAction()
    {
        //print("pide propina");
        //animacion pedir propina
        //Borrar bocadillos del cliente para que parezca que le está haciendo caso xd
        cliente.BorrarBocadillos();
        if (!objectSpawned)
        {
            bocAux = Instantiate(bocadilloPedirPropina, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
            objectSpawned = true;
        }
        StartCoroutine(WaitSeconds());

        //hacer que espere con el uso de una variable que se comprueba en el success

    }

    private ReturnValues PedirPropinaSuccessCheck()
    {
        //Write here the code for the success check for PedirPropina
        if (successPropina)
        {
            successPropina = false;
            return ReturnValues.Succeed;
        }
        else
            return ReturnValues.Running;
    }

    private void PropinaClienteAction()
    {
        //print("RecibePropina");
        recibePropina = cliente.recibirPropina();
        StartCoroutine(DoNothingForSeconds());

    }

    private ReturnValues PropinaClienteSuccessCheck()
    {
        //Write here the code for the success check for PropinaCliente
        if (recibePropina)
        {
            if (esperaUnPoco)
            {
                esperaUnPoco = false;
                return ReturnValues.Succeed;
            }else
                return ReturnValues.Running;
        }
        else
        {
            if (esperaUnPoco)
            {
                esperaUnPoco = false;
                return ReturnValues.Failed;
            }
            else
                return ReturnValues.Running;
        }
    }

    private void EstarContentaAction()
    {
        //print("Contenta");
        //bocadillo contenta
        if (!objectSpawned)
        {
            bocAux = Instantiate(bocadilloContento, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
            objectSpawned = true;
        }
        StartCoroutine(ShowEmotionsAfterSeconds());
    }

    private ReturnValues EstarContentaSuccessCheck()
    {
        //Write here the code for the success check for EstarContenta
        //comprobar animacion finalizar contenta
        if (showEmotions)
        {
            showEmotions = false;
            cliente.pausaBailarina = false;
            return ReturnValues.Succeed;
        }
        else
            return ReturnValues.Running;
    }

    private void EnfadarseAction()
    {
        //print("Enfado");
        if (!objectSpawned)
        {
            bocAux = Instantiate(bocadilloEnfado, new Vector3(this.transform.position.x, this.transform.position.y + 5, this.transform.position.z), Quaternion.identity);
            objectSpawned = true;
        }
        StartCoroutine(ShowEmotionsAfterSeconds());
    }

    private ReturnValues EnfadarseSuccessCheck()
    {
        //Write here the code for the success check for Enfadarse
        //Comprobar animacion finalizar enfadar
        if (showEmotions)
        {
            showEmotions = false;
            cliente.pausaBailarina = false;
            return ReturnValues.Succeed;
        }
        else
            return ReturnValues.Running;
    }
    private void GoInitPosAction()
    {
        navMeshAgent.destination = initPos;
    }
    private ReturnValues GoInitPosSuccessCheck()
    {
        if (worldController.musicaSonando)
        {
            return ReturnValues.Failed;
        }
        if (navMeshAgent.remainingDistance > 0)
        {
            return ReturnValues.Running;
        }
        if (navMeshAgent.remainingDistance == 0)
        {
            return ReturnValues.Succeed;
        }
        return ReturnValues.Succeed;
    }
    private void BailarAction()
    {
        //print("Bailando");
    }

    private ReturnValues BailarSuccessCheck()
    {
        //Write here the code for the success check for Bailar
        return ReturnValues.Succeed;
    }
    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(3);
        if (objectSpawned && bocAux != null)
        {
            Destroy(bocAux.transform.GetChild(0).gameObject);
            Destroy(bocAux.gameObject);
            objectSpawned = false;
        }
        successPropina = true;
    }
    IEnumerator ShowEmotionsAfterSeconds()
    {
        yield return new WaitForSeconds(2);
        if (objectSpawned && bocAux != null)
        {
            Destroy(bocAux.transform.GetChild(0).gameObject);
            Destroy(bocAux.gameObject);
            objectSpawned = false;
        }
        showEmotions = true;
    }
    IEnumerator DoNothingForSeconds()
    {
        yield return new WaitForSeconds(2);
        esperaUnPoco = true;
    }
    public Vector3 GetInitPos()
    {
        return this.initPos;
    }
}