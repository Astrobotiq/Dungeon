using UnityEngine;

public class DrumDamageable : IDamagable
{
    public override void Damage(int damage, bool willPush) {
        base.Damage(damage);
        //InGameUITextMesh.Instance.updatePublicBar();
    }
}
