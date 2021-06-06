using RpgCombatKata.Exceptions;
using RpgCombatKata.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace RpgCombatKata
{
    public class Character
    {
        public static readonly Character DeadCharacter = new() { Health = Health.Zero };

        private readonly HashSet<Faction> _factions = new();
        
        public Health Health { get; private set; }
        public Level Level { get; private set; }
        public Location Location { get; private set; }
        public virtual AttackRange MaxAttackRange { get; } = AttackRange.None;
        public IReadOnlySet<Faction> Factions => _factions;

        public bool IsAlive => Health != Health.Zero;

        public Character(Health? health = null, Level? level = null, Location? location = null)
        {
            Health = health ?? Health.Initial;
            Level = level ?? Level.Initial;
            Location = location ?? Location.Initial;
        }

        public void Attack(Character other, Damage damage)
        {
            if (other == this)
            {
                throw new CharacterCannotDealDamageToItselfException();
            }

            if (MaxAttackRange < Location.DistanceTo(other.Location))
            {
                throw new TargetIsNotInRangeException();
            }

            if (IsAlliedWith(other))
            {
                throw new AlliesCannotDealDamageToOneAnotherException();
            }

            other.Defend(this, damage);
        }

        private void Defend(Character attacker, Damage damage)
        {
            Health -= damage.AdjustByLevelDifference(attacker.Level, Level);
        }

        public void Attack(IHaveHealth thing, Damage damage)
        {
            thing.Defend(damage);
        }

        public void Heal(Healing healing)
        {
            if (!IsAlive)
            {
                throw new CannotHealDeadCharacterException();
            }

            Health += healing;
        }

        public void Join(params Faction[] factions)
        {
            _factions.UnionWith(factions);
        }

        public void Leave(params Faction[] factions)
        {
            _factions.ExceptWith(factions);
        }

        public bool IsAlliedWith(Character other)
        {
            return _factions.Intersect(other._factions).Any();
        }

        public void Heal(Character other, Healing healing)
        {
            if (!IsAlliedWith(other))
            {
                throw new CannotHealOtherCharacterException();
            }

            other.Heal(healing);
        }
    }
}
