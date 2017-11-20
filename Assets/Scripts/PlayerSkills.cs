using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour {
    public PlayerController player;
    [Header("Speed Boost")]
    public int speedToAdd;
    public float speedDuration;
    public float ManaCostSB;
    public float cooldownSB;
    public float cdsb;
    [Header("Self Heal")]
    public int healthOverTime;
    public float healDuration;
    public float ManaCostSH;
    public float cooldownSH;
    public float cdsh;
    class Ability
    {
        public IEnumerator SpeedBoost(PlayerController player, int Amount,float Duration, float ManaCost)
        {
            player.localPlayerData.currentEnergy -= ManaCost;
            player.localPlayerData.moveSpeed += Amount;
            yield return new WaitForSeconds(Duration);
            player.localPlayerData.moveSpeed -= Amount;
        }
        public IEnumerator SelfHeal(PlayerController player, int Amount,float Duration,float ManaCost)
        {
            player.localPlayerData.currentEnergy -= ManaCost;
            float dur = Duration;
            while (dur > 0)
            {
                dur -= 1;
                player.localPlayerData.currentHealth += Amount;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    private void Start()
    {
        cdsh = cooldownSH;
        cdsb = cooldownSB;
        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        cdsb -= Time.deltaTime;
        cdsh -= Time.deltaTime;
        Ability Ability = new Ability();
        if (Input.GetKeyDown(KeyCode.Alpha1) && (player.localPlayerData.currentEnergy >= ManaCostSB))
        {
            cdsb = cooldownSB;
            StartCoroutine(Ability.SpeedBoost(player, speedToAdd, speedDuration, ManaCostSB));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && (player.localPlayerData.currentEnergy >= ManaCostSH))
        {
            cdsh = cooldownSH;
            StartCoroutine(Ability.SelfHeal(player, healthOverTime, healDuration, ManaCostSH));
        }
    }
}
