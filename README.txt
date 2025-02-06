Jason Lei
CS 4488 Project 3 - Creatures

For this project, I have chosen to make the creatures. The code generates 5 instances of what I call "Matcha Monsters", a mythical creature that resembles the color and shape of a Matcha snack.

To properly test different seeds, look for the Game Object "SoR", which will have the public variable Seed to set at the very top (alongside all the other prefabs and public variables). Feel free to change the seed to see the different randomized (seeded) Matcha Monsters.

The Matcha Monsters have up to 3 components that are generated with curved surfaces. Namely, the main body (green), the base body (beige), and some Matcha Monsters are equipped with a halo (angelic!). The main and base body are made using Bezier coordinates with Surface of Revolution. The Bezier points are randomized to create a unique different shape, and furthermore there is another width variable that applies scaling to the mesh. These are all done with random variables taken from the seed. For the halo, it is also made using the Surface of Revolution, utilizing the torus by rotating the surface revolution of a circle. Therefore, the main body and base body for each Matcha Monster is randomized, with it's own width and Bezier points to give a unique shape.

Each Matcha Monster has 3 different swappable parts, each with 3 variants (for a total of 9). Namely, there are 3 different legs, wings (or back attachments), and eyes.

Clawed Legs:
The clawed legs come in either one leg or two leg variants depending on the size of the Matcha   Monster. These also are randomized with scale, and self adjusted to move up or down depending on the scaling (to account for the offset). As such, the most notable difference would be the scaling and length between different Matcha Monsters.

Quad Legs:
These are legs that always come in 4, like a spider but less legs. The rotation of the leg is randomized for each leg. While this is random, rotation is based off of it's own script and doesn't depend on the seed.

Wheel Leg:
Instead of legs, these Matcha Monster variants have a wheel instead! The rotation of the wheel leg is also randomized, and the scale of the wheel itself will also scale up or down a fixed amount. Again, these randomized variables are not dependent on the seed, and the random rotation and scale values come from its own script.

Angelic Wings:
Certain Matcha Monsters have angelic wings that help them fly. These are very pretty and have randomized height and width scaling. They can get really large or really small! The wing size changes depending on the seed, and are randomized. Most Matcha Monsters donning Angelic Wings often have halos, but some may not have developed them yet.

Demonic Wings:
Some people say demonic but they're more like fallen angels. Certain Matcha Monsters have black wings instead, and it is iconic to see many of the feathers fallen off. This dark and edgy look appears evil, but this Matcha Monster is no more evil than it's other counterparts. The wing is randomized to have random width and height scaling depending on the seed.

Propellor:
Matcha Monsters are also innovative, and certain ones without wings have gone to develop their own means of faster transport. Some will have propellors, which help it to move at increased speeds. The propellor has width and height scaling too, scaling width and height equally but randomly, based on the seed.

Regular Eyes:
Matcha Monsters can have regular eyes, with the eyeball being white and the iris being black. These have random rotation, and can move up to a certain angle inwards or outwards based on the random seed.

Sage of Six Paths (Yellow eyeball, horizontal black iris):
Certain Matcha Monsters have mastered the Six Paths, drawing upon the power of the ancient frogs to channel their Chakra. These eyes have randomized rotation based on the seed, and each eye will randomly move inwards a random amount.

Sharingan (Red eyeball, black iris with cool other markings):
Very rarely, some natural-born Matcha Monsters come with the Sharingan, which allows them to see the unseen and identify movements. These eyes have randomized rotation based on the seed, and each eye will equally but randomly move inwards a random amount.