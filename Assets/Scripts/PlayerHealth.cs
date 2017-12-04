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

    public float healthIncreaseCooldown;
    public float staminaIncreaseCooldown;
    public float hungerIncreaseCooldown;
    public float thirstIncreaseCooldown;
    public float temperatureIncreaseCooldown;

    public float healthReductionCooldown;
    public float staminaReductionCooldown;
    public float hungerReductionCooldown;
    public float thirstReductionCooldown;
    public float temperatureReductionCooldown;

    public bool startHealthIncrement;
    public bool startStaminaIncrement;
    public bool startHungerIncrement;
    public bool startThirstIncrement;
    public bool startTemperatureIncrement;

    public bool startHealthReduction;
    public bool startStaminaReduction;
    public bool startHungerReduction;
    public bool startThirstReduction;
    public bool startTemperatureReduction;

    private AudioSource audioSource;
    public AudioClip eating;

    public void Start()
    {
        SetInitialReferences();
        audioSource = GetComponent<AudioSource>();
    }

    void SetInitialReferences()
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

        startHealthIncrement = false;
        startStaminaIncrement = false;
        startHungerIncrement = false;
        startThirstIncrement = false;
        startTemperatureIncrement = false;

        startHealthReduction = false;
        startStaminaReduction = false;
        startHungerReduction = true;
        startThirstReduction = true;
        startTemperatureReduction = true;

        healthIncreaseCooldown = 0f;
        staminaIncreaseCooldown = 0f;
        hungerIncreaseCooldown = 0f;
        thirstIncreaseCooldown = 0f;
        temperatureIncreaseCooldown = 0f;

        healthReductionCooldown = 0f;
        staminaReductionCooldown = 0f;
        hungerReductionCooldown = 0f;
        thirstReductionCooldown = 0f;
        temperatureReductionCooldown = 0f;
    }

    public void FixedUpdate()
    {
        IncrmentOrDecreaseStatsOverTime();
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

    // Controls wether to reduce or increment health, stamina, hunger, thirst and temperature over time
    public void IncrmentOrDecreaseStatsOverTime()
    {
        if (startHungerReduction)
        {
            if (hungerReductionCooldown <= Time.time)
            {
                ReduceHunger(1, 3f);
            }
        }
        else if (startHungerIncrement)
        {
            if (hungerIncreaseCooldown <= Time.time)
            {
                IncreaseHunger(1, 1f);
            }
        }

        if (startThirstReduction)
        {
            if (thirstReductionCooldown <= Time.time)
            {
                ReduceThirst(1, 2f);
            }
        }
        else if (startThirstIncrement)
        {
            if (thirstIncreaseCooldown <= Time.time)
            {
                IncreaseThirst(1, 1f);
            }
        }

        if (startTemperatureReduction)
        {
            if (temperatureReductionCooldown <= Time.time)
            {
                LowerTemperature(1, 1f);
            }
        }
        else if (startTemperatureIncrement)
        {
            if (temperatureIncreaseCooldown <= Time.time)
            {
                IncreaseTemperature(1, 1f);
            }
        }

        if (startStaminaReduction)
        {
            if (staminaReductionCooldown <= Time.time)
            {
                ReduceStamina(1, 1f);
            }
        }
        else if (startStaminaIncrement)
        {
            if (staminaIncreaseCooldown <= Time.time)
            {
                IncreaseStamina(1, 1f);
            }
        }

        if (startHealthReduction)
        {
            if (healthReductionCooldown <= Time.time)
            {
                ReduceHealth(1, 1f);
            }
        }
        else if (startHealthIncrement)
        {
            if (healthIncreaseCooldown <= Time.time)
            {
                IncreaseHealth(1, 1f);
            }
        }
    }

    // increase health over time
    void IncreaseHealth(int amount, float frequency)
    {
        currentHealth += amount;

        healthIncreaseCooldown = Time.time + frequency;
    }

    // reduce health over time
    void ReduceHealth(int amount, float frequency)
    {
        //Debug.Log("reducing hp by: " + amount);
        if (currentHealth > 0)
        {
            currentHealth -= amount;
            healthBarContent.fillAmount -= (amount / 100f);
            GetComponentInParent<NetworkPlayerSetup>().playerHealth = currentHealth;

            healthReductionCooldown = Time.time + frequency;
        }
        else KnockDownState();
    }

    void KnockDownState()
    {
        Debug.Log("KnockdownState");
    }

    // increase hunger over time (or the lack of it, increase is actually good with this stat
   public void IncreaseHunger(int amount, float frequency)
    {
        currentHunger += amount;

        hungerIncreaseCooldown = Time.time + frequency;
    }

    // reduce hunger over time (except for temperature bar, when bar fillAmount is reduced its bad for the player)
    void ReduceHunger(int amount, float frequency)
    {
        if (currentHunger > 0)
        {
            currentHunger -= amount;
            hungerBarContent.fillAmount -= (amount / 100f);
        }
        else ReduceStamina(2, frequency);

        hungerReductionCooldown = Time.time + frequency;
    }

    // increase thirst over time
    void IncreaseThirst(int amount, float frequency)
    {
        currentThirst += amount;

        thirstIncreaseCooldown = Time.time + frequency;
    }

    // reduce thirst over time
    void ReduceThirst(int amount, float frequency)
    {
        if (currentThirst > 0)
        {
            currentThirst -= amount;
            thirstBarContent.fillAmount -= (amount / 100f);
        }
        else ReduceStamina(1, frequency);

        thirstReductionCooldown = Time.time + frequency;
    }

    // increase stamina over time
    void IncreaseStamina(int amount, float frequency)
    {
        currentStamina += amount;

        staminaIncreaseCooldown = Time.time + frequency;
    }

    // reduce stamina over time
    void ReduceStamina(int amount, float frequency)
    {
        if (currentStamina > 0)
        {
            currentStamina -= amount;
            staminaBarContent.fillAmount -= (amount / 100f);
        }
        else {
            currentStamina = 0;
            staminaBarContent.fillAmount = 0;
            ReduceHealth(1, frequency);
        }

        staminaReductionCooldown = Time.time + frequency;
    }

    // increase temperature over time
    void IncreaseTemperature(int amount, float frequency)
    {
        currentTemperature -= amount;

        temperatureIncreaseCooldown = Time.time + frequency;
    }

    // decrease temperature over time
    void LowerTemperature(int amount, float frequency)
    {
        if (currentTemperature < 100)
        {
            currentTemperature += amount;
            temperatureBarContent.fillAmount += (amount / 100f);
        }
        if (currentTemperature >= 100)
        {
            ReduceStamina(5, frequency);
        }
        else if (currentTemperature >= 80)
        {
            ReduceStamina(4, frequency);
        }
        else if (currentTemperature >= 60)
        {
            ReduceStamina(3, frequency);
        }
        else if (currentTemperature >= 40)
        {
            ReduceStamina(2, frequency);
        }
        else if (currentTemperature >= 20)
        {
            ReduceStamina(1, frequency);
        }

        temperatureReductionCooldown = Time.time + frequency;
    }

    // instantly increase health
   public void InstantlyIncreaseHealth(int amount)
    {
        currentHealth += amount;
        healthBarContent.fillAmount += (amount / 100f);
    }

    // instantly reduce health
    public void InstantlyReduceHealth(int amount)
    {
        currentHealth -= amount;
        healthBarContent.fillAmount -= (amount / 100f);
    }

    public void InstantlyIncreaseHunger(int amount)
    {
        currentHunger += amount;
        hungerBarContent.fillAmount += (amount / 100f);
        audioSource.PlayOneShot(eating);
    }
    public void InstantlyIncreaseThirst(int amount)
    {
        currentThirst += amount;
        thirstBarContent.fillAmount += (amount / 100f);
        //audioSource.PlayOneShot(eating);
    }

    // instantly increase stamina
    void InstantlyIncreaseStamina(int amount)
    {
        currentStamina += amount;
        staminaBarContent.fillAmount += (amount / 100f);
    }

    // instantly reduce stamina
    void InstantlyReduceStamina(int amount)
    {
        currentStamina -= amount;
        staminaBarContent.fillAmount -= (amount / 100f);
    }

    // instantly increase temperature
    void InstantlyIncreaseTemperature(int amount)
    {
        currentTemperature += amount;
        temperatureBarContent.fillAmount += (amount / 100f);
    }

    // instantly reduce temperature
    void InstantlyReduceTemperature(int amount)
    {
        currentTemperature -= amount;
        temperatureBarContent.fillAmount -= (amount / 100f);
    }
}