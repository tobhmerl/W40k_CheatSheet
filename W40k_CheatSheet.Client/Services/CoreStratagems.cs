using W40k_CheatSheet.Client.Models;

namespace W40k_CheatSheet.Client.Services;

public static class CoreStratagems
{
    public static readonly List<Stratagem> All =
    [
        new()
        {
            Name = "Command Re-roll",
            Cost = "1CP",
            Category = "Core – Battle Tactic",
            Phases = GamePhase.All,
            TurnColor = StratagemTurn.Green,
            When = "Any phase, just after you make an Advance roll, a Charge roll, a Desperate Escape test or a Hazardous test for a unit from your army, or a Hit roll, a Wound roll, a Damage roll or a saving throw for a model from that unit, or a roll to determine the number of attacks made with a weapon equipped by a model in that unit.",
            Target = "That unit or model from your army.",
            Effect = "You re-roll that roll, test or saving throw."
        },
        new()
        {
            Name = "Insane Bravery",
            Cost = "1CP",
            Category = "Core – Epic Deed",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Blue,
            When = "Battle-shock step of your Command phase, just before you take a Battle-shock test for a unit from your army.",
            Target = "That unit from your army.",
            Effect = "Your unit automatically passes that Battle-shock test.",
            Restriction = "You cannot use this Stratagem more than once per battle."
        },
        new()
        {
            Name = "New Orders",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Blue,
            When = "End of your Command phase.",
            Target = "One of your active Secondary Mission cards.",
            Effect = "Discard it and draw one new Secondary Mission card."
        },
        new()
        {
            Name = "Heroic Intervention",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Red,
            When = "Your opponent's Charge phase, just after an enemy unit ends a Charge move.",
            Target = "One unit from your army that is within 6\" of that enemy unit and would be eligible to declare a charge against that enemy unit if it were your Charge phase.",
            Effect = "Your unit now declares a charge only that enemy unit, and you resolve that charge as if it were your Charge phase.",
            Restriction = "You can only select a VEHICLE unit from your army if it is a WALKER. Note that even if this charge is successful, your unit does not receive any Charge bonus this turn."
        },
        new()
        {
            Name = "Counter-Offensive",
            Cost = "2CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.BlueRed,
            When = "Fight phase, just after an enemy unit has fought.",
            Target = "One unit from your army that is within Engagement Range of one or more enemy units and that has not already been selected to fight this phase.",
            Effect = "Your unit fights next."
        },
        new()
        {
            Name = "Smokescreen",
            Cost = "1CP",
            Category = "Core – Wargear",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            RequiredKeywords = ["Smoke"],
            When = "Your opponent's Shooting phase, just after an enemy unit has selected its targets.",
            Target = "One SMOKE unit from your army that was selected as the target of one or more of the attacking unit's attacks.",
            Effect = "Until the end of the phase, all models in your unit have the Benefit of Cover and the Stealth ability."
        },
        new()
        {
            Name = "Epic Challenge",
            Cost = "1CP",
            Category = "Core – Epic Deed",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.BlueRed,
            RequiredKeywords = ["Character"],
            When = "Fight phase, when a CHARACTER unit from your army that is within Engagement Range of one or more Attached units is selected to fight.",
            Target = "One CHARACTER model in your unit.",
            Effect = "Until the end of the phase, all melee attacks made by that model have the [PRECISION] ability."
        },
        new()
        {
            Name = "Grenade",
            Cost = "1CP",
            Category = "Core – Wargear",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Grenades"],
            When = "Your Shooting phase.",
            Target = "One GRENADES unit from your army (excluding units that Advanced, Fell Back or have shot this turn) that is not within Engagement Range of one or more enemy units.",
            Effect = "Select one GRENADES model in your unit and one enemy unit that is not within Engagement Range of one or more units from your army and is within 8\" of and visible to your GRENADES model. Roll six D6: for each 4+, that enemy unit suffers 1 mortal wound."
        },
        new()
        {
            Name = "Tank Shock",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Vehicle"],
            When = "Your Charge phase, just after a VEHICLE unit from your army ends a Charge move.",
            Target = "That VEHICLE unit.",
            Effect = "Select one enemy unit within Engagement Range of your unit, and select one VEHICLE model in your unit that is within Engagement Range of that enemy unit. Roll a number of D6 equal to the Toughness characteristic of the selected VEHICLE model. For each 5+, that enemy unit suffers 1 mortal wound (to a maximum of 6 mortal wounds)."
        },
        new()
        {
            Name = "Rapid Ingress",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Red,
            When = "End of your opponent's Movement phase.",
            Target = "One unit from your army that is in Reserves.",
            Effect = "Your unit can arrive on the battlefield as if it were the Reinforcements step of your Movement phase, and if every model in that unit has the Deep Strike ability, you can set that unit up as described in the Deep Strike ability (even though it is not your Movement phase).",
            Restriction = "You cannot use this Stratagem to enable a unit to arrive on the battlefield during a battle round it would not normally be able to do so in."
        },
        new()
        {
            Name = "Fire Overwatch",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Move | GamePhase.Charge,
            TurnColor = StratagemTurn.Red,
            When = "Your opponent's Movement or Charge phase, just after an enemy unit is set up or when an enemy unit starts or ends a Normal, Advance or Fall Back move or declares a Charge.",
            Target = "One unit from your army (excluding TITANIC units) that is within 24\" of that enemy unit and that would be eligible to shoot if it were your Shooting phase.",
            Effect = "If that enemy unit is visible to your unit, your unit can shoot that enemy unit as if it were your Shooting phase.",
            Restriction = "You cannot target a TITANIC unit with this Stratagem. Until the end of the phase, each time a model in your unit makes a ranged attack, an unmodified Hit roll of 6 is required to score a hit. You can only use this Stratagem once per turn."
        },
        new()
        {
            Name = "Go To Ground",
            Cost = "1CP",
            Category = "Core – Battle Tactic",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            RequiredKeywords = ["Infantry"],
            When = "Your opponent's Shooting phase, just after an enemy unit has selected its targets.",
            Target = "One INFANTRY unit from your army that was selected as the target of one or more of the attacking unit's attacks.",
            Effect = "Until the end of the phase, all models in your unit have a 6+ invulnerable save and have the Benefit of Cover."
        },

        // ── Awakened Dynasty ──
        new()
        {
            Name = "Protocol of the Conquering Tyrant",
            Cost = "1CP",
            Category = "Awakened Dynasty – Battle Tactic",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Blue,
            When = "Your Shooting phase.",
            Target = "One NECRONS unit from your army that has not been selected to shoot this phase.",
            Effect = "Until the end of the phase, each time a model in your unit makes an attack that targets a unit within half range, re-roll a Hit roll of 1. If a NECRONS CHARACTER is leading your unit, until the end of the phase, you can re-roll the Hit roll for that attack instead."
        },
        new()
        {
            Name = "Protocol of the Undying Legions",
            Cost = "1CP",
            Category = "Awakened Dynasty – Strategic Ploy",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            MyTurnPhases = GamePhase.Fight,
            EnemyTurnPhases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.RedBlue,
            When = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has resolved its attacks.",
            Target = "One NECRONS unit from your army that had one or more of its models destroyed as a result of the attacking unit's attacks.",
            Effect = "Your unit activates its Reanimation Protocols and reanimates D3 wounds (or D3+1 wounds if a NECRONS CHARACTER is leading your unit)."
        },
        new()
        {
            Name = "Protocol of the Eternal Revenant",
            Cost = "1CP",
            Category = "Awakened Dynasty – Epic Deed",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.All,
            TurnColor = StratagemTurn.Green,
            RequiredKeywords = ["Character"],
            When = "Any phase.",
            Target = "One NECRONS INFANTRY CHARACTER model from your army that was just destroyed. You can use this Stratagem on that model even though it was just destroyed.",
            Effect = "At the end of the phase, set your model back up on the battlefield as close as possible to where it was destroyed and not within Engagement Range of any enemy units, with half of its starting number of wounds remaining.",
            Restriction = "Each model can only be targeted with this Stratagem once per battle."
        },
        new()
        {
            Name = "Protocol of the Hungry Void",
            Cost = "1CP",
            Category = "Awakened Dynasty – Battle Tactic",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.BlueRed,
            When = "Fight phase.",
            Target = "One NECRONS unit from your army that has not been selected to fight this phase.",
            Effect = "Until the end of the phase, add 1 to the Strength characteristic of melee weapons equipped by models in your unit. In addition, if a NECRONS CHARACTER is leading your unit, until the end of the phase, improve the Armour Penetration characteristic of melee weapons equipped by models in your unit by 1 (this is not cumulative with any other modifiers that improve Armour Penetration)."
        },
        new()
        {
            Name = "Protocol of the Sudden Storm",
            Cost = "1CP",
            Category = "Awakened Dynasty – Strategic Ploy",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Blue,
            When = "Your Movement phase.",
            Target = "One NECRONS unit from your army.",
            Effect = "Until the end of the turn, ranged weapons equipped by models in your unit have the [ASSAULT] ability. In addition, if a NECRONS CHARACTER is leading your unit, until the end of the phase, you can re-roll Advance rolls made for your unit."
        },
        new()
        {
            Name = "Protocol of the Vengeful Stars",
            Cost = "2CP",
            Category = "Awakened Dynasty – Strategic Ploy",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            RequiredKeywords = ["Character"],
            When = "Your opponent's Shooting phase, just after an enemy unit destroys a NECRONS unit from your army.",
            Target = "One NECRONS CHARACTER unit from your army that was within 6\" of that NECRONS unit when it was destroyed.",
            Effect = "After the attacking unit has resolved its attacks, your unit can shoot as if it were your Shooting phase, but it must target only that enemy unit when doing so, and can only do so if that enemy unit is an eligible target."
        }
    ];
}
