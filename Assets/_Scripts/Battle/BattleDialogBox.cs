using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{

    [SerializeField] int _lettersPerSecond;
    [SerializeField] Color _highlightedColor;

    [SerializeField] Text _dialogText;
    [SerializeField] GameObject _actionSelector;
    [SerializeField] GameObject _moveSelector;
    [SerializeField] GameObject _moveDetails;

    [SerializeField] List<Text> _actionTexts;
    [SerializeField] List<Text> _moveTexts;
    [SerializeField] List<GameObject> _movebuttons;
    [SerializeField] Text _ppText;
    [SerializeField] Text _typeText;

    [SerializeField] BattleSystem _battleSystem;
    private void Start()
    {
       
    }
    public void SetDialog(string dialog)
    {
        _dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        _dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            _dialogText.text += letter;
            yield return new WaitForSeconds(1f / _lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    public void EnableMoveSelector(bool enabled)
    {
        _moveSelector.SetActive(enabled);
    }

    public void SetMoveNames(List<Move> moves, BattleUnit monster= null)
    {
        for (int i = 0; i < _moveTexts.Count; i++)
        {
            if (i < moves.Count && _battleSystem._playerSelectedUnit.isPowerUsed)
            {
                _moveTexts[i].text = moves[i].Base.Name;
                _moveTexts[4].text = "•";
                _movebuttons[4].SetActive(false);
            }
            else if (i < moves.Count &&
                _battleSystem._playerSelectedUnit.thisMonster.Base.LearnableMoves[i]._Activated)
            {
                _moveTexts[i].text = moves[i].Base.Name;
                _movebuttons[i].SetActive(true);
            }
            else
            {
                _moveTexts[i].text = "•";
                _movebuttons[i].SetActive(false);
            }
        }
    }



}
