namespace RpgCombatKata.ValueObjects
{
    public record Damage(double Value)
    {
        public static Damage operator +(Damage that, Damage other)
        {
            return new Damage(that.Value + other.Value);
        }

        public static Damage operator -(Damage that, Damage other)
        {
            return new Damage(that.Value - other.Value);
        }

        public static Damage operator *(Damage that, double value)
        {
            return new Damage(that.Value * value);
        }

        public static Damage operator /(Damage that, double value)
        {
            return new Damage(that.Value / value);
        }

        public Damage AdjustByLevelDifference(Level attacker, Level target)
        {
            double adjustment = 1d;

            // If the target is 5 or more Levels above the attacker, Damage is reduced by 50%
            if (target - attacker >= 5)
            {
                adjustment = 0.5d;
            }

            // if the target is 5 or more Levels below the attacker, Damage is increased by 50%
            else if (target - attacker <= -5)
            {
                adjustment = 1.5d;
            }

            return this with { Value = Value * adjustment };
        }
    }
}
