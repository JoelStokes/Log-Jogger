using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public GameObject[] SkinButtons;

    public SkinLoader storeMole;
    //public GameObject Sparkles;   //Play sparkles over mole every time skin changed

    public TextMeshProUGUI WormCount;
    
    private SaveManager saveManager;

    void Start()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        for (int i=0; i<SkinButtons.Length; i++){
            SetButtonDetails(i);
        }

        SetNewWormCount();
    }

    public void SkinClicked(int index){
        //If owned, change current skin. If not, check price vs current worm count. If enough, subtract worms & buy skin
        if (saveManager.IsSkinOwned(index)){
            saveManager.state.currentSkin = index;
            SetNewSkin();
        } else {
            int cost = int.Parse(SkinButtons[index].transform.Find("Price").gameObject.GetComponent<TextMeshProUGUI>().text);

            if (saveManager.state.wormCount >= cost){
                saveManager.UnlockSkin(index);
                saveManager.state.wormCount -= cost;
                saveManager.state.currentSkin = index;

                saveManager.Save();
                SetNewPurchase(index);
            } else {
                //play error noise
            }
        }
    }

    private void SetButtonDetails(int index){
        //If owned, hide lock & text, set button to white
        if (saveManager.IsSkinOwned(index)){
            SkinButtons[index].transform.Find("Lock").gameObject.SetActive(false);
            SkinButtons[index].transform.Find("Price").gameObject.SetActive(false);

            SkinButtons[index].GetComponent<Image>().color = Color.white;
        }
    }

    private void SetNewSkin(){
        storeMole.UpdateSkin();
    }

    private void SetNewPurchase(int index){
        SetButtonDetails(index);
        SetNewSkin();
        SetNewWormCount();
    }

    private void SetNewWormCount(){
        WormCount.text = "Worms: " + saveManager.state.wormCount.ToString();
    }
}
