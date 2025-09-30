# UniQuest (Remix Empire Deluxe Morgan)

## UniQuest  
*Le RPG où chaque ligne de code est un pas vers l’épopée.*

---

## Introduction du sujet  

Les jeux de rôle (RPG) sont apparus dans les années 70, fortement influencés par les univers de **Donjons & Dragons** et les récits de fantasy.  
En 1981, un certain **Richard Garriott** lança *Ultima*, un des tout premiers RPG vidéoludiques, programmé en BASIC sur Apple II. Ce jeu, développé dans la chambre d’un étudiant, allait poser les bases d’un genre aujourd’hui incontournable.  
Merci Richard, tu as codé nos rêves !  

Depuis, les RPG ont évolué : du texte à la 3D, des dés aux arbres de compétences, ils ont su captiver des générations de joueurs et de développeurs.  
Avec **Unity et C#**, créer un RPG est aujourd’hui à la portée des étudiants... comme nous !  

**UniQuest**, c’est notre aventure.  
Un projet scolaire devenu épopée numérique, où l’on code des combats, forge des quêtes, et apprend à chaque ligne.  

*Spoiler alert : il n’y a pas de boss final, mais il y a une soutenance.*

---

## Contexte  

Comme vous l’avez sûrement compris, l’objectif de ce sujet sera de réaliser un jeu de type **RPG tour par tour** à l’aide de **C#**.  

Il devra y avoir plusieurs éléments essentiels au jeu :  

- Une **map** où le joueur pourra se déplacer à l’aide des touches fléchées du clavier comme un véritable explorateur.  
- Sur cette carte, des **combats aléatoires** peuvent survenir durant les déplacements.  
- Un **menu d’équipe** pour consulter l’ensemble de ses personnages (vie, statistiques, attaques possibles …).  
- Un **inventaire** avec potions, clés, objets de boost de statistiques (et peut-être quelques snacks pour la route), ainsi qu’un **menu de sauvegarde/chargement**.  

---

### Système de combat  

- Les combats se dérouleront en **1v1 au tour par tour**.  
- L’ennemi sera piloté par une **IA**, allant du simple attaquant aveugle au stratège exploitant vos faiblesses.  
- Si un personnage tombe au combat, il pourra être remplacé par un autre membre de l’équipe.  
- Attention : si toute l’équipe est KO → **Game Over**.  

À chaque tour, il faudra choisir une action parmi :  
- **Attaque**  
- **Magie** (consomme du mana qui se recharge progressivement)  
- **Objet**  

Chaque personnage aura :  
- un **type**,  
- des **statistiques variées** (attaque, défense, vitesse, PV, PM, précision),  
- un **système d’expérience** → montée de niveau, amélioration des stats, nouvelles attaques.  

Les attaques auront :  
- des **types**,  
- des **stats d’attaque et de précision**,  
- des **chances de coup critique**.  

La victoire octroiera de l’**expérience**.  

---

### Phases de jeu  

1. **Phase carte**  
   - Déplacement  
   - Dialogue avec PNJ  
   - Ouverture de coffres  
   - Déclenchement de combats  

2. **Phase combat**  
   - Tour par tour  
   - Exploitation des mécaniques décrites ci-dessus  

---

### Architecture et fiabilité  

- Planification de la structure du jeu dès le départ.  
- Diagrammes de classes pour une vision claire.  
- Utilisation de l’héritage (personnages, attaques, objets, etc.).  
- Mise en place de **tests unitaires** pour garantir la cohérence et la fiabilité des calculs.  

---

## Compétences visées  

- Installer et configurer son environnement de travail en fonction du projet.  
- Développer des composants métier.  
- Contribuer à la gestion d'un projet informatique.  
- Analyser les besoins et maquetter une application.  
- Définir l'architecture logicielle d'une application.  
- Préparer et exécuter les plans de tests d'une application.  

---

## Rendu  

Votre travail est évalué en présentation avec un support et une revue de code.  
Le slide doit être composé de :  

- Organisation de votre équipe  
- Problèmes rencontrés et solutions apportées  
- Démonstration jouable de votre jeu  

👉 Le projet est à rendre sur :  
[https://github.com/prenom-nom/uniquest](https://github.com/prenom-nom/uniquest)

---

## Base de connaissances  

- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)  
- [MSDN Console Documentation](https://learn.microsoft.com/fr-fr/dotnet/api/system.console?view=net-7.0)  
- [Programmation Orientée Objet](https://learn.microsoft.com/fr-fr/dotnet/csharp/fundamentals/tutorials/oop)  
- [Tests unitaires avec Visual Studio](https://learn.microsoft.com/fr-fr/visualstudio/test/unit-test-basics?view=vs-2022)  

---

## Références supplémentaires  

- [https://github.com/prenom-nom/CSHARP](https://github.com/prenom-nom/CSHARP)  
- [https://learn.microsoft.com/en-us/dotnet/csharp/](https://learn.microsoft.com/en-us/dotnet/csharp/)  
- [https://learn.microsoft.com/fr-fr/dotnet/api/system.console?view=net-7.0](https://learn.microsoft.com/fr-fr/dotnet/api/system.console?view=net-7.0)  
- [https://learn.microsoft.com/fr-fr/dotnet/csharp/fundamentals/tutorials/oop](https://learn.microsoft.com/fr-fr/dotnet/csharp/fundamentals/tutorials/oop)  
- [https://learn.microsoft.com/fr-fr/visualstudio/test/unit-test-basics?view=vs-2022](https://learn.microsoft.com/fr-fr/visualstudio/test/unit-test-basics?view=vs-2022)  

---
