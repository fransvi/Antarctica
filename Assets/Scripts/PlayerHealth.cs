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

    public int currentStamina;
    public int currentHunger;
    public int currentThirst;
    public int currentTemperature;

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

    public bool startHealthReduction;
    public bool startStaminaReduction;
    public bool startHungerReduction;
    public bool startThirstReduction;
    public bool startTemperatureReduction;

    public void Start()
    {
        GetComponentInParent<NetworkPlayerSetup>().playerHealth = currentHealth;
        healthBarContent = transform.GetChild(5).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        staminaBarContent = transform.GetChild(5).GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        hungerBarContent = transform.GetChild(5).GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        thirstBarContent = transform.GetChild(5).GetChild(3).GetChild(0).GetChild(0).GetComponent<Image>();
        temperatureBarContent = transform.GetChild(5).GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>();

        currentHealth = (int)(healthBarContent.fillAmount * 100);
        currentStamina = (int)(staminaBarContent.fillAmount * 100);
        currentHunger = (int)(hungerBarContent.fillAmount * 100);
        currentThirst = (int)(thirstBarContent.fillAmount * 100);
        currentTemperature = (int)(temperatureBarContent.fillAmount * 100);

        startHealthReduction = true;
        startStaminaReduction = true;
        startHungerReduction = true;
        startThirstReduction = true;
        startTemperatureReduction = true;

        //allowHealthReduction = true;
        //allowStaminaReduction = true;
        allowHungerReduction = true;
        allowThirstReduction = true;
        allowTemperatureReduction = true;
    }

    public void Update()
    {
        if (startHungerReduction)
        {
            if (allowHungerReduction)
            {
                allowHungerReduction = false;
                StartCoroutine(ReduceHunger(1, 3f));
            }
        }
        else
        {
            if (allowHungerIncrement)
            {
                allowHungerIncrement = false;
                StartCoroutine(IncreaseHunger(1, 1f));
            }
        }

        if (startThirstReduction)
        {
            if (allowThirstReduction)
            {
                allowThirstReduction = false;
                StartCoroutine(ReduceThirst(1, 2f));
            }
        }
        else
        {
            if (allowThirstIncrement)
            {
                allowThirstIncrement = false;
                StartCoroutine(IncreaseThirst(1, 1f));
            }
        }

        if (startTemperatureReduction)
        {
            if (allowTemperatureReduction)
            {
                allowTemperatureReduction = false;
                StartCoroutine(LowerTemperature(1, 1f));
            }
        }
        else
        {
            if (allowTemperatureIncrement)
            {
                allowTemperatureIncrement = false;
                StartCoroutine(IncreaseTemperature(1, 1f));
            }
        }

        if (startStaminaReduction)
        {
            if (allowStaminaReduction)
            {
                allowStaminaReduction = false;
                StartCoroutine(ReduceStamina(1, 1f));
            }
        }
        else
        {
            if (allowStaminaIncrement)
            {
                allowStaminaIncrement = false;
                StartCoroutine(IncreaseStamina(1, 1f));
            }
        }

        if (startHealthReduction)
        {
            if (allowHealthReduction)
            {
                allowHealthReduction = false;
                StartCoroutine(ReduceHealth(1, 1f));
            }
        }
        else
        {
            if (allowHealthIncrement)
            {
                allowHealthIncrement = false;
                StartCoroutine(IncreaseHealth(1, 1f));
            }
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
        currentHealth -= amount;
        healthBarContent.fillAmount -= (amount / 100f);
        GetComponentInParent<NetworkPlayerSetup>().playerHealth = currentHealth;
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
        if (currentHunger > 0)
        {
            currentHunger -= amount;
            hungerBarContent.fillAmount -= (amount / 100f);
        }
        else StartCoroutine(ReduceStamina(2, 1f));

        allowHungerReduction = true;
    }

    IEnumerator IncreaseThirst(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentThirst += amount;
    }

    IEnumerator ReduceThirst(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        if (currentThirst > 0)
        {
            currentThirst -= amount;
            thirstBarContent.fillAmount -= (amount / 100f);
        }
        else StartCoroutine(ReduceStamina(1, 1f));

        allowThirstReduction = true;
    }

    IEnumerator IncreaseStamina(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentStamina += amount;
    }

    IEnumerator ReduceStamina(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        if (currentStamina > 0)
        {
            currentStamina -= amount;
            staminaBarContent.fillAmount -= (amount / 100f);
        }
        else StartCoroutine(ReduceHealth(1, 1f));

        allowStaminaReduction = true;
    }

    IEnumerator IncreaseTemperature(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        currentTemperature -= amount;
    }

    IEnumerator LowerTemperature(int amount, float frequency)
    {
        yield return new WaitForSeconds(frequency);
        if (currentTemperature < 100)
        {
            currentTemperature += amount;
            temperatureBarContent.fillAmount += (amount / 100f);
        }
        if (currentTemperature >= 100)
        {
            StartCoroutine(ReduceStamina(5, 1f));
        }
        else if (currentTemperature >= 80)
        {
            StartCoroutine(ReduceStamina(4, 1f));
        }
        else if (currentTemperature >= 60)
        {
            StartCoroutine(ReduceStamina(3, 1f));
        }
        else if (currentTemperature >= 40)
        {
            StartCoroutine(ReduceStamina(2, 1f));
        }
        else if (currentTemperature >= 20)
        {
            StartCoroutine(ReduceStamina(1, 1f));
        }

        allowTemperatureReduction = true;
    }
}