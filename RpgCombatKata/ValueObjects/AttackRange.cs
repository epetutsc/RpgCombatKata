namespace RpgCombatKata.ValueObjects
{
    public record AttackRange(int Value)
    {
        public static readonly AttackRange None = new(0);

        public static bool operator >(AttackRange range, int value)
        {
            return range.Value > value;
        }

        public static bool operator <(AttackRange range, int value)
        {
            return range.Value < value;
        }
    }
}
