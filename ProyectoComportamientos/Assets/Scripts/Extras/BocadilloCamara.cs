using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocadilloCamara : MonoBehaviour
{
    private Transform camera;
    //public bool autoDestroy = true;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        /*if (autoDestroy) { 
            Destroy(this.transform.GetChild(0).gameObject, 5);
            Destroy(this.gameObject, 5);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(camera);
    }
    /*void setAutoDestroy(bool destroy)
    {
        this.autoDestroy = destroy;
    }*/
}
