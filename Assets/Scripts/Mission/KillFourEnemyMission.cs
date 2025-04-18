public class KillFourEnemyMission : IMission
{
    private int _killedEnemyCount;
    void OnEnable()
    {
        _killedEnemyCount = 0;
        isCompleted = false;
        EventManager.onEnemyKilled += UpdateOnMission;
    }

    void OnDisable()
    {
        EventManager.onEnemyKilled -= UpdateOnMission;
    }
    public override void UpdateOnMission()
    {
        _killedEnemyCount += 1;
        
        //Burada UI güncelle

        if (_killedEnemyCount>=4)
        {
            isCompleted = true;
            //Burada UI güncelle
        }
    }
}