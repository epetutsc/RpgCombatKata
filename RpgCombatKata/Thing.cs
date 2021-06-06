using RpgCombatKata.ValueObjects;

namespace RpgCombatKata
{
    public class Thing : IHaveHealth
    {
        public bool IsDestroyed => Health == Health.Zero;

        public Health Health { get; private set; }

        public Thing(Health health)
        {
            Health = health;
        }

        public void Defend(Damage damage)
        {
            Health -= damage;
        }
    }
}
