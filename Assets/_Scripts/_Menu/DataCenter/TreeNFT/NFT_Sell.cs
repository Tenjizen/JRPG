using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFT_Sell : MonoBehaviour
{
    [SerializeField] GameController _GameController;
    public GameObject nFTSelected;
    private NFT_Properties _NFT_Properties;
    [SerializeField] NFT_ActionsDeactivatePlayer _NFT_ActionsDeactivatePlayer;
    [SerializeField] NFT_ActionsDesactivatePartner1 _NFT_ActionsDesactivatePartner1;
    [SerializeField] NFT_ActionsDesactivatePartner2 _NFT_ActionsDesactivatePartner2;
    public int actionId;

    public void OnClickSell()
    {
        if (nFTSelected != null)
        {
            _NFT_Properties = nFTSelected.GetComponent<NFT_Properties>();
            if (_NFT_Properties.nFTBuy == true)
            {
                _NFT_Properties.nFTBuy = false;
                _GameController.mHR += _NFT_Properties.nFTPrice;
                switch (actionId)
                {
                    case 1:
                        _NFT_ActionsDeactivatePlayer.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsDeactivatePlayer.NFTPlayerChoice();
                        break;
                    case 2:
                        _NFT_ActionsDesactivatePartner1.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsDesactivatePartner1.NFTPlayerChoice();
                        break;
                    case 3:
                        _NFT_ActionsDesactivatePartner2.nFTNumber = _NFT_Properties.nFTID;
                        _NFT_ActionsDesactivatePartner2.NFTPlayerChoice();
                        break;
                }
                Debug.Log("Selling");
            }
            else
                Debug.Log("Non activé.");
        }
    }
}
