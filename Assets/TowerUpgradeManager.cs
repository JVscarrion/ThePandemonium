using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public Button touchButton;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradePrizeText;
    public TextMeshProUGUI sellPrizeText;
    private TowerController towerController;
    private GameObject rangeCircle;
    private SpriteRenderer towerSprite1;
    private SpriteRenderer towerSprite2;
    private SpriteRenderer towerSprite3;
    private int currentLevel;
    private int currentUpgradePrize;
    private int currentsellPrize;
    [System.Serializable]
    public struct TowerUpgradeSprite
    {     
        public Sprite lvl2Sprite1;
        public Sprite lvl2Sprite2;
        public Sprite lvl2Sprite3;
        [Space(20)]
        public Sprite lvl3Sprite1;
        public Sprite lvl3Sprite2;
        public Sprite lvl3Sprite3;
    }
    public TowerUpgradeSprite upgradeSprite;
    void Start()
    {
        upgradeCanvas.SetActive(false);
        currentLevel = 1;
        currentUpgradePrize = CoinManager.instance.upgradeStartPrize;
        currentsellPrize = CoinManager.instance.sellStartPrize;
        levelText.text = "Level: " + currentLevel.ToString();
        upgradePrizeText.text = "Upgrade: " + currentUpgradePrize.ToString();
        sellPrizeText.text = "Sell: " + currentsellPrize.ToString();
        towerController = gameObject.GetComponent<TowerController>();
        rangeCircle = transform.GetChild(3).gameObject;
        towerSprite1 = transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>();
        towerSprite2 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        towerSprite3 = towerSprite1.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Touch);
        }
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(Upgrade);
        }
        if(sellButton != null)
        {
            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(Sell);
        }
    }
    private void Update()
    {
        if (upgradePrizeText != null)
        {
            upgradePrizeText.text = "Upgrade: " + currentUpgradePrize.ToString();

            if (currentUpgradePrize <= CoinManager.instance.currentCoins)
            {
                upgradePrizeText.color = Color.white;
            }
            else if (currentUpgradePrize > CoinManager.instance.currentCoins)
            {
                upgradePrizeText.color = Color.red;
            }
        }
    }
    public void Upgrade()
    {
        if (towerController != null && currentLevel < 3)
        {
            if (CoinManager.instance.SpendCoins(currentUpgradePrize))
            {
                towerController.attackRange += 0.5f;
                towerController.attackRate += 0.5f;
                towerController.damage += 5;

                //towerController.iceVariables.iceSlowRate += 0.05f;

                towerController.fireVariables.damageOverTimeInterval -= 0.1f;
                towerController.fireVariables.fireDamage += 1;

                towerController.stoneVariables.damageOverTimeInterval -= 0.1f;
                towerController.stoneVariables.stoneDamage += 1;

                towerController.RangeCircle();

               

                currentLevel += 1;

                currentUpgradePrize += CoinManager.instance.upgradePlusPrize;
                currentsellPrize += CoinManager.instance.sellPlusPrize;
                upgradePrizeText.text = "Upgrade: " + currentUpgradePrize.ToString();
                sellPrizeText.text = "Sell: " + currentsellPrize.ToString();

                levelText.text = "Level: " + currentLevel.ToString();
                if (currentLevel >= 3)
                {
                    levelText.text = "Max: " + currentLevel.ToString();
                }
                if(currentLevel == 2)
                {
                    towerSprite1.sprite = upgradeSprite.lvl2Sprite1;
                    towerSprite2.sprite = upgradeSprite.lvl2Sprite2;
                    towerSprite3.sprite = upgradeSprite.lvl2Sprite3;
                }
                else if(currentLevel == 3)
                {
                    towerSprite1.sprite = upgradeSprite.lvl3Sprite1;
                    towerSprite2.sprite = upgradeSprite.lvl3Sprite2;
                    towerSprite3.sprite = upgradeSprite.lvl3Sprite3;
                }
            }
            else if (towerController == null)
            {
                towerController = gameObject.GetComponent<TowerController>();
                return;
            }
        }
    }
    public void Sell()
    {
        CoinManager.instance.AddCoins(currentsellPrize);       
        sellPrizeText.text = "Prize: " + currentsellPrize.ToString();
        Destroy(gameObject);
    }
    public void Touch()
    {
        upgradeCanvas.SetActive(true);

        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Close);
        }
        if(rangeCircle != null)
        {
            rangeCircle.SetActive(true);
        }
    }
    public void Close()
    {
        upgradeCanvas.SetActive(false);

        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Touch);
        }
        if (rangeCircle != null)
        {
            rangeCircle.SetActive(false);
        }
    }
}
