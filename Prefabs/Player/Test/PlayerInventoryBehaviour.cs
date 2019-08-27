﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryBehaviour : MonoBehaviour {

    [SerializeField]
    private ItemCollection Items;

	public void OnAdd(OnInventoryAddEventData e){
        Items.AddItemToFirstEmptyIndex(e.Item);
    }

    public void UseItemByKeyEvent(OnKeyDownEventData e){
        switch(e.Key){
            case KeyCode.F1:
                Items.GetItemAtIndex(0).Emitter.Emit(
                    new OnItemUseEventData(gameObject)
                );
                break;
            case KeyCode.F2:
                Items.GetItemAtIndex(1).Emitter.Emit(
                    new OnItemUseEventData(gameObject)
                );
                break;
            case KeyCode.F3:
                Items.GetItemAtIndex(2).Emitter.Emit(
                    new OnItemUseEventData(gameObject)
                );
                break;
            case KeyCode.F4:
                Items.GetItemAtIndex(3).Emitter.Emit(
                    new OnItemUseEventData(gameObject)
                );
                break;
            case KeyCode.F5:
                Items.GetItemAtIndex(4).Emitter.Emit(
                    new OnItemUseEventData(gameObject)
                );
                break;
        }
    }
}
