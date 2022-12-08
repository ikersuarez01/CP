using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    [SerializeField] public GameObject puertaPos;
    public bool musicaSonando;
    public int cAux = -1;

    public void addCLient()
    {
        GameObject obj = Instantiate(prefabCliente, puertaPos.transform.position, puertaPos.transform.rotation);
        obj.transform.SetParent(parentCliente.transform);
        obj.GetComponent<ClienteController>().worldController = this;
        listaClientes.Add(obj.GetComponent<ClienteController>());
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
        GameObject obj = Instantiate(prefabCamarero, new Vector3(parentCamarero.transform.position.x, parentCamarero.transform.position.y, parentCamarero.transform.position.z - 2 * listaCamareros.Count), parentCamarero.transform.rotation);
        obj.transform.SetParent(parentCamarero.transform);
        obj.GetComponent<CamareroController>().worldController = this;
        obj.GetComponent<CamareroController>().index = listaCamareros.Count;
        obj.GetComponent<CamareroController>().barPos = new Vector3 ((barPos.transform.position.x + listaCamareros.Count), barPos.transform.position.y, barPos.transform.position.z);
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
        GameObject obj = Instantiate(prefabBarman, new Vector3(parentBarman.transform.position.x+1*listaBarman.Count, parentBarman.transform.position.y, parentBarman.transform.position.z), parentBarman.transform.rotation);
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
        for(int i = 0; i < listaCamareros.Count; i++)
        {
            if(listaCamareros[i].state == 0 || listaCamareros[i].state == 6)
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
        int num = Random.Range(0, listaClientes.Count);
        c = listaClientes[num];
        if (c.state == 0 || c.state == 1) //añadir clientes que se van
        {
            return null;
        }
        else
        {
            if (num == cAux) //para que no pida propina dos veces seguidas al mismo cliente, que es abusar
                return null;
            cAux = num;
            return c;
        }
    }
    public void Start()
    {
        FindObjectOfType<AudioManager>().Play("Musica");
        musicaSonando = true;
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
        bailarina.GetComponent<DecoratorBT>().CreateBehaviourTree();
        bailarina.transform.position = bailarina.GetComponent<DecoratorBT>().GetInitPos();
    }
}
