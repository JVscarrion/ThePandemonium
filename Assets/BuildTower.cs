using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildTower : MonoBehaviour
{
    [Header("Prizes")]
    public int prize;
    
    private TextMeshProUGUI prizeText;

    private GameObject buildCanvas;
    private GameObject upgradeTouchCanvas;
    private GameObject rangeCircle;
    private TowerController towerController;

    private Button buildButton;
    private Button destroyBuildButton;

    private BuildController buildController;
    void Start()
    {
        towerController = GetComponent<TowerController>();
        buildController = GameObject.FindWithTag("Manager").transform.GetChild(0).gameObject.GetComponent<BuildController>();
        buildCanvas = transform.GetChild(0).gameObject;
        upgradeTouchCanvas = transform.GetChild(1).gameObject;
        prizeText = buildCanvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        buildCanvas.SetActive(true);
        upgradeTouchCanvas.SetActive(false);
        rangeCircle = transform.GetChild(3).gameObject;
        AssignButtonFunction();
        if (towerController != null )
        {
            towerController.enabled = false;
        }             
    }

    // Update is called once per frame
    void Update()
    {
        if(prizeText != null)
        {
            prizeText.text = "Prize: " + prize;

            if(prize < CoinManager.instance.currentCoins)
            {
               prizeText.color = Color.white;
            }
            else if (prize > CoinManager.instance.currentCoins)
            {
                prizeText.color = Color.red;
            }
        }
    }
    public void Build()
    {
        if (CoinManager.instance.SpendCoins(prize))
        {
            if (towerController != null)
            {
                towerController.enabled = true;
                upgradeTouchCanvas.SetActive(true);
                buildCanvas.SetActive(false);
                gameObject.layer = LayerMask.NameToLayer("Tower");
                if (buildController != null)
                {
                    buildController.spawnedPrefab = null;
                }
                if (rangeCircle != null)
                {
                    rangeCircle.SetActive(false);
                }
            }
        }
    }
    public void DestroyBuild()
    {
        Destroy(gameObject);
        Debug.Log(gameObject);
    }
    public void AssignButtonFunction()
    {
        if(buildCanvas != null )
        {
            buildButton = buildCanvas.transform.GetChild(0).gameObject.GetComponent<Button>();
            destroyBuildButton = buildCanvas.transform.GetChild(1).gameObject.GetComponent<Button>();

            buildButton.onClick.RemoveAllListeners();
            destroyBuildButton.onClick.RemoveAllListeners();

            buildButton.onClick.AddListener(Build);
            destroyBuildButton.onClick.AddListener(DestroyBuild);
        }
    }
}