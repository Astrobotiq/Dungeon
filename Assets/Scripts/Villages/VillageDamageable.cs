using UnityEngine;

public class VillageDamageable : IDamagable
{
    public override void Damage(int damage, bool willPush) {
        base.Damage(damage);
        //InGameUITextMesh.Instance.updatePublicBar();
    }
}
