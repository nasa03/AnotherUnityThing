﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryItems : MonoBehaviour
{

    [SerializeField]
    private ItemCollection Items;
    [SerializeField]
    private List<Image> ItemImages;
    [SerializeField]
    private Sprite NoItemImage;
    private bool IsOpen = false;

    void Start() {
        SetIsOpen(false);
    }

    void Update() {
        List<Item> Items = this.Items.GetItems();
        for(int n = 0; n < ItemImages.Count; n++) {
            if(n < Items.Count && Items[n] != null){
                ItemImages[n].sprite = Items[n].UIItemImage;
            } else {
                ItemImages[n].sprite = NoItemImage;
            }
        }
    }

    public void ToggleIsOpen(){
        IsOpen = !IsOpen; 
        gameObject.SetActive(IsOpen);
    }

    public void SetIsOpen(bool IsOpen){
        this.IsOpen = IsOpen;
        gameObject.SetActive(IsOpen);
    }
}
