using RpgCombatKata.ValueObjects;

namespace RpgCombatKata
{
    public class MeleeFighter : Character
    {
        public override AttackRange MaxAttackRange { get; } = new(2);

        public MeleeFighter(Health? health = null, Level? level = null, Location? location = null)
            : base(health, level, location)
        {
        }
    }
}
