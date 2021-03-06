﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinSpearmanBehaviour : MonoBehaviour
{
    [SerializeField]
    private GoapEventHandler GoapSystem;
    [SerializeField]
    private MovableBody Movement;
    [SerializeField]
    private Image LockOnReticule;
    [SerializeField]
    private Health Health;
    [SerializeField]
    private OnLockEventEmitter LockOnEmitter;
    [SerializeField]
    private ScrollingFadingTextBehaviourFactory DamageTextFactory;
    [SerializeField]
    private GameObject Corpse;

    void Start() { }
    
    public void TakeDamage(OnDamageRecievedEventData DamageData)
    {
        DamageTextFactory.Make("-" + DamageData.Damage.ToString());
        Health.TakeDamage(DamageData.Damage);
        if(Health.CurrentValue < 0) {
            LockOnEmitter.Emit(new OnLockReleaseEventData());
            GameObject g = Instantiate(Corpse);
            g.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void LockAttained(OnLockAttainEventData e){
        LockOnReticule.enabled = true;
    }

    public void ReleaseLockOnDeath(OnLockReleaseEventData e){
        LockOnReticule.enabled = false;
    }

}
