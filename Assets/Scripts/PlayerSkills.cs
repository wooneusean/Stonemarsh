using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [System.Serializable]
    public struct SkillProperties
    {
        public PlayerController player;
        public int amount;
        public float duration;
        public float manacost;
        public float timeToCooldown;
        public float currentCooldown;
    }
    public enum Skills { None, Heal, SpeedBoost };
    public Skills skills;
    public SkillProperties skillProperties = new SkillProperties();

    private void Start()
    {
        if (!skillProperties.player)
        {
            skillProperties.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }
    private void Update()
    {
        skillProperties.currentCooldown -= Time.deltaTime;
    }
    public void CastAbility()
    {
        switch (skills)
        {
            case Skills.None:
                Debug.Log("Casting nothing...");
                break;
            case Skills.Heal:
                StartCoroutine(Heal(skillProperties));
                //StartCoroutine(Heal(skillProperties.player, 1 * SkillLevel,1 * SkillLevel,10 + Mathf.RoundToInt(SkillLevel / 10)));
                break;
            case Skills.SpeedBoost:
                StartCoroutine(SpeedBoost(skillProperties));
                //StartCoroutine(SpeedBoost(skillProperties.player, 1 * SkillLevel,1 * SkillLevel,10 + Mathf.RoundToInt(SkillLevel / 10)));
                break;
        }
        skillProperties.currentCooldown = skillProperties.timeToCooldown;
    }
    public IEnumerator Heal(SkillProperties sp)
    {
        sp.player.localPlayerData.currentEnergy -= sp.manacost;
        float dur = sp.duration;
        while (dur > 0)
        {
            dur -= 1;
            sp.player.localPlayerData.currentHealth += sp.amount;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public IEnumerator SpeedBoost(SkillProperties sp)
    {
        sp.player.localPlayerData.currentEnergy -= sp.manacost;
        sp.player.localPlayerData.moveSpeed += sp.amount;
        yield return new WaitForSeconds(sp.duration);
        sp.player.localPlayerData.moveSpeed -= sp.amount;
    }
}
