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
        public class CharacterIdentification
        {
            public string id = "";
            public string name = "";
            public string race = "";
            public enum CharClass
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
            public CharClass charClass = CharClass.None;
        }

        public CharacterIdentification[] Identification;

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
        }
    }
}
