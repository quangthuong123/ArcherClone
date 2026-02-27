using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private MobilePlayer parentScript;

    void Awake()
    {
        // Automatically find your main script on the Player object
        parentScript = GetComponentInParent<MobilePlayer>();
    }

    // The Animator calls this function
    public void SpawnArrow()
    {
        if (parentScript != null) parentScript.SpawnArrow();
    }
    public void PerformAttack()
    {
        // Safely find the EnemyAI script on the parent object
        EnemyAI aiScript = GetComponentInParent<EnemyAI>();

        if (aiScript != null)
        {
            // Send the message DIRECTLY to the AI's object, skipping this object entirely!
            aiScript.gameObject.SendMessage("PerformAttack", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Add these empty functions to mute the asset pack's leftover sound events
    public void FootL() { }
    public void FootR() { }
    public void Hit() { }
}
