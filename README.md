# ACureForTheCrown

This project is a resource management and decision-making game developed for a game jam with the theme "Cell". The theme is interpreted through two lenses: biological cells (cancer) and carceral cells (prison).

#Concept and Story The player takes on the role of a King whose daughter is suffering from an incurable form of cancer. To find a cure, the King invites subjects and specialists to propose treatments. The core objective is to manage the kingdom's resources effectively while attempting to reduce the daughter's cancer progression to zero.

#Gameplay Mechanics The game utilizes card-based interaction mechanics, where players evaluate proposals from various characters (e.g., Alchemists, Witchers, Doctors).

#Decision Options Swipe Right (Accept): Implement the proposed solution. This typically consumes gold/resources but may affect health bars.

Swipe Left (Reject): Decline the proposal. This usually preserves resources but may have social or health consequences.

Imprison: Send the proposer to the dungeon. This is a unique mechanic where the player "cells" the individual to protect the economy or honor if the proposal is deemed mocking or dangerous.

#Resource Management The player must balance five primary indicators:

Cancer (Target): The main objective is to drive this bar to 0%.

Mental: The psychological state of the daughter or the royal family.

Immune: The biological resilience of the daughter.

Resources (Gold): The financial treasury of the kingdom.

Honor: The public perception and moral standing of the Crown.

#Technical Details Engine: Unity

Language: C#

Platform: PC / WebGL

Visual Style: 2D Hand-drawn / Pixel Art hybrid UI.

How to Play Read the character's dialogue and the associated cost/benefit.

Choose one of the three actions (Accept, Reject, Imprison).

Monitor the impact on the five resource bars.

Game ends if resources are depleted or if the King's honor is ruined.

Victory is achieved by successfully curing the daughter (Cancer bar = 0).
