# Coding our custom AI

> [!IMPORTANT]  
> You should use a decompiler (e.g., [dnSpy](https://github.com/dnSpyEx/dnSpy), [ILSpy](https://github.com/icsharpcode/ILSpy)) to look how the enemy AI scripts work in the game. All the game code is contained inside `Lethal Company/Lethal Company_Data/Managed/Assembly-CSharp.dll`, so you should open that file in your decompiler!
>
> Also, check the resources at the bottom of this page!

### Tips For Testing Your Mod

You can directly open the game's exe file, and this will allow you to open two instances and test multiplayer in LAN mode. This also opens the game slightly faster than through steam.

You can also use external mods to spawn the enemy for testing. I don't know one right now that works fine on v49. If you have any suggestions, feel free to open an issue or something. Alternatively, you could implement testing functionality in your mod directly.

### Introduction to Abstract Class EnemyAI

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

The `DoAIInterval()` method runs in an interval we've set in Unity on our custom AI script which is attached to our enemy prefab. We have set this to 0.2 seconds, which is also used in the game by for example the BaboonHawk enemy and probably other enemies too.

```cs
public override void DoAIInterval()
{
    base.DoAIInterval(); // do all the stuff the base game needs to do to make our AI work.
    if (moveTowardsDestination) {
        // agent is the Nav Mesh Agent attached to our prefab
        agent.SetDestination(destination);
    }
    // Updates serverPosition to current enemy position on server if distance from
    // serverPosition to current position is above updatePositionThreshold, which we set
    // in our custom AI script in Unity.
    SyncPositionToClients();
}
```

As we can see, the enemy updates its destination when `moveTowardsDestination` is True. It is True by default, and also gets set True if you run the `SetDestinationToPosition()` method, returning true, which means the enemy was able to pathfind to the player. Running that method also sets `movingTowardsTargetPlayer` to False, and updates the `destination` variable.

We also use the `override` modifier on the function to override the base class. This properly tells C# to use our method instead of the empty method from `EnemyAI`.

The `OnCollideWithPlayer()` method will run when an object with a trigger collider and the Enemy AI Collision Detect (Script). This is also the collider we can hit with a shovel, and we need to implement `HitEnemy()` for our enemy to be able to take damage and die.

### Enemy Movement Examples

The `TargetClosestPlayer()` method can be used to make the enemy change its targetPlayer to the closest player.
Then, we can for example do `SetDestinationToPosition(targetPlayer.transform.position)` to make our enemy pathfind to where targetPlayer stands.

### Using Random Without Desync

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

### Making more complex AI
In order to properly structure our AI when it gets more complex is to use enums. Enums can be used to more explicitly define the "state" that our AI is in. For example:

```cs
public MyComplexAI : EnemyAI {
    enum State {
        WANDERING,
        CHASING
        // ... and many more states
    }
    public State currentState { get; private set; } // this allows other classes to read what state we are in but not set it - that can go very wrong.
    // ...
}
```

Now we have two states in this example, the `WANDERING` state and the `CHASING` state. What's great about enums is that we can very quickly add a new state to our AI. In order to use our new states we need to modify our `DoAIInterval()` method.
```cs
public MyComplexAI : EnemyAI
{
    enum State
    {
        WANDERING,
        CHASING
        // ... and many more states
    }
    public State currentState { get; private set; } // this allows other classes to read what state we are in but not set it - that can go very wrong.
    // ...

    public override void DoAIInterval()
    {
        base.DoAIInterval(); // do all the stuff the base game needs to do to make our AI work.
        switch(currentState) // the switch is a more advanced if that allows us to more easily define what should happen in each state vs using just a bunch of ifs
        { 
            case State.WANDERING:
                // ... handle code for looking for a player or just wandering around
                break; // this break is VERY IMPORTANT, it means that it won't just continue onto our next bit of code for the different states.
            case State.CHASING:
                if (moveTowardsDestination) {
                    // agent is the Nav Mesh Agent attached to our prefab
                    agent.SetDestination(destination);
                }
                // ... other code to handle when we are chasing at a player
                break;
        }

        // We should call this function outside of the switch so that no matter what state we are currently in, it still gets ran.
        SyncPositionToClients();
    }
}
```
Now all we need to do is instruct *when* the AI should change state:
```cs
switch(currentState) // the switch is a more advanced if that allows us to more easily define what should happen in each state vs using just a bunch of ifs
{ 
    case State.WANDERING:
        // ... handle code for looking for a player or just wandering around
        if(foundPlayer)
        {
            currentState = State.CHASING; // this sets our state to chasing so the next time this switch statement is called it will instead go into the chasing state
            // if you need to for debugging as well you can put a log statement here saying that you changed into the chasing state.
        }
        break; // this break is VERY IMPORTANT, it means that it won't just continue onto our next bit of code for the different states.
    case State.CHASING:
        if (moveTowardsDestination) {
            // agent is the Nav Mesh Agent attached to our prefab
            agent.SetDestination(destination);
        }
        // ... other code to handle when we are chasing at a player
        if (lostPlayer)
        {
            currentState = State.WANDERING;
        }
        break;
}
```
We've now converted our AI into a state machine by using an enum! This helps you organize larger AI systems into chunks that can't interfere with each other so you'll encounter less bugs. It's also a lot easier for you to now add more states to your AI without having to use a bunch of `if` checks.

## Resources

### Basics

 - Override methods - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override
 - Enums            - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
 - Switch statement - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement

### Networking

> [!IMPORTANT]
> We are using [Unity Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodePatcher) MSBuild plugin to make our custom Rpc methods work.

 - ClientRpc - https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/clientrpc/
 - ServerRpc - https://docs-multiplayer.unity3d.com/netcode/current/advanced-topics/message-system/serverrpc/

**Pages: [Blender](./Blender.md) | [Unity](./Unity.md)**
