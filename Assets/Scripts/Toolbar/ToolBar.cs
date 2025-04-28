using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
    public List<Sprite> toolBarSprites = new List<Sprite>();
    public Image toolBar;

    private void Start()
    {
        toolBar.sprite = toolBarSprites[0];
    }

    void QuitarMano()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolBar.sprite = toolBarSprites[0];
            QuitarMano();

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolBar.sprite = toolBarSprites[1];
            QuitarMano();

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
