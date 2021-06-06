using System;

namespace RpgCombatKata.ValueObjects
{
    public record Location(int Value)
    {
        public static readonly Location Initial = new(0);

        public int DistanceTo(Location other)
        {
            return Math.Abs(Value - other.Value);
        }
    }
}
