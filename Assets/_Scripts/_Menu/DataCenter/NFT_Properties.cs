using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT_Properties : MonoBehaviour
{
    //Apparence
    [SerializeField] Sprite _nFTSpriteLock;
    [SerializeField] Sprite _nFTSpriteUnlock;
    [SerializeField] Sprite _nFTSpriteSelected;
    [SerializeField] Image _nFTSprite;

    //Texte propre au NFT
    [SerializeField] TextMeshProUGUI _nFTTextName;
    [SerializeField] TextMeshProUGUI _nFTTextDesc;

    //Texte modifi�
    [SerializeField] TextMeshProUGUI _nFTModifTextName;
    [SerializeField] TextMeshProUGUI _nFTModifTextDesc;

    //NFT count
    [SerializeField] GameObject _nFTCountTrading;

    //Nombre
    public List<GameObject> NFTCount;

    /*private void Start()
    {
        int NbNFTCount = NFTCount.Count;
        switch(NbNFTCount)
        {
            case 1:
                if (tag == "NFTPower")
                {

                }
                break;
            case 3:

                break;
        }
    }*/

    public void OnClickNFT()
    {
        _nFTSprite.sprite = _nFTSpriteSelected;
        _nFTModifTextName.text = _nFTTextName.text;
        _nFTModifTextDesc.text = _nFTTextDesc.text;
        _nFTCountTrading.SetActive(true);
    }
}