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
		}
        public override void Update(){
            base.Update();
            if(isEnemyDead){
                return;
            }
            timeSinceHittingLocalPlayer += Time.deltaTime;
            timeSinceNewRandPos += Time.deltaTime;
            if(PlayerIsTargetable != null && !isSearching){
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
                        StartSearch(base.transform.position, scoutingSearchRoutine);
                        isSearching = true;
                        movingTowardsTargetPlayer = false;
                        // This is flawed creatureAnimator logic
                    }
                    
                }
            }
            if (PlayerIsTargetable(targetPlayer) && !isSearching) {
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
                agent.speed = 4f;
            }
            else{
                agent.speed = 3.5f;
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            // Also I think there is a better way to do this logging thing, but idk how.
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

        public override void HitEnemy(int force = 1, PlayerControllerB playerWhoHit = null, bool playHitSFX = false)
        {
            base.HitEnemy(force, playerWhoHit, playHitSFX);
            if(isEnemyDead){
                return;
            }
            enemyHP -= force;
            if (IsOwner) {
                if (enemyHP <= 0) {
                    //creatureAnimator.SetTrigger("Killed");
                    KillEnemyOnOwnerClient();
                    return;
                }
            }
        }
    }
}