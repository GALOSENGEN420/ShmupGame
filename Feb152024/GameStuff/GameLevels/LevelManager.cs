﻿using Feb152024.Extensions;
using Feb152024.GameStuff.CharacterBehaviors;
using Feb152024.GameStuff.EnemyBehaviors;
using Feb152024.GameStuff.Enums;
using Feb152024.GameStuff.ShotStuff.ShotPatternBehavior;
using Feb152024.GameStuff.ShotStuff.ShotPatternBehavior.Enemies;
using Feb152024.GameStuff.Statics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Feb152024.GameStuff.GameLevels
{
    public class LevelManager
    {
        private GameTimer timer;
        private Random Rand = new Random();
        private LevelConfiguration Levels;
        private double LevelTimer;
        private SpriteFont font;

        public LevelManager()
        {
            timer = Globals.TimerSpawner.CreateTimer(2000);
            Levels = JsonConvert.DeserializeObject<LevelConfiguration>(File.ReadAllText("H:\\Repo\\Feb152024\\Feb152024\\GameStuff\\GameLevels\\Levels.json"));
            font = Globals.ContentManager.Load<SpriteFont>("Arial");

        }

        public void Update(GameTime gameTime)
        {
            LevelTimer += (double)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var enemy in Levels.Enemies)
            {
                if((int)LevelTimer == enemy.SpawnTime && enemy.Spawned == false)
                {
                    EnemyBuilder EnemyBuilder = new EnemyBuilder();
                    var characters = EnemyBuilder.AddEnemyData(enemy.Name.ParseEnum<Enemies>(), enemy.X, enemy.Y)
                                                 .AddCharacterType(CharacterTypeEnum.Enemy)
                                                 .AddBehavior(new EnemyDownBehavior())
                                                 .AddShotBehavior(new EnemyStraightShotBehavior(), 1000)
                                                 .BuildEnemy();

                    Battlefield.Characters.Add(characters);
                    enemy.Spawned = true;
                }
            }

            //if (timer.CheckTimer())
            //{
            //    EnemyBuilder EnemyBuilder = new EnemyBuilder();
            //    var characters = EnemyBuilder.AddEnemyData(Enemies.Tewi, Rand.Next(0, 600), 0)
            //                                 .AddCharacterType(CharacterTypeEnum.Enemy)
            //                                 .AddBehavior(new EnemyDownBehavior())
            //                                 .AddShotBehavior(new EnemyStraightShotBehavior(), 1000)
            //                                 .BuildEnemy();

            //    Battlefield.Characters.Add(characters);
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, $"Time: {(int)LevelTimer}", new Vector2(10,10), Color.White);
        }
    }
}