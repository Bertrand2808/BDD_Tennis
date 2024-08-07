﻿Feature: Tennis

@tennis
Scenario: Le joueur A marque un point
    Given Le match commence entre A et B
    Then Le match devrait être EnCours
    Given Le set est EnCours
    And Le jeu est EnCours
    When Joueur A marque un point
    Then le score devrait être : 
      | Joueur | Score | Set 1 | Set 2 | Set 3 |
      | A      |  15   |   0   |   0   |   0   |
      | B      |   0   |   0   |   0   |   0   |
    And Le jeu devrait être EnCours
    
Scenario: Le joueur A remporte un jeu
    Given Le match commence entre A et B
    Then Le match devrait être EnCours
    Given Le set est EnCours
    And Le jeu est EnCours
    And Le score est de :
      | Joueur | Score | Set 1 | Set 2 | Set 3 |
      | A      |  40   |   0   |   0   |   0   |
      | B      |   0   |   0   |   0   |   0   |
    When Joueur A marque un point
    Then Le jeu devrait être Terminé
    And le score devrait être : 
      | Joueur | Score | Set 1 | Set 2 | Set 3 |
      | A      |   0   |   1   |   0   |   0   |
      | B      |   0   |   0   |   0   |   0   |
    And Le set devrait être EnCours
    And le resultat du jeu devrait être "Le Joueur A mène 1 jeu à 0"   

Scenario: Le joueur A remporte un set
    Given Le match commence entre A et B
    Then Le match devrait être EnCours
    Given Le set est EnCours
    And Le set en cours est le set 1
    Then Le set en cours devrait être le set 1
    Given Le jeu est EnCours
    And Le score est de :
      | Joueur | Score | Set 1 | Set 2 | Set 3 |
      | A      |  40   |   5   |   0   |   0   |
      | B      |   0   |   0   |   0   |   0   |
    When Joueur A marque un point
    Then Le jeu devrait être Terminé
    And le score devrait être : 
      | Joueur | Score | Set 1 | Set 2 | Set 3 |
      | A      |   0   |   6   |   0   |   0   |
      | B      |   0   |   0   |   0   |   0   |
    And Le set devrait être Terminé
    And Le set en cours devrait être le set 2
    And le resultat du set devrait être "Le Joueur A remporte le set 6 jeux à 0"
    
Scenario: Le joueur A remporte le match 
  Given Le match commence entre A et B
  Then Le match devrait être EnCours
  Given Le set est EnCours
  And Le set en cours est le set 2
  And Le jeu est EnCours
  And Le score est de :
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |  40   |   6   |   5   |   0   |
    | B      |   0   |   0   |   0   |   0   |
  When Joueur A marque un point
  Then Le jeu devrait être Terminé
  And le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |   0   |   6   |   6   |   0   |
    | B      |   0   |   0   |   0   |   0   |
  And Le set devrait être Terminé
  And le resultat du match devrait être "Jeu, set et match. Victoire du Joueur A : 6-0, 6-0"
      
Scenario: Le joueur B revient à 40-40 et gagne le jeu
  Given Le match commence entre A et B
  Then Le match devrait être EnCours
  Given Le set est EnCours
  And Le jeu est EnCours
  And Le score est de :
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |  40   |   5   |   0   |   0   |
    | B      |  30   |   0   |   0   |   0   |
  When Joueur B marque un point
  Then le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |  40   |   5   |   0   |   0   |
    | B      |  40   |   0   |   0   |   0   |
  And Le jeu devrait être EnCours
  When Joueur B marque un point
  Then le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |  40   |   5   |   0   |   0   |
    | B      |  AV   |   0   |   0   |   0   |
  And Le jeu devrait être EnCours
  When Joueur B marque un point
  Then Le jeu devrait être Terminé
  And le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |   0   |   5   |   0   |   0   |
    | B      |   0   |   1   |   0   |   0   |
  And le resultat du jeu devrait être "Le Joueur A mène 5 jeux à 1"
  
Scenario: Il y a un tie break
  Given Le match commence entre A et B
  Then Le match devrait être EnCours
  Given Le set est EnCours
  And Le set en cours est le set 2
  And Le jeu est EnCours
  And Le score est de :
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |  15   |   6   |   6   |   0   |
    | B      |  40   |   4   |   5   |   0   |
  When Joueur B marque un point
  Then le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |   0   |   6   |   6   |   0   |
    | B      |   0   |   4   |   6   |   0   |
  And Le jeu devrait être Terminé
  And le resultat du jeu devrait être "6-6 dans le set en cours, tie break"
  
Scenario: Joueur A gagne au tie break
  Given Le match commence entre A et B
  Then Le match devrait être EnCours
  Given Le set en cours est le set 2
  And Le jeu est EnCours
  And Le set est TieBreak
  And Le score est de :
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |   6   |   2   |   6   |   0   |
    | B      |   1   |   6   |   6   |   0   |
  When Joueur A marque un point
  Then le score devrait être : 
    | Joueur | Score | Set 1 | Set 2 | Set 3 |
    | A      |   0   |   2   |   7   |   0   |
    | B      |   0   |   6   |   6   |   0   |
  And Le jeu devrait être Terminé
  And Le set devrait être Terminé
  And Le set en cours devrait être le set 3
  And le resultat du set devrait être "Le Joueur A remporte le set 7 jeux à 6"