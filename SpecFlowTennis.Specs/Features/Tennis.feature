Feature: Tennis

@tennis
Scenario: Le joueur A marque un point
    Given Le match commence entre A et B
    And Le set commence
    And Le jeu commence
    And Le joueur A marque un point
    When L'échange devrait être terminé
    Then le score devrait être : 
        | Joueur | Score | Set 1 | Set 2 | Set 3 |
        | A      |  15   |   0   |   0   |   0   |
        | B      |   0   |   0   |   0   |   0   |
    And Le jeu devrait être "EnCours"