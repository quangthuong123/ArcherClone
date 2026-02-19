using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject levelUpPanel;
    public Button[] optionButtons;
    public TMP_Text[] optionTexts;

    [Header("Data")]
    public UpgradeData[] allUpgrades; // Drag your ScriptableObjects here

    private PlayerStats playerStats;
    private MobilePlayer playerMovement; // Reference to change speed

    void Start()
    {
        levelUpPanel.SetActive(false);

        // Find the player scripts automatically
        playerStats = FindFirstObjectByType<PlayerStats>();
        playerMovement = FindFirstObjectByType<MobilePlayer>();
    }

    public void ShowLevelUpOptions()
    {
        // 1. Pause the game
        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        // --- NEW LOGIC START ---

        // Create a temporary copy of your "Deck" so we can remove cards from it
        System.Collections.Generic.List<UpgradeData> deck = new System.Collections.Generic.List<UpgradeData>(allUpgrades);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            // Safety Check: Do we have enough upgrades left in the deck?
            if (deck.Count == 0)
            {
                optionButtons[i].gameObject.SetActive(false); // Hide button if no upgrades left
                continue;
            }

            // Turn button on just in case it was off
            optionButtons[i].gameObject.SetActive(true);

            // Pick a random index from the REMAINING cards
            int randomIndex = Random.Range(0, deck.Count);
            UpgradeData data = deck[randomIndex];

            // REMOVE it from the deck so it can't be picked again this round
            deck.RemoveAt(randomIndex);

            // --- NEW LOGIC END ---

            // Setup the text
            if (optionTexts[i] != null)
                optionTexts[i].text = "<b>" + data.upgradeName + "</b>\n" + data.description;

            // Click Event
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectUpgrade(data));
        }
    }

    void SelectUpgrade(UpgradeData upgrade)
    {
        ApplyStats(upgrade);

        // Resume
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }

    void ApplyStats(UpgradeData data)
    {
        Debug.Log("Selected: " + data.upgradeName);

        switch (data.statType)
        {
            // --- STATS ---
            case StatType.MaxHealth:
                playerStats.maxHealth += (int)data.amount;
                playerStats.Heal((int)data.amount);
                break;

            case StatType.Damage:
                playerStats.damage += (int)data.amount;
                break;

            case StatType.MoveSpeed:
                if (playerMovement != null)
                {
                    playerMovement.moveSpeed += data.amount;
                }
                break;

            // --- WEAPONS ---
            case StatType.Multishot:
                playerStats.multishotCount++;
                break;

            case StatType.Ricochet:
                playerStats.hasRicochet = true;
                break;

            case StatType.FrontArrow:
                // Logic for front arrow if you add it later
                break;
        }
    }
}