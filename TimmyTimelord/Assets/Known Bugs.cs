/*
 * 
 * 
 * 
 * 
 * Crawlers will walk of platforms. (Fix?: add trigger in front of crawler and detect if it is on the ground or not. flip crawler accordingly)
 * OR, fix by making edge of each surface wall (see scene, also solves some wall-ground issues)
 * OR, I could easily fix this by just adding triggers only visible to enemies
 * 
 *
 * Spiking a crawler on a platform and holding down will cause it to be stuck on the platform
 * 
 * Sometimes I just get stuck after getting hit or coming out of time travel or both idk
 * 
 * lives not going down every time i get hit when i build game? (fixed?)
 * 
 * 
 * Time travels not working correctly if I hit crawler during TT.
 * 
 * 
 * First boss can be someone who requires near-simultaneous hits from both sides to be harmed
 * 
 * 
 * The following is not necessary because I can just pass a integer value to determine the attack. Create different attacks on different layers so that the enemy can determin what attack it is. Handling the collision from the attack perspective
 * does not work because each enemy has different scripts and there will be more enemies that unique attacks. Instead I can pull values from the attacks
 * and use that if I choose not to make the values standard.
 */