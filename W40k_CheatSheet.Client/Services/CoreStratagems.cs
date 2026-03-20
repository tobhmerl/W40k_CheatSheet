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
            Effect = "Re-roll that roll, test, or save.",
            FullWhen = "Any phase, just after you have made a Hit roll, a Wound roll, a Damage roll, a saving throw, a Feel No Pain roll, an Advance roll, a Charge roll, a Desperate Escape roll, a Hazardous roll, or just after you have rolled the dice to determine the number of attacks made with a weapon, for an attack, model or unit from your army.",
            FullTarget = "That attack, model or unit.",
            FullEffect = "Re-roll that roll, saving throw or test."
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
            Restriction = "Once per battle.",
            FullWhen = "Battle-shock step of your Command phase, just before you take a Battle-shock test for a unit from your army.",
            FullTarget = "That unit from your army.",
            FullEffect = "Your unit automatically passes that Battle-shock test.",
            FullRestriction = "You cannot use this Stratagem more than once per battle."
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
            Effect = "Discard → draw a new Secondary Mission card.",
            FullWhen = "End of your Command phase.",
            FullTarget = "One of your active Secondary Mission cards.",
            FullEffect = "Discard that Secondary Mission card and draw a new Secondary Mission card."
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
            Restriction = "VEHICLE only if WALKER. No Charge bonus this turn.",
            FullWhen = "Your opponent's Charge phase, just after an enemy unit ends a Charge move.",
            FullTarget = "One unit from your army that is within 6\" of that enemy unit and would be eligible to declare a charge against that enemy unit if it were your Charge phase.",
            FullEffect = "Your unit now declares a charge that targets only that enemy unit, and you resolve that charge as if it were your Charge phase.",
            FullRestriction = "You cannot select a Vehicle unit unless it has the Walker keyword. You cannot select a unit that is within Engagement Range of one or more enemy units. The charging unit does not receive any Charge bonus this turn."
        },
        new()
        {
            Name = "Counter-Offensive",
            Cost = "2CP",
            Category = "Core – Strategic Ploy",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            When = "Fight phase — after an enemy has fought.",
            Target = "1 unit in ER of 1+ enemies, not yet fought.",
            Effect = "Your unit fights next.",
            FullWhen = "Fight phase, just after an enemy unit has fought.",
            FullTarget = "One unit from your army that is within Engagement Range of one or more enemy units and that has not already been selected to fight this phase.",
            FullEffect = "Your unit fights next."
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
            Effect = "Til phase end → Cover + Stealth.",
            FullWhen = "Your opponent's Shooting phase, just after an enemy unit has selected its targets.",
            FullTarget = "One unit from your army that was selected as the target of one or more of the attacking unit's attacks and has the Smoke keyword.",
            FullEffect = "Until the end of the phase, your unit has the Benefit of Cover and the Stealth ability."
        },
        new()
        {
            Name = "Epic Challenge",
            Cost = "1CP",
            Category = "Core – Epic Deed",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            RequiredKeywords = ["Character"],
            When = "Fight phase — when a CHARACTER in ER of 1+ Attached units is selected to fight.",
            Target = "1 CHARACTER model in your unit.",
            Effect = "Til phase end → melee attacks gain [PRECISION].",
            FullWhen = "Fight phase, when a unit from your army that contains a Character model that is within Engagement Range of one or more Attached units is selected to fight.",
            FullTarget = "One Character model in your unit.",
            FullEffect = "Until the end of the phase, melee weapons equipped by that model have the [Precision] ability."
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
            Effect = "Pick 1 GRENADES model → 1 enemy not in ER, ≤8\" & visible. Roll 6D6: each 4+ = 1 MW.",
            FullWhen = "Your Shooting phase.",
            FullTarget = "One unit from your army that has the Grenades keyword, has not been selected to shoot this phase, and is not within Engagement Range of one or more enemy units. That unit is not eligible to shoot this phase.",
            FullEffect = "Select one model in your unit; that model makes a ranged attack against one enemy unit that is not within Engagement Range of one or more units from your army and is within 8\" of and visible to that model. When resolving that attack, do not make a Hit roll or a Wound roll: instead roll six D6, and for each 4+ the target suffers 1 mortal wound."
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
            Effect = "Pick 1 enemy in ER → roll D6 = your VEHICLE's T. Each 5+ = 1 MW (max 6).",
            FullWhen = "Your Charge phase, just after a Vehicle unit from your army ends a Charge move.",
            FullTarget = "That Vehicle unit.",
            FullEffect = "Select one enemy unit within Engagement Range of your unit and roll a number of D6 equal to the Toughness characteristic of your unit. For each 5+, that enemy unit suffers 1 mortal wound (to a maximum of 6 mortal wounds)."
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
            Restriction = "Normal battle round arrival restrictions apply.",
            FullWhen = "End of your opponent's Movement phase.",
            FullTarget = "One unit from your army that is in Reserves.",
            FullEffect = "Set up your unit anywhere on the battlefield that is more than 9\" horizontally away from all enemy models. If your unit has the Deep Strike ability, you can set it up as described in that ability instead.",
            FullRestriction = "A unit cannot be set up on the battlefield using this Stratagem during a battle round in which it was placed into Reserves."
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
            Restriction = "Nat 6 to hit only. Once per turn.",
            FullWhen = "Your opponent's Movement or Charge phase, just after an enemy unit is set up or when an enemy unit starts or ends a Normal, Advance, Fall Back, or Charge move, or just after an enemy unit declares a charge.",
            FullTarget = "One unit from your army that is within 24\" of that enemy unit and that would be eligible to shoot if it were your Shooting phase, excluding Titanic units.",
            FullEffect = "Your unit can shoot that enemy unit as if it were your Shooting phase. When resolving those attacks, your unit can only target that enemy unit, and can only do so if that enemy unit is an eligible target. In addition, unmodified Hit rolls of 1–5 automatically fail, irrespective of any abilities that say otherwise.",
            FullRestriction = "You can only use this Stratagem once per turn."
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
            Effect = "Til phase end → 6+ invuln + Cover.",
            FullWhen = "Your opponent's Shooting phase, just after an enemy unit has selected its targets.",
            FullTarget = "One Infantry unit from your army that was selected as the target of one or more of the attacking unit's attacks.",
            FullEffect = "Until the end of the phase, all models in your unit have a 6+ invulnerable save and have the Benefit of Cover."
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
            Effect = "Til phase end → ≤half range: re-roll hit rolls of 1. With CHARACTER leader: re-roll any hit roll instead.",
            FullWhen = "Your Shooting phase.",
            FullTarget = "One Necrons unit from your army that has not been selected to shoot this phase.",
            FullEffect = "Until the end of the phase, each time a model in your unit makes a ranged attack that targets a unit within half range, re-roll a Hit roll of 1. If your unit is being led by a Character model, you can re-roll the Hit roll instead."
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
            TurnColor = StratagemTurn.Green,
            When = "Enemy Shooting phase or Fight phase — after an enemy resolves attacks.",
            Target = "1 NECRONS unit that lost 1+ models from those attacks.",
            Effect = "Reanimate D3 wounds. With CHARACTER leader: D3+1.",
            FullWhen = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has resolved its attacks.",
            FullTarget = "One Necrons unit from your army that had one or more of its models destroyed as a result of the attacking unit's attacks.",
            FullEffect = "D3 wounds' worth of slain models are returned to your unit. If your unit is being led by a Character model, return D3+1 wounds' worth of slain models instead."
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
            Restriction = "Once per model per battle.",
            FullWhen = "Any phase, when a Necrons Infantry Character model from your army is destroyed.",
            FullTarget = "That Character model (even though it was just destroyed).",
            FullEffect = "At the end of the phase, set that model back up on the battlefield as close as possible to where it was destroyed and not within Engagement Range of any enemy models, with half of its starting number of wounds remaining (rounding up).",
            FullRestriction = "Each model can only be the target of this Stratagem once per battle."
        },
        new()
        {
            Name = "Protocol of the Hungry Void",
            Cost = "1CP",
            Category = "Awakened Dynasty – Battle Tactic",
            Detachment = "Awakened Dynasty",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            When = "Fight phase.",
            Target = "1 NECRONS unit, not yet fought.",
            Effect = "Til phase end → melee weapons +1S. With CHARACTER leader: also melee AP improved by 1.",
            FullWhen = "Fight phase.",
            FullTarget = "One Necrons unit from your army that has not been selected to fight this phase.",
            FullEffect = "Until the end of the phase, melee weapons equipped by models in your unit have a Strength characteristic that is 1 higher. If your unit is being led by a Character model, those melee weapons also have an Armour Penetration characteristic that is 1 better."
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
            Effect = "Til turn end → ranged weapons gain [ASSAULT]. With CHARACTER leader: also re-roll Advance rolls.",
            FullWhen = "Your Movement phase.",
            FullTarget = "One Necrons unit from your army.",
            FullEffect = "Until the end of the turn, ranged weapons equipped by models in your unit have the [Assault] ability. If your unit is being led by a Character model, until the end of the turn you can also re-roll Advance rolls made for your unit."
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
            Effect = "Shoot back at that enemy unit (if eligible target).",
            FullWhen = "Your opponent's Shooting phase, just after an enemy unit has resolved its attacks against one of your Necrons units and that Necrons unit is destroyed.",
            FullTarget = "One Necrons Character unit from your army that is within 6\" of your destroyed unit.",
            FullEffect = "Your unit can shoot as if it were your Shooting phase, but it must target only the enemy unit that destroyed your unit, and can only do so if that enemy unit is an eligible target."
        },

        // ── Hypercrypt Legion ──

        new()
        {
            Name = "Dimensional Corridor",
            Cost = "2CP",
            Category = "Hypercrypt Legion – Strategic Ploy",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Blue,
            When = "Your Charge phase.",
            Target = "1 NECRONS unit set up this turn using a MONOLITH's Eternity Gate ability (MONOLITH started turn on battlefield).",
            Effect = "Your unit is eligible to charge this phase.",
            FullWhen = "Your Charge phase.",
            FullTarget = "One Necrons unit from your army that was set up on the battlefield this turn using the Eternity Gate ability of a Monolith model that started the turn on the battlefield.",
            FullEffect = "Your unit is eligible to declare a charge this phase."
        },
        new()
        {
            Name = "Reanimation Crypts",
            Cost = "1CP",
            Category = "Hypercrypt Legion – Strategic Ploy",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Blue,
            When = "Your Command phase.",
            Target = "Your NECRONS WARLORD.",
            Effect = "For each NECRONS unit in Reserves, that unit's Reanimation Protocols activate.",
            FullWhen = "Your Command phase.",
            FullTarget = "Your Necrons Warlord.",
            FullEffect = "For each of your Necrons units in Reserves, that Reserves unit's Reanimation Protocols activate."
        },
        new()
        {
            Name = "Cosmic Precision",
            Cost = "1CP",
            Category = "Hypercrypt Legion – Strategic Ploy",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Blue,
            When = "Your Movement phase.",
            Target = "1 NECRONS unit (not MONSTER) arriving via Deep Strike or Hyperphasing this phase.",
            Effect = "Set up anywhere on the battlefield >6\" from all enemies.",
            Restriction = "Not eligible to charge this turn.",
            FullWhen = "Your Movement phase.",
            FullTarget = "One Necrons unit from your army (excluding Monster units) that is arriving using the Deep Strike or Hyperphasing abilities this phase.",
            FullEffect = "Your unit can be set up anywhere on the battlefield that is more than 6\" horizontally away from all enemy models.",
            FullRestriction = "A unit targeted with this Stratagem is not eligible to declare a charge in the same turn."
        },
        new()
        {
            Name = "Entropic Damping",
            Cost = "1CP",
            Category = "Hypercrypt Legion – Wargear",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            When = "Enemy Shooting phase — after an enemy selects targets.",
            Target = "1 TITANIC model targeted by the attacker and ≤18\" of it.",
            Effect = "Til phase end → weapons equipped by models in the attacking unit gain [HAZARDOUS].",
            FullWhen = "Your opponent's Shooting phase, just after an enemy unit has selected its targets.",
            FullTarget = "One Titanic model from your army that was selected as the target of one or more of the attacking unit's attacks and is within 18\" of the attacking unit.",
            FullEffect = "Until the end of the phase, weapons equipped by models in the attacking unit have the [Hazardous] ability."
        },
        new()
        {
            Name = "Hyperphasic Recall",
            Cost = "2CP",
            Category = "Hypercrypt Legion – Strategic Ploy",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            MyTurnPhases = GamePhase.Fight,
            EnemyTurnPhases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            When = "Enemy Shooting phase or Fight phase — after an enemy has shot or fought.",
            Target = "1 NECRONS INFANTRY unit that lost 1+ models from those attacks + 1 friendly MONOLITH.",
            Effect = "Remove INFANTRY unit → set back up wholly within 6\" of your MONOLITH, not in ER of enemies.",
            FullWhen = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has shot or fought.",
            FullTarget = "One Necrons Infantry unit from your army that had one or more of its models destroyed as a result of the attacking unit's attacks and one friendly Monolith model.",
            FullEffect = "Remove your Infantry unit from the battlefield and then set it back up anywhere on the battlefield that is wholly within 6\" of your Monolith model and not within Engagement Range of one or more enemy units."
        },
        new()
        {
            Name = "Quantum Deflection",
            Cost = "1CP",
            Category = "Hypercrypt Legion – Wargear",
            Detachment = "Hypercrypt Legion",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            MyTurnPhases = GamePhase.Fight,
            EnemyTurnPhases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            When = "Enemy Shooting phase or Fight phase — after an enemy selects targets.",
            Target = "1 NECRONS VEHICLE unit targeted by the attacker.",
            Effect = "Til phase end → models in your unit have 4+ invuln.",
            FullWhen = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has selected its targets.",
            FullTarget = "One Necrons Vehicle unit from your army that was selected as the target of one or more of the attacking unit's attacks.",
            FullEffect = "Until the end of the phase, models in your unit have a 4+ invulnerable save."
        },

        // ── Wrath of the Rock ──

        new()
        {
            Name = "Tactical Mastery",
            Cost = "1CP",
            Category = "Wrath of the Rock – Battle Tactic",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Blue,
            When = "Your Movement phase.",
            Target = "1 ADEPTUS ASTARTES unit.",
            Effect = "Til end of turn → eligible to shoot and charge after Advancing. RAVENWING: also after Falling Back.",
            FullWhen = "Your Movement phase.",
            FullTarget = "One Adeptus Astartes unit from your army.",
            FullEffect = "Until the end of the turn, your unit is eligible to shoot and declare a charge in a turn in which it Advanced. If your unit has the Ravenwing keyword, it is also eligible to shoot and declare a charge in a turn in which it Fell Back."
        },
        new()
        {
            Name = "Relics of the Dark Age",
            Cost = "1CP",
            Category = "Wrath of the Rock – Strategic Ploy",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Infantry", "Mounted"],
            When = "Your Shooting phase.",
            Target = "1 ADEPTUS ASTARTES INFANTRY or MOUNTED unit, not yet shot.",
            Effect = "Til phase end → ranged weapons +2S.",
            FullWhen = "Your Shooting phase.",
            FullTarget = "One Adeptus Astartes Infantry or Adeptus Astartes Mounted unit from your army that has not been selected to shoot this phase.",
            FullEffect = "Until the end of the phase, add 2 to the Strength characteristic of ranged weapons equipped by models in your unit."
        },
        new()
        {
            Name = "Leonine Aggression",
            Cost = "1CP",
            Category = "Wrath of the Rock – Strategic Ploy",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.Charge,
            TurnColor = StratagemTurn.Red,
            When = "End of enemy Charge phase.",
            Target = "1 ADEPTUS ASTARTES unit ≤3\" of enemies, or 1 DEATHWING unit ≤6\" of enemies.",
            Effect = "Your unit charges those enemy units.",
            Restriction = "No Charge bonus this turn.",
            FullWhen = "End of your opponent's Charge phase.",
            FullTarget = "One Adeptus Astartes unit from your army within 3\" of one or more enemy units, or one Deathwing unit from your army within 6\" of one or more enemy units.",
            FullEffect = "Your unit now declares a charge that only targets one or more of those enemy units, and you resolve that charge.",
            FullRestriction = "Note that even if this charge is successful, your unit does not receive any Charge bonus this turn."
        },
        new()
        {
            Name = "Inescapable Justice",
            Cost = "2CP",
            Category = "Wrath of the Rock – Battle Tactic",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.All,
            TurnColor = StratagemTurn.Green,
            RequiredKeywords = ["Character"],
            When = "Any phase — after your Oath of Moment target is destroyed.",
            Target = "1 ADEPTUS ASTARTES CHARACTER unit on the battlefield.",
            Effect = "Pick 1 enemy ≤12\" & visible → becomes Oath of Moment target til next Command phase.",
            FullWhen = "Any phase, just after your Oath of Moment target is destroyed.",
            FullTarget = "One Adeptus Astartes Character unit that is on the battlefield.",
            FullEffect = "Select one enemy unit within 12\" and visible to your unit. That enemy unit becomes your Oath of Moment target until the start of your next Command phase."
        },
        new()
        {
            Name = "Lion's Will",
            Cost = "1CP",
            Category = "Wrath of the Rock – Strategic Ploy",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.Command,
            TurnColor = StratagemTurn.Green,
            When = "Command phase.",
            Target = "1 ADEPTUS ASTARTES unit in ER of 1+ enemies.",
            Effect = "Til next Command phase → +1 OC. Non-DEATHWING/RAVENWING/VEHICLE: also +1 to hit.",
            FullWhen = "Command phase.",
            FullTarget = "One Adeptus Astartes unit from your army that is within Engagement Range of one or more enemy units.",
            FullEffect = "Until the start of your next Command phase, add 1 to the Objective Control characteristic of models in your unit. In addition, until the end of the turn, if your unit does not have the Deathwing, Ravenwing or Vehicle keyword, each time a model in your unit makes an attack, add 1 to the Hit roll."
        },
        new()
        {
            Name = "Armour of Contempt",
            Cost = "1CP",
            Category = "Wrath of the Rock – Battle Tactic",
            Detachment = "Wrath of the Rock",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            MyTurnPhases = GamePhase.Fight,
            EnemyTurnPhases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            When = "Enemy Shooting phase or Fight phase — after an enemy selects targets.",
            Target = "1 ADEPTUS ASTARTES unit targeted by the attacker.",
            Effect = "Til attacker finishes → worsen AP of attacks targeting your unit by 1.",
            FullWhen = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has selected its targets.",
            FullTarget = "One Adeptus Astartes unit from your army that was selected as the target of one or more of the attacking unit's attacks.",
            FullEffect = "Until the attacking unit has finished making its attacks, each time an attack targets your unit, worsen the Armour Penetration characteristic of that attack by 1."
        },

        // ── Starshatter Arsenal ──

        new()
        {
            Name = "Endless Servitude",
            Cost = "1CP",
            Category = "Starshatter Arsenal – Strategic Ploy",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Fight,
            TurnColor = StratagemTurn.Blue,
            When = "End of your Fight phase.",
            Target = "1 NECRONS unit (excl. TITANIC) in range of a controlled objective.",
            Effect = "Reanimation Protocols activate.",
            FullWhen = "End of your Fight phase.",
            FullTarget = "One Necrons unit (excluding Titanic units) from your army that is within range of one or more objective markers you control.",
            FullEffect = "Your unit's Reanimation Protocols activate."
        },
        new()
        {
            Name = "Chronoshift",
            Cost = "1CP",
            Category = "Starshatter Arsenal – Strategic Ploy",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Vehicle", "Mounted"],
            When = "Your Movement phase.",
            Target = "1 NECRONS VEHICLE or MOUNTED unit (excl. TITANIC), not yet moved.",
            Effect = "If Advancing → no Advance roll, instead +6\" to M.",
            FullWhen = "Your Movement phase.",
            FullTarget = "One Necrons Vehicle or Necrons Mounted unit (excluding Titanic units) from your army that has not been selected to move this phase.",
            FullEffect = "Until the end of the phase, if your unit Advances, do not make an Advance roll for it. Instead, until the end of the phase, add 6\" to the Move characteristic of models in your unit."
        },
        new()
        {
            Name = "Dimensional Tunnel",
            Cost = "1CP",
            Category = "Starshatter Arsenal – Strategic Ploy",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Move,
            TurnColor = StratagemTurn.Blue,
            RequiredKeywords = ["Vehicle", "Mounted"],
            When = "Your Movement phase.",
            Target = "1 NECRONS VEHICLE or MOUNTED unit (excl. TITANIC).",
            Effect = "Til phase end → move through models and terrain horizontally.",
            FullWhen = "Your Movement phase.",
            FullTarget = "One Necrons Vehicle or Necrons Mounted unit (excluding Titanic units) from your army.",
            FullEffect = "Until the end of the phase, models in your unit can move horizontally through models and terrain features."
        },
        new()
        {
            Name = "Reactive Reposition",
            Cost = "1CP",
            Category = "Starshatter Arsenal – Strategic Ploy",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Shoot,
            TurnColor = StratagemTurn.Red,
            When = "Enemy Shooting phase — after an enemy unit has shot.",
            Target = "1 NECRONS unit (excl. TITANIC) targeted by the attacker.",
            Effect = "Normal move up to D6\".",
            FullWhen = "Your opponent's Shooting phase, just after an enemy unit has shot.",
            FullTarget = "One Necrons unit from your army (excluding Titanic units) that was the target of one or more of the attacking unit's attacks.",
            FullEffect = "Your unit can make a Normal move of up to D6\"."
        },
        new()
        {
            Name = "Merciless Reclamation",
            Cost = "2CP",
            Category = "Starshatter Arsenal – Battle Tactic",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.Blue,
            When = "Your Shooting phase or Fight phase.",
            Target = "1 NECRONS unit (excl. TITANIC), not yet shot or fought.",
            Effect = "Til phase end → +1 to wound if target is in range of an objective.",
            FullWhen = "Your Shooting phase or the Fight phase.",
            FullTarget = "One Necrons unit (excluding Titanic units) from your army that has not been selected to shoot or fight this phase.",
            FullEffect = "Until the end of the phase, each time a model in your unit makes an attack, if the target of that attack is within range of one or more objective markers, add 1 to the Wound roll."
        },
        new()
        {
            Name = "Unyielding Forms",
            Cost = "2CP",
            Category = "Starshatter Arsenal – Battle Tactic",
            Detachment = "Starshatter Arsenal",
            Phases = GamePhase.Shoot | GamePhase.Fight,
            MyTurnPhases = GamePhase.Fight,
            EnemyTurnPhases = GamePhase.Shoot | GamePhase.Fight,
            TurnColor = StratagemTurn.Green,
            RequiredKeywords = ["Vehicle", "Mounted"],
            When = "Enemy Shooting phase or Fight phase — after an enemy selects targets.",
            Target = "1 NECRONS VEHICLE or MOUNTED unit (excl. TITANIC) targeted by the attacker.",
            Effect = "Til phase end → if S > T, subtract 1 from wound roll.",
            FullWhen = "Your opponent's Shooting phase or the Fight phase, just after an enemy unit has selected its targets.",
            FullTarget = "One Necrons Vehicle or Necrons Mounted unit (excluding Titanic units) from your army that was selected as the target of one or more of the attacking unit's attacks.",
            FullEffect = "Until the end of the phase, each time an attack targets a model in your unit, if the Strength characteristic of that attack is greater than the Toughness characteristic of that unit, subtract 1 from the Wound roll."
        }
    ];
}
