using EnterTheLoop.Goons;

namespace EnterTheLoop
{
    public class FightController
    {
        private static Queue<Goon> goonQueue = new Queue<Goon>();
        private static List<TurnHistory> fightHistory = new List<TurnHistory>();
        private static int turnCounter = 0;
        private static int goonCounter = 0;

        public static FightResults StartFight(Character c, Fight fight, String playerSide) {
            return StartFight(c, new List<Fight> {fight}, playerSide);
        }

        public static FightResults StartFight(Character c, List<Fight> allFights, String playerSide) {
            //setup
            ResetStaticFields(c, allFights);

            List<Goon> goonsInFight = allFights.SelectMany(f => f.Goons).ToList();

            if (playerSide.Equals("Right")) {
                goonsInFight.Reverse();
            }

            foreach (Goon g in goonsInFight)
            {
                goonQueue.Enqueue(g.Copy());
            }

            // trigger all OnStart Goon Perks
            foreach (Goon g in goonQueue)
            {
                bool hasPerkBeenTriggered = g.TriggerPerk(PerkTrigger.AtStart);

                if (hasPerkBeenTriggered) {
                    TurnOnAtStartPerk(g);
                }
            }

            // Begin combat
            while(goonQueue.Count > 0 && !c.IsDead) {
                turnCounter++;

                // Character's turn
                if (turnCounter % 2 == 1) {
                    TakeCharacterTurn(c);
                } else {
                    TakeGoonTurn(c);
                }
            }

            return new FightResults(allFights, fightHistory, turnCounter, c, goonQueue, playerSide);
        }

        private static void TurnOnAtStartPerk(Goon g)
        {
            TurnOnAndOffAtStartPerk(g, 1);
        }

        private static void TurnOffAtStartPerk(Goon g)
        {
            TurnOnAndOffAtStartPerk(g, -1);
        }

        private static void TurnOnAndOffAtStartPerk(Goon g, int onOrOff)
        {
            int currentGoonIndex = -1;
            switch(g) {
                case OfficerGoon officerGoon:
                    currentGoonIndex = goonQueue.ToList().IndexOf(g);
                    if (currentGoonIndex > 0) {
                        goonQueue.ElementAt(currentGoonIndex - 1).Dmg += onOrOff;
                    }

                    if (currentGoonIndex < goonQueue.Count - 2) {
                        goonQueue.ElementAt(currentGoonIndex + 1).Dmg += onOrOff;
                    }
                break;

                case DefenderGoon defenderGoon:
                    currentGoonIndex = goonQueue.ToList().IndexOf(g);
                    if (currentGoonIndex > 0) {
                        goonQueue.ElementAt(currentGoonIndex - 1).Hearts += onOrOff;
                    }

                    if (currentGoonIndex < goonQueue.Count - 2) {
                        goonQueue.ElementAt(currentGoonIndex + 1).Hearts += onOrOff;
                    }
                break;
            }
        }

        private static void TriggerOnDeathPerk(Goon currentGoon, Character c)
        {
            switch(currentGoon) {
                case DrunkGoon drunkGoon:
                    c.TakeDamage(3);
                    break;
            }
        }

        private static void TakeCharacterTurn(Character c)
        {
            bool usedHeal = false;
            Goon currentGoon = goonQueue.Peek();
            float goonHpBeforeAttack = currentGoon.Hearts;

            // use healing if goon would kill them next turn
            if (currentGoon.Dmg >= c.Hp && c.HasHealing) {
                c.UseCrimsonAsh();
                usedHeal = true;
            }

            // rolls attack and returns the Direct and Partial hits
            int[] attackInHits = c.RollGun();
            // converts the Direct and Partial hits into a total float value
            float totalAttackInHearts = attackInHits[0] * 1 + attackInHits[1] * .5f;

            bool isGoonDead = currentGoon.TakeDamage(totalAttackInHearts);

            if (isGoonDead) {
                // if the current Goon triggered its Perk at the start of the Fight, turn off that Perk
                switch(currentGoon.WhenDoesPerkTrigger) {
                    case PerkTrigger.AtStart:
                        TurnOffAtStartPerk(currentGoon);
                        break;
                    case PerkTrigger.OnDeath:
                        HandleGoonPerk(currentGoon, c, PerkTrigger.OnDeath);
                        break;
                }

                goonQueue.Dequeue();
            }

            CharacterHistory ch = new CharacterHistory(turnCounter, c, currentGoon, attackInHits, usedHeal, isGoonDead);
            ch.DrunkExploded = isGoonDead && currentGoon is DrunkGoon;
            fightHistory.Add(ch);
        }

        private static void TakeGoonTurn(Character c)
        {
            if (goonCounter >= goonQueue.Count) {
                goonCounter = 0;
            }

            Goon attackingGoon = goonQueue.ElementAt(goonCounter);

            bool skipTurn = false;
            if (attackingGoon.PerkCondition == null || attackingGoon.PerkCondition(goonQueue.Count)) {
                skipTurn = HandleGoonPerk(attackingGoon, c, PerkTrigger.OnTurn, goonCounter);
                // TriggerPerk returns TriggerResponse, which can be internal, meaning effecting just the GOon, or external, meanign it effects the whole fight
            }

            // need to Skip these steps if the HandleGoonPerk returns true
            int goonDmg = 0;
            int characterDamageTaken = 0;

            if (!skipTurn) {
                goonDmg = attackingGoon.Dmg;
                characterDamageTaken = c.TakeDamage(goonDmg);
            } 

            fightHistory.Add(new GoonHistory(turnCounter, attackingGoon, c, goonCounter, goonDmg, c.IsDead, characterDamageTaken));

            goonCounter++;
        }
        
        private static bool HandleGoonPerk(Goon currentGoon, Character c, PerkTrigger perkTrigger)
        {
            return HandleGoonPerk(currentGoon, c, perkTrigger, -1);
        }

        private static bool HandleGoonPerk(Goon attackingGoon, Character c, PerkTrigger perkTrigger, int goonCounter)
        {
            bool isPerkTriggered = attackingGoon.TriggerPerk(perkTrigger);

            if (isPerkTriggered) {
                switch (attackingGoon) {
                    case DrunkGoon drunkGoon:
                        c.TakeDamage(3);
                        break;
                     case FlagellatorGoon flagellatorGoon:
                        return true;
                    case CellGoon cellGoon:
                        goonQueue.ToList().ForEach(g => {g.Dmg+=1; g.Hearts+=1;});
                        break;
                }
            }

            return false;

        }

        private static void ResetStaticFields(Character c, List<Fight> allFights)
        {
            c.Reset();
            allFights.ForEach(f => f.Reset());
            goonQueue = new Queue<Goon>();
            fightHistory = new List<TurnHistory>();
            turnCounter = 0;
            goonCounter = 0;
        }
    
    }
}