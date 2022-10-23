namespace Stat.CharacterStats
{
    public enum StatModType
    {
        Flat,
        PercentAdd,
        //PercentMult,
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly object Source;

        // Keep track of which what types of modifiers we have
        public StatModifier(float value, StatModType type, object source) {
            Value = value;
            Type = type;
            Source = source;
        }

        public StatModifier(float value, StatModType type) : this(value, type, null) { }
    }
}
