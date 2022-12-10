using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.AI;
public class WorldController : MonoBehaviour
{
    [SerializeField] public List<Mesa> listaMesas;
    [SerializeField] public List<CamareroController> listaCamareros;
    [SerializeField] public List<BarmanController> listaBarman;
    [SerializeField] public List<ClienteController> listaClientes;
    [SerializeField] public List<Bebida> listaBebidasEnEspera;

    [SerializeField] private GameObject prefabCamarero;
    [SerializeField] private GameObject prefabBarman;
    [SerializeField] private GameObject prefabCliente;

    [SerializeField] private GameObject parentCamarero;
    [SerializeField] private GameObject parentBarman;
    [SerializeField] private GameObject parentCliente;

    [SerializeField] private GameObject barPos;

    [SerializeField] private GameObject bailarina;

    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public TMP_Text numText;
    [SerializeField] public TMP_Text numEnfadosPuerta;
    [SerializeField] public TMP_Text numEnfadosMesa;
    [SerializeField] public TMP_Text numClientesContentos;

    public int numEMesa = 0;
    public int numEPuerta = 0;
    public int numCContento = 0;
    [SerializeField] public GameObject puertaPos;
    public bool musicaSonando;
    public int cAux = -1;
    public float time;
    public float t=0f;
    public void Start()
    {
        time = 0f;
        FindObjectOfType<AudioManager>().Play("Musica");
        musicaSonando = true;
    }

    private void Update()
    {
        numEnfadosPuerta.text = numEPuerta.ToString();
        numEnfadosMesa.text = numEMesa.ToString();
        numClientesContentos.text = numCContento.ToString();
        if (time != 0)
        {
            t += Time.deltaTime / time;

            if (t >= 1f)
            {
                t = 0;
                addCLient();
            }
        }
    }

    public void addCLient()
    {
        float aux = UnityEngine.Random.Range(0f, 100f);
        if (aux < 60)
        {
            //cliente normal
        }
        else if (aux < 80)
        {
            //cliente exigente
        }else
        {
            //cliente borracho
        }

        GameObject obj = Instantiate(prefabCliente, parentCliente.transform.position, parentCliente.transform.rotation);
        obj.transform.SetParent(parentCliente.transform);
        obj.GetComponent<ClienteController>().puertaPos = new Vector3(puertaPos.transform.position.x+0.2f*UnityEngine.Random.Range(0f,40f), puertaPos.transform.position.y, + puertaPos.transform.position.z+0.2f* UnityEngine.Random.Range(0f, 40f));
        obj.GetComponent<ClienteController>().worldController = this;
        listaClientes.Add(obj.GetComponent<ClienteController>());
    }
    public void removeConcreteClient(ClienteController aux)
    {
        GameObject obj = aux.gameObject;
        listaClientes.Remove(aux);
        aux.BorrarBocadillos();
        Destroy(obj);
    }
    public void removeCLient()
    {
        if(listaClientes.Count > 0)
        {
            GameObject obj = listaClientes[listaClientes.Count - 1].gameObject;
            obj.GetComponent<ClienteController>().BorrarBocadillos();
            listaClientes.RemoveAt(listaClientes.Count - 1);
            Destroy(obj);
        }
    }

