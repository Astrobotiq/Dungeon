using UnityEngine;

public class VillageDamageable : IDamagable
{
    public override void Damage(int damage) {
        base.Damage(damage);
        InGameUITextMesh.Instance.updatePublicBar();
    }
}
