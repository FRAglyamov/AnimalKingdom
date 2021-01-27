using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalController : MonoBehaviour
{
    public AnimalType animalType;
    public float curHealth;
    public short playerNumber;
    public bool isLevelUped = false;


    //public Canvas canvasPrefab;
    public GameObject canvas;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI damageText;

    //Effect components
    [SerializeField]
    private GameObject smokePuffEffect;
    [SerializeField]
    private GameObject getDamageEffect;
    [SerializeField]
    private GameObject healEffect;
    private GameObject smokePuffPrefab;
    private GameObject getDamagePrefab;
    private GameObject healPrefab;

    void Start()
    {
        smokePuffPrefab = Instantiate(smokePuffEffect, transform);

        // If new unit is Level 1 and don't look at health of two previous units, then set default value
        if (curHealth == 0f)
            curHealth = animalType.maxHP;

        //GameObject canvas = Instantiate(canvasPrefab.gameObject, transform);
        //canvas.name = canvasPrefab.name;
        GameObject unitCanvas = Instantiate(canvas, transform);
        healthText = unitCanvas.transform.Find("Health Object").Find("Health Text").GetComponent<TextMeshProUGUI>();
        damageText = unitCanvas.transform.Find("Attack Object").Find("Attack Text").GetComponent<TextMeshProUGUI>();

        UpdateHealthText();
        UpdateDamageText();
    }

    void Update()
    {
        if (smokePuffPrefab != null)
        {
            if (!smokePuffPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(smokePuffPrefab);
                smokePuffPrefab = null;
            }
        }
        if (getDamagePrefab != null)
        {
            if (!getDamagePrefab.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(getDamagePrefab);
                getDamagePrefab = null;
            }
        }
        if (healPrefab != null)
        {
            if (!healPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(healPrefab);
                healPrefab = null;
            }
        }
    }

    public void Attack(AnimalController animalController)
    {
        if (getDamagePrefab == null)
        {
            animalController.getDamagePrefab = Instantiate(getDamageEffect, animalController.transform);
        }
        //Debug.Log(animalController.getDamagePrefab);
        animalController.curHealth -= animalType.damage;
        animalController.UpdateHealthText();
    }
    public void GetDamage(float damage)
    {
        if (getDamagePrefab == null)
        {
            getDamagePrefab = Instantiate(getDamageEffect, transform);
        }
        curHealth -= damage;
        UpdateHealthText();
    }

    public void GetHeal(float hp)
    {
        if (curHealth != animalType.maxHP)
        {
            if (healPrefab == null)
            {
                healPrefab = Instantiate(healEffect, transform);
            }
            curHealth += 1;
            UpdateHealthText();
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = curHealth.ToString();
    }
    public void UpdateDamageText()
    {
        //damageText.text = curDamage.ToString();
        damageText.text = animalType.damage.ToString();
    }

}
