using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    private TutorialStep _currentTutorialStep;
    
    private bool _isTutorialActive = false;
    
    private PlayerManager playerManager;
    
    private EnemyManager enemyManager;
    
    [SerializeField] 
    private List<TutorialStep> tutorialSteps;
    
    [SerializeField] 
    private TutorialLevelSO tutorialLevelSo;

    [SerializeField] 
    private int currentTutorialStep;

    public TextAsset GetCurrentTutorialStep() => tutorialLevelSo.GetTutorialSteps[currentTutorialStep];

    public bool isInTutorialLevel = false;

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
    
    
    public float VillageInstantiateSoundVolume = 1f;
    public float PlayerNEnemyInstantiateSoundVolume = 1f;
    public float WaterInstantiateSoundVolume = 1f;
    public float MountainInstantiateSoundVolume = 1f;
    
    private Queue<IEnumerator> tutorialQueue = new Queue<IEnumerator>();
    private bool isTutorialRunning = false;

    #region StackTypeTutorial

    // Public method to enqueue a tutorial
    public void EnqueueTutorial(TutorialType tutorialType)
    {
        if (!isInTutorialLevel)
        {
            return;
        }
        tutorialQueue.Enqueue(ShowTutorialRoutine(tutorialType));
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (isTutorialRunning || tutorialQueue.Count == 0) return;

        StartCoroutine(ProcessTask());
    }

    private IEnumerator ProcessTask()
    {
        isTutorialRunning = true;

        yield return StartCoroutine(tutorialQueue.Dequeue());

        isTutorialRunning = false;
        ProcessQueue(); // sıradaki tutorial
    }

    private IEnumerator ShowTutorialRoutine(TutorialType tutorialType)
    {
        // Zaten açık bir tutorial varsa loglayıp geç
        if (_isTutorialActive)
        {
            Debug.Log("Another tutorial is active.");
            yield break;
        }

        foreach (var step in tutorialSteps)
        {
            if (step.TutorialType == tutorialType)
            {
                Debug.Log($"Tutorial type : {tutorialType}");
                _isTutorialActive = true;
                _currentTutorialStep = step;
                step.EnterTutorial();

                // Aktif olduğu sürece bekle
                while (_isTutorialActive)
                    yield return null;

                yield return new WaitForSeconds(1f);
            }
        }
    }

    #endregion
    
    void Start()
    {
        tutorialSteps = GetComponents<TutorialStep>().ToList();
        currentTutorialStep = -1;
        playerManager = PlayerManager.Instance;
        enemyManager = EnemyManager.Instance;
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
    }

    public void CloseTutorial()
    {
        if (!_isTutorialActive) return;
        _isTutorialActive = false;
        _currentTutorialStep = null;
    }
    
    public void BuildTutorialLevel()
    {
        isInTutorialLevel = true;
        StartCoroutine(TutorialLevelDesign());
    }

    IEnumerator TutorialLevelDesign()
    {
        
        this.currentTutorialStep++;
        
        if (this.currentTutorialStep>= tutorialLevelSo.GetTutorialSteps.Count)
        {
            CameraManager.Instance.OnLevelCompleted();
            isInTutorialLevel = false;
            yield break;
        }
        
        while (!GridManager.Instance.hasInstantiated)
            yield return null;

        if (this.currentTutorialStep>0)
        {
            yield return StartCoroutine(DestroyTutorial());
        }
        
        var currentTutorialStep = tutorialLevelSo.GetTutorialSteps[this.currentTutorialStep];
        
        string[] lines = currentTutorialStep.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        
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
                    case 'B':
                    case 'R':
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
        EnemyType type = symbol switch
        {
            'R' => EnemyType.Ranger,
            '$' => EnemyType.Spider,
            'B' => EnemyType.Wizard,
            _ => EnemyType.Ranger
        };
        
        var enemyTuple = enemyFactory.Build(type,new Vector3(i, 0f, j), Quaternion.identity);
        var enemy = enemyTuple;

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
    
    TurnBasedManager.Instance.StartCombat(tutorialLevelSo.GetTutorialSteps.Count());
    }

    public IEnumerator DestroyTutorial()
    {
        List<GameObject> objects = new();
        var gridLists = GridManager.Instance.GridList;
        foreach (var gridList in gridLists)
        {
            foreach (var gridObj in gridList)
            {
                var grid = gridObj.GetComponent<Grid>();
                if (grid.GridObject)
                {
                    objects.Add(grid.GridObject);
                    grid.GridObject = null;
                }
            }
        }

        foreach (var tileObj in objects)
        {
            tileObj.gameObject.transform.DOMoveY(3f, 1f).OnComplete((() => Destroy(tileObj)));
        }

        yield return new WaitForSeconds(2f);            
    }
}

public enum TutorialType
{
    PlayerSelect,
    PlayerMove,
    WizardAttack,
    PlayerSkill,
    Water,
    Undo,
    PaladinAttack,
    JesterAttack,
    EndTurnButton
}

