using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MoveBase;
using Random = UnityEngine.Random;

public enum BattleState { Start, ActionSelection, MoveSelection, PerformMove, EnnemiSelection, Busy, PartyScreen, BattleOver }


public class BattleSystem : MonoBehaviour
{
    [SerializeField] List<BattleUnit> _playerUnits;
    public List<BattleUnit> _playerUnitsDead;

    public bool isEnnemiBattle = false;

    [SerializeField] List<BattleUnit> _ennemyUnits;

    [SerializeField] BattleDialogBox _dialogBox;

    public BattleUnit _playerSeletedUnit;
    public BattleUnit _targetSeletedUnit;

    public event Action<bool> onBattleOver;

    public bool canSelected = true;
    public bool canSelectedEnnemi = false;
    public bool EnnemiSelected = false;

    private BattleState _state;

    private int _currentPlayer = 0;
    private int _currentEnnemy = 0;
    private int _currentAction;
    private int _currentMove;
    private int _currentMember = 0;
    private int _currentEnnemySelection = 0;

    private MonsterParty _playerParty;
    private MonsterParty _ennemiParty;
    private Monster _wildMonster;


    private PlayerController _player;
    private EnnemiController _ennemi;

    BattleUnit target;

    public void StartBattle(MonsterParty playerParty, Monster wildMonster)
    {
        this._playerParty = playerParty;
        this._wildMonster = wildMonster;
        StartCoroutine(SetupBattle());
    }
    public void StartEnnemiBattle(MonsterParty playerParty, MonsterParty ennemiParty)
    {
        this._playerParty = playerParty;
        this._ennemiParty = ennemiParty;

        isEnnemiBattle = true;
        _player = playerParty.GetComponent<PlayerController>();
        _ennemi = ennemiParty.GetComponent<EnnemiController>();
        StartCoroutine(SetupBattle());
    }

    private void ResetBattleState()
    {
        canSelected = true;
        canSelectedEnnemi = false;
        EnnemiSelected = false;
        _playerSeletedUnit = null;
        _targetSeletedUnit = null;
        _playerUnitsDead.RemoveRange(0, _playerUnitsDead.Count);
    }

