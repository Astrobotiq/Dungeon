using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class RotateSkillScript : ISkillEffect
{
    public override void StartMoving(Grid targetGrid) {
        //Bu gidicek bir şey mi emin olamadım ondan burasını boş bıraktım
    }

    public override void ApplyEffect(Grid targetGrid) {
        targetGrid.GridObject.gameObject.transform.Rotate(Vector3.up, 90);
    }
}
