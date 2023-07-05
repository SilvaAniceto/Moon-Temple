namespace CharacterCreator
{
    public enum Abilities
    {
        None,
        Strenght,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }

    public class CharactersAbilities
    {
        public Abilities m_ability { get; set; }
        public int m_baseScore { get; set; }
        public int m_racialBonus { get; set; }
        public int m_abilityImprovements { get; set; }
        public int m_miscBonus { get; set; }
        public int m_totalScore { get { return m_baseScore + m_racialBonus + m_abilityImprovements + m_miscBonus; } }

        public static int m_modifier { get; private set; }

        public static int ScoreCalculation(int value)
        {
            switch (value)
            {
                case 0: m_modifier = 0; break;
                case 1: m_modifier = -5; break;
                case 2: m_modifier = -4; break;
                case 3: m_modifier = -4; break;
                case 4: m_modifier = -3; break;
                case 5: m_modifier = -3; break;
                case 6: m_modifier = -2; break;
                case 7: m_modifier = -2; break;
                case 8: m_modifier = -1; break;
                case 9: m_modifier = -1; break;
                case 10: m_modifier = 0; break;
                case 11: m_modifier = 0; break;
                case 12: m_modifier = 1; break;
                case 13: m_modifier = 1; break;
                case 14: m_modifier = 2; break;
                case 15: m_modifier = 2; break;
                case 16: m_modifier = 3; break;
                case 17: m_modifier = 3; break;
                case 18: m_modifier = 4; break;
                case 19: m_modifier = 4; break;
                case 20: m_modifier = 5; break;
                case 21: m_modifier = 5; break;
                case 22: m_modifier = 6; break;
                case 23: m_modifier = 6; break;
                case 24: m_modifier = 7; break;
                case 25: m_modifier = 7; break;
                case 26: m_modifier = 8; break;
                case 27: m_modifier = 8; break;
                case 28: m_modifier = 9; break;
                case 29: m_modifier = 9; break;
                case 30: m_modifier = 10; break;
            }

            return m_modifier;
        }
    }
}
