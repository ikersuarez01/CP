using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bebida: MonoBehaviour
{
    public Vector3 posicionCliente;
    //Color
    public int tipo; // 0 = amarillo / 1 = azul / 2 = rojo / 3 = Verde
    [SerializeField] public  GameObject[] modeloBebidas;
    //lo q sea
    public BarmanController barman;
    public ClienteController cliente;
    public CamareroController camarero;
    public Bebida(Vector3 pos)
    {
        posicionCliente = pos;
    }
}
