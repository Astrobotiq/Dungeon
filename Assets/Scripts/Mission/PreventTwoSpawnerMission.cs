public class PreventTwoSpawnerMission : IMission
{
    private int preventedSpawnerCount;
    void OnEnable()
    {
        preventedSpawnerCount = 0;
        isCompleted = false;
        EventManager.onEnemyKilled += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onEnemyKilled -= UpdateOnMission;
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