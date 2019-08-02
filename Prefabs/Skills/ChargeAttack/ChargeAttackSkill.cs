﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ChargeAttackSkill : MonoBehaviour
{
    private float MovementWeighting = 100f;
    private bool IsCharging = false;
    private OnAttackEventHandler OnAttackEventHandler;
    private OnAttackEventEmitter OnAttackEventEmitter;
    private MovableBody MovableBody;
    private ColorShifter ColorShifter;
    private GameObject Caster;
    private Vector3 Target;
    private Vector3 Origin;

    public void CancelCharge(OnCancelCastData e){
        IsCharging = false;
    }

    public void DamageTarget(OnAttackConnectEventData e){
        OnDamageEventEmitter Emitter = e.With.GetComponent<OnDamageEventEmitter>();
        if(Emitter != null){
            Emitter.Emit(new OnDamageRecievedEventData(Caster, e.Damage));
        }
    }

    public void Charge(OnPointTargetCastEventData e){
        //such that the skill cannot be cast again if already in motion
        if(!IsCharging){
            Caster = e.Caster;
            OnAttackEventHandler = Caster.GetComponentInChildren<OnAttackEventHandler>();
            if(OnAttackEventHandler == null){
                throw new MissingComponentException(
                    "No `OnAttackEventHandler` component found on caster when attempting to use charge skill"
                );
            }
            OnAttackEventEmitter = Caster.GetComponentInChildren<OnAttackEventEmitter>();
            if(OnAttackEventEmitter == null){
                throw new MissingComponentException(
                    "No `OnAttackEventEmitter` component found on caster when attempting to use charge skill"
                );
            }
            ColorShifter = Caster.GetComponentInChildren<ColorShifter>();
            if(ColorShifter == null) {
                Debug.LogWarning("No `ColorShifter` shifter component found when attempting to use charge skill");
            }
            MovableBody = Caster.GetComponentInChildren<MovableBody>();
            if(MovableBody == null){
                throw new MissingComponentException(
                    "No `MovableBody` component found on caster when attempting to use charge skill"
                );
            }
            Target = e.Target;
            Origin = e.Caster.transform.position;
            StartCharging(
                e.Stats.GetValue("ChargeDuration", 0.33f),
                e.Stats.GetValue("ChargeBonusSpeed", 2),
                e.Stats.GetValue("ChargeBonusAcceleration", 10)
            );
        }
    }

    public void StartCharging(
        float duration,
        float bonusSpeed,
        float bonusAcceleration
    ){
        IsCharging = true;
        OnAttackEventHandler.OnAttackConnect.AddListener(DamageTarget);
        if(ColorShifter != null){
            ColorShifter.ShiftToColor(new Color(1f,1f,1f), new Color(1f,0f,0f), duration / 4f);
        }
        Vector3 direction = (Target - Origin).normalized;
        StartCoroutine(EndAfter(
            duration,
            bonusSpeed,
            bonusAcceleration,
            direction
        ));
        MovableBody.SetMaxSpeed(MovableBody.GetMaxSpeed() + bonusSpeed);
        MovableBody.SetAcceleration(MovableBody.GetAcceleration() + bonusAcceleration);
        MovableBody.AddToDirection(direction, MovementWeighting);
    }

    private IEnumerator EndAfter(
        float duration,
        float bonusSpeed,
        float bonusAcceleration,
        Vector3 direction
    ){
        float timer = 0;
        while(timer < duration && IsCharging == true) {
            timer += Time.deltaTime;
            Ray ray = new Ray(Caster.transform.position, direction);
            RaycastHit[] hits = Physics.RaycastAll(ray, 0.125f); //Physics.OverlapSphere(Caster.transform.position, 0.25f);
            foreach(RaycastHit hit in hits){
                if(hit.collider.gameObject == Caster.gameObject) continue; //dont collider with self
                if(hit.collider.gameObject.layer == Caster.layer) continue; //dont collide with shared tags as caster
                if(hit.collider.GetComponent<OnDamageEventEmitter>() != null)
                {
                    OnAttackEventEmitter.Emit(new OnAttackConnectEventData(
                        hit.collider.gameObject,
                        25f //this should come from passed in stats
                    ));
                    IsCharging = false;
                }
            }
            yield return null;
        }
        IsCharging = false;
        OnAttackEventHandler.OnAttackConnect.RemoveListener(DamageTarget);
        MovableBody.SetMaxSpeed(MovableBody.GetMaxSpeed() - bonusSpeed);
        MovableBody.SetAcceleration(MovableBody.GetAcceleration() - bonusAcceleration);
        MovableBody.AddToDirection(direction, -MovementWeighting);
        if(ColorShifter != null) {
            ColorShifter.ShiftToColor(new Color(1f,0f,0f), new Color(1f,1f,1f), duration / 4f);
        }
        OnAttackEventEmitter.Emit(new OnAttackEndEventData());
    }
}