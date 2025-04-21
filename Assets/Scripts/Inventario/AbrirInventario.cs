using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirInventario : MonoBehaviour
{
    public MonoBehaviour controlCamara;
    public MonoBehaviour controlMovimiento;
    public GameObject inventario;
    public bool banderaaInventario;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        inventario.SetActive(false);
        banderaaInventario = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (banderaaInventario == false)
            {
                inventario.SetActive(true);
                banderaaInventario = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
            }
            else
            {
                inventario.SetActive(false);
                banderaaInventario = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                controlMovimiento.enabled = true;
            }
        }
    }
}
