using GameNetcodeStuff;
using UnityEngine;

namespace ToiletLeechIsReal {

    // You may be wondering, how does the Toilet Leech know it is from class ToiletLeechAI?
    // Well, we give it a reference to to this class in the Unity project where we make the asset bundle.
    // Asset bundles cannot contain scripts, so our script lives here. It is important to get the
    // reference right, or else it will not find this file. TODO: explain how to do the reference right.

    // Also, I have no idea if this syncs well on multiplayer.

    // For reference, see EnemyAI with a decompiler. Also stuff like BlobAI is good to check too.

    class ToiletLeechAI : EnemyAI {

        float timeSinceHittingLocalPlayer;

        public override void Start()
		{
			base.Start();
            var myLogSource = BepInEx.Logging.Logger.CreateLogSource("Toilet Leech");
            myLogSource.LogInfo("Toilet Leech Spawned");
            timeSinceHittingLocalPlayer = 0;
		}
        public override void Update(){
            base.Update();
            timeSinceHittingLocalPlayer += Time.deltaTime;
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (!isEnemyDead && !StartOfRound.Instance.allPlayersDead)
            {
                if (TargetClosestPlayer(4f))
                {
                    StopSearch(null);
                    movingTowardsTargetPlayer = true;
                    creatureAnimator.SetTrigger("startWalk");
                }
                else
                {
                    StartSearch(base.transform.position, null);
                    movingTowardsTargetPlayer = false;
                    // This is flawed creatureAnimator logic
                    creatureAnimator.SetTrigger("stopWalk");

                }
            }
        }

        // this is perhaps not the most optimal way to deal with this, but idk
        private void OnTriggerStay(Collider other)
        {
            // Also I think there is a better way to do this logging thing, but idk how.
            var myLogSource = BepInEx.Logging.Logger.CreateLogSource("Toilet Leech");
            if (timeSinceHittingLocalPlayer < 0.25f)
            {
                return;
            }

            PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(other);
            if (playerControllerB != null)
            {
                myLogSource.LogInfo("Toilet Leech Collision!!");
                timeSinceHittingLocalPlayer = 0f;
                playerControllerB.DamagePlayer(20);
            }
        }
    }
}