    public IEnumerator SetupBattle()
    {
        foreach (var _playerUnit in _playerUnits)
        {
            _playerUnit.Clear();
            _playerUnit.isAttacking = false;
        }
        foreach (var _ennemyUnit in _ennemyUnits)
        {
            _ennemyUnit.Clear();
        }


        ResetBattleState();

        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].Setup(_playerParty.Monsters[_currentMember + i]);
        }
        for (int i = 0; i < _ennemyUnits.Count; i++)
        {
            if (i < _ennemiParty.Monsters.Count)
            {
                _ennemyUnits[i].Setup(_ennemiParty.Monsters[_currentMember + i]);
                _ennemyUnits[i].gameObject.SetActive(true);
            }
            else
                _ennemyUnits[i].gameObject.SetActive(false);
        }
        foreach (var _playerUnit in _playerUnits)
        {
            _playerUnit.Show();
        }
        foreach (var _ennemyUnit in _ennemyUnits)
        {
            _ennemyUnit.Show();
        }
        yield return new WaitForSeconds(0.5f);
    }

    void BattleOver(bool won)
    {
        _state = BattleState.BattleOver;
        _playerParty.Monsters.ForEach(p => p.OnBattleOver());
        onBattleOver(won);
    }

    private void ActionSelection()
    {
        _state = BattleState.ActionSelection;


        StartCoroutine(_dialogBox.TypeDialog("Choose an action"));
        _dialogBox.EnableActionSelector(true);
    }
    private void OpenPartyScreen()
    {
        _state = BattleState.PartyScreen;
    }
    public void MoveSelection()
    {
        _state = BattleState.MoveSelection;
        _dialogBox.SetMoveNames(_playerSeletedUnit.Monster.Moves);
        Debug.Log(_state);
    }
    public void EnnemiSelection()
    {
        _state = BattleState.EnnemiSelection;
        foreach (var item in _playerUnits)
        {
            if (item.isSelected)
            {
                canSelected = false;
                break;
            }
            else
                canSelected = true;
        }
        canSelectedEnnemi = true;
        Debug.Log(canSelected);
        Debug.Log(_state);
        //if(EnnemiSelected && _targetSeletedUnit.Monster.HP > 0)
        //    StartCoroutine(PlayerMove());

    }

    /*public void PlayerMoveSelection()
    {

        _state = BattleState.PerformMove;

    }*/

    public IEnumerator PlayerMove()
    {
        _state = BattleState.PerformMove;

        foreach (var item in _playerUnits)
        {
            if (item.isSelected)
            {
                canSelected = false;
                break;
            }
            else
                canSelected = true;
        }


        //if (_targetSeletedUnit.Monster.HP <= 0 && _ennemiParty.GetHealthyMonster() != null)


        /*target = _ennemyUnits[Random.Range(0, _ennemiParty.Monsters.Count)];
        if (target.Monster.HP <= 0 && _ennemiParty.GetHealthyMonster() != null)
        {

            for (int i = _ennemiParty.Monsters.Count - 1; i >= 0; i--)
            {
                yield return RunMove(_playerSeletedUnit, _targetSeletedUnit, move);
                
            }
        }*/
        var move = _playerSeletedUnit.Monster.Moves[_currentMove];
        if (move.Base.Target == MoveTarget.AllEnnemi)
        {
            yield return RunMoveForAllEnnemis(_playerSeletedUnit, move);

        }
        else
            yield return RunMove(_playerSeletedUnit, _targetSeletedUnit, move);

        foreach (var item in _playerUnits)
        {
            if (item.isSelected)
            {
                item.isSelected = false;
                canSelected = true;
                break;
            }
        }
        int playerCount = 0;
        for (int i = 0; i < _playerUnits.Count; i++)
        {
            if (_playerUnits[i].isAttacking)
                playerCount++;
            if (playerCount >= 3 - _playerUnitsDead.Count)
            {
                StartCoroutine(EnemyMove());
                break;
            }
            else
                EnnemiSelected = false;

        }
    }
    IEnumerator EnemyMove()
    {
        _state = BattleState.PerformMove;
        _currentEnnemy = 0;
        for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
        {
            if (_ennemyUnits[i].Monster.HP > 0)
            {
                var move = _ennemyUnits[i].Monster.GetRandomMove();

                bool provoqued = _ennemyUnits[i].Monster.OnFocusTarget();
                if (provoqued)
                {
                    target = _playerUnits[1];
                }
                else
                {
                    target = _playerUnits[Random.Range(0, _playerParty.Monsters.Count)];
                    if (target.Monster.HP <= 0 && _playerParty.GetHealthyMonster() != null)
                    {
                        for (int y = _playerParty.Monsters.Count - 1; y >= 0; y--)
                        {
                            if (_playerUnits[y].Monster.HP > 0)
                            {
                                target = _playerUnits[y];
                                break;
                            }
                        }
                    }
                }
                yield return RunMove(_ennemyUnits[i], target, move);
            }
        }

        var playerParty = _playerParty.GetHealthyMonster();
        if (playerParty != null)
        {

            for (int i = 0; i < _playerUnits.Count; i++)
            {
                _playerUnits[i].isAttacking = false;
            }
        }
    }
    IEnumerator RunMoveForAllEnnemis(BattleUnit sourceUnit, Move move)
    {
        move.PP--;
        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
        {
            if (_ennemyUnits[i].Monster.HP >= 0)
            {
                _ennemyUnits[i].PlayHitAnimation();
                if (move.Base.Category == MoveCategory.Status)
                {
                    StartCoroutine(RunMoveEffects(move, sourceUnit.Monster, _ennemyUnits[i].Monster));
                }
                else
                {
                    _ennemyUnits[i].Monster.TakeDamage(move, sourceUnit.Monster);
                    StartCoroutine(_ennemyUnits[i].Hud.UpdateHP());
                }
            }
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < _ennemiParty.Monsters.Count; i++)
        {
            if (_ennemyUnits[i].Monster.HP <= 0)
            {
                _ennemyUnits[i].PlayFaintAnimation();
                Debug.Log("dedMan");

                var _hudTarget = _ennemyUnits[i].GetComponentInChildren<BattleHud>(true);
                _hudTarget.gameObject.SetActive(false);
            }

            if (i == _ennemiParty.Monsters.Count - 1)
            {
                yield return new WaitForSeconds(1f);
                CheckForBattleOver(_ennemyUnits[i]);
            }
        }

        sourceUnit.Monster.OnAfterTurn();
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Monster.HP <= 0)
        {
            if (sourceUnit.isPlayerUnit)
                _playerUnitsDead.Add(_playerSeletedUnit);

            yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();

            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Monster.OnBeforeMove();
        if (!canRunMove)
        {
            yield break;
        }

        move.PP--;
        yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} used {move.Base.Name}");

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayHitAnimation();

        if (move.Base.Category == MoveCategory.Status)
        {
            yield return RunMoveEffects(move, sourceUnit.Monster, targetUnit.Monster);
        }
        else
        {
            var damageDetails = targetUnit.Monster.TakeDamage(move, sourceUnit.Monster);
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);
        }

        if (targetUnit.Monster.HP <= 0)
        {
            if (targetUnit.isPlayerUnit)
                _playerUnitsDead.Add(_playerSeletedUnit);

            yield return _dialogBox.TypeDialog($"{targetUnit.Monster.Base.Name} Fainted");
            targetUnit.PlayFaintAnimation();

            var _hudTarget = targetUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(targetUnit);
        }
        sourceUnit.Monster.OnAfterTurn();
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Monster.HP <= 0)
        {
            if (sourceUnit.isPlayerUnit)
                _playerUnitsDead.Add(_playerSeletedUnit);

            yield return _dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} Fainted");
            sourceUnit.PlayFaintAnimation();

            var _hudTarget = sourceUnit.GetComponentInChildren<BattleHud>(true);
            _hudTarget.gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
    }

    IEnumerator RunMoveEffects(Move move, Monster source, Monster target)
    {
        var effects = move.Base.Effects;


        //Boost
        if (effects.Boosts != null)
        {
            if (move.Base.Target == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }
        //Status
        if (effects.Status != ConditionID.none)
        {
            if (move.Base.Target == MoveTarget.Self)
                source.SetStatus(effects.Status);
            else
                target.SetStatus(effects.Status);

        }


        yield return new WaitForSeconds(0.1f);
    }



    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextMonster = _playerParty.GetHealthyMonster();

            if (nextMonster == null)
                BattleOver(false);
        }
        else
        {
            if (!isEnnemiBattle)
            {
                BattleOver(true);
            }
            else
            {
                var nextEnnemi = _ennemiParty.GetHealthyMonster();
                if (nextEnnemi == null)
                    BattleOver(true);
            }
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return _dialogBox.TypeDialog("A critical hit !");


        if (damageDetails.TypeEffectiveness > 1f)
            yield return _dialogBox.TypeDialog("Super effective !");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return _dialogBox.TypeDialog("Not very effective !");
    }


    IEnumerator TryToEscape()
    {
        _state = BattleState.Busy;

        if (isEnnemiBattle)
        {
            yield return _dialogBox.TypeDialog("You can run from ennemi battle");
            _state = BattleState.ActionSelection;
            yield break;
        }
        else
        {
            if (UnityEngine.Random.Range(1, 101) <= 50)
            {
                yield return _dialogBox.TypeDialog("Ran away safely!");
                BattleOver(true);
            }
            else
            {
                yield return _dialogBox.TypeDialog("Can't escape");
                StartCoroutine(EnemyMove());
            }
        }
    }

    public void onClickFight()
    {
        _currentAction = 0;
        MoveSelection();
    }
    public void onClickRun()
    {
        _currentAction = 1;
        Debug.Log(_currentAction);
    }
    public void onClickMove(int move)
    {
        _playerSeletedUnit.isAttacking = true;
        _dialogBox.EnableMoveSelector(false);
        _currentMove = move;
        EnnemiSelection();
        //StartCoroutine(PlayerMove());
    }

}
