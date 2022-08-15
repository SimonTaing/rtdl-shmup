using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiHpBar : MonoBehaviour
{
    [SerializeField] Text hpDisplay;
    public Image hpBarMid;
    public Image hpBarFront;
    [SerializeField] Image hpBarFlash;
    [SerializeField] bool critHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI(string changeType, float hp, float hpMax)
    {
        //Update UI
        hpDisplay.text = hp + "/" + hpMax;

        float fillAmountValue = hp / hpMax;

        if (changeType == "Hurt")
        {
            hpBarFront.fillAmount = fillAmountValue;
            hpBarFlash.fillAmount = fillAmountValue;

            hpBarFlash.DOColor(new Color(1, 1, 1, 1), 0.1f).OnComplete(()=>{
                hpBarFlash.DOColor(new Color(1, 1, 1, 0), 0.1f).OnComplete(()=> {
                    hpBarMid.DOFillAmount(fillAmountValue, 0.5f).SetDelay(1);
                });
            });

        }
        else if (changeType == "Heal")
        {
            hpBarMid.fillAmount = fillAmountValue;

            hpBarFlash.DOColor(new Color(1, 1, 1, 1), 0.1f).OnComplete(() => {
                hpBarFlash.DOColor(new Color(1, 1, 1, 0), 0.1f).OnComplete(() => {
                    hpBarFront.DOFillAmount(fillAmountValue, 0.5f).SetDelay(1);
                    hpBarFlash.fillAmount = fillAmountValue;
                });
            });
        }

        //If hp is over 1/2.5
        if (hp > hpMax / 2.5)
        {
            //Change to blue
            Color blue = new Color(0.3716981f, 0.9606702f, 1, 1);
            hpBarFront.DOColor(blue, 0.1f);
            critHealth = false;
        }
        //If hp is below 1/2.5
        else if (hp < hpMax / 2.5)
        {
            //Change to red
            Color red = new Color(1, 0.07924521f, 0.1927032f, 1);
            hpBarFront.DOColor(red, 0.1f);

            critHealth = true;
            CritHealthFlash();
        }

        void CritHealthFlash()
        {
            if (critHealth)
            {
                hpBarFlash.DOColor(new Color(1, 1, 1, 1), 0f).SetDelay(0.5f).OnComplete(() => {
                    hpBarFlash.DOColor(new Color(1, 1, 1, 0), 0f).SetDelay(0.1f).OnComplete(() => {
                        CritHealthFlash();
                    });
                });
            }
        }
    }
}
