namespace SpecFlowTennis;

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
        joueur.ScorePoint();
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
    
    // Vérifier si un joueur a gagné le jeu (refacto)
    public void VerifierEtatJeu()
    {
        if (PlayerA.Avantage)
        {
            if(PlayerA.Score > PlayerB.Score)
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else
            {
                PlayerA.Avantage = false;
                PlayerB.Avantage = false;
            }
        } else if (PlayerB.Avantage)
        {
            if(PlayerB.Score > PlayerA.Score)
            {
                GagnerJeu(PlayerB, PlayerA);
            }
            else
            {
                PlayerA.Avantage = false;
                PlayerB.Avantage = false;
            }
        }
        else if (PlayerA.Score == 40 && PlayerB.Score == 40)
        {

        }
        else if (PlayerA.Score == 40 || PlayerB.Score == 40)
        {
            if (PlayerA.Score == 40)
            {
                GagnerJeu(PlayerA, PlayerB);
            }
            else
            {
                GagnerJeu(PlayerB, PlayerA);
            }
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
    
    // Vérifier si un joueur a gagné le set (refacto)
    public void VerifierEtatSet(Player joueur, Player adversaire)
    {
        if (joueur.Sets[joueur.CurrentSet] >= 6 && (joueur.Sets[joueur.CurrentSet] - adversaire.Sets[adversaire.CurrentSet]) >= 2)
        {
            joueur.GagnerSet();
            if(joueur.Sets.Sum() >= 2) // Match au meilleur des 3 sets
            {
                ChangerEtatMatch(EtatMatch.Terminé);
            }
            else
            {
                joueur.CurrentSet += 1;
                adversaire.CurrentSet += 1;
                ChangerEtatSet(EtatSet.EnCours);
            }
        }
        else
        {
            ChangerEtatSet(EtatSet.EnCours);
        }
    }
    
    /*private void VerifierEtatMatch(Player joueur, Player adversaire)
    {
        if (joueur.Sets.Sum() >= 2) // Match au meilleur des 3 sets
        {
            ChangerEtatMatch(EtatMatch.Terminé);
        }
        else
        {
            joueur.GagnerSet();
            ChangerEtatSet(EtatSet.EnCours);
        }
    }*/


    
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
        var joueurEnTete = PlayerA.Sets[PlayerA.CurrentSet] > PlayerB.Sets[PlayerB.CurrentSet] ? PlayerA.Name : PlayerB.Name;
        var jeuxA = PlayerA.Sets[PlayerA.CurrentSet];
        var jeuxB = PlayerB.Sets[PlayerB.CurrentSet];
        return joueurEnTete == PlayerA.Name ? $"Le Joueur {joueurEnTete} mène {jeuxA} jeu à {jeuxB}" : $"{joueurEnTete} mène {jeuxB} jeux à {jeuxA}";
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
