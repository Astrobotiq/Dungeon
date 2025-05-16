public class NoDamageToVillageMission : IMission
{
    void OnEnable()
    {
        isCompleted = true;
        EventManager.onVillageTakeDamage += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onVillageTakeDamage -= UpdateOnMission;
    }
    public override void UpdateOnMission()
    {
        isCompleted = false;
    }
}