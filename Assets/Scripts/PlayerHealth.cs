using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public Image healthBarContent;
    public Image staminaBarContent;
    public Image hungerBarContent;
    public Image thirstBarContent;
    public Image temperatureBarContent;

    public const int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    public int currentStamina = 100;
    public int currentHunger = 50;
    public int currentThirst = 70;
    public int currentTemperature = 100;

    public bool allowHealthReduction;
    public bool allowStaminaReduction;
    public bool allowHungerReduction;
    public bool allowThirstReduction;
    public bool allowTemperatureReduction;

    public bool allowHealthIncrement;
    public bool allowStaminaIncrement;
    public bool allowHungerIncrement;
    public bool allowThirstIncrement;
    public bool allowTemperatureIncrement;

    public void Start()
    {
        healthBarContent = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        staminaBarContent = transform.GetChild(5).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        hungerBarContent = transform.GetChild(5).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        thirstBarContent = transform.GetChild(5).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>();
        temperatureBarContent = transform.GetChild(5).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>();

        allowHealthReduction = true;
        allowStaminaReduction = true;
        allowHungerReduction = true;
        allowThirstReduction = true;
        allowTemperatureReduction = true;

        allowHealthIncrement = true;
        allowStaminaIncrement = true;
        allowHungerIncrement = true;
        allowThirstIncrement = true;
        allowTemperatureIncrement = true;
    }

    public void Update()
    {
        //healthBarContent.fillAmount = (float)currentHealth / 100f;
        //staminaBarContent.fillAmount = (float)currentStamina / 100f;
        //hungerBarContent.fillAmount = (float)currentHunger / 100f;
        //thirstBarContent.fillAmount = (float)currentThirst / 100f;
        //temperatureBarContent.fillAmount = (float)currentTemperature / 100f;

        if (allowHealthReduction)
        {
            allowHealthReduction = false;
            StartCoroutine(ReduceHealth(1, 1f));
        }

    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
    }

    void OnChangeHealth(int health)
    {
        currentHealth = health;
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);

    }

    IEnumerator IncreaseHealth(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentHealth += amount;
    }

    IEnumerator ReduceHealth(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        //Debug.Log("reducing health...");
        currentHealth -= amount;
        healthBarContent.fillAmount -= 0.01f;
        allowHealthReduction = true;
    }

    IEnumerator IncreaseHunger(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentHunger += amount;
    }

    IEnumerator ReduceHunger(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentHunger -= amount;
    }

    IEnumerator IncreaseThirst(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentThirst += amount;
    }

    IEnumerator ReduceThirst(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentThirst -= amount;
    }

    IEnumerator IncreaseStamina(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentStamina += amount;
    }

    IEnumerator ReduceStamina(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentStamina -= amount;
    }

    IEnumerator IncreaseTemperature(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentTemperature += amount;
    }

    IEnumerator LowerTemperature(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentTemperature -= amount;
    }

}