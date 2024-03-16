# SwarmIntelligence
 Package with implementation of swarming intelligence alogrithms in Unity

# Installation

To added the package in your project, just go to __Windows -> PackageManager__, click on the "+" and choose __"Add package from git URL"__. Then, use this link : __https://github.com/eraflo/SwarmIntelligence__

# Implemented

Actually, there is an implementation of :
- Boids algorithms : simulation of flocking behavior. There is a basic one (with speed limit and keep in zone added) and one with perching behaviour (you can specify a surface on which, when the boid come in contact with, he stay on it a finite amount of time)

# Next

I will be adding :
- A variant of the boid algoritms for prey boids with anti-flocking behaviour
- A variant with wind or current force applied
- The ant colony optimization from Dorigo

# Sources :
- http://www.kfish.org/boids/pseudocode.html
- https://www.diva-portal.org/smash/get/diva2:1778360/FULLTEXT01.pdf
- https://www.red3d.com/cwr/boids/
- https://cs.stanford.edu/people/eroberts/courses/soco/projects/2008-09/modeling-natural-systems/boids.html
- https://en.wikipedia.org/wiki/Swarm_intelligence