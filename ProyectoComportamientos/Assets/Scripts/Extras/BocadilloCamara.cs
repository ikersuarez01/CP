using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BocadilloCamara : MonoBehaviour
{
    private Transform camera;

    public ClienteController client;
    public bool movement = false;
    private float time = 1.5f;
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        t = 0f;
        time = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(camera);
        
        if (movement)
        {
            if(client == null)
            {
                Destroy(this.gameObject.transform.GetChild(0).gameObject);
                Destroy(this.gameObject);
                return;
            }
            transform.position = new Vector3(client.gameObject.transform.position.x, transform.position.y, client.gameObject.transform.position.z);
            t += Time.deltaTime / time;
            if (t > 1)
            {
                Destroy(transform.GetChild(0).gameObject);
                Destroy(gameObject);
            }
        }else if(client != null)
        {
            transform.position = new Vector3(client.gameObject.transform.position.x, transform.position.y, client.gameObject.transform.position.z);
        }
    }
}
