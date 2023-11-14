using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace CustomRPGSystem
{
    public static class CharacterStats
    {
        static int m_hitDie;
        static int m_maxHitPoints;
        static int m_armorClass = 10;
        public static int HitDie(Race race)
        {
            switch (race)
            {
                case Race.None:
                    m_hitDie = 0;
                    break;
                case Race.Dragonborn:
                    m_hitDie = 0;
                    break;
                case Race.Hill_Dwarf:
                    m_hitDie = 0;
                    break;
                case Race.Mountain_Dwarf:
                    m_hitDie = 0;
                    break;
                case Race.High_Elf:
                    m_hitDie = 0;
                    break;
                case Race.Wood_Elf:
                    m_hitDie = 0;
                    break;
                case Race.Shadow_Elf:
                    m_hitDie = 0;
                    break;
                case Race.Forest_Gnome:
                    m_hitDie = 0;
                    break;
                case Race.Rock_Gnome:
                    m_hitDie = 0;
                    break;
                case Race.Half_Elf:
                    m_hitDie = 0;
                    break;
                case Race.Half_Orc:
                    m_hitDie = 0;
                    break;
                case Race.Lightfoot_Halfling:
                    m_hitDie = 0;
                    break;
                case Race.Stout_Halfling:
                    m_hitDie = 0;
                    break;
                case Race.Human:
                    m_hitDie = 0;
                    break;
                case Race.Tiefling:
                    m_hitDie = 0;
                    break;
            }
            return m_hitDie;
        }
        public static int MaxHitPoint(Level level, Class characterClass, int constituition)
        {
            if ((int)level == 0) m_maxHitPoints = 0;
            else if ((int)level == 1) m_maxHitPoints = m_hitDie + constituition;
            else
            {
                switch (characterClass)
                {
                    case Class.Barbarian:
                        m_maxHitPoints += 7 + constituition;
                        break;              
                    case Class.Bard:        
                        m_maxHitPoints += 5 + constituition;
                        break;              
                    case Class.Cleric:      
                        m_maxHitPoints += 5 + constituition;
                        break;              
                    case Class.Druid:       
                        m_maxHitPoints += 5 + constituition;
                        break;              
                    case Class.Fighter:     
                        m_maxHitPoints += 6 + constituition;
                        break;
                    case Class.Monk:
                        m_maxHitPoints += 5 + constituition;
                        break;
                    case Class.Paladin:
                        m_maxHitPoints += 6 + constituition;
                        break;
                    case Class.Ranger:
                        m_maxHitPoints += 6 + constituition;
                        break;
                    case Class.Rogue:
                        m_maxHitPoints += 5 + constituition;
                        break;
                    case Class.Sorcerer:
                        m_maxHitPoints += 4 + constituition;
                        break;
                    case Class.Warlock:
                        m_maxHitPoints += 5 + constituition;
                        break;
                    case Class.Wizard:
                        m_maxHitPoints += 4 + constituition;
                        break;
                }
            }

            return m_maxHitPoints;
        }
        public static int ArmorClass(int abilityModifier, int[] bonus)
        {
            return m_armorClass + abilityModifier + bonus.Sum();
        }
    }
    public static class CharacterRace
    {

    }
    public enum Level
    {
        None = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirteen,
        Fourteen,
        Fifteen,
        Sixteen,
        Seventeen,
        Eighteen,
        Nineteen,
        Twenty
    }
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
    }
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

    [Serializable]
    public class PlayerCharacterData
    {
        public static string m_id = "";

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
            public int inspirationPoints = 0;
            [Range(0, 18)] public int raceProficiencyPoints;
            [Range(0, 18)] public int classProficiencyPoints;
            public int availablePoints = 27;
            public int extraPoints = 0;
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

        [Serializable]
        public class SpellCasting
        {
            public int knownMagic = 0;
            public int knownCantrip = 0;
            public enum MagicTier
            {
                None = 0,
                First,
                Second,
                Third,
                Fourth,
                Fifth,
                Sixth,
                Seventh,
                Eighth,
                Ninth
            }
            public MagicTier magicTier;

            [System.Serializable]
            public struct Slot
            {
                public MagicTier tier;
                public int availableSlots;
                public int currentAvailableSlots;
            }

            public List<Slot> magicSlots = new List<Slot>();

            public AbilityScore.Ability conjuringAbility = AbilityScore.Ability.None;
            public int magicAttackModifier = 0;
            public int magicResistance = 0;

            public enum SpellSchool
            {
                None,
                Abjuration,
                Conjuration,
                Divination,
                Enchantment,
                Evocation,
                Illusion,
                Necromancy,
                Transmutation
            }
            public SpellSchool spellSchool = SpellSchool.None;
        }

        public bool HasAvailablePoints
        {
            get
            {
                return info.availablePoints > 0 || info.extraPoints > 0 || info.raceProficiencyPoints > 0 || info.classProficiencyPoints > 0 ? true : false;
            }
        }

        /*[HideInInspector]*/ public CharacterInfo info = new CharacterInfo();
        /*[HideInInspector]*/ public AbilityScore[] abilityScore;
        /*[HideInInspector]*/ public Skills[] skills;
        /*[HideInInspector]*/ public List<Skills> raceSkills = new List<Skills>();
        /*[HideInInspector]*/ public List<Skills> classSkills = new List<Skills>();
        /*[HideInInspector]*/ public SpellCasting spellCasting = new SpellCasting();

        public PlayerCharacterData(string p_characterName, int p_level, CharacterInfo.Race p_race,CharacterInfo.Class p_class)
        {
            CharacterInfo m_info = new CharacterInfo();
            List<AbilityScore> m_abilityScore = new List<AbilityScore>();
            List<Skills> m_skills = new List<Skills>();


            m_info.id = IdHelper.GenerateID();
            m_info.name = p_characterName;
            m_info.level = p_level + 1;
            m_info.race = p_race;
            m_info.classes = p_class;
            m_info.extraPoints += SetLevelAbilityPoints(m_info.level);
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

            int SetLevelAbilityPoints(int value)
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
            int SetProficiencyBonus(int value)
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

        private void SetRace(PlayerCharacterData player)
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
                    player.info.extraPoints += 2;
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
        private void SetClass(PlayerCharacterData player)
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
            int constituition = 0;

            for (int i = 0; i < player.abilityScore.Length; i++)
            {
                if (player.abilityScore[i].ability == AbilityScore.Ability.Constitution)
                {
                    constituition = player.abilityScore[i].modifier;
                }
            }

            if (player.info.level == 1)
            {
                player.info.maxHitPoints = player.info.hitDie + constituition;
            }
            else if(player.info.level > 1)
            {
                if (player.info.maxHitPoints == 0)
                {
                    player.info.maxHitPoints = player.info.hitDie + constituition;
                }

                for (int i = 1; i < player.info.dice; i++)
                {
                    switch (player.info.classes)
                    {
                        case CharacterInfo.Class.None:
                            break;
                        case CharacterInfo.Class.Barbarian:
                            player.info.maxHitPoints +=  7 + constituition;
                            break;
                        case CharacterInfo.Class.Bard:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                        case CharacterInfo.Class.Cleric:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                        case CharacterInfo.Class.Druid:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                        case CharacterInfo.Class.Fighter:
                            player.info.maxHitPoints +=  6 + constituition;
                            break;
                        case CharacterInfo.Class.Monk:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                        case CharacterInfo.Class.Paladin:
                            player.info.maxHitPoints +=  6 + constituition;
                            break;
                        case CharacterInfo.Class.Ranger:
                            player.info.maxHitPoints +=  6 + constituition;
                            break;
                        case CharacterInfo.Class.Rogue:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                      case CharacterInfo.Class.Sorcerer:
                            player.info.maxHitPoints +=  4 + constituition;
                            break;
                        case CharacterInfo.Class.Warlock:
                            player.info.maxHitPoints +=  5 + constituition;
                            break;
                        case CharacterInfo.Class.Wizard:
                            player.info.maxHitPoints +=  4 + constituition;
                            break;
                        default:
                            break;
                    }
                }
            }

            player.info.currentHitPoints = player.info.maxHitPoints;
        }
        public void SetSpellCasting(PlayerCharacterData player, int p_level)
        {
            switch (player.info.classes)
            {
                case CharacterInfo.Class.None:
                    break;

                case CharacterInfo.Class.Barbarian:
                    player.spellCasting.magicResistance = 0;
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 2, true);
                            break;
                        case 2:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 2, true);
                            break;
                        case 3:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 3, true);
                            break;
                        case 4:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 3, true);
                            break;
                        case 5:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 3, true);
                            break;
                        case 6:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 7:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 8:
                            player.spellCasting.magicAttackModifier = 2;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 9:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 10:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 11:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 4, true);
                            break;
                        case 12:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 5, true);
                            break;
                        case 13:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 5, true);
                            break;
                        case 14:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 5, true);
                            break;
                        case 15:
                            player.spellCasting.magicAttackModifier = 3;
                            SetSlot(SpellCasting.MagicTier.None, 5, true);
                            break;
                        case 16:
                            player.spellCasting.magicAttackModifier = 4;
                            SetSlot(SpellCasting.MagicTier.None, 5, true);
                            break;
                        case 17:
                            player.spellCasting.magicAttackModifier = 4;
                            SetSlot(SpellCasting.MagicTier.None, 6, true);
                            break;
                        case 18:
                            player.spellCasting.magicAttackModifier = 4;
                            SetSlot(SpellCasting.MagicTier.None, 6, true);
                            break;
                        case 19:
                            player.spellCasting.magicAttackModifier = 4;
                            SetSlot(SpellCasting.MagicTier.None, 6, true);
                            break;
                        case 20:
                            player.spellCasting.magicAttackModifier = 4;
                            SetSlot(SpellCasting.MagicTier.None, 0, true);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Bard:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Charisma;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 2:
                            player.spellCasting.knownMagic = 5;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 3:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 7;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 9;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 14;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 16;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 14:
                            player.spellCasting.knownMagic = 18;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 19;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 16:
                            player.spellCasting.knownMagic = 19;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 20;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 22;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 22;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 22;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 2);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Cleric:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Wisdom;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.knownMagic = player.abilityScore[i].modifier + p_level;
                        }
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 2:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 3:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 4:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 5:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 6:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 7:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 8:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 9:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 10:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 11:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 12:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 13:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 14:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 15:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 16:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 17:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 2);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Druid:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Wisdom;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.knownMagic = player.abilityScore[i].modifier + p_level;
                        }
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 2:
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 3:
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 4:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 5:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 6:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 7:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 8:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 9:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 10:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 11:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 12:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 13:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 14:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 15:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 16:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 17:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 2);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Fighter:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Intelligence;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 3:
                            player.spellCasting.knownMagic = 3;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 5;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 7;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 9;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 14:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 16:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 13;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Monk:
                    player.spellCasting.magicResistance = 0;
                    player.spellCasting.magicAttackModifier = 0;
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Wisdom;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    if (p_level > 1)
                    {
                        SetSlot(SpellCasting.MagicTier.None, p_level, true);
                    }
                    switch (p_level)
                    {
                        case 1:
                            spellCasting.knownCantrip = 4;
                            break;
                        case 2:
                            spellCasting.knownCantrip = 4;
                            break;
                        case 3:
                            spellCasting.knownCantrip = 4;
                            break;
                        case 4:
                            spellCasting.knownCantrip = 4;
                            break;
                        case 5:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 6:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 7:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 8:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 9:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 10:
                            spellCasting.knownCantrip = 6;
                            break;
                        case 11:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 12:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 13:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 14:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 15:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 16:
                            spellCasting.knownCantrip = 8;
                            break;
                        case 17:
                            spellCasting.knownCantrip = 10;
                            break;
                        case 18:
                            spellCasting.knownCantrip = 10;
                            break;
                        case 19:
                            spellCasting.knownCantrip = 10;
                            break;
                        case 20:
                            spellCasting.knownCantrip = 10;
                            break;
                    }
                    break;

                case CharacterInfo.Class.Paladin:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Charisma;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 2:
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 3:
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 4:
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 5:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 6:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 7:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 8:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 9:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 10:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 11:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 12:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 13:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 14:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 15:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 16:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 17:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 18:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 19:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 20:
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Ranger:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Wisdom;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 2:
                            player.spellCasting.knownMagic = 2;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 3:
                            player.spellCasting.knownMagic = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 7;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 7;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 8;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 14:
                            player.spellCasting.knownMagic = 8;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 9;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 16:
                            player.spellCasting.knownMagic = 9;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 10;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 10;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 11;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 11;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Rogue:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Intelligence;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 3:
                            player.spellCasting.knownMagic = 3;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 5;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 7;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 9;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 14:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 16:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 13;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Sorcerer:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Charisma;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    if (p_level > 1)
                    {
                        SetSlot(SpellCasting.MagicTier.None, p_level);
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownMagic = 2;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 2:
                            player.spellCasting.knownMagic = 3;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 3:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 5;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 7;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 9;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 13;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 14:
                            player.spellCasting.knownMagic = 13;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 14;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 16:
                            player.spellCasting.knownMagic = 14;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 6;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 2);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Warlock:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Charisma;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownMagic = 2;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 1, true);
                            break;
                        case 2:
                            player.spellCasting.knownMagic = 3;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 2);
                            break;
                        case 3:
                            player.spellCasting.knownMagic = 4;
                            player.spellCasting.knownCantrip = 2;
                            SetSlot(SpellCasting.MagicTier.Second, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 2);
                            break;
                        case 4:
                            player.spellCasting.knownMagic = 5;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Second, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 3);
                            break;
                        case 5:
                            player.spellCasting.knownMagic = 6;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Third, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 3);
                            break;
                        case 6:
                            player.spellCasting.knownMagic = 7;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Third, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 4);
                            break;
                        case 7:
                            player.spellCasting.knownMagic = 8;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Fourth, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 4);
                            break;
                        case 8:
                            player.spellCasting.knownMagic = 9;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Fourth, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 4);
                            break;
                        case 9:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.Fifth, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 5);
                            break;
                        case 10:
                            player.spellCasting.knownMagic = 10;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 2, true);
                            SetSlot(SpellCasting.MagicTier.None, 5);
                            break;
                        case 11:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 3, true);
                            SetSlot(SpellCasting.MagicTier.None, 5);
                            break;
                        case 12:
                            player.spellCasting.knownMagic = 11;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 3, true);
                            SetSlot(SpellCasting.MagicTier.None, 6);
                            break;
                        case 13:
                            player.spellCasting.knownMagic = 12;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 3, true);
                            SetSlot(SpellCasting.MagicTier.None, 6);
                            break;
                        case 15:
                            player.spellCasting.knownMagic = 13;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 3, true);
                            SetSlot(SpellCasting.MagicTier.None, 7);
                            break;
                        case 17:
                            player.spellCasting.knownMagic = 14;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 4);
                            SetSlot(SpellCasting.MagicTier.None, 7);
                            break;
                        case 18:
                            player.spellCasting.knownMagic = 14;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 4);
                            SetSlot(SpellCasting.MagicTier.None, 8);
                            break;
                        case 19:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 4);
                            SetSlot(SpellCasting.MagicTier.None, 8);
                            break;
                        case 20:
                            player.spellCasting.knownMagic = 15;
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.Fifth, 4);
                            SetSlot(SpellCasting.MagicTier.None, 8);
                            break;
                    }
                    break;

                case CharacterInfo.Class.Wizard:
                    player.spellCasting.conjuringAbility = AbilityScore.Ability.Intelligence;
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                        }
                    }
                    for (int i = 0; i < player.abilityScore.Length; i++)
                    {
                        if (player.abilityScore[i].ability == player.spellCasting.conjuringAbility)
                        {
                            player.spellCasting.magicResistance = 8 + player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.magicAttackModifier = player.info.proficiencyBonus + player.abilityScore[i].modifier;
                            player.spellCasting.knownMagic = player.abilityScore[i].modifier + p_level;
                        }
                    }
                    switch (p_level)
                    {
                        case 1:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 2, true);
                            break;
                        case 2:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 3, true);
                            break;
                        case 3:
                            player.spellCasting.knownCantrip = 3;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 2);
                            break;
                        case 4:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            break;
                        case 5:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 2);
                            break;
                        case 6:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            break;
                        case 7:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 1);
                            break;
                        case 8:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 2);
                            break;
                        case 9:
                            player.spellCasting.knownCantrip = 4;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 1);
                            break;
                        case 10:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            break;
                        case 11:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 12:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            break;
                        case 13:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 14:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            break;
                        case 15:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 16:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            break;
                        case 17:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 2);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 18:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 1);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 19:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 1);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                        case 20:
                            player.spellCasting.knownCantrip = 5;
                            SetSlot(SpellCasting.MagicTier.First, 4, true);
                            SetSlot(SpellCasting.MagicTier.Second, 3);
                            SetSlot(SpellCasting.MagicTier.Third, 3);
                            SetSlot(SpellCasting.MagicTier.Fourth, 3);
                            SetSlot(SpellCasting.MagicTier.Fifth, 3);
                            SetSlot(SpellCasting.MagicTier.Sixth, 2);
                            SetSlot(SpellCasting.MagicTier.Seventh, 2);
                            SetSlot(SpellCasting.MagicTier.Eighth, 1);
                            SetSlot(SpellCasting.MagicTier.Ninth, 1);
                            break;
                    }
                    break;
            }

            void SetSlot(SpellCasting.MagicTier tier, int value, bool clearSlots = false)
            {
                if (clearSlots) player.spellCasting.magicSlots.Clear();

                player.spellCasting.magicTier = tier;

                SpellCasting.Slot slot = new SpellCasting.Slot();
                slot.tier = tier;
                slot.availableSlots = value;
                slot.currentAvailableSlots = slot.availableSlots;

                if (!player.spellCasting.magicSlots.Contains(slot))
                {
                    player.spellCasting.magicSlots.Add(slot);
                }
                else
                {
                    SpellCasting.Slot s = player.spellCasting.magicSlots.Find(x => x.tier == tier);
                    int index = player.spellCasting.magicSlots.IndexOf(s);

                    s.availableSlots = value;
                    slot.currentAvailableSlots = slot.availableSlots;

                    player.spellCasting.magicSlots[index] = s;
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

            int SetAbilityModifier(int value)
            {
                int modifierValue = 0;

                switch (value)
                {
                    case 1:
                        modifierValue = -5;
                        break;
                    case 2:
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
    }
}
