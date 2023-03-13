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

            public int hitDie = 0;
            public int dice = 0;
            public int armorClass = 10;
            public int maxHitPoints = 0;
            public int currentHitPoints = 0;
            public int speed = 0;
            public int initiative = 0;
            public int proficiencyBonus = 0;
            [Range(0, 18)] public int raceProficiencyPoints;
            [Range(0, 18)] public int classProficiencyPoints;
            public int abilityPoints = 72;
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
            public bool isChangable = false;
        }

        /*[HideInInspector]*/ public CharacterInfo info = new CharacterInfo();
        /*[HideInInspector]*/ public AbilityScore[] abilityScore;
        /*[HideInInspector]*/ public Skills[] skills;
        /*[HideInInspector]*/ public List<Skills> raceSkills = new List<Skills>();
        /*[HideInInspector]*/ public List<Skills> classSkills = new List<Skills>();

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
            m_info.abilityPoints += SetLevelAbilityPoints(m_info.level);
            m_info.proficiencyBonus = SetProficiencyBonus(m_info.level);
            m_info.dice = m_info.level;
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

                this.raceSkills.Add(new Skills());
                this.raceSkills[i].skill = (Skills.Skill)i;

                this.classSkills.Add(new Skills());
                this.classSkills[i].skill = (Skills.Skill)i;

                switch (m_skills[i].skill)
                {
                    case Skills.Skill.Acrobatics:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Animal_Handling:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Arcana:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Athletics:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Strenght;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Strenght;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Strenght;
                        break;
                    case Skills.Skill.Deception:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.History:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Insight:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Intimidation:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Investigation:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Medicine:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Nature:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Perception:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                    case Skills.Skill.Performance:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Persuasion:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Charisma;
                        break;
                    case Skills.Skill.Religion:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Intelligence;
                        break;
                    case Skills.Skill.Sleight_Of_Hand:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Stealth:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Dexterity;
                        break;
                    case Skills.Skill.Survival:
                        m_skills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        raceSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        classSkills[i].abilityModifier = AbilityScore.Ability.Wisdom;
                        break;
                }
            }

            this.skills = m_skills.ToArray();

            SetRace(this);
            SetClass(this);
            SetHitPoints(this);
        }

        public void SetRace(PlayerCharacterData player)
        {
            switch (player.info.race)
            {
                case CharacterInfo.Race.None:
                    player.info.raceProficiencyPoints += 0;
                    break;

                case CharacterInfo.Race.Dragonborn:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Hill_Dwarf:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Mountain_Dwarf:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.High_Elf:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                        if (player.raceSkills[i].skill == Skills.Skill.Perception) player.raceSkills[i].proficient = true;
                    }
                    break;

                case CharacterInfo.Race.Wood_Elf:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                        if (player.raceSkills[i].skill == Skills.Skill.Perception) player.raceSkills[i].proficient = true;
                    }
                    break;

                case CharacterInfo.Race.Shadow_Elf:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                        if (player.raceSkills[i].skill == Skills.Skill.Perception) player.raceSkills[i].proficient = true;
                    }
                    break;

                case CharacterInfo.Race.Forest_Gnome:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Rock_Gnome:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Half_Elf:
                    player.info.raceProficiencyPoints += 2;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 2);
                    player.info.abilityPoints += 2;
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Race.Half_Orc:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                        if (player.raceSkills[i].skill == Skills.Skill.Intimidation) player.raceSkills[i].proficient = true;
                    }
                    break;

                case CharacterInfo.Race.Lightfoot_Halfling:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Stout_Halfling:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 25;
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 2);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;

                case CharacterInfo.Race.Human:
                    player.info.raceProficiencyPoints += 1;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Strenght, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Dexterity, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Constitution, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Wisdom, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 1);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Race.Tiefling:
                    player.info.raceProficiencyPoints += 0;
                    player.info.speed = 30;
                    SetAbilityScore(player, AbilityScore.Ability.Intelligence, 1);
                    SetAbilityScore(player, AbilityScore.Ability.Charisma, 2);
                    for (int i = 0; i < player.raceSkills.Count; i++)
                    {
                        player.raceSkills[i].isChangable = false;
                    }
                    break;
            }
        }
        public void SetClass(PlayerCharacterData player)
        {
            switch (player.info.classes)
            {
                case CharacterInfo.Class.None:
                    player.info.classProficiencyPoints += 0;
                    break;

                case CharacterInfo.Class.Barbarian:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 12;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);

                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Animal_Handling) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Nature) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Survival) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Bard:
                    player.info.classProficiencyPoints += 3;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Cleric:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.History)  player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Medicine) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Persuasion) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Druid:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Arcana) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Animal_Handling) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Medicine) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Nature) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Survival) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Fighter:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 10;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Acrobatics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Animal_Handling) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.History) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Survival) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Monk:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Acrobatics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Stealth) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.History) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Paladin:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 10;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Medicine) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Persuasion) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Ranger:
                    player.info.classProficiencyPoints += 3;
                    player.info.hitDie = 10;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Strenght, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Animal_Handling) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Stealth) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Investigation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Nature) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Survival) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Rogue:
                    player.info.classProficiencyPoints += 4;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Dexterity, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Acrobatics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Athletics) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Performance) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Deception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Stealth) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Investigation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Persuasion) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Sleight_Of_Hand) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Sorcerer:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 6;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Constitution, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Arcana) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Deception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Perception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Warlock:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 8;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Charisma, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Arcana) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Deception) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.History) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Intimidation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Investigation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Nature) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;

                case CharacterInfo.Class.Wizard:
                    player.info.classProficiencyPoints += 2;
                    player.info.hitDie = 6;
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Intelligence, true);
                    SetAbilitySavingThrow(player, AbilityScore.Ability.Wisdom, true);
                    for (int i = 0; i < player.classSkills.Count; i++)
                    {
                        player.classSkills[i].isChangable = false;
                        if (player.classSkills[i].skill == Skills.Skill.Arcana) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.History) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Insight) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Investigation) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Medicine) player.classSkills[i].isChangable = true;
                        if (player.classSkills[i].skill == Skills.Skill.Religion) player.classSkills[i].isChangable = true;
                    }
                    break;
            }
        }
        public void SetHitPoints(PlayerCharacterData player)
        {
            if (player.info.level == 1)
            {
                player.info.maxHitPoints = player.info.hitDie;
            }
            else if(player.info.level > 1)
            {
                player.info.maxHitPoints = player.info.hitDie;

                for (int i = 1; i < player.info.dice; i++)
                {
                    player.info.maxHitPoints += UnityEngine.Random.Range(1, player.info.hitDie);
                }
            }

            player.info.currentHitPoints = player.info.maxHitPoints;
        }

        public void SetRaceSkills(PlayerCharacterData player,Skills.Skill skill, bool p_proficient = false, bool p_isChangable = false)
        {
            List<Skills> s = new List<Skills>();

            foreach (Skills skl in player.skills)
            {
                s.Add(skl);
            }

            Skills sk = s.Find(x => x.skill == skill);

            sk.isChangable = p_isChangable;

            if (p_proficient)
            {
                sk.proficient = p_proficient;
                //player.info.proficiencyPoints--;
            }

            for (int i = 0; i < player.skills.Length; i++)
            {
                if (player.skills[i] == sk)
                {
                    player.skills[i] = sk;
                }
            }
        }
        public void SetClassSkills(PlayerCharacterData player, Skills.Skill skill, bool p_proficient = false, bool p_isAvailable = false)
        {
            List<Skills> s = new List<Skills>();

            foreach (Skills skl in player.skills)
            {
                s.Add(skl);
            }

            Skills sk = s.Find(x => x.skill == skill);

            sk.isChangable = p_isAvailable;

            if (p_proficient)
            {
                sk.proficient = p_proficient;
                //player.info.proficiencyPoints--;
            }

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
        public int SetAbilityModifier(int value)
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
        private int SetLevelAbilityPoints(int value)
        {
            int availablePoints = 0;

            if (value >= 4 && value < 8)
            {
                availablePoints = 2;
            }
            else if (value >= 8 && value < 12)
            {
                availablePoints = 4;
            }
            else if (value >= 12 && value < 16)
            {
                availablePoints = 6;
            }
            else if (value >= 16 && value < 19)
            {
                availablePoints = 8;
            }
            else if (value >= 19)
            {
                availablePoints = 10;
            }

            return availablePoints;
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
