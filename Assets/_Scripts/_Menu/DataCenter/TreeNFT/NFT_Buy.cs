using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_Buy : MonoBehaviour
{
    [SerializeField] GameController _GameController;
    public GameObject nFTSelected;
    private NFT_Properties _NFT_Properties;
    [SerializeField] NFT_ActionsActivatePlayer _NFT_ActionsActivatePlayer;
    [SerializeField] NFT_ActionsActivatePartner1 _NFT_ActionsActivatePartner1;
    [SerializeField] NFT_ActionsActivatePartner2 _NFT_ActionsActivatePartner2;
    public int actionId;

    public void OnClickBuy()
    {
        if (nFTSelected != null)
        {
            _NFT_Properties = nFTSelected.GetComponent<NFT_Properties>();
            if (_GameController.mHR >= _NFT_Properties.nFTPrice && _NFT_Properties.nFTBuy == false)
            {
                _NFT_Properties.nFTBuy = true;
                _GameController.mHR -= _NFT_Properties.nFTPrice;
                switch (actionId)
                {
                    case 1:
                        _NFT_ActionsActivatePlayer.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsActivatePlayer.NFTPlayerChoice();
                        break;
                    case 2:
                        _NFT_ActionsActivatePartner1.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsActivatePartner1.NFTPlayerChoice();
                        break;
                    case 3:
                        _NFT_ActionsActivatePartner2.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsActivatePartner2.NFTPlayerChoice();
                        break;
                }
                Debug.Log("Buying");
            }
            else
                Debug.Log("Pas assez de sous ou NFT déjà activé.");
        }
    }
}