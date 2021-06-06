namespace RpgCombatKata.ValueObjects
{
    public record Level(int Value)
    {
        public static readonly Level Initial = new(1);

        public static int operator -(Level level1, Level level2)
        {
            return level1.Value - level2.Value;
        }
    }
}
