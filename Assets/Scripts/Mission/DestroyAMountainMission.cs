public class DestroyAMountainMission : IMission
{
    void OnEnable()
    {
        isCompleted = false;
        EventManager.onMountainDestroyed += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onMountainDestroyed -= UpdateOnMission;
    }
    public override void UpdateOnMission()
    {
        isCompleted = true;
        //Make changes in UI
    }
}