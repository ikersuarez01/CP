using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bebida: MonoBehaviour
{
    public Vector3 posicionCliente;
    //Color
    //lo q sea
    public BarmanController barman;
    public ClienteController cliente;
    public Bebida(Vector3 pos)
    {
        posicionCliente = pos;
    }
}
