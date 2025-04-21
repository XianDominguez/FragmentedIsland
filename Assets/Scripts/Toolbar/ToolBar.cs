using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : MonoBehaviour
{
    public List<Sprite> toolBarSprites = new List<Sprite>();
    public Image toolBar;

    public List<GameObject> handImages = new List<GameObject>();

    private void Start()
    {
        toolBar.sprite = toolBarSprites[0];
        handImages[0].SetActive(true);
    }

    void QuitarMano()
    {
        handImages[0].SetActive(false);
        handImages[1].SetActive(false);
        handImages[2].SetActive(false);
        handImages[3].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            toolBar.sprite = toolBarSprites[0];
            QuitarMano();
            handImages[0].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            toolBar.sprite = toolBarSprites[1];
            QuitarMano();
            handImages[1].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            toolBar.sprite = toolBarSprites[2];
            QuitarMano();
            handImages[2].SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            toolBar.sprite = toolBarSprites[3];
            QuitarMano();
            handImages[3].SetActive(true);
        }
    }
}
