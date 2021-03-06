using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnnemiController : MonoBehaviour, Interactable
{

    public bool isVirus = false;
    [SerializeField] string _name;
    public int moneyAfterBattle = 0;
    [SerializeField] SpriteRenderer _ennemi;
    [SerializeField] Dialog _dialog;
    [SerializeField] Dialog _dialogAfterBattle;
    [SerializeField] GameObject _exclamation;
    [SerializeField] GameObject _fov;
    [SerializeField] DialogManager _dialogManager;

    public BoxCollider2D collider2D;

    bool _battleLost = false;

    public void Interact()
    {
        if (!_battleLost && !isVirus)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>

            StartCoroutine(StartBattle())

            ));
        }
        else if (!_battleLost && isVirus)
        {
            StartCoroutine(StartBattle());
        }
        else
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialogAfterBattle)); ;
    }

    IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(0f);
        FadeBattle.Instance.imageFadeBattle.DOFade(1, 1).OnComplete(BattleStart);

    }
    void BattleStart()
    {
        _dialogManager.dialogBox.SetActive(false);
        GameController.Instance.StartEnnemiBattle(this);

    }
   
    public void BattleLost()
    {
        _battleLost = true;
        _fov.SetActive(false);
        collider2D.enabled = true;
        if (isVirus) {
            this.gameObject.SetActive(false);
        }
    }

    public IEnumerator TriggerEnnemiBattle(PlayerController player)
    {
        _exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _exclamation.SetActive(false);

        var pos = player.transform.position;
        pos.y = player.transform.position.y + 1;
        transform.position = pos;
        _ennemi.enabled = true;

        StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, () =>
              {
                  StartCoroutine(StartBattle());
              }));
    }

    public string Name { get => _name; }
}
