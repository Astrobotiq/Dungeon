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

    public float VillageInstantiateSoundVolume = 1f;
    public float PlayerNEnemyInstantiateSoundVolume = 1f;
    public float WaterInstantiateSoundVolume = 1f;
    public float MountainInstantiateSoundVolume = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLevelIndex = -1;
        
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
    }

    public void StartNewLevel()
    {
        currentLevel = LevelDB.Instance.GetLevelByIndex(++currentLevelIndex);
        BuildLevel();
    }

    public void BuildLevel()
    {
        StartCoroutine(LevelDesign());
    }

    IEnumerator LevelDesign()
    {
        PlayerManager playerManager = PlayerManager.Instance;
        EnemyManager enemyManager = EnemyManager.Instance;
        
        Debug.Log("Level");
        while (!GridManager.Instance.hasInstantiated)
        {
            yield return null;
        }

        Debug.Log("Level hazırlanıyor");

        string[] lines = currentLevel.LevelLayout.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

        var enemynumber = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            
            var line = lines[i];
            for (int j = 0; j < line.Length; j++)
            {
                if (line[j].Equals('0'))
                {
                    continue;
                }

                if (line[j].Equals('#'))
                {
                    Debug.Log("Player bulundu");
                    var player =  playerFactory.Build(PlayerType.Paladin, new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var yPosTarget = player.transform.position.y;

                    player.transform.position = new Vector3(player.transform.position.x,
                        player.transform.position.y + 3, player.transform.position.z);

                    player.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        player.GetComponent<Player>().SetGridStart(grid.gameObject, 1.4f);
                        playerManager.playerListForEnemyAI.Add(player);
                        
                        soundManager.PlaySound(SoundType.CharacterNEnemyInstantiateSound,PlayerNEnemyInstantiateSoundVolume);
                    }));

                    yield return new WaitForSeconds(1f);
                }
                
                if (line[j].Equals('?'))
                {
                    Debug.Log("Player bulundu");
                    var player =  playerFactory.Build(PlayerType.Wizard, new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var yPosTarget = player.transform.position.y;

                    player.transform.position = new Vector3(player.transform.position.x,
                        player.transform.position.y + 3, player.transform.position.z);

                    player.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        player.GetComponent<Player>().SetGridStart(grid.gameObject, 1.4f);
                        playerManager.playerListForEnemyAI.Add(player);
                        
                        soundManager.PlaySound(SoundType.CharacterNEnemyInstantiateSound,PlayerNEnemyInstantiateSoundVolume);
                    }));

                    yield return new WaitForSeconds(1f);
                }

                if (line[j].Equals('$'))
                {
                    Debug.Log("EnemyBulundu");
                    var Enemy = enemyFactory.BuildRandom(new Vector3(i, 0f, j),
                        Quaternion.identity);

                    var EnemyObj = Enemy.Item1;

                    EnemyObj.name = EnemyObj.name + enemynumber;
                    enemynumber++;
                    
                    var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0f, j));
                    grid.GridObject = EnemyObj;
                    
                    if(EnemyObj.GetComponent<EnemyBrain>() != null)
                        EnemyObj.GetComponent<EnemyBrain>().SetGrid(grid);
                    
                    enemyManager.enemyListForEnemyAI.Add(EnemyObj);
                    
                    soundManager.PlaySound(SoundType.CharacterNEnemyInstantiateSound,PlayerNEnemyInstantiateSoundVolume);
                }

                if (line[j].Equals('+'))
                {
                    Debug.Log("Village bulundu");
                    var Village = VillageFactory.BuildRandom(new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var offset = Village.Item2;
                    var VillageGO = Village.Item1;

                    var yPosTarget = VillageGO.transform.position.y;

                    VillageGO.transform.position = new Vector3(VillageGO.transform.position.x,
                        VillageGO.transform.position.y + 3, VillageGO.transform.position.z);

                    VillageGO.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        VillageGO.GetComponent<Village>().SetGrid(grid);
                        playerManager.playerListForEnemyAI.Add(VillageGO);
                        
                        soundManager.PlaySound(SoundType.VillageInstantiateSound,VillageInstantiateSoundVolume);
                    }));
                    
                    yield return new WaitForSeconds(1f);
                }

                if (line[j].Equals('%'))
                {
                    Debug.Log("Village bulundu");
                    var Village = EnviromentFactory.Build(EnviromentType.Drum,new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var yPosTarget = Village.transform.position.y;

                    Village.transform.position = new Vector3(Village.transform.position.x,
                        Village.transform.position.y + 3, Village.transform.position.z);

                    Village.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        grid.GridObject = Village;
                        playerManager.playerListForEnemyAI.Add(Village);
                        
                        soundManager.PlaySound(SoundType.VillageInstantiateSound,VillageInstantiateSoundVolume);
                    }));
                    
                    yield return new WaitForSeconds(1f);
                }

                if (line[j].Equals('W'))
                {
                    Debug.Log("Village bulundu");
                    var Village = EnviromentFactory.Build(EnviromentType.Water,new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var yPosTarget = Village.transform.position.y;

                    Village.transform.position = new Vector3(Village.transform.position.x,
                        Village.transform.position.y + 3, Village.transform.position.z);
                    
                    soundManager.PlaySound(SoundType.WaterInstantiateSound,WaterInstantiateSoundVolume);
                    
                    Village.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        grid.GridObject = Village;
                        //playerManager.playerListForEnemyAI.Add(Village);
                    }));
                    
                    yield return new WaitForSeconds(1f);
                }

                if (line[j].Equals('M'))
                {
                    Debug.Log("Mountain bulundu");
                    var Village = EnviromentFactory.Build(EnviromentType.Mountain,new Vector3(i, 1.4f, j),
                        quaternion.identity);

                    var yPosTarget = Village.transform.position.y;

                    Village.transform.position = new Vector3(Village.transform.position.x,
                        Village.transform.position.y + 3, Village.transform.position.z);

                    Village.transform.DOMoveY(yPosTarget, 1f).OnComplete((() =>
                    {
                        var grid = GridManager.Instance.getGridFromLocation(new Vector3(i, 0, j));
                        grid.GridObject = Village;
                        playerManager.playerListForEnemyAI.Add(Village);
                        
                        soundManager.PlaySound(SoundType.MountainInstantiateSound,MountainInstantiateSoundVolume);
                    }));
                    
                    yield return new WaitForSeconds(1f);
                }
                
            }
        }
        InGameUITextMesh.Instance.OpenInGameUICanvas();
        InGameUITextMesh.Instance.UpdatePlayerBars();
        InGameUITextMesh.Instance.updatePublicBar();
        InGameUITextMesh.Instance.UpdateMissionInformation(currentLevel.getMissions());
        
        foreach (var mission in currentLevel.getMissions())
        {
            MissionManager.Instance.StartMission(mission);
        }
        
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