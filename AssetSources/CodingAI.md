# Coding our custom AI

> [!IMPORTANT]  
> You should use a decompiler (e.g., [dnSpy](https://github.com/dnSpyEx/dnSpy), [ILSpy](https://github.com/icsharpcode/ILSpy)) to look how the enemy AI scripts work in the game. All the game code is contained inside `Lethal Company/Lethal Company_Data/Managed/Assembly-CSharp.dll`, so you should open that file in your decompiler!
>
> Also, check the resources at the bottom of this page!

### Abstract class EnemyAI

Every enemy in Lethal Company inherits from the EnemyAI class, so we do the same.

The `Start()` method will run when the enemy spawns in a level. We can initialize our variables here.

The `Update()` method will run every frame, and we should try to avoiding intensive calculations here. Also, if `movingTowardsTargetPlayer` is set to True, the Enemy will automatically set its destination to the target player, at an interval of 0.25 seconds.

This is also where the enemy position gets updated for clients other than the host:
```cs
if (!inSpecialAnimation)
{
    base.transform.position = Vector3.SmoothDamp(base.transform.position, serverPosition, ref tempVelocity, syncMovementSpeed);
    base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Mathf.LerpAngle(base.transform.eulerAngles.y, targetYRotation, 15f * Time.deltaTime), base.transform.eulerAngles.z);
}
```
This also means that if `syncMovementSpeed` is zero, or a very big number, the enemy movement will appear janky on clients other than the host.

It also seems that the enemy will also point towards `targetYRotation`, but I just realised this now as I'm reading the code.

The `DoAIInterval()` method runs in an interval we've set in Unity on our custom AI script which is attached to our enemy prefab. We have set this to 0.2 seconds, which is also used in the game by for example the BaboonHawk enemy and probably other enemies too.

```cs
public virtual void DoAIInterval()
{
    if (moveTowardsDestination) {
        // agent is the Nav Mesh Agent attached to our prefab
        agent.SetDestination(destination);
    }
    // Updates serverPosition to current enemy position on server, if distance from
    // serverPosition to current position is above updatePositionThreshold, which we set
    // in our custom AI script in Unity.
    SyncPositionToClients();
}
```

As we can see, the enemy updates its destination when `moveTowardsDestination` is True. It is True by default, and also gets set True if you run the `SetDestinationToPosition()` method, returning true, which means the enemy was able to pathfind to the player. Running that method also sets `movingTowardsTargetPlayer` to False, and updates the `destination` variable.

The `OnCollideWithPlayer()` method will run when an object with a trigger collider and the Enemy AI Collision Detect (Script). This is also the collider we can hit with a shovel, and we need to implement `HitEnemy()` for our enemy to be able to take damage and die.

### Using Random without desync

We can implement our own random variable which we initialize with a set seed in our `Start()` method, and use it like this:
```cs
System.Random enemyRandom;

public override void Start()
{
    base.Start();
    enemyRandom = new System.Random(StartOfRound.Instance.randomMapSeed + thisEnemyIndex);
}
``` 
And we can use it for example this: `enemyRandom.Next(0, 5)`. This will choose the next random integer in our range.


## Resources

### Basics

Override methods - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override

### Networking

> [!IMPORTANT]
> We are using [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher) MSBuild plugin to make our custom Rpc methods work.

ClientRpc - https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/  
ServerRpc - https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/

**Pages: [Blender](./Blender.md) | [Unity](./Unity.md)**