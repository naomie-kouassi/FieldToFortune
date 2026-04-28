# Field to Fortune
 
> *Set off on the road to riches and play Field to Fortune!*
 
**[English](#english) | [Français](#français)**
 
---
 
## English
 
### Overview
 
Field to Fortune is a commodity trading simulation game built in C#. The player starts with a capital of $5,000 and must grow their portfolio by buying and selling agricultural commodities before the end of the game.
 
It challenges users to navigate commodity markets using technical signals and risk management strategies.

**Link to demo:** https://naomie-kouassi.github.io/FieldToFortune/
 
### Core Mechanics
 
The market simulates six agricultural assets, each with its own volatility profile, with prices following a **mean-reverting stochastic process** (Ornstein-Uhlenbeck / Schwartz one-factor model) calibrated on real historical data. Players can trade these assets turn by turn and buy **European call options** priced with Black-Scholes.

Every 3 turns a **news headline** surfaces hinting at the next market move, though only 1 in 3 headlines carries a true signal. The game ships with **three difficulty levels** and a hard **50% maximum drawdown limit**.

  
  
### Price Simulation
 
Commodity prices follow a **log-normal mean-reverting process** (Schwartz 1997) :
 
```
ln(P_{t+1}) = ln(P_t) + θ(ln(μ) - ln(P_t))dt + σ√dt · Z
```
 
Where:
- `μ` : long-term equilibrium price
- `θ` : mean-reversion speed
- `σ` : volatility, calculated from real monthly historical data (source: IMF)
- `Z` : standard normal random variable (Box-Muller transform)
 
**Reference:** Schwartz, E.S. (1997). *The Stochastic Behavior of Commodity Prices: Implications for Valuation and Hedging.* Journal of Finance, 52(3), 923–973.
 
### Option Pricing
 
Call options are priced using the **Black-Scholes formula** :
 
```
C = S·N(d₁) - K·e^(-rT)·N(d₂)
 
d₁ = [ln(S/K) + (r + σ²/2)T] / (σ√T)
d₂ = d₁ - σ√T
```
 
The cumulative normal distribution N() is approximated using the Abramowitz & Stegun method.
 
### Tech Stack
 
| Layer | Technology |
|---|---|
| Backend | C# / .NET 10 |
| Frontend | Blazor WebAssembly |
| Charts | ApexCharts.Blazor |


### Future Roadmap
- Integration of SQLite for local performance tracking and leaderboard history.
- Live data feed integration for historical "backtesting" scenarios (e.g., 2008 Crisis).
 
 
### Author
 
Built by **Naomie Kouassi**, computational finance student at **Grenoble INP - Ensimag**.

You can contact me at naomie.kouasssi@gmail.com
 
---
 
## Français
  
### Présentation
 
Field to Fortune est un jeu de simulation de trading de matières premières développé en C#. Le joueur commence avec un capital de 5 000 $ et doit faire fructifier son portfolio en achetant et en vendant des matières premières agricoles.
 
Réussirez-vous à naviguer le marché des matières premières en mettant au point une stratégie efficace de gestion des risques ?

**Lien vers la démo :** https://naomie-kouassi.github.io/FieldToFortune/
 
### Mécaniques de jeu
 
Le marché simule six actifs agricoles, chacun avec son propre profil de volatilité, dont les prix suivent un **processus stochastique avec retour à la moyenne** (modèle à un facteur d'Ornstein-Uhlenbeck / Schwartz) calibré à partir de données historiques réelles. Les joueurs peuvent trader les actifs à chaque tour et acquérir des **options d'achat européennes** dont le prix est calculé selon le modèle Black-Scholes.

Tous les 3 tours, une **actualité** apparaît, laissant entrevoir le prochain mouvement du marché. Seule 1 actualité sur 3 contient un signal fiable. Le jeu propose **trois niveaux de difficulté** et une limite stricte de **50 % de réduction du capital initial**.


### Simulation des prix
 
Les prix des matières premières suivent un **processus log-normal avec retour à la moyenne** (Schwartz 1997) :
 
```
ln(P_{t+1}) = ln(P_t) + θ(ln(μ) - ln(P_t))dt + σ√dt · Z
```
 
Où :
- `μ` : prix d'équilibre à long terme
- `θ` : vitesse de retour à la moyenne
- `σ` : volatilité, calibrée sur de vraies données historiques mensuelles (source : FMI)
- `Z` : variable aléatoire normale centrée réduite (transformation de Box-Muller)
 
**Référence :** Schwartz, E.S. (1997). *The Stochastic Behavior of Commodity Prices: Implications for Valuation and Hedging.* Journal of Finance, 52(3), 923–973.
 
### Pricing des options
 
Les options d'achat sont pricées avec la **formule de Black-Scholes** :
 
```
C = S·N(d₁) - K·e^(-rT)·N(d₂)
 
d₁ = [ln(S/K) + (r + σ²/2)T] / (σ√T)
d₂ = d₁ - σ√T
```
 
La distribution normale cumulée N() est approchée par la méthode d'Abramowitz & Stegun.
  
### Stack technique
 
| Couche | Technologie |
|---|---|
| Backend | C# / .NET 10 |
| Frontend | Blazor WebAssembly |
| Graphiques | ApexCharts.Blazor |

### Améliorations à venir
- Intégration de SQLite pour le suivi des performances locales et ajout d'un classement.
- Intégration d'un flux de données historiques pour rejouer des scénarios comme la crise de 2008.
 

### Auteur
 
Développé par **Naomie Kouassi**, étudiante en ingénierie financière à **Grenoble-INP Ensimag**.

Je suis joignable à l'adresse naomie.kouass@gmail.com
 
