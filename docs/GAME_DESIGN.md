# SHADOWS OF THE VALLEY — GAME DESIGN DOCUMENT

## High concept
A third-person 3D Egyptian dark-fantasy action adventure starring Sekhem, a fallen royal guardian. The combat identity is fast, mobile, and cinematic: chained melee, enemy pulls, aerial juggling, evasive movement, and heavy finishers. The game uses original characters, lore, creature designs, and visual identity.

## World and story
The fictional kingdom of Akhet draws inspiration from ancient Egyptian architecture and desert landscapes. A mysterious Sunstone once powered its sacred temples. After it vanished, daylight faded and creatures emerged from sealed tombs.

**Sekhem** is a former royal guard, burdened by the loss of his family during the Night of Silence.

## Combat identity: Flow of the Shadow Blade
- Three-hit ground combo: quick cut, advancing reverse cut, rising finisher.
- **Shadow Tether:** spectral binding pulls lighter enemies toward Sekhem; heavy monsters resist or are only staggered.
- **Aerial Launcher:** lifts a nearby enemy to begin an air sequence.
- **Air Combo:** four chained aerial hits, with the final hit extending the juggle.
- **Ground Slam:** radial impact that damages nearby enemies and ends an aerial sequence.
- **Directional Dodge:** quick evasive burst with a short cooldown.
- **Jump:** standard jump for traversal and aerial combat setup.
- Future combat upgrades: timed parry, perfect dodge reward, lock-on targeting, weapon skills, enemy executions, and boss-specific counters.

The current scripts provide starter mechanics. Final animation timing, hit-stop, camera shake, VFX, and animation cancel windows require tuning in Unity with real character rigs.

## Mobile controls
- Left virtual joystick: movement.
- Right drag region: camera.
- Attack: three-hit ground combo.
- Jump: traversal and aerial setup.
- Dodge: directional evade.
- Shadow Pull: tether a nearby enemy.
- Air Launcher: lift a target.
- Air Attack: chain aerial hits.
- Ground Slam: finish with an area impact.
- Optional target lock and contextual interaction are planned.

## Monsters
1. **Tombbound:** wrapped tomb guardian; slow, readable melee attacks; tutorial enemy.
2. **Ash Stalker:** quick ash creature that circles and punishes stationary players.
3. **Basalt Brute:** massive stone-and-bronze guardian; resists tether and launch, but can be staggered.
4. **Sand Wraith:** ranged spirit firing compressed sand shards, then relocating.
5. **The Gatekeeper:** first boss; broad swings, ground shockwaves, phase changes, and an exposed weak point.

Monster behavior should communicate intent clearly through anticipation, attack windows, recovery, and distinct silhouettes.

## First chapter: The Black Gate
Target first playable slice: 10–15 minutes.
1. The Drowned Dunes — movement, jump, dodge, and camera tutorial.
2. Outer Gate — fight two Tombbound and learn the ground combo.
3. Inscribed Passage — environmental interaction and traversal.
4. Broken Courtyard — introduce Shadow Tether, launcher, and air combo.
5. Gatekeeper Chamber — boss encounter and story reveal.

Opening line: “The curse was never inside the stone. It was inside those who guarded it.”

## 3D art and presentation target
Aim for a premium console-inspired presentation adapted to mobile hardware, not a promise of literal console-level fidelity on every phone.
- Detailed original Sekhem hero model with layered armor, cloth, weapon materials, and readable silhouette.
- Distinct monster models with believable scale, surface wear, and clear weak-point design.
- Ancient Egyptian-inspired temples, eroded sandstone, black basalt, bronze mechanisms, desert ruins, and atmospheric interiors.
- Physically Based Rendering materials, normal maps, ambient occlusion, reflection probes, baked global illumination, controlled fog, and selective volumetric-style light shafts where performance allows.
- Cinematic third-person camera with damping, collision avoidance, impact shake, and subtle FOV response to dodges and finishers.
- Responsive animation state machine: locomotion, start/stop, jump, dodge, ground combo, launcher, air combo, slam, hit reactions, stagger, and defeat.
- Distinct hit VFX: restrained turquoise shadow energy, dust bursts, sparks, and ground impact rings.
- Layered sound: weapon whooshes, impact weight, creature vocalizations, desert ambience, and adaptive combat music.

## Performance and scalability
Target mid-range Android phones with 4–6 GB RAM. Use Unity 6 URP, baked lighting, limited real-time shadow casters, texture compression, LOD groups, occlusion culling, GPU instancing, pooled combat effects, and scalable quality presets. Profile sustained combat on real devices for heat, battery, CPU, GPU, and memory.

## First prototype scope
One playable arena, one player capsule/placeholder rig, one Tombbound enemy with chase/melee AI, a three-hit ground combo, jump, dodge, Shadow Tether, aerial launcher, air combo, ground slam, and mobile touch controls. Replace greybox geometry and placeholders with final art only after the core loop is stable. No open world, multiplayer, or complex inventory until this slice is proven.
