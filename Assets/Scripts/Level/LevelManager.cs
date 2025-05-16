using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //Bu class'ı bir amaçla açtım ama sonradan bunu başka bir yerde de yapabileceğimi fark ettim. Şimdilik burada dursun sonra birşeyler eklenebilir.
    [SerializeField] LevelSO currentLevel;

    [SerializeField]
    private PlayerFactory playerFactory;
    
    [SerializeField]
    private EnemyFactory enemyFactory;

    [SerializeField] 
    private VillageFactory VillageFactory;
    
    [SerializeField] 
    private EnviromentFactory EnviromentFactory;
    
    [SerializeField] 
    private SoundManager soundManager;

    [SerializeField]
    private int currentLevelIndex;

    [SerializeField] 
    private bool hasSeenTutorial;

    

    public float VillageInstantiateSoundVolume = 1f;
    public float PlayerNEnemyInstantiateSoundVolume = 1f;
    public float WaterInstantiateSoundVolume = 1f;
    public float MountainInstantiateSoundVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevelIndex = -1;
        
        if (soundManager == null)
        {
            Debug.Log("Soundmanager'ım yok, ben LevelManager");
            soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }
    }

    public void StartNewLevel()
    {
        if (hasSeenTutorial)
        {
            currentLevel = LevelDB.Instance.GetLevelByIndex(++currentLevelIndex);
            BuildLevel();
            return;
        }

        hasSeenTutorial = true;
        TutorialManager.Instance.BuildTutorialLevel();
    }

    public void BuildLevel()
    {
        StartCoroutine(LevelDesign());
    }

    
    
    IEnumerator LevelDesign()
{
    PlayerManager playerManager = PlayerManager.Instance;
    EnemyManager enemyManager = EnemyManager.Instance;

    while (!GridManager.Instance.hasInstantiated)
        yield return null;

    string[] lines = currentLevel.LevelLayout.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

    List<(char symbol, int i, int j)> waterAndMountain = new();
    List<(char symbol, int i, int j)> villageAndDrum = new();
    List<(char symbol, int i, int j)> players = new();
    List<(char symbol, int i, int j)> enemies = new();

    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        for (int j = 0; j < line.Length; j++)
        {
            char symbol = line[j];
            switch (symbol)
            {
                case 'W':
                case 'M':
                    waterAndMountain.Add((symbol, i, j));
                    break;
                case '+':
                case '%':
                    villageAndDrum.Add((symbol, i, j));
                    break;
                case '#':
                case '?':
                case 'j':
                    players.Add((symbol, i, j));
                    break;
                case '$':
                    enemies.Add((symbol, i, j));
                    break;
            }
        }
    }

    // 1. Water and Mountain
    foreach (var (symbol, i, j) in waterAndMountain)
    {
        var type = symbol == 'W' ? EnviromentType.Water : EnviromentType.Mountain;
        var obj = EnviromentFactory.Build(type, new Vector3(i, 1.4f, j), quaternion.identity);

        var yTarget = obj.transform.position.y;
        obj.transform.position += Vector3.up * 3;
        obj.transform.DOMoveY(yTarget, 1f).OnComplete(() =>
        {
            var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
            grid.GridObject = obj;

            if (symbol == 'M')
                playerManager.playerListForEnemyAI.Add(obj);

            var sound = symbol == 'W' ? SoundType.WaterInstantiateSound : SoundType.MountainInstantiateSound;
            var vol = symbol == 'W' ? WaterInstantiateSoundVolume : MountainInstantiateSoundVolume;
            soundManager.PlaySound(sound, vol);
        });
    }
    yield return new WaitForSeconds(1f);

    // 2. Village and Drum
    foreach (var (symbol, i, j) in villageAndDrum)
    {
        GameObject obj = null;
        if (symbol == '+')
        {
            var result = VillageFactory.BuildRandom(new Vector3(i, 1.4f, j), quaternion.identity);
            obj = result.Item1;
        }
        else if (symbol == '%')
        {
            obj = EnviromentFactory.Build(EnviromentType.Drum, new Vector3(i, 1.4f, j), quaternion.identity);
        }

        var yTarget = obj.transform.position.y;
        obj.transform.position += Vector3.up * 3;
        obj.transform.DOMoveY(yTarget, 1f).OnComplete(() =>
        {
            var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
            grid.GridObject = obj;

            playerManager.playerListForEnemyAI.Add(obj);
            soundManager.PlaySound(SoundType.VillageInstantiateSound, VillageInstantiateSoundVolume);
        });
    }
    yield return new WaitForSeconds(1f);

    // 3. Players
    foreach (var (symbol, i, j) in players)
    {
        PlayerType type = symbol switch
        {
            '#' => PlayerType.Paladin,
            '?' => PlayerType.Wizard,
            'j' => PlayerType.Rouge,
            _ => PlayerType.Paladin
        };

        var player = playerFactory.Build(type, new Vector3(i, 1.4f, j), quaternion.identity);
        var yTarget = player.transform.position.y;
        player.transform.position += Vector3.up * 3;
        player.transform.DOMoveY(yTarget, 1f).OnComplete(() =>
        {
            var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
            player.GetComponent<Player>().SetGridStart(grid.gameObject, yTarget);
            playerManager.playerListForEnemyAI.Add(player);
            soundManager.PlaySound(SoundType.CharacterNEnemyInstantiateSound, PlayerNEnemyInstantiateSoundVolume);
        });
    }
    yield return new WaitForSeconds(1f);

    // 4. Enemies
    int enemynumber = 0;
    foreach (var (symbol, i, j) in enemies)
    {
        var enemyTuple = enemyFactory.BuildRandom(new Vector3(i, 0f, j), Quaternion.identity);
        var enemy = enemyTuple.Item1;

        enemy.name += enemynumber++;
        var yTarget = enemy.transform.position.y;
        enemy.transform.position += Vector3.up * 3;

        enemy.transform.DOMoveY(yTarget, 1f).OnComplete(() =>
        {
            var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0f, j));
            grid.GridObject = enemy;

            if (enemy.TryGetComponent<EnemyBrain>(out var brain))
                brain.SetGrid(grid);

            enemyManager.enemyListForEnemyAI.Add(enemy);
            soundManager.PlaySound(SoundType.CharacterNEnemyInstantiateSound, PlayerNEnemyInstantiateSoundVolume);
        });
    }
    yield return new WaitForSeconds(1f);

    // Game start UI and mission logic
    InGameUITextMesh.Instance.OpenInGameUICanvas();
    InGameUITextMesh.Instance.UpdatePlayerBars();
    InGameUITextMesh.Instance.updatePublicBar(false);
    InGameUITextMesh.Instance.UpdateMissionInformation(currentLevel.getMissions());

    var missions = currentLevel.getMissions();

    foreach (var mission in missions)
        MissionManager.Instance.StartMission(mission);

    TurnBasedManager.Instance.StartCombat(currentLevel.MaxTurnNumber);
}


    public void DestroyLevel()
    {
        PlayerManager.Instance.ClearPlayers();
        EnemyManager.ClearEnemyList();
        VillageManager.Instance.ClearVillageList();

        foreach (var gridObjects in GridManager.Instance.GridList)
        {
            foreach (var gridObject in gridObjects)
            {
                var grid = gridObject.gameObject.GetComponent<Grid>();

                if (grid.GridObject)
                {
                    var tempGridObject = grid.GridObject;
                    grid.GridObject = null;
                    Destroy(tempGridObject);
                }
            }
        }
    }
}