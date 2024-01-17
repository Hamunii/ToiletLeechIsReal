using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Logging;
using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace ToiletLeechIsReal {

    // You may be wondering, how does the Toilet Leech know it is from class ToiletLeechAI?
    // Well, we give it a reference to to this class in the Unity project where we make the asset bundle.
    // Asset bundles cannot contain scripts, so our script lives here. It is important to get the
    // reference right, or else it will not find this file. TODO: explain how to do the reference right.

    // Also, I have no idea if this syncs well on multiplayer.

    // For reference, see EnemyAI with a decompiler. Also stuff like BlobAI is good to check too.

    class ToiletLeechAI : EnemyAI {

        // We set these in our Asset Bundle, so we can disable warning CS0649
        #pragma warning disable 0649
        public Transform turnCompass;
        public Transform attackArea;
        public AISearchRoutine scoutingSearchRoutine;
        #pragma warning restore 0649
        float timeSinceHittingLocalPlayer;
        float timeSinceNewRandPos;
        Vector3 positionRandomness;
        Vector3 StalkPos;
        private ManualLogSource myLogSource;
        bool isSearching;
        System.Random enemyRandom;

        public override void Start()
		{
			base.Start();
            myLogSource = BepInEx.Logging.Logger.CreateLogSource("Toilet Leech AI");
            myLogSource.LogInfo("Toilet Leech Spawned");
            timeSinceHittingLocalPlayer = 0;
            timeSinceNewRandPos = 0;
            positionRandomness = new Vector3(0, 0, 0);
            isSearching = false;
            creatureAnimator.SetTrigger("startWalk");
            enemyRandom = new System.Random(StartOfRound.Instance.randomMapSeed + thisEnemyIndex);
		}
        public override void Update(){
            base.Update();
            if(isEnemyDead){
                myLogSource.LogInfo("Dead");
                return;
            }
            timeSinceHittingLocalPlayer += Time.deltaTime;
            timeSinceNewRandPos += Time.deltaTime;
            if(targetPlayer != null && PlayerIsTargetable(targetPlayer) && !isSearching){
                turnCompass.LookAt(targetPlayer.gameplayCamera.transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, turnCompass.eulerAngles.y, 0f)), 3f * Time.deltaTime);
            }
            // myLogSource.LogInfo($"Time: {timeSinceNewRandPos}");
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (isEnemyDead)
            {
                agent.speed = 0f;
                return;
            }
            if(!IsOwner){
                return;
            }
            if (!isEnemyDead && !StartOfRound.Instance.allPlayersDead)
            {
                if (TargetClosestPlayer(4f) && Vector3.Distance(transform.position, targetPlayer.transform.position) < 25)
                {
                    if(isSearching){
                        myLogSource.LogInfo("Target Player");
                        StopSearch(scoutingSearchRoutine);
                        isSearching = false;
                        movingTowardsTargetPlayer = true;
                    }
                }
                else
                {
                    if(!isSearching){
                        myLogSource.LogInfo("Stop Target Player");
                        StartSearch(transform.position, scoutingSearchRoutine);
                        isSearching = true;
                        movingTowardsTargetPlayer = false;
                        // This is flawed creatureAnimator logic
                    }
                    
                }
            }
            if (targetPlayer != null && PlayerIsTargetable(targetPlayer) && !isSearching) {
                if(timeSinceNewRandPos > 0.7f){
                    timeSinceNewRandPos = 0;
                    if(enemyRandom.Next(0, 6) == 0){
                        // Attack
                        StartCoroutine(SwingAttack());

                    }
                    else{
                        // In front of player
                        positionRandomness = new Vector3(enemyRandom.Next(-2, 2), 0, enemyRandom.Next(-2, 2));
                        StalkPos = targetPlayer.transform.position - Vector3.Scale(new Vector3(-5, 0, -5), targetPlayer.transform.forward) + positionRandomness;
                    }
                }
                SetDestinationToPosition(StalkPos);
                agent.speed = 4f;
            }
            else{
                agent.speed = 3.5f;
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            myLogSource.LogInfo("Toilet Leech Collision");
            if (timeSinceHittingLocalPlayer < 1f)
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
            if(isEnemyDead){
                yield break;
            }
            myLogSource.LogInfo($"swingAttack animation");
            SwingAttackAnimationClientRPC();
            yield return new WaitForSeconds(0.24f);
            SwingAttackHitClientRPC();
        }

        public override void HitEnemy(int force = 1, PlayerControllerB playerWhoHit = null, bool playHitSFX = false)
        {
            base.HitEnemy(force, playerWhoHit, playHitSFX);
            if(isEnemyDead){
                return;
            }
            enemyHP -= force;
            if (IsOwner) {
                if (enemyHP <= 0) {
                    creatureVoice.Stop();
                    creatureAnimator.SetTrigger("setDie");
                    // Our death sound will be played through creatureVoice when KillEnemy() is called
                    KillEnemyOnOwnerClient();
                    return;
                }
            }
        }

        [ClientRpc]
        public void SwingAttackAnimationClientRPC()
        {
            myLogSource.LogInfo("SwingAttackAnimationClientRPC");
            creatureAnimator.SetTrigger("swingAttack");
        }
        [ClientRpc]
        public void SwingAttackHitClientRPC()
        {
            myLogSource.LogInfo("SwingAttackHitClientRPC");
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