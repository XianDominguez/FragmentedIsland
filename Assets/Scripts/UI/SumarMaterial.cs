using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SumarMaterial : MonoBehaviour
{
    Animator animator;
    public Image material;
    public TextMeshProUGUI sumaMaterial;
    public TextMeshProUGUI totalMaterial;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnimacionSumar(Collider other)
    {
        ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();

        sumaMaterial.text = "+ 1 " + itemObject.ItemData.name;

        material.sprite = itemObject.ItemData.iconoItem;

        animator.Play("TextoSumarMaterial", -1, 0f);
    }
}
