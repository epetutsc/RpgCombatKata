using RpgCombatKata.ValueObjects;

namespace RpgCombatKata
{
    public interface IHaveHealth
    {
        Health Health { get; }

        void Defend(Damage damage);
    }
}