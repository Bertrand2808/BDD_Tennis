namespace SpecFlowTennis
{
    public class Tennis
    {
        public Player PlayerA { get; set; }
        public Player PlayerB { get; set; }
        public EtatMatch EtatDuMatch { get; set; }
        public EtatSet EtatDuSet { get; set; }
        public EtatJeu EtatDuJeu { get; set; }

        public Tennis(string joueurA, string joueurB)
        {
            PlayerA = new Player(joueurA);
            PlayerB = new Player(joueurB);
            EtatDuMatch = EtatMatch.EnCours;
            EtatDuSet = EtatSet.EnCours;
            EtatDuJeu = EtatJeu.EnCours;
        }

        public void MarquerPoint(Player joueur)
        {
            var adversaire = joueur == PlayerA ? PlayerB : PlayerA;
            joueur.ScorePoint(adversaire);
            VerifierEtatJeu();
        }

        public EtatMatch ChangerEtatMatch(EtatMatch etatMatch)
        {
            EtatDuMatch = etatMatch;
            return EtatDuMatch;
        }

        public EtatSet ChangerEtatSet(EtatSet etatSet)
        {
            EtatDuSet = etatSet;
            return EtatDuSet;
        }

        public EtatJeu ChangerEtatJeu(EtatJeu etatJeu)
        {
            EtatDuJeu = etatJeu;
            return EtatDuJeu;
        }

        public void VerifierEtatJeu()
        {
            if (PlayerA.Score == 60)
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else if (PlayerB.Score == 60)
            {
                GagnerJeu(PlayerB, PlayerA);
            }
            else if (PlayerA.Score == 40 && PlayerB.Score < 40)
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else if (PlayerB.Score == 40 && PlayerA.Score < 40)
            {
                GagnerJeu(PlayerB, PlayerA);
            }
            else if (PlayerA.Score == 40 && PlayerB.Score == 40)
            {
                // Both players are at deuce
                PlayerA.Avantage = false;
                PlayerB.Avantage = false;
                ChangerEtatJeu(EtatJeu.EnCours);
            }
            else if (PlayerA.Score == 50)
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
            else if (PlayerB.Score == 50)
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
            else
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
        }

        private void GagnerJeu(Player gagnant, Player perdant)
        {
            gagnant.GagnerJeu();
            gagnant.ResetScore();
            perdant.ResetScore();
            ChangerEtatJeu(EtatJeu.Terminé);
            VerifierEtatSet(gagnant, perdant);
        }

        private void VerifierEtatSet(Player gagnant, Player perdant)
        {
            if (gagnant.Sets[gagnant.CurrentSet] >= 6 && (gagnant.Sets[gagnant.CurrentSet] - perdant.Sets[perdant.CurrentSet]) >= 2)
            {
                ChangerEtatSet(EtatSet.Terminé);
                gagnant.GagnerSet();
                if (gagnant.Sets.Sum() >= 2) // Match au meilleur des 3 sets
                {
                    ChangerEtatMatch(EtatMatch.Terminé);
                }
                else
                {
                    gagnant.CurrentSet += 1;
                    perdant.CurrentSet += 1;
                    ChangerEtatSet(EtatSet.EnCours);
                }
            }
            else
            {
                ChangerEtatSet(EtatSet.EnCours);
            }
        }


        public void SetScores(Dictionary<string, PlayerScore> scores)
        {
            foreach (var entry in scores)
            {
                var player = entry.Key == PlayerA.Name ? PlayerA : PlayerB;
                player.SetScore(entry.Value.Score);
                player.SetSets(entry.Value.Sets);
            }
        }

        public Dictionary<string, PlayerScore> GetScores()
        {
            return new Dictionary<string, PlayerScore>
            {
                { PlayerA.Name, new PlayerScore { Score = PlayerA.Score, Sets = PlayerA.Sets } },
                { PlayerB.Name, new PlayerScore { Score = PlayerB.Score, Sets = PlayerB.Sets } }
            };
        }

        public string GetResultatJeu()
        {
            var jeuxA = PlayerA.Sets[PlayerA.CurrentSet];
            var jeuxB = PlayerB.Sets[PlayerB.CurrentSet];
    
            string joueurEnTete = jeuxA > jeuxB ? PlayerA.Name : PlayerB.Name;
    
            string jeuA = jeuxA == 1 ? "jeu" : "jeux";
            string jeuB = jeuxB == 1 ? "jeu" : "jeux";

            if (jeuxA == jeuxB)
            {
                joueurEnTete = "égalité";
                return $"Le score est de {jeuxA} {jeuA} partout";
            }
    
            if (joueurEnTete == PlayerA.Name)
            {
                return $"Le Joueur {PlayerA.Name} mène {jeuxA} {jeuA} à {jeuxB}";
            }
            else
            {
                return $"{PlayerB.Name} mène {jeuxB} {jeuB} à {jeuxA}";
            }
        }




        public string GetResultatSet()
        {
            var joueurEnTete = PlayerA.Sets.Sum() > PlayerB.Sets.Sum() ? PlayerA.Name : PlayerB.Name;
            var setsA = PlayerA.Sets.Sum();
            var setsB = PlayerB.Sets.Sum();
            return joueurEnTete == PlayerA.Name ? $"Le Joueur {joueurEnTete} remporte le set {setsA} jeux à {setsB}" : $"{joueurEnTete} remporte le set {setsB} jeux à {setsA}";
        }

        public string GetResultatMatch()
        {
            var joueurEnTete = PlayerA.Sets.Sum() > PlayerB.Sets.Sum() ? PlayerA.Name : PlayerB.Name;
            var setsA = PlayerA.Sets;
            var setsB = PlayerB.Sets;

            var resultatSets = new List<string>();
            for (int i = 0; i < setsA.Length; i++)
            {
                if (setsA[i] > 0 || setsB[i] > 0)
                {
                    resultatSets.Add($"{setsA[i]}-{setsB[i]}");
                }
            }

            var resultatSetsStr = string.Join(", ", resultatSets);

            return $"Jeu, set et match. Victoire du Joueur {joueurEnTete} : {resultatSetsStr}";
        }
    }

    public enum EtatJeu
    {
        EnCours,
        Terminé
    }

    public enum EtatSet
    {
        EnCours,
        Terminé
    }

    public enum EtatMatch
    {
        EnCours,
        Terminé
    }
}