    public void addCamarero()
    {
        if (listaCamareros.Count >= 8)
        {
            return;
        }
        GameObject obj = Instantiate(prefabCamarero, new Vector3(parentCamarero.transform.position.x + 1 * listaCamareros.Count, parentCamarero.transform.position.y, parentCamarero.transform.position.z), parentCamarero.transform.rotation);
        obj.transform.SetParent(parentCamarero.transform);
        obj.GetComponent<CamareroController>().navMeshAgent = obj.GetComponent<NavMeshAgent>();
        obj.GetComponent<CamareroController>().worldController = this;
        obj.GetComponent<CamareroController>().index = listaCamareros.Count;
        obj.GetComponent<CamareroController>().barPos = new Vector3 ((barPos.transform.position.x - 1 * listaCamareros.Count), barPos.transform.position.y, barPos.transform.position.z);
        listaCamareros.Add(obj.GetComponent<CamareroController>());
    }
    public void removeCamarero()
    {
        if (listaCamareros.Count > 0)
        {
            GameObject obj = listaCamareros[listaCamareros.Count - 1].gameObject;
            obj.GetComponent<CamareroController>().BorrarBocadillos();
            listaCamareros.RemoveAt(listaCamareros.Count - 1);
            Destroy(obj);
        }
    }
    public void addBarman()
    {
        if (listaBarman.Count >= 8)
        {
            return;
        }
        GameObject obj = Instantiate(prefabBarman, new Vector3(parentBarman.transform.position.x-1*listaBarman.Count, parentBarman.transform.position.y, parentBarman.transform.position.z), parentBarman.transform.rotation);
        obj.transform.SetParent(parentBarman.transform);
        obj.GetComponent<BarmanController>().worldController = this;
        listaBarman.Add(obj.GetComponent<BarmanController>());
    }
    public void removeBarman()
    {
        if (listaBarman.Count > 0)
        {
            GameObject obj = listaBarman[listaBarman.Count - 1].gameObject;
            obj.GetComponent<BarmanController>().BorrarBocadillos();
            listaBarman.RemoveAt(listaBarman.Count - 1);
            Destroy(obj);
        }
    }
    public void removeCLient(ClienteController obj)
    {
        listaClientes.Remove(obj);
        Destroy(obj.gameObject);
    }
    public Mesa mesaLibre()
    {
        for (int i = 0; i < listaMesas.Count; i++)
        {
            if (!listaMesas[i].ocupado)
            {
                listaMesas[i].ocupado = true;
                return listaMesas[i];
            }
        }
        return null;
    }
    public CamareroController camareroLibre()
    {
        for (int i = 0; i < listaCamareros.Count; i++)
        {
            if (listaCamareros[i].state == 0 || listaCamareros[i].state == 6)
            {
                return listaCamareros[i];
            }
        }
        return null;
    }
    public BarmanController barmanLibre()
    {
        for (int i = 0; i < listaBarman.Count; i++)
        {
            if (listaBarman[i].state == 0)
            {
                return listaBarman[i];
            }
        }
        return null;
    }
    public Bebida bebidaParaPreparar()
    {
        if (listaBebidasEnEspera.Count > 0)
        {
            Bebida aux = listaBebidasEnEspera[0];
            listaBebidasEnEspera.RemoveAt(0);
            return aux;
        }
        return null;
    }
    public void addBebidaParaPreparar(Bebida bebida)
    {
        listaBebidasEnEspera.Add(bebida);
    }
    public ClienteController clienteLibre()
    {
        if (listaClientes.Count == 0)
        {
            return null;
        }
        ClienteController c = null;
        int num = UnityEngine.Random.Range(0, listaClientes.Count);
        c = listaClientes[num];
        if(c.state == 2 || c.state == 3)
        {
            if (num == cAux) //para que no pida propina dos veces seguidas al mismo cliente, que es abusar
                return null;
            cAux = num;
            return c;
        }
        else 
        { 
            return null; 
        }
    }


    public void changeMusic()
    {
        if (musicaSonando)
        {
            //parar musica
            musicaSonando = false;
            FindObjectOfType<AudioManager>().Stop("Musica");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Musica");
            musicaSonando = true;
        }
    }
    public void ResetListas()
    {
        numEMesa = 0;
        numEPuerta = 0;
        numCContento = 0;
        time = 0f;
        setTimeZero();
        //borrar clientes
        if (listaClientes.Count > 0)
        {
            for (int i = listaClientes.Count; i >= 0; i--) 
                removeCLient();
        }
        //borrar camareros
        if (listaCamareros.Count > 0)
        {
            for (int i = listaCamareros.Count; i >= 0; i--)
                removeCamarero();
        }
        //borrar barman
        if (listaBarman.Count > 0)
        {
            for (int i = listaBarman.Count; i >= 0; i--)
                removeBarman();
        }
        //vaciar mesas
        for (int i = 0; i < listaMesas.Count; i++)
        {
            listaMesas[i].ocupado = false;
        }
        //tp bailarina a init
        bailarina.GetComponent<BTBailarina>().CreateBehaviourTree();
        bailarina.transform.position = bailarina.GetComponent<BTBailarina>().GetInitPos();
    }
    public void setTime()
    {
        if(inputField.text != "")
        {
            int aux = Convert.ToInt32(inputField.text);
            if (aux < 0)
            {
                aux = aux * -1;
            }
            time = 60f / aux;
            numText.text = aux.ToString();
            t = 0;
        }
    }
    public void setTimeZero()
    {
        inputField.text = "";
        numText.text = "0";
        t = 0;
    }
}
