using RpgCombatKata.Exceptions;
using RpgCombatKata.ValueObjects;
using Shouldly;

namespace RpgCombatKata.Tests
{
    public class CharacterTests
    {
        private readonly Character _character1;
        private readonly Character _character2;

        public CharacterTests()
        {
            _character1 = new Character();
            _character2 = new Character();
        }

        [ReadableFact]
        public void All_Characters_when_created__Health_starts_at_1000()
        {
            _character1.Health.ShouldBe(new Health(1000));
        }

        [ReadableFact]
        public void All_Characters_when_created__Level_starts_at_1()
        {
            _character1.Level.ShouldBe(new Level(1));
        }

        [ReadableFact]
        public void All_Characters_when_created__May_be_alive_or_dead()
        {
            _character1.IsAlive.ShouldBe(true);
        }

        [ReadableFact]
        public void Characters_can_deal_damage_to_characters__Damage_is_subtracted_from_health()
        {
            var damage = new Damage(100);
            _character1.Attack(_character2, damage);
            _character2.Health.ShouldBe(Health.Initial - damage);
        }

        [ReadableFact]
        public void Characters_can_deal_damage_to_characters__When_damage_exceeds_health_character_dies()
        {
            _character1.Attack(_character2, new Damage(2000));
            _character2.Health.ShouldBe(Health.Zero);
            _character2.IsAlive.ShouldBeFalse();
        }

        [ReadableFact]
        public void A_Character_can_heal_a_character__Dead_characters_cannot_be_healed()
        {
            Should.Throw<CannotHealDeadCharacterException>(() =>
            {
                Character.DeadCharacter.Heal(new Healing(2000));
            });

            Character.DeadCharacter.Health.ShouldBe(Health.Zero);
            Character.DeadCharacter.IsAlive.ShouldBeFalse();
        }

        [ReadableFact]
        public void A_Character_can_heal_a_character__Healing_cannot_raise_health_above_1000()
        {
            _character1.Heal(new Healing(2000));
            _character2.Health.ShouldBe(Health.MaxHealth);
        }

        [ReadableFact]
        public void A_Character_can_heal_a_character()
        {
            var healing = new Healing(100);
            _character1.Heal(healing);
            _character2.Health.ShouldBe(Health.Initial + healing);
        }

        [ReadableFact]
        public void A_Character_cannot_Deal_Damage_to_itself()
        {
            Should.Throw<CharacterCannotDealDamageToItselfException>(() =>
            {
                _character1.Attack(_character1, new Damage(1));
            });
        }

        [ReadableFact]
        public void A_Character_can_only_Heal_itself()
        {
            _character1.Heal(new Healing(1));
        }

        [ReadableFact]
        public void When_dealing_damage__If_level_difference_is_less_than_5__Damage_is_unchanged()
        {
            var lvl1 = new Character(level: new Level(1));
            var lvl5 = new Character(level: new Level(5));

            var damage = new Damage(100);

            lvl1.Attack(lvl5, damage);
            lvl5.Health.ShouldBe(Health.Initial - damage);

            lvl5.Attack(lvl1, damage);
            lvl1.Health.ShouldBe(Health.Initial - damage);
        }

        [ReadableFact]
        public void When_dealing_damage__If_the_target_is_5_or_more_Levels_above_the_attacker__Damage_is_reduced_by_50_percent()
        {
            var lvl1 = new Character(level: new Level(1));
            var lvl6 = new Character(level: new Level(6));

            var damage = new Damage(100);
            var damage50Pct = damage * 0.5;

            lvl1.Attack(lvl6, damage);
            lvl6.Health.ShouldBe(Health.Initial - (damage - damage50Pct));
        }

        [ReadableFact]
        public void When_dealing_damage__If_the_target_is_5_or_more_Levels_below_the_attacker__Damage_is_increased_by_50_percent()
        {
            var lvl1 = new Character(level: new Level(1));
            var lvl6 = new Character(level: new Level(6));

            var damage = new Damage(100);
            var damage50Pct = damage * 0.5;

            lvl6.Attack(lvl1, damage);
            lvl1.Health.ShouldBe(Health.Initial - (damage + damage50Pct));
        }

        [ReadableFact]
        public void Melee_Characters_must_be_in_range_to_deal_damage_to_a_target()
        {
            var attacker = new MeleeFighter(location: new Location(1));
            var target1 = new Character(location: new Location(3));
            var target2 = new Character(location: new Location(10));

            var damage = new Damage(1);

            attacker.Attack(target1, damage);
            target1.Health.ShouldBe(Health.Initial - damage);

            Should.Throw<TargetIsNotInRangeException>(() =>
            {
                attacker.Attack(target2, damage);
            });
        }

        [ReadableFact]
        public void Ranged_Characters_must_be_in_range_to_deal_damage_to_a_target()
        {
            var attacker = new RangedFighter(location: new Location(1));
            var target1 = new Character(location: new Location(20));
            var target2 = new Character(location: new Location(30));

            var damage = new Damage(1);

            attacker.Attack(target1, damage);
            target1.Health.ShouldBe(Health.Initial - damage);

            Should.Throw<TargetIsNotInRangeException>(() =>
            {
                attacker.Attack(target2, damage);
            });
        }

        [ReadableFact]
        public void Newly_created_Characters_belong_to_no_Faction()
        {
            new Character().Factions.ShouldBeEmpty();
        }

        [ReadableFact]
        public void A_Character_may_Join_one_or_more_Factions()
        {
            _character1.Join(new Faction("A"), new Faction("B"));
            _character1.Factions.Count.ShouldBe(2);
        }

        [ReadableFact]
        public void A_Character_may_Leave_one_or_more_Factions()
        {
            _character1.Join(new Faction("A"), new Faction("B"));
            _character1.Leave(new Faction("A"), new Faction("B"));
            _character1.Factions.Count.ShouldBe(0);
        }

        [ReadableFact]
        public void Players_belonging_to_the_same_Faction_are_considered_Allies()
        {
            _character1.Join(new Faction("A"), new Faction("B"));
            _character2.Join(new Faction("B"), new Faction("C"));
            var character3 = new Character();

            _character1.IsAlliedWith(_character2).ShouldBeTrue();
            _character1.IsAlliedWith(character3).ShouldBeFalse();
        }

        [ReadableFact]
        public void Allies_cannot_Deal_Damage_to_one_another()
        {
            _character1.Join(new Faction("A"), new Faction("B"));
            _character2.Join(new Faction("B"), new Faction("C"));

            Should.Throw<AlliesCannotDealDamageToOneAnotherException>(() =>
            {
                _character1.Attack(_character2, new Damage(1));
            });
        }

        [ReadableFact]
        public void Allies_can_Heal_one_another()
        {
            _character1.Join(new Faction("A"), new Faction("B"));
            _character2.Join(new Faction("B"), new Faction("C"));
            _character1.Heal(_character2, new Healing(1));
        }

        [ReadableFact]
        public void NonAllies_cannot_Heal_one_another()
        {
            _character1.Join(new Faction("A"));
            _character2.Join(new Faction("B"));

            Should.Throw<CannotHealOtherCharacterException>(() =>
            {
                _character1.Heal(_character2, new Healing(1));
            });
        }

        [ReadableFact]
        public void Characters_can_damage_non_character_things()
        {
            var tree = new Thing(new Health(2000));
            _character1.Attack(tree, new Damage(1));
        }

        [ReadableFact]
        public void Tree_can_be_destroyed()
        {
            var tree = new Thing(new Health(2000));
            _character1.Attack(tree, new Damage(5000));
            tree.IsDestroyed.ShouldBeTrue();
        }
    }
}
