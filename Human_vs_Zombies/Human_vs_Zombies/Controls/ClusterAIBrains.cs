﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Human_vs_Zombies.HvZClasses.Mobs;
using Human_vs_Zombies.HvZClasses;
using Human_vs_Zombies.Controls;
using Microsoft.Xna.Framework;

namespace Human_vs_Zombies.Controls
{
    public class ClusterAIBrains : Brains
    {
        // Zombies will be in "awaiting attack" mode while their position
        // is in the circles of radius minSafeRadius and maxSafeRadius centered
        // on the player.
        // Please ensure at all times that minSafeRadius <= happyRadius <= maxSafeRadius
        public const float minSafeRadius = 200f; // When a zombie is closer to the player than this, the zombie will attack.
        public const float happyRadius = 300f;   // Zombies will desire to be this distance from the player before the onslaught.
        public const float maxSafeRadius = 400f; // When a zombie is further than this, the zombie will readjust its position.

        private HvZWorld m_HvZWorld;

        private Vector2 m_Shoot;
        private Vector2 m_Walk;
        private Vector2 m_Destination;
        private bool m_isReady, m_isAttacking;

        public ClusterAIBrains(HvZWorld hvzWorld)
        {
            this.m_HvZWorld = hvzWorld;
            this.m_Shoot = new Vector2();
            this.m_Walk = new Vector2();
            this.m_isReady = false;
            this.m_isAttacking = false;
        }

        public override void Update(float dTime, Vector2 position)
        {
            Vector2 toPlayer = m_HvZWorld.GetPlayer().GetPosition() - position;
            float playerDistance = toPlayer.Length();
            toPlayer.Normalize();

            if (playerDistance < ClusterAIBrains.happyRadius)  // If the zombie is potentially too close to the human.
            {
                if (playerDistance < ClusterAIBrains.minSafeRadius)
                {
                    // If the zombie is too close to the human, the zombie should attack!
                    this.m_isAttacking = true;
                    this.m_isReady = false;
                }
                else
                {
                    // The zombie is between minSafeRadius and happyRadius
                    this.m_isAttacking = false;
                    this.m_isReady = true;
                }
            }
            else
            {
                if (playerDistance > ClusterAIBrains.maxSafeRadius)
                {
                    // If the zombie is too far away from the human, readjust accordingly
                    this.m_isReady = false;
                    this.m_isAttacking = false;
                }
                else
                {
                    // The zombie is between happyRadius and maxSafeRadius
                    this.m_isAttacking = false;
                    this.m_isReady = true;
                }
            }

            if (this.m_isAttacking)
            {
                this.m_Walk = toPlayer;
            } else if (!this.m_isReady)
            {
                this.m_Walk = toPlayer;
                // Control the velocity here?
            }
            else
            {
                this.m_Walk = Vector2.Zero;
            }
            this.m_Shoot = toPlayer;
            //path.Normalize();
            //this.m_Walk = path;
            //this.m_Shoot = this.m_Walk;
        }

        public override Vector2 GetWalk()
        {
            return m_Walk;
        }

        public override Vector2 GetShoot()
        {
            return m_Shoot;
        }
    }
}