using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomRPGSystem
{
    [Serializable]
    public class PlayerCharacterData
    {
        [Serializable]
        public class CharacterInfo
        {
            public string id = "";
            public string name = "";
            public enum Race
            {
                None,
                Dragonborn,
                Hill_Dwarf,
                Mountain_Dwarf,
                High_Elf,
                Wood_Elf,
                Shadow_Elf,
                Forest_Gnome,
                Rock_Gnome,
                Half_Elf,
                Half_Orc,
                Lightfoot_Halfling,
                Stout_Halfling,
                Human,
                Tiefling,
            };
            public Race race = Race.None;
            public enum Class
            {
                None,
                Barbarian,
                Bard,
                Cleric,
                Druid,
                Fighter,
                Monk,
                Paladin,
                Ranger,
                Rogue,
                Sorcerer,
                Warlock,
                Wizard
            };
            public Class classes = Class.None;

            [Range(0, 18)] public int proficiencyPoints = 0;
            public int proficiencyBonus = 0;
            public int abilityPoints = 0;
            [Range(1, 20)] public int level = 1;
        }

        [Serializable]
        public class AbilityScore
        {
            public enum Ability
            {
                None,
                Strenght,
                Dexterity,
                Constitution,
                Intelligence,
                Wisdom,
                Charisma
            }
            public Ability ability = Ability.None;

            public int score = 0;
            public int modifier = 0;
            public bool savingThrows = false;
        }

        [Serializable]
        public class Skills
        {
            public AbilityScore.Ability abilityModifier = AbilityScore.Ability.None;

            public enum Skill
            {
                Acrobatics,
                Animal_Handling,
                Arcana,
                Athletics,
                Deception,
                History,
                Insight,
                Intimidation,
                Investigation,
                Medicine,
                Nature,
                Perception,
                Performance,
                Persuasion,
                Religion,
                Sleight_Of_Hand,
                Stealth,
                Survival
            }
            public Skill skill;

            public bool proficient = false;
            public int bonus = 0;
        }

        [HideInInspector] public CharacterInfo info = new CharacterInfo();
        [HideInInspector] public AbilityScore[] abilityScore;
        [HideInInspector] public Skills[] skills;

        public PlayerCharacterData(string p_characterName, int p_level, CharacterInfo.Race p_race,CharacterInfo.Class p_class)
        {
            CharacterInfo m_info = new CharacterInfo();
            List<AbilityScore> m_abilityScore = new List<AbilityScore>();
            List<Skills> m_skills = new List<Skills>();

            m_info.id = IdHelper.GenerateIdD();
            m_info.name = p_characterName;
            m_info.level = p_level + 1;
            m_info.race = p_race;
            m_info.classes = p_class;
            m_info.abilityPoints = 84;
            m_info.proficiencyBonus = SetProficiencyBonus(m_info.level);

            this.info = m_info;

            for (int i = 1; i < 7; i++)
            {
                m_abilityScore.Add(new AbilityScore());
                m_abilityScore[i-1].ability = (AbilityScore.Ability)i;
            }

            this.abilityScore = m_abilityScore.ToArray();

            for (int i = 0; i < 18; i++)
            {
                m_skills.Add(new Skills());
                m_skills[i].skill = (Skills.Skill)i;

                switch (m_skills[i].skill)
                {
                    case Skills.Skill.Acrobatics:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Animal_Handling:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Arcana:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Athletics:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Strenght;
                        break;
                    case Skills.Skill.Deception:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.History:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Insight:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Intimidation:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Investigation:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Medicine:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Nature:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Perception:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Performance:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Persuasion:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Religion:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Sleight_Of_Hand:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Stealth:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Survival:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                }
            }

            this.skills = m_skills.ToArray();

            SetRace(this);
            SetClass(this);
        }

        public void SetRace(PlayerCharacterData player)
        {
            switch (player.info.race)
            {
                case CharacterInfo.Race.None:
                    player.info.proficiencyPoints = 0;
                    break;
                case CharacterInfo.Race.Dragonborn:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    break;
                case CharacterInfo.Race.Hill_Dwarf:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 2);
                    break;
                case CharacterInfo.Race.Mountain_Dwarf:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    break;
                case CharacterInfo.Race.High_Elf:
                    player.info.proficiencyPoints = 1;
                    SetSkills(player, Skills.Skill.Perception, true);
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    break;
                case CharacterInfo.Race.Wood_Elf:
                    player.info.proficiencyPoints = 1;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 1);
                    SetSkills(player, Skills.Skill.Perception, true);
                    break;
                case CharacterInfo.Race.Shadow_Elf:
                    player.info.proficiencyPoints = 1;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    SetSkills(player, Skills.Skill.Perception, true);
                    break;
                case CharacterInfo.Race.Forest_Gnome:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 1);
                    SetSkills(player, Skills.Skill.Perception, true);
                    break;
                case CharacterInfo.Race.Rock_Gnome:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    SetSkills(player, Skills.Skill.Perception, true);
                    break;
                case CharacterInfo.Race.Half_Elf:
                    player.info.proficiencyPoints = 2;
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    break;
                case CharacterInfo.Race.Half_Orc:
                    player.info.proficiencyPoints = 1;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    SetSkills(player, Skills.Skill.Intimidation, true);
                    break;
                case CharacterInfo.Race.Lightfoot_Halfling:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    break;
                case CharacterInfo.Race.Stout_Halfling:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    break;
                case CharacterInfo.Race.Human:
                    player.info.proficiencyPoints = 1;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    break;
                case CharacterInfo.Race.Tiefling:
                    player.info.proficiencyPoints = 0;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 2);
                    break;
            }
        }
        public void SetClass(PlayerCharacterData player)
        {
            switch (player.info.classes)
            {
                case CharacterInfo.Class.None:
                    player.info.proficiencyPoints = 0;
                    break;
                case CharacterInfo.Class.Barbarian:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);
                    break;
                case CharacterInfo.Class.Bard:
                    player.info.proficiencyPoints = 3;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    break;
                case CharacterInfo.Class.Cleric:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    break;
                case CharacterInfo.Class.Druid:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    break;
                case CharacterInfo.Class.Fighter:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);
                    break;
                case CharacterInfo.Class.Monk:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    break;
                case CharacterInfo.Class.Paladin:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    break;
                case CharacterInfo.Class.Ranger:
                    player.info.proficiencyPoints = 3;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    break;
                case CharacterInfo.Class.Rogue:
                    player.info.proficiencyPoints = 4;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    break;
                case CharacterInfo.Class.Sorcerer:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    break;
                case CharacterInfo.Class.Warlock:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    break;
                case CharacterInfo.Class.Wizard:
                    player.info.proficiencyPoints = 2;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    break;
            }
        }

        public void SetSkills(PlayerCharacterData player,Skills.Skill skill, bool p_proficient)
        {
            List<Skills> s = new List<Skills>();

            foreach (Skills skl in player.skills)
            {
                s.Add(skl);
            }

            Skills sk = s.Find(x => x.skill == skill);

            sk.proficient = p_proficient;
            player.info.proficiencyPoints--;

            for (int i = 0; i < player.skills.Length; i++)
            {
                if (player.skills[i] == sk)
                {
                    player.skills[i] = sk;
                }
            }
        }
        public void SetAbilityScore(PlayerCharacterData player, AbilityScore.Ability ability, int p_score)
        {
            List<AbilityScore> a = new List<AbilityScore>();

            foreach (AbilityScore aScore in player.abilityScore)
            {
                a.Add(aScore);
            }

            AbilityScore ab = a.Find(x => x.ability == ability);

            ab.score = p_score;
            ab.modifier = SetAbilityModifier(ab.score);

            for (int i = 0; i < player.abilityScore.Length; i++)
            {
                if (player.abilityScore[i] == ab)
                {
                    player.abilityScore[i] = ab;
                }
            }
        }
        public void SetAbilitySavingThrow(PlayerCharacterData player, AbilityScore.Ability ability, bool p_savingThrow = false)
        {
            List<AbilityScore> a = new List<AbilityScore>();

            foreach (AbilityScore aScore in player.abilityScore)
            {
                a.Add(aScore);
            }

            AbilityScore ab = a.Find(x => x.ability == ability);

            ab.savingThrows = p_savingThrow;

            for (int i = 0; i < player.abilityScore.Length; i++)
            {
                if (player.abilityScore[i] == ab)
                {
                    player.abilityScore[i] = ab;
                }
            }
        }
        private int SetAbilityModifier(int value)
        {
            int modifierValue = 0;

            switch (value)
            {
                case 1:
                    modifierValue = -5;
                    break;
                case 2 :
                    modifierValue = -4;
                    break;
                case 3:
                    modifierValue = -4;
                    break;
                case 4:
                    modifierValue = -3;
                    break;
                case 5:
                    modifierValue = -3;
                    break;
                case 6:
                    modifierValue = -2;
                    break;
                case 7:
                    modifierValue = -2;
                    break;
                case 8:
                    modifierValue = -1;
                    break;
                case 9:
                    modifierValue = -1;
                    break;
                case 10:
                    modifierValue = 0;
                    break;
                case 11:
                    modifierValue = 0;
                    break;
                case 12:
                    modifierValue = 1;
                    break;
                case 13:
                    modifierValue = 1;
                    break;
                case 14:
                    modifierValue = 2;
                    break;
                case 15:
                    modifierValue = 2;
                    break;
                case 16:
                    modifierValue = 3;
                    break;
                case 17:
                    modifierValue = 3;
                    break;
                case 18:
                    modifierValue = 4;
                    break;
                case 19:
                    modifierValue = 4;
                    break;
                case 20:
                    modifierValue = 5;
                    break;
                case 21:
                    modifierValue = 5;
                    break;
                case 22:
                    modifierValue = 6;
                    break;
                case 23:
                    modifierValue = 6;
                    break;
                case 24:
                    modifierValue = 7;
                    break;
                case 25:
                    modifierValue = 7;
                    break;
                case 26:
                    modifierValue = 8;
                    break;
                case 27:
                    modifierValue = 8;
                    break;
                case 28:
                    modifierValue = 9;
                    break;
                case 29:
                    modifierValue = 9;
                    break;
                case 30:
                    modifierValue = 10;
                    break;
            }

            if (value > 30) modifierValue = 10;

            return modifierValue;
        }
        private int SetProficiencyBonus(int value)
        {
            int bonus = 0;

            switch (value)
            {
                case 1:
                    bonus = 2;
                    break;
                case 2:
                    bonus = 2;
                    break;
                case 3:
                    bonus = 2;
                    break;
                case 4:
                    bonus = 2;
                    break;
                case 5:
                    bonus = 3;
                    break;
                case 6:
                    bonus = 3;
                    break;
                case 7:
                    bonus = 3;
                    break;
                case 8:
                    bonus = 3;
                    break;
                case 9:
                    bonus = 4;
                    break;
                case 10:
                    bonus = 4;
                    break;
                case 11:
                    bonus = 4;
                    break;
                case 12:
                    bonus = 4;
                    break;
                case 13:
                    bonus = 5;
                    break;
                case 14:
                    bonus = 5;
                    break;
                case 15:
                    bonus = 5;
                    break;
                case 16:
                    bonus = 5;
                    break;
                case 17:
                    bonus = 6;
                    break;
                case 18:
                    bonus = 6;
                    break;
                case 19:
                    bonus = 6;
                    break;
                case 20:
                    bonus = 6;
                    break;
            }

            return bonus;
        }
    }
}
