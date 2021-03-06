﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EnvenomSkill : MonoBehaviour
{
    [SerializeField]
    private Status PosionStatus;

    public void Cast(OnPointTargetCastEventData e) {
        StatusCollection statusCollection =
            e.Caster.GetComponentInChildren<StatusCollection>();
        if(statusCollection != null){
            Status posionStatus = Instantiate(PosionStatus);
            statusCollection.AddStatus(posionStatus);
            posionStatus.Emitter.Emit(
                new OnStatusStartEventData(e.Caster, gameObject, 3f)
            );
        }
    }
}