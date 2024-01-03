using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Logging;
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

        public Transform turnCompass;
        public Transform attackArea;
        float timeSinceHittingLocalPlayer;
        float timeSinceNewRandPos;
        Vector3 positionRandomness;
        Vector3 StalkPos;
        private ManualLogSource myLogSource;

        public override void Start()
		{
			base.Start();
            myLogSource = BepInEx.Logging.Logger.CreateLogSource("Toilet Leech");
            myLogSource.LogInfo("Toilet Leech Spawned");
            timeSinceHittingLocalPlayer = 0;
            timeSinceNewRandPos = 0;
            positionRandomness = new Vector3(0, 0, 0);
		}
        public override void Update(){
            base.Update();
            if(isEnemyDead){
                return;
            }
            timeSinceHittingLocalPlayer += Time.deltaTime;
            timeSinceNewRandPos += Time.deltaTime;
            if(PlayerIsTargetable != null){
                turnCompass.LookAt(targetPlayer.gameplayCamera.transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, turnCompass.eulerAngles.y, 0f)), 3f * Time.deltaTime);
            }
            // myLogSource.LogInfo($"Time: {timeSinceNewRandPos}");
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
            if (PlayerIsTargetable(targetPlayer)) {
                if(timeSinceNewRandPos > 0.7f){
                    timeSinceNewRandPos = 0;
                    if(UnityEngine.Random.Range(0, 5) == 0){
                        // Attack
                        StartCoroutine(SwingAttack());
                    }
                    else{
                        // In front of player
                        positionRandomness = new Vector3(UnityEngine.Random.Range(-2, 2f), 0, UnityEngine.Random.Range(-2f, 2f));
                        StalkPos = targetPlayer.transform.position - Vector3.Scale(new Vector3(-6, 0, -6), targetPlayer.transform.forward) + positionRandomness;
                    }
                }
                SetDestinationToPosition(StalkPos);
                agent.speed = 5f;
            }
            else{
                agent.speed = 3.5f;
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            // Also I think there is a better way to do this logging thing, but idk how.
            myLogSource.LogInfo("Toilet Leech Collision");
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

        IEnumerator SwingAttack(){
            StalkPos = targetPlayer.transform.position;
            yield return new WaitForSeconds(0.5f);
            creatureAnimator.SetTrigger("swingAttack");
            myLogSource.LogInfo($"swingAttack animation");
            yield return new WaitForSeconds(0.24f);
            Collider[] hitColliders = Physics.OverlapBox(attackArea.position, attackArea.localScale, Quaternion.identity, 1 << 3);
            if(hitColliders.Length > 0){
                foreach (var player in hitColliders){
                    PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(player);
                    if (playerControllerB != null)
                    {
                        myLogSource.LogInfo("Swing attack!!!!");
                        timeSinceHittingLocalPlayer = 0f;
                        playerControllerB.DamagePlayer(20);
                    }
                }
            }
        }
    }
}