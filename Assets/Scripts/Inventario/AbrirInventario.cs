using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirInventario : MonoBehaviour
{
    public MonoBehaviour controlCamara;
    public MonoBehaviour controlMovimiento;
    public GameObject inventario;
    public GameObject UiCrafteo;
    public bool banderaaInventario;
    public bool banderaCrafteo;

    // Start is called before the first frame update
    private void Awake()
    {
        inventario.SetActive(true);
        UiCrafteo.SetActive(true);
    }

    void Start()
    {
        inventario.SetActive(false);
        UiCrafteo.SetActive(false);
        banderaaInventario = false;
        banderaCrafteo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (banderaCrafteo == false)
            {
                UiCrafteo.SetActive(true);

                Crafteo crafteo = UiCrafteo.GetComponent<Crafteo>();

                crafteo.ComprobarPosibilidades();

                banderaCrafteo = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                controlCamara.enabled = false;
                controlMovimiento.enabled = false;
            }
            else
            {
                UiCrafteo.SetActive(false);
                banderaCrafteo = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                controlCamara.enabled = true;
                controlMovimiento.enabled = true;
            }
        }
    }
}
