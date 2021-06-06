using System;

namespace RpgCombatKata.ValueObjects
{
    public record Health(double Value)
    {
        public static readonly Health Zero = new(0);
        public static readonly Health MaxHealth = new(1000);
        public static readonly Health Initial = new(1000);

        public static Health operator -(Health that, Damage damage)
        {
            var newHealth = Math.Max(0, that.Value - damage.Value);
            return new Health(newHealth);
        }

        public static Health operator +(Health that, Healing healing)
        {
            var newHealth = Math.Min(MaxHealth.Value, that.Value + healing.Value);
            return new Health(newHealth);
        }
    }
}
