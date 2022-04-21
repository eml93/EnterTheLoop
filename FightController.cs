namespace EnterTheLoop
{
    public class FightController
    {
        private static Queue<Goon> goonQueue = new Queue<Goon>();
        private static List<TurnHistory> fightHistory = new List<TurnHistory>();
        private static int turnCounter = 0;
        private static int goonCounter = 0;

        public static FightResults IndividualFight(Character c, Fight f) {
            //setup
            ResetStaticFields(c, f);

            foreach (Goon g in f.Goons)
            {
                goonQueue.Enqueue(g.Copy());
            }

            while(goonQueue.Count > 0 && !c.IsDead) {
                turnCounter++;
                updateOngoingPerks();

                // Character's turn
                if (turnCounter % 2 == 1) {
                    TakeCharacterTurn(c);
                } else {
                    TakeGoonTurn(c);
                }
            }

            return new FightResults(f, fightHistory, turnCounter, c, goonQueue);
        }

        private static void updateOngoingPerks()
        {
            for (int i = 0; i < goonQueue.Count; i++)
            {
                String currentGoonName = goonQueue.ElementAt(i).Name;
                if (currentGoonName.Equals("Officer")) {
                    if (i - 1 >= 0) {
                        goonQueue.ElementAt(i - 1).Dmg += 1;
                    }
                }
            }
        }

        private static void ResetStaticFields(Character c, Fight f)
        {
            c.Reset();
            f.Reset();
            goonQueue = new Queue<Goon>();
            fightHistory = new List<TurnHistory>();
            turnCounter = 0;
            goonCounter = 0;
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
                goonQueue.Dequeue();
            }

            fightHistory.Add(new CharacterHistory(turnCounter, c, currentGoon, attackInHits, usedHeal, isGoonDead));
        }
       private static void TakeGoonTurn(Character c)
        {
            if (goonCounter >= goonQueue.Count) {
                goonCounter = 0;
            }

            Goon attackingGoon = goonQueue.ElementAt(goonCounter);
            int goonDmg = attackingGoon.Dmg;

            int characterDamageTaken = c.TakeDamage(goonDmg);

            fightHistory.Add(new GoonHistory(turnCounter, attackingGoon, c, goonCounter, goonDmg, c.IsDead, characterDamageTaken));

            goonCounter++;
        }
    
    }
}