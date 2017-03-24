# ComplexGame
AIE 2017 semester 1: Complex Game Systems

Design Brief:  Utility AI - Space Explorers
===

A Exploration Mothership is drifting through an asteroid field, and hosts a fleet of drones.  
Points of Scientific Interest also randomly spawn with some rarity [src: Data].  
Asteroids contain resources in random abundance [src: Ore, Reactant].  
Asteroids that strike Mothership deal damage to Shield.  
Anthing that strikes Drones deals damage to DroneHP  

Mothership performs resource conversion [src: Metal, Fuel, Energy, Shield, Science, Drone].  
Drones perform resource acquisition [src: Data, Ore, Reactant].    

Mothership and Drones perfom tasks based on fuzzy needs assessment.  
Mothership MAY split "Engineering" and "Flight Deck" tasks into parallel sets.  


Mothership 
---
 Properties (constant, 'unattended' process):
 - Converts Fuel into Energy 
 - Converts Energy into Shield
 Tasks:
 - Converts Energy and Ore into Metal
 - Converts Energy and Reactant into Fuel
 - Converts Energy and Data into Science
 - Uses Energy to Scan Asteroid [src: Intel] <br/> ("discover" resources and possible collision damage)
 - Converts Energy, Fuel and Metal into Drones
 - Launches Drones
 - Docks Drones
 - Repairs Drones (DroneHP)
 - Reclaims Drones
 - Empties Drone Cargo (Ore and Reactant)
 - Fills Drone Fuel

Drones:
---
 Properties:
 - Converts Fuel into Energy
 - Converts Fuel into Acceleration
 Tasks:
 - Mine Asteroid (src: Ore or Reactant)
 - Attack Asteroid (prevent collision damage)
 - Travel ("Move To")
 - Inspect PoSI (src: Data)
 - Request Dock at Mothership
 - Request Undock at Mothership


Pending time constraints, Drone Maneouvring AI may be subject to a Genetic Algorithm to improve handling.