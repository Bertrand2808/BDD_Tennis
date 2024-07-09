﻿public class Tennis
{
    public String JoueurA { get; set; }
    public String JoueurB { get; set; }
    private bool matchTermine;
    private bool jeuTermine;
    private bool setTermine;
    private bool echangeTermine;
    private Dictionary<String, int> _score = new();
    private Dictionary<String, int[]> _scoreSet = new();
    private int _currentSet = 0;
    
    public enum EtatMatch
    {
        MatchTermine,
        JeuTermine,
        SetTermine,
        EchangeTermine,
        EnCours
    }
    
    // Méthode pour avoir l'état du jeu 
    public EtatMatch GetEtatMatch()
    {
        if (matchTermine)
        {
            return EtatMatch.MatchTermine;
        }
        if (jeuTermine)
        {
            return EtatMatch.JeuTermine;
        }
        if (setTermine)
        {
            return EtatMatch.SetTermine;
        }
        if (echangeTermine)
        {
            return EtatMatch.EchangeTermine;
        }
        return EtatMatch.EnCours;
    }
    
    public void ChangeEtatMatch(EtatMatch etat)
    {
        switch (etat)
        {
            case EtatMatch.MatchTermine:
                TerminerMatch();
                break;
            case EtatMatch.JeuTermine:
                TerminerJeu();
                break;
            case EtatMatch.SetTermine:
                TerminerSet();
                break;
            case EtatMatch.EchangeTermine:
                TerminerEchange();
                break;
            default:
                break;
        }
    }

    // Méthode pour lancer une exception suivant l'état du match
    public void ThrowException()
    {
        switch (GetEtatMatch())
        {
            case EtatMatch.EchangeTermine:
                throw new InvalidOperationException("L'échange est terminé");
            case EtatMatch.SetTermine:
                throw new InvalidOperationException("Le set est terminé");
            case EtatMatch.JeuTermine:
                throw new InvalidOperationException("Le jeu est terminé");
            case EtatMatch.MatchTermine:
                throw new InvalidOperationException("Le match est terminé");
            default:
                break;
        }
    }
    
    public Tennis(string joueurA, string joueurB)
    {
        JoueurA = joueurA;
        JoueurB = joueurB;
        InitScores();
    }
    
    private void InitScores()
    {
        _score[JoueurA] = 0;
        _score[JoueurB] = 0;
        _scoreSet[JoueurA] = new int[3];
        _scoreSet[JoueurB] = new int[3];
    }
    
    // Méthode pour commencer le match 
    public void CommencerMatch()
    {
        Console.WriteLine("Le match commence !");
        matchTermine = false;
    }
    
    // Méthode pour commencer le jeu 
    public void CommencerJeu()
    {
        Console.WriteLine("Le jeu commence !");
        jeuTermine = false;
    }
    
    // Méthode pour commencer le set
    public void CommencerSet()
    {
        Console.WriteLine("Le set commence !");
        setTermine = false;
    }
    
    // Méthode pour commencer l'échange 
    public void CommencerEchange()
    {
        Console.WriteLine("L'échange commence !");
        echangeTermine = false;
    }
    
    // Méthode pour terminer le match 
    public void TerminerMatch()
    {
        Console.WriteLine("Le match est terminé !");
        matchTermine = true;
    }
    
    // Méthode pour terminer le jeu 
    public void TerminerJeu()
    {
        Console.WriteLine("Le jeu commence !");
        jeuTermine = false;
    }
    
    // Méthode pour terminer le set
    public void TerminerSet()
    {
        Console.WriteLine("Le set commence !");
        setTermine = false;
    }
    
    // Méthode pour terminer l'échange 
    public void TerminerEchange()
    {
        Console.WriteLine("L'échange commence !");
        echangeTermine = false;
    }
    
    // Initialise le dictonnaire des sets
    public void Init(Dictionary<String, int[]>? sets)
    {
        if (sets != null)
        {
            _scoreSet = sets;
            CommencerSet();
        }
    }
    
    // Méthode pour marquer un point
    public void marquerPoint(String player, int score)
    {
        ThrowException();
        if (!_score.ContainsKey(player))
        {
            _score[player] = 0;
        }

        if (_score[player] == 30)
        {
            _score[player] = 40;
        }
        else
        {
            _score[player] += score;
        }
    }
    
    // Méthode pour update un set 
    public void UpdateSet(String player, int set)
    {
        ThrowException();
        if (!_scoreSet.ContainsKey(player))
        {
            _scoreSet[player] = new int[3];
        }
        _scoreSet[player][_currentSet] = set;
    }
    
    // Méthode pour récupérer le score 
    public Dictionary<String,PlayerScore> GetScore()
    {
        var result = new Dictionary<String,PlayerScore>();
        foreach (var player in _score.Keys)
        {
            result.Add(player, new PlayerScore
            {
                Score = _score[player],
                Sets = _scoreSet.ContainsKey(player) ? _scoreSet[player] : new int[3]
            });
        }
        return result;
    }
    
}

public class PlayerScore
{
    public int Score { get; set; }
    public int[] Sets { get; set; }
}