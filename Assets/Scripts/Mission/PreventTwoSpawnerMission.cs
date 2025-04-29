public class PreventTwoSpawnerMission : IMission
{
    private int preventedSpawnerCount;
    void OnEnable()
    {
        preventedSpawnerCount = 0;
        isCompleted = false;
        EventManager.onSpawnerPrevented += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onSpawnerPrevented -= UpdateOnMission;
    }
    public override void UpdateOnMission()
    {
        preventedSpawnerCount += 1;

        if (preventedSpawnerCount>=2)
        {
            isCompleted = true;
            //MakeUI changes
        }
    }
}