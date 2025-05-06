using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
    public List<Sprite> toolBarSprites = new List<Sprite>();
    public Image toolBar;

    public GameObject espadaMano;
    public GameObject palaMano;

    private void Start()
    {
        toolBar.sprite = toolBarSprites[0];
        espadaMano.SetActive(true);
        palaMano.SetActive(false);
    }

    void QuitarMano()
    {
        espadaMano.SetActive(false);
        palaMano.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolBar.sprite = toolBarSprites[0];
            QuitarMano();
            espadaMano.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolBar.sprite = toolBarSprites[1];
            QuitarMano();
            palaMano.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toolBar.sprite = toolBarSprites[2];
            QuitarMano();

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            toolBar.sprite = toolBarSprites[3];
            QuitarMano();
        }
    }
}
