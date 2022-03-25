using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{

    public bool isPlayerUnit;
    [SerializeField] BattleHud _hud;
    [SerializeField] HPBar _hpBar;

    private BattleState _state;

    [SerializeField] SeeOrNot _seeOrNot;
    [SerializeField] BattleSystem _battleSystem;

    [SerializeField] BattleDialogBox _dialogBox;



    public bool isAttacking;
    public bool isSelected;

    public Image _image;
    Vector3 _originalPos;
    public Color originalColor;

    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public BattleHud Hud { get { return _hud; } }

    public Monster Monster { get; set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _originalPos = _image.transform.localPosition;
    }


    public void Setup(Monster monster)
    {
        Monster = monster;

        if (isPlayerUnit)
            _image.sprite = Monster.Base.BackSprite;
        else
            _image.sprite = Monster.Base.FrontSprite;
        Monster.HP = Monster.MaxHp;

        _hpBar.SetHP((float)Monster.HP / Monster.MaxHp);

        _hud.SetData(Monster);

        _image.color = originalColor;
        PlayEnterAnimation();
    }


    public void Clear()
    {
        _hud.gameObject.SetActive(false);
    }
    public void Show()
    {
        _hud.gameObject.SetActive(true);
    }
    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
            _image.transform.localPosition = new Vector3(-500f, _originalPos.y);
        else
            _image.transform.localPosition = new Vector3(500f, _originalPos.y);

        _image.transform.DOLocalMoveX(_originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequance = DOTween.Sequence();
        if (isPlayerUnit)
            sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x + 50f, 0.25f));
        else
            sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x - 50f, 0.25f));

        sequance.Append(_image.transform.DOLocalMoveX(_originalPos.x, 0.25f));
    }
    public void PlayHitAnimation()
    {
        var sequance = DOTween.Sequence();

        sequance.Append(_image.DOColor(Color.red, 0.1f));
        sequance.Append(_image.DOColor(originalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequance = DOTween.Sequence();
        sequance.Join(_image.DOFade(0f, 0.5f));
    }
    public void OnMouseExit()
    {
        if (isPlayerUnit)
            StartCoroutine(_seeOrNot.enableOrDisableObject());
    }

    private void OnMouseDown()
    {
        Debug.Log(_state);
        if (_battleSystem.canSelectedEnnemi&& !_battleSystem.EnnemiSelected && !isPlayerUnit)
        {
            _battleSystem._targetSeletedUnit = this;
            _battleSystem.canSelectedEnnemi = false;
            _battleSystem.EnnemiSelected= true;
            if (_battleSystem._targetSeletedUnit.Monster.HP > 0)
                StartCoroutine(_battleSystem.PlayerMove());
        }
        else if (!isAttacking && _battleSystem.canSelected && isPlayerUnit)
        {
            isSelected = true;
            _battleSystem._playerSeletedUnit = this;
            var pos = _battleSystem._playerSeletedUnit.transform.position;
            pos.x += 4.5f;
            _seeOrNot.transform.position = pos;
            _battleSystem.MoveSelection();
            _dialogBox.EnableMoveSelector(true);
            Debug.Log(_state);
        }

    }



}
