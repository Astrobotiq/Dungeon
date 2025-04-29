using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region SerializeFields

    [Header("UI")] 
    [SerializeField] GameView gameView;

    [SerializeField] Sprite playerSprite;

    [Header("Move")] [SerializeField, Range(1, 5)]
    public int range;

    [SerializeField] Transform footPivot;

    [SerializeField] Transform handPivot;

    [SerializeField] Move move;

    [SerializeField] 
    private AttackPreview attackPreview;
    
    private bool _isPlayerTurn = false;
    
    private bool _hasWebbed = false;

    [SerializeField] bool hasPreviewedTheSkill = false;

    [Header("Selecteds")] public GameObject Grid { get; private set; }
    [SerializeField] GameObject _selectedSkillEffect;

    [SerializeField] 
    private List<uint> startSkillList;

    #endregion

    #region Publics

    public WornSkill SelectedSkill { get; private set; }

    public WornSkills WornSkills;
    public bool HasUsedSkill { get; private set; } = false;
    public bool HasTraveled { get; private set; } = false;

    public bool IsInWater = false;

    public Sprite PlayerSprite
    {
        get { return playerSprite; }
    }

    #endregion
    
    [SerializeField] 
    private SoundManager soundManager;

    public float PlayerEffectSoundVolume = 1f;
    
    void Start()
    {
        PlayerManager.Instance.Subscribe(this);
        gameView = GameObject.FindWithTag("UI").GetComponent<GameView>();
        //Burası şimdilik duruyor. Elle girmemiz gereken bir noktadayız
        
        WornSkills = new WornSkills();

        if (startSkillList.Count>0)
        {
            foreach (var skillId in startSkillList)
            {
                WornSkill skill = new WornSkill(skillId);
                WornSkills.SetWornSkill(skill);
            }
        }
        else
        {
            Debug.Log("There is no skill to use, Boss");
        }
        soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
    }

    void OnDestroy()
    {
        
    }

    public void SetPlayerTurn(bool isIt)
    {
        _isPlayerTurn = isIt;
        HasTraveled = false;
        HasUsedSkill = false;
        SelectedSkill = null;
        hasPreviewedTheSkill = false;
    }

    public bool IsPlayerTurn => _isPlayerTurn;

    public void SetPlayerWebbed(bool hasWebbed)
    {
        _hasWebbed = hasWebbed;
    }

    public bool IsPlayerWebbed => _hasWebbed;

    public void SetGrid(GameObject Grid, float offset)
    {
        if (!_isPlayerTurn)
            return;
        
        
        if (SelectedSkill == null && !HasTraveled && !HasUsedSkill)
        {
            if (_hasWebbed)
            {
                return;
            }
            
            if (this.Grid == Grid)
            {
                return;
            }

            if (!canTravel(Grid))
            {
                PlayerManager.Instance.SetSelectedPlayer(null);
                return;
            }

            Command command = new Command(this, this.Grid, offset);
            CommandManager.Instance.AddCommand(command);

            move.StartMove(this.Grid.GetComponent<Grid>(), Grid.GetComponent<Grid>());
            GridManager.Instance.ResetTable();
            onPositionChange(Grid);
            HasTraveled = true;
        }
        else
        {
            if (IsInWater)
            {
                return;
            }

            if (hasPreviewedTheSkill)
            {
                InputManager.Instance.canTakeInput = false;
                attackPreview.ClosePreviews();
                StartCoroutine(move.Turn(this.Grid.transform.position, Grid.transform.position, (InstantiateSkill)));
                return;
            }

            switch (SelectedSkill.Skill.AttackPreviewType)
            {
                case AttackPreviewType.None:
                    break;
                case AttackPreviewType.Rotate:
                    attackPreview.PreviewRotateable(Grid.gameObject.transform.position);
                    break;
                case AttackPreviewType.PushFourSide:
                    attackPreview.PreviewFourSidedPushable(Grid.gameObject.transform.position);
                    break;
                case AttackPreviewType.PushOneSide:
                    attackPreview.PreviewOneSidedPushable(transform.position,Grid.gameObject.transform.position);
                    break;
            }

            hasPreviewedTheSkill = true;

            void InstantiateSkill()
            {
                var skillGo = SelectedSkill.Skill.SkillGO;
                var go = Instantiate(skillGo, handPivot.position, quaternion.identity);
                go.GetComponent<ISkillEffect>().StartMoving(Grid.GetComponent<Grid>());
                DestroyImmediate(_selectedSkillEffect);
                gameView.ResetGameView();
                CommandManager.Instance.ClearCommands();
                HasUsedSkill = true;
                var playerTurn = TurnBasedManager.Instance.GetCurrentTurn() as PlayerTurn;
                playerTurn?.SetPlayerAsPlayed(this);
                InputManager.Instance.canTakeInput = true;
            }

            _hasWebbed = false;
        }
    }

    bool canTravel(GameObject Grid)
    {
        return Mathf.Abs(Grid.transform.position.x - transform.position.x) +
            Mathf.Abs(Grid.transform.position.z - transform.position.z) <= range;
    }

    public void SetGridStart(GameObject Grid, float offset)
    {
        if (this.Grid == Grid)
        {
            return;
        }

        onPositionChange(Grid);

        transform.position =
            new Vector3(Grid.transform.position.x, Grid.transform.position.y + offset, Grid.transform.position.z);
    }

    public void MakeSelectedPlayerThis()
    {
        HandleUI(true, this);
        PlayerManager.Instance.SetSelectedPlayer(this.gameObject);
    }

    public void SetSelectedPlayerFromOutside()
    {
        if (PlayerManager.Instance.GetSelectedPlayer() == this.gameObject)
        {
            return;
        }
        HandleUI(true, this);
        PlayerManager.Instance.SetSelectedPlayer(this.gameObject);
    }

    void HandleUI(bool isActive, Player player = null)
    {
        if (!HasUsedSkill && _isPlayerTurn)
        {
            gameView.OpenSkillPanel(isActive, player);
        }
        gameView.OpenPlayerPanel(isActive, player);
    }

    public void SetSelectedSkill(SkillType type)
    {
        SelectedSkill = WornSkills.GetSkill(type);

        if (SelectedSkill != null &&
            (!_selectedSkillEffect || SelectedSkill.Skill.PlayerEffect != _selectedSkillEffect))
        {
            _selectedSkillEffect = SelectedSkill.Skill.PlayerEffect;
            _selectedSkillEffect = Instantiate(_selectedSkillEffect, footPivot.position, Quaternion.identity);
            
            soundManager.PlaySound(SelectedSkill.Skill.SoundType);

            GridManager.Instance.StartSearchForSkill(SelectedSkill.Skill.SearchType);
        }
    }

    public void Undo(GameObject grid, float offset)
    {
        move.StartMove(Grid.GetComponent<Grid>(), grid.GetComponent<Grid>());
        onPositionChange(grid);
        GridManager.Instance.SetSelectedGridFromOutside(Grid.transform.position, false);
        HasTraveled = false;
        hasPreviewedTheSkill = false;
    }

    public void onDeselected()
    {
        if (_selectedSkillEffect != null)
        {
            Destroy(_selectedSkillEffect);
            _selectedSkillEffect = null;
        }

        SelectedSkill = null;
        hasPreviewedTheSkill = false;
        attackPreview.ClosePreviews();
        
        HandleUI(false);
    }

    public void onPositionChange(GameObject newOne)
    {
        if (Grid != null)
        {
            Grid.GetComponent<Grid>().GridObject = null;
        }

        Grid = newOne;
        Grid.GetComponent<Grid>().GridObject = this.gameObject;
    }

}