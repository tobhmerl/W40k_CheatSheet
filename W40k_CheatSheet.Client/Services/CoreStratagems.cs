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
            When = "Any phase — after any roll, test, or save.",
            Target = "That unit/model.",
            Effect = "Re-roll that roll, test, or save."
        },
        new()
        {
            Name = "Insane Bravery",
            Cost = "1CP",
            Category = "Core – Epic Deed",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Blue,
            When = "Battle-shock step — before a Battle-shock test.",
            Target = "That unit.",
            Effect = "Auto-pass that Battle-shock test.",
            Restriction = "Once per battle."
        },
        new()
        {
            Name = "New Orders",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Blue,
            When = "End of your Command phase.",
            Target = "1 active Secondary Mission card.",
            Effect = "Discard → draw a new Secondary Mission card."
        },
        new()
        {
            Name = "Heroic Intervention",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Red,
            When = "Enemy Charge phase — after an enemy ends a Charge move.",
            Target = "1 unit ≤6\" of that enemy, eligible to charge it.",
            Effect = "Your unit charges that enemy unit.",
            Restriction = "VEHICLE only if WALKER. No Charge bonus this turn."
        },
        new()
        {
            Name = "Counter-Offensive",
            Cost = "2CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.BlueRed,
            When = "Fight phase — after an enemy has fought.",
            Target = "1 unit in ER of 1+ enemies, not yet fought.",
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
            When = "Enemy Shooting phase — after an enemy selects targets.",
            Target = "1 SMOKE unit targeted by the attacker.",
            Effect = "Til phase end → Cover + Stealth."
        },
        new()
        {
            Name = "Epic Challenge",
            Cost = "1CP",
            Category = "Core – Epic Deed",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.BlueRed,
            RequiredKeywords = ["Character"],
            When = "Fight phase — when a CHARACTER in ER of 1+ Attached units is selected to fight.",
            Target = "1 CHARACTER model in your unit.",
            Effect = "Til phase end → melee attacks gain [PRECISION]."
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
            Target = "1 GRENADES unit (didn't Advance/Fall Back/shoot), not in ER.",
            Effect = "Pick 1 GRENADES model → 1 enemy not in ER, ≤8\" & visible. Roll 6D6: each 4+ = 1 MW."
        },
        new()
        {
            Name = "Tank Shock",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Vehicle"],
            When = "Your Charge phase — after a VEHICLE ends a Charge move.",
            Target = "That VEHICLE unit.",
            Effect = "Pick 1 enemy in ER → roll D6 = your VEHICLE's T. Each 5+ = 1 MW (max 6)."
        },
        new()
        {
            Name = "Rapid Ingress",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Red,
            When = "End of enemy Movement phase.",
            Target = "1 unit in Reserves.",
            Effect = "Arrive as Reinforcements. Deep Strike models can Deep Strike.",
            Restriction = "Normal battle round arrival restrictions apply."
        },
        new()
        {
            Name = "Fire Overwatch",
            Cost = "1CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Move | GamePhase.Charge,
            TurnColor = StratagemTurn.Red,
            When = "Enemy Move/Charge phase — after enemy set up, starts/ends a move, or declares a Charge.",
            Target = "1 non-TITANIC unit ≤24\" of that enemy, eligible to shoot.",
            Effect = "If visible → shoot that enemy unit.",
            Restriction = "Nat 6 to hit only. Once per turn."
        },
        new()
        {
            Name = "Go To Ground",
            Cost = "1CP",
            Category = "Core – Battle Tactic",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            RequiredKeywords = ["Infantry"],
            When = "Enemy Shooting phase — after an enemy selects targets.",
            Target = "1 INFANTRY unit targeted by the attacker.",
            Effect = "Til phase end → 6+ invuln + Cover."
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
            Target = "1 NECRONS unit, not yet shot.",
            Effect = "Til phase end → ≤half range: re-roll hit rolls of 1. With CHARACTER leader: re-roll any hit roll instead."
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
            When = "Enemy Shooting phase or Fight phase — after an enemy resolves attacks.",
            Target = "1 NECRONS unit that lost 1+ models from those attacks.",
            Effect = "Reanimate D3 wounds. With CHARACTER leader: D3+1."
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
            When = "Any phase — when a NECRONS INFANTRY CHARACTER is destroyed.",
            Target = "That CHARACTER model (even though just destroyed).",
            Effect = "End of phase → return near where destroyed, not in ER, at half wounds.",
            Restriction = "Once per model per battle."
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
            Target = "1 NECRONS unit, not yet fought.",
            Effect = "Til phase end → melee weapons +1S. With CHARACTER leader: also melee AP improved by 1."
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
            Target = "1 NECRONS unit.",
            Effect = "Til turn end → ranged weapons gain [ASSAULT]. With CHARACTER leader: also re-roll Advance rolls."
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
            When = "Enemy Shooting phase — after an enemy destroys a NECRONS unit.",
            Target = "1 NECRONS CHARACTER unit ≤6\" of the destroyed unit.",
            Effect = "Shoot back at that enemy unit (if eligible target)."
        }
    ];
}
