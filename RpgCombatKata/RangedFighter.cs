using RpgCombatKata.ValueObjects;

namespace RpgCombatKata
{
    public class RangedFighter : Character
    {
        public override AttackRange MaxAttackRange { get; } = new(20);

        public RangedFighter(Health? health = null, Level? level = null, Location? location = null)
            : base(health, level, location)
        {
        }
    }
}
