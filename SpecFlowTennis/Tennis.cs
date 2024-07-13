namespace SpecFlowTennis
{
    public class Tennis
    {
        public Player PlayerA { get; set; }
        public Player PlayerB { get; set; }
        public EtatMatch EtatDuMatch { get; private set; }
        public EtatSet EtatDuSet { get; private set; }
        public EtatJeu EtatDuJeu { get; private set; }

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
            if (EtatDuSet == EtatSet.TieBreak)
            {
                joueur.ScoreTieBreak(adversaire);
            }
            else
            {
                joueur.ScorePoint(adversaire);
            }
            VerifierEtatJeu();
        }

        private EtatMatch ChangerEtatMatch(EtatMatch etatMatch)
        {
            EtatDuMatch = etatMatch;
            return EtatDuMatch;
        }

        public EtatSet ChangerEtatSet(EtatSet etatSet)
        {
            EtatDuSet = etatSet;
            return EtatDuSet;
        }

        private EtatJeu ChangerEtatJeu(EtatJeu etatJeu)
        {
            EtatDuJeu = etatJeu;
            return EtatDuJeu;
        }

        /* 
         * Vérifie l'état du jeu en cours
         * (refactorisation)
         */
        private void VerifierEtatJeu()
        {
            if (EtatDuSet == EtatSet.TieBreak)
            {
                VerifierEtatJeuTieBreak();
            }
            else
            {
                VerifierEtatJeuNormal();
            }
        }
        
        /* 
         * Vérifie l'état du jeu en cours en cas de tie break (refactorisation de VerifierEtatJeu)
         */
        private void VerifierEtatJeuTieBreak()
        {
            if (PlayerA.Score >= 7 && PlayerA.Score - PlayerB.Score >= 2)
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else if (PlayerB.Score >= 7 && PlayerB.Score - PlayerA.Score >= 2)
            {
                GagnerJeu(PlayerB, PlayerA);
            }
            else
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
        }
        
        /* 
         * Vérifie l'état du jeu en cours en dehors du tie break (refactorisation de VerifierEtatJeu)
         */
        private void VerifierEtatJeuNormal()
        {
            if (PlayerA.Score == 60 || (PlayerA.Score == 40 && PlayerB.Score < 40))
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else if (PlayerB.Score == 60 || (PlayerB.Score == 40 && PlayerA.Score < 40))
            {
                GagnerJeu(PlayerB, PlayerA);
            }
            else if (PlayerA.Score == 40 && PlayerB.Score == 40)
            {
                PlayerA.Avantage = false;
                PlayerB.Avantage = false;
                ChangerEtatJeu(EtatJeu.EnCours);
            }
            else if (PlayerA.Score == 50 || PlayerB.Score == 50)
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
            else
            {
                ChangerEtatJeu(EtatJeu.EnCours);
            }
        }
        
        /* 
         * Gagne le jeu pour le joueur gagnant et vérifie si le set est terminé en cas de tie break
         */
        private void GagnerJeu(Player gagnant, Player perdant)
        {
            gagnant.GagnerJeu();
            gagnant.ResetScore();
            perdant.ResetScore();
            ChangerEtatJeu(EtatJeu.Terminé);
            if (EtatDuSet == EtatSet.TieBreak)
            {
                gagnant.GagnerSet();
                ChangerEtatSet(EtatSet.Terminé);
                if (gagnant.Sets.Sum() >= 2) // Match au meilleur des 3 sets
                {
                    ChangerEtatMatch(EtatMatch.Terminé);
                }
            }
            VerifierEtatSet(gagnant, perdant);
        }

        /* 
         * Vérifie si le set est terminé et si le match est terminé
         */
        private void VerifierEtatSet(Player gagnant, Player perdant)
        {
            switch (gagnant.Sets[gagnant.CurrentSet])
            {
                case 6 when perdant.Sets[gagnant.CurrentSet] == 6:
                    ChangerEtatSet(EtatSet.TieBreak);
                    break;
                case >= 6 when (gagnant.Sets[gagnant.CurrentSet] - perdant.Sets[gagnant.CurrentSet]) >= 2:
                {
                    gagnant.GagnerSet();
                    ChangerEtatSet(EtatSet.Terminé);
                    if (gagnant.Sets.Sum() >= 2) // Match au meilleur des 3 sets
                    {
                        ChangerEtatMatch(EtatMatch.Terminé);
                    }
                    break;
                }
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
            if (EtatDuSet == EtatSet.TieBreak)
            {
                return $"6-6 dans le set en cours, tie break";
            }

            var jeuxA = PlayerA.Sets[PlayerA.CurrentSet];
            var jeuxB = PlayerB.Sets[PlayerB.CurrentSet];
    
            var joueurEnTete = jeuxA > jeuxB ? PlayerA.Name : PlayerB.Name;
    
            var jeuA = jeuxA == 1 ? "jeu" : "jeux";
            var jeuB = jeuxB == 1 ? "jeu" : "jeux";

            if (jeuxA == jeuxB)
            {
                return $"Le score est de {jeuxA} {jeuA} partout";
            }
    
            return joueurEnTete == PlayerA.Name ? $"Le Joueur {PlayerA.Name} mène {jeuxA} {jeuA} à {jeuxB}" : $"{PlayerB.Name} mène {jeuxB} {jeuB} à {jeuxA}";
        }

        public string GetResultatSet()
        {
            var currentSetIndex = PlayerA.CurrentSet - 1;
            var jeuxA = PlayerA.Sets[currentSetIndex];
            var jeuxB = PlayerB.Sets[currentSetIndex];
            var joueurEnTete = jeuxA > jeuxB ? PlayerA.Name : PlayerB.Name;

            return joueurEnTete == PlayerA.Name 
                ? $"Le Joueur {PlayerA.Name} remporte le set {jeuxA} jeux à {jeuxB}" 
                : $"Le Joueur {PlayerB.Name} remporte le set {jeuxB} jeux à {jeuxA}";
        }




        public string GetResultatMatch()
        {
            var joueurEnTete = PlayerA.Sets.Sum() > PlayerB.Sets.Sum() ? PlayerA.Name : PlayerB.Name;
            var setsA = PlayerA.Sets;
            var setsB = PlayerB.Sets;

            var resultatSets = new List<string>();
            for (var i = 0; i < setsA.Length; i++)
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
        Terminé,
        TieBreak
    }

    public enum EtatMatch
    {
        EnCours,
        Terminé
    }
}
