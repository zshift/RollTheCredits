using System;
using System.Collections.Generic;
using System.Linq;
using Cocos2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RollTheCredits.Common;
using RollTheCredits.Scenes;

namespace RollTheCredits.Entities
{
    public class Level
    {
        #region Global Vars

        #region Game Textures

        public CCSpriteBatchNode PlayerCharacterSheet;
        // Physical structure of the level.
        private CCSprite[] layers;
        private CCSprite[] frontLayers;

        private CCPoint FrontLayerVelocity = new CCPoint(-1.0f, 0);

        private CCPoint Layer1Velocity = new CCPoint(-2.0f, 0);
        private CCPoint Layer2Velocity = new CCPoint(-4.0f, 0);
        private CCPoint Layer3Velocity = new CCPoint(-6.0f, 0);

        private CCPoint MaxLayer2PositionY = new CCPoint(0, -123);

        CCColor3B SkyColor = new CCColor3B(new Color(28, 178, 222));
        #endregion

        private CCPoint _layer1Vector;
        private CCPoint _layer2Vector;
        private CCPoint _layer3Vector;

        private CCPoint _ilayer1Vector;
        private CCPoint _ilayer2Vector;
        private CCPoint _ilayer3Vector;

        private CCPoint _jlayer1Vector;
        private CCPoint _jlayer3Vector;

        private CCPoint _frontLayer1Vector;
        private CCPoint _frontLayer2Vector;
        private CCPoint _frontLayer3Vector;

        public int EnemySpawnTimer = 2400;//600;
        public float DistanceCntTimer = 200;//100;
        public int CloudTimer = 600;//300;
        public int ObstacleTimer = 200;//100;
        public int SpikeTimer = 400;//200;

        public int DistanceCnt = 0;
        public int DistanceScore = 0;

        public StartGameScene StartGameScene;
        public List<Platform> TopPlatforms = new List<Platform>(60);
        public List<Platform> MidPlatforms = new List<Platform>(60);
        public List<Cloud> Clouds = new List<Cloud>(3);
        public List<Spike> Spikes = new List<Spike>(20);
        public List<CreditText> AttackCreditTexts = new List<CreditText>(5); 

        public Random Rand = new Random();

        // Entities in the level.
        public Ball Ball;
        public int Score;

        public float PlatformVelocity = -5;//-10;
        public int SpawnTimer = 6;//3;
        public int MidTileSpawnTimer = 4;//2;

        public CCColor3B CyanColor = new CCColor3B(Color.Cyan);//R:0 B:255 G:255 A:255
        public CCColor3B WhiteColor = new CCColor3B(Color.White);////R:255 B:255 G:255 A:255

        public CCPoint HighCloudPoint = new CCPoint(1300, AppDelegate.GameHeight - 30);
        public CCPoint MidCloudPoint = new CCPoint(1300, AppDelegate.GameHeight - 60);
        public CCPoint LowCloudPoint = new CCPoint(1300, AppDelegate.GameHeight - 90);

        public Platform TrapDoor;

        public List<Platform> Platforms;
        public List<CreditText> Credits;
        public List<Wall> Walls;
        public List<Question> Questions; 

        #endregion

        #region Loading

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="anindiegame"></param>
        public Level(StartGameScene startGameScene)
        {
            StartGameScene = startGameScene;
            // Load background layer textures. For now, all levels must
            // use the same backgrounds and only use the left-most part of them.
            AppDelegate.GameSpriteBatchNode.RemoveAllChildren();

            PlayerCharacterSheet = AppDelegate.GameSpriteBatchNode;

            layers = new CCSprite[12];
            layers[0] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 677, 1539, 360));
            layers[1] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 677, 1539, 360));
            layers[2] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 677, 1539, 360));
            layers[3] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1114, 1536, 277));
            layers[4] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1114, 1536, 277));

            layers[5] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 300, 1014, 377));
            layers[6] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 300, 1014, 377));
            layers[7] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 300, 1014, 377));
            layers[8] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1400, 894, 400));
            layers[9] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1400, 894, 400));
            layers[9].FlipX = true;
            layers[10] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(920, 1400, 901, 480));
            layers[11] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(920, 1400, 901, 480));
            layers[11].FlipX = true;

            frontLayers = new CCSprite[3];
            frontLayers[0] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1802, 755, 96));
            frontLayers[1] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(800, 1900, 755, 96));
            frontLayers[2] = new CCSprite(PlayerCharacterSheet.Texture, new CCRect(0, 1900, 755, 96));

            _layer1Vector = new CCPoint(0, (AppDelegate.GameHeight / 2) + 70);
            _layer2Vector = new CCPoint(layers[3].BoundingBox.Size.Width / 2, (AppDelegate.GameHeight / 2));
            _layer3Vector = new CCPoint(0, (AppDelegate.GameHeight / 2) - 100);

            _ilayer1Vector = new CCPoint(layers[0].BoundingBox.Size.Width, (AppDelegate.GameHeight / 2) + 70);
            _ilayer2Vector = new CCPoint(layers[3].BoundingBox.Size.Width * 1.5f, AppDelegate.GameHeight / 2);
            _ilayer3Vector = new CCPoint(layers[5].BoundingBox.Size.Width, (AppDelegate.GameHeight / 2) - 100);

            _jlayer1Vector = new CCPoint(layers[0].BoundingBox.Size.Width * 2, (AppDelegate.GameHeight / 2) + 70);
            _jlayer3Vector = new CCPoint(layers[5].BoundingBox.Size.Width * 2, (AppDelegate.GameHeight / 2) - 100);

            _frontLayer1Vector = new CCPoint(0, frontLayers[0].BoundingBox.Size.Height / 2);
            _frontLayer2Vector = new CCPoint(frontLayers[0].BoundingBox.Size.Width, frontLayers[1].BoundingBox.Size.Height / 2);
            _frontLayer3Vector = new CCPoint((frontLayers[0].BoundingBox.Size.Width * 2), frontLayers[2].BoundingBox.Size.Height / 2);


            var playerSpawnPoint = new CCPoint((AppDelegate.GameWidth / 3), 100);
            Ball = new Ball(this, new CCPoint(AppDelegate.GameWidth / 2, AppDelegate.GameHeight + 200));

            Credits = new List<CreditText>(5);
            Platforms = new List<Platform>(3);
            Walls = new List<Wall>(2);
            Questions = new List<Question>(7);

            Platforms.Add(new Platform("sprites/platform_big.png", PlatformTypes.Large, new CCPoint(0, -80)));
            Platforms.Add(new Platform("sprites/platform_big.png", PlatformTypes.Large, new CCPoint(AppDelegate.GameWidth, -80)));
            TrapDoor = new Platform("sprites/platform.png", PlatformTypes.YouWin, new CCPoint(AppDelegate.GameWidth / 2, 10));
            Platforms.Add(this.TrapDoor);

            Walls.Add(new Wall("sprites/wall.png", new CCPoint(0, AppDelegate.GameHeight / 2)));
            Walls.Add(new Wall("sprites/wall.png", new CCPoint(AppDelegate.GameWidth, AppDelegate.GameHeight / 2)));

            Questions.Add(new Question("Wait... what is he doing?", new CCPoint(AppDelegate.GameWidth / 2, -200)));
            Questions.Add(new Question("Where is he going?", new CCPoint(AppDelegate.GameWidth / 2, -700)));
            Questions.Add(new Question("The game is over; why is he trying to leave?", new CCPoint(AppDelegate.GameWidth / 2, -1200)));
            Questions.Add(new Question("Is he starting another journey?", new CCPoint(AppDelegate.GameWidth / 2, -1700)));
            Questions.Add(new Question("What do we do now?", new CCPoint(AppDelegate.GameWidth / 2, -2200)));
            Questions.Add(new Question("We can't allow that.", new CCPoint(AppDelegate.GameWidth / 2, -2700)));
            Questions.Add(new Question("Stop him!", new CCPoint(AppDelegate.GameWidth / 2, -3200), 200));

            //setup classic segment platforms
            for (int i = 0; i < TopPlatforms.Capacity; i++)
            {
                var newPlatform = new Platform(this, PlatformTypes.Top);
                if (i > 40)
                    newPlatform.isAlive = false;
                else
                {
                    newPlatform.StopAllActions();
                    newPlatform.Rotation = 0;
                    newPlatform.Position = new CCPoint(((newPlatform.BoundingBox.Size.Width) * (i)) + (newPlatform.BoundingBox.Size.Width / 2), (newPlatform.BoundingBox.Size.Height / 2) + newPlatform.BoundingBox.Size.Height);
                    //if(i != 24)
                    //    newPlatform.reachedSpawnPoint = true;
                }
                TopPlatforms.Add(newPlatform);
            }

            for (int i = 0; i < MidPlatforms.Capacity; i++)
            {
                var newPlatform = new Platform(this, PlatformTypes.Middle);
                MidPlatforms.Add(newPlatform);
                if (i > 40)
                    newPlatform.isAlive = false;
                else
                {
                    newPlatform.StopAllActions();
                    newPlatform.Rotation = 0;
                    newPlatform.Position = new CCPoint(((newPlatform.BoundingBox.Size.Width) * (i)) + (newPlatform.BoundingBox.Size.Width / 2), (newPlatform.BoundingBox.Size.Height / 2));
                }
            }

            for (int i = 0; i < Spikes.Capacity; i++)
            {
                var spike = new Spike(this)
                {
                    isAlive = false
                };
                Spikes.Add(spike);
            }

            for (int i = 0; i < AttackCreditTexts.Capacity; i++)
            {
                var credit = new CreditText(this, new CCPoint(100, AppDelegate.GameWidth+200), CreditTypes.AttackingFirstLast);
                credit.CreditNames.ForEach(name=>name.Color = CCColor3B.Red);
                AttackCreditTexts.Add(credit);
            }

           InitClouds();

            //add elements to screen
            layers[0].Position = _layer1Vector;
            layers[1].Position = _ilayer1Vector;
            layers[2].Position = _jlayer1Vector;

            layers[0].Color = SkyColor;
            layers[1].Color = SkyColor;
            layers[2].Color = SkyColor;


            PlayerCharacterSheet.AddChild(layers[0], 0);
            PlayerCharacterSheet.AddChild(layers[1], 0);
            PlayerCharacterSheet.AddChild(layers[2], 0);

            layers[3].Position = _layer2Vector + new CCPoint(0, -123);
            layers[4].Position = _ilayer2Vector + new CCPoint(0, -123);

            PlayerCharacterSheet.AddChild(layers[3], 2);
            PlayerCharacterSheet.AddChild(layers[4], 2);

            PlayerCharacterSheet.AddChild(layers[8], 2);
            PlayerCharacterSheet.AddChild(layers[9], 2);
            PlayerCharacterSheet.AddChild(layers[10], 2);
            PlayerCharacterSheet.AddChild(layers[11], 2);

            foreach (var cloud in Clouds)
            {
                PlayerCharacterSheet.AddChild(cloud, 3);
                if (!cloud.IsAlive)
                    cloud.Visible = false;
            }

            layers[5].Position = _layer3Vector;
            layers[6].Position = _ilayer3Vector;
            layers[7].Position = _jlayer3Vector;

            PlayerCharacterSheet.AddChild(layers[5], 4);
            PlayerCharacterSheet.AddChild(layers[6], 4);
            PlayerCharacterSheet.AddChild(layers[7], 4);

            Platforms.ForEach(platform => StartGameScene.AddChild(platform, 5));
            Walls.ForEach(wall => StartGameScene.AddChild(wall, 4));
            StartGameScene.AddChild(Ball, 4);
            Questions.ForEach(question => StartGameScene.AddChild(question, 4));

            Credits.Add(new CreditText(this, new CCPoint(AppDelegate.GameWidth / 2, -200), CreditTypes.RollingFirstLast));
            Credits.Add(new CreditText(this, new CCPoint(AppDelegate.GameWidth / 2, -400), CreditTypes.RollingFirstMiddleLast));
            Credits.Add(new CreditText(this, new CCPoint(AppDelegate.GameWidth / 2, -600), CreditTypes.RollingFirstLast));
            Credits.Add(new CreditText(this, new CCPoint(AppDelegate.GameWidth / 2, -800), CreditTypes.RollingFirstMiddleLast));
            Credits.Add(new CreditText(this, new CCPoint(AppDelegate.GameWidth / 2, -1000), CreditTypes.RollingFirstLast));

            Ball.Reset(new CCPoint(AppDelegate.GameWidth / 2, AppDelegate.GameHeight + 200));
          
            foreach (var s in Spikes)
            {
                PlayerCharacterSheet.AddChild(s, 4);
                if (!s.isAlive)
                    s.Visible = false;
            }

            foreach (var p in TopPlatforms)
            {
                PlayerCharacterSheet.AddChild(p, 5);
                if (!p.isAlive)
                {
                    p.Visible = false;
                }
            }

            foreach (var p in MidPlatforms)
            {
                PlayerCharacterSheet.AddChild(p, 5);
                if (!p.isAlive)
                {
                    p.Visible = false;
                }
            }

            frontLayers[0].Position = _frontLayer1Vector;
            frontLayers[1].Position = _frontLayer2Vector;
            frontLayers[2].Position = _frontLayer3Vector;

            PlayerCharacterSheet.AddChild(frontLayers[0], 6);
            PlayerCharacterSheet.AddChild(frontLayers[1], 6);
            PlayerCharacterSheet.AddChild(frontLayers[2], 6);

            StartGameScene.AddChild(PlayerCharacterSheet);

            frontLayers[0].Visible = false;
            frontLayers[1].Visible = false;
            frontLayers[2].Visible = false;

            layers[0].Visible = false;
            layers[1].Visible = false;
            layers[2].Visible = false;
            layers[3].Visible = false;
            layers[4].Visible = false;
            layers[5].Visible = false;
            layers[6].Visible = false;
            layers[7].Visible = false;
            layers[8].Visible = false;
            layers[9].Visible = false;
            layers[10].Visible = false;
            layers[11].Visible = false;

            TopPlatforms.ForEach(tile=>tile.Visible = false);
            MidPlatforms.ForEach(tile => tile.Visible = false);
        }

        private void InitClouds()
        {
            for (int i = 0; i < Clouds.Capacity; i++)
            {
                int randomCloundIndex = Rand.Next(3);
                switch (randomCloundIndex)
                {
                    case 0:
                        int randomCloundTypeIndex = Rand.Next(3);
                        switch (randomCloundTypeIndex)
                        {
                            case 0:
                                Clouds.Add(new Cloud(this, CloudFiles.One, CloudTypes.High) { IsAlive = false });
                                break;
                            case 1:
                                Clouds.Add(new Cloud(this, CloudFiles.One, CloudTypes.Middle) { IsAlive = false });
                                break;
                            case 2:
                                Clouds.Add(new Cloud(this, CloudFiles.One, CloudTypes.Low) { IsAlive = false });
                                break;
                            default:
                                Clouds.Add(new Cloud(this, CloudFiles.One, CloudTypes.High) { IsAlive = false });
                                break;
                        }
                        break;
                    case 1:
                        randomCloundTypeIndex = Rand.Next(3);
                        switch (randomCloundTypeIndex)
                        {
                            case 0:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.High) { IsAlive = false });
                                break;
                            case 1:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Middle) { IsAlive = false });
                                break;
                            case 2:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Low) { IsAlive = false });
                                break;
                            default:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Middle) { IsAlive = false });
                                break;
                        }
                        break;
                    case 2:
                        randomCloundTypeIndex = Rand.Next(3);
                        switch (randomCloundTypeIndex)
                        {
                            case 0:
                                Clouds.Add(new Cloud(this, CloudFiles.Three, CloudTypes.High) { IsAlive = false });
                                break;
                            case 1:
                                Clouds.Add(new Cloud(this, CloudFiles.Three, CloudTypes.Middle) { IsAlive = false });
                                break;
                            case 2:
                                Clouds.Add(new Cloud(this, CloudFiles.Three, CloudTypes.Low) { IsAlive = false });
                                break;
                            default:
                                Clouds.Add(new Cloud(this, CloudFiles.Three, CloudTypes.Low) { IsAlive = false });
                                break;
                        }
                        break;
                    default:
                        randomCloundTypeIndex = Rand.Next(3);
                        switch (randomCloundTypeIndex)
                        {
                            case 0:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.High) { IsAlive = false });
                                break;
                            case 1:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Middle) { IsAlive = false });
                                break;
                            case 2:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Low) { IsAlive = false });
                                break;
                            default:
                                Clouds.Add(new Cloud(this, CloudFiles.Two, CloudTypes.Middle) { IsAlive = false });
                                break;
                        }
                        break;

                }
            }
        }

        public Ball GetPlayer()
        {
            return Ball;
        }

        public int GetDistanceCount()
        {
            return DistanceCnt;
        }

        public int GetDistanceScore()
        {
            return DistanceCnt; //distanceScore;
        }

        public Level GetLevel()
        {
            return this;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(
            float gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState)
        {
            UpdateLevel(gameTime, keyboardState, gamePadState);
        }

        public void UpdateLevel(float gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            UpdateBall(gameTime);

            if (!StartGameScene.StartCredits)
            {
                return;
            }

            UpdatePlatforms();
            UpdateSpikes(gameTime);
            UpdateWalls();

            if (!TrapDoor.Visible)
            {
                UpdateCreditTexts();
            }

            UpdateQuestions();

            if (!StartGameScene.IsGameOver)
            {
                UpdateBackground();
            }

            if (StartGameScene.HasGameStarted && !StartGameScene.IsGameOver)
            {
                UpdateGameStats();
                PlatformVelocity -= 0.0003f / 2.0f;

                UpdateAttackCredits();

                // reset our timer
                GetPlayer().Update(gameTime, keyboardState, gamePadState);

                UpdateClouds();          
            }

            DrawLevel(gameTime);
        }

        public void UpdatePlatforms()
        {
            foreach (var platform in Platforms.Where(x => x.Visible && x.PlatformType != PlatformTypes.Top && x.PlatformType != PlatformTypes.Middle))
            {
                platform.Update();
            }

            if (!StartGameScene.HasGameStarted) return;

            SpawnTimer--;
            foreach (Platform platform in TopPlatforms)
            {
                if (platform.isAlive)
                {
                    platform.Update();
                }
            }

            if (SpawnTimer == 0)//if (platform.reachedSpawnPoint && !platform.spawnPlatform)
            {
                // we spawn 1-3 enemies per spawn
                foreach (Platform _platform in TopPlatforms)
                {
                    if (!_platform.isAlive)
                    {
                        _platform.Reset();
                        break;
                    }
                }
                foreach (Platform _platform in MidPlatforms)
                {
                    if (!_platform.isAlive)
                    {
                        _platform.Reset();
                        break;
                    }
                }
                SpawnTimer = 3;
            }

            foreach (Platform platform in MidPlatforms)
            {
                if (platform.isAlive)
                {
                    platform.Update();
                }
            }
        }

        private void UpdateSpikes(float gameTime)
        {
            if (SpikeTimer < 0)
            {
                int spikeCount = Rand.Next(7) + 1;
                SpikeTimer = Rand.Next(200) + 200;
                for (int i = 0; i < spikeCount; i++)
                {
                    foreach (var spike in Spikes)
                    {
                        if (!spike.isAlive)
                        {
                            spike.Reset(i);
                            break;
                        }
                    }
                }
            }

            foreach (var spike in Spikes)
            {
                if (spike.isAlive)
                {
                    spike.Update();
                }
            }
        }

        public void UpdateBall(float gameTime)
        {
            var _keyboardState = Keyboard.GetState();
            var _gamePadState = GamePad.GetState(PlayerIndex.One);

            Ball.Update(gameTime, _keyboardState, _gamePadState);

            if (!Ball.ExitedTheGame)
            if (Ball.PositionX - Ball.BoundingBox.Size.Width/2 > AppDelegate.GameWidth ||
                Ball.PositionX + Ball.BoundingBox.Size.Width/2 < 0)
            {
                StartGameScene.theBallHasLeftTheBuilding = true;
                StartGameScene.HasGameStarted = true;
                Ball.IsRolling = true;
                frontLayers[0].Visible = true;
                frontLayers[1].Visible = true;
                frontLayers[2].Visible = true;

                layers[0].Visible = true;
                layers[1].Visible = true;
                layers[2].Visible = true;
                layers[3].Visible = true;
                layers[4].Visible = true;
                layers[5].Visible = true;
                layers[6].Visible = true;

                TopPlatforms.ForEach(tile => tile.Visible = true);
                MidPlatforms.ForEach(tile => tile.Visible = true);

                Walls.ForEach(StartGameScene.RemoveChild);
                Platforms.ForEach(StartGameScene.RemoveChild);

                Ball.PositionX = AppDelegate.GameWidth / 4;
                Ball.PositionY = AppDelegate.GameHeight / 4;
                Ball.ExitedTheGame = true;
            }
        }

        private void UpdateAttackCredits()
        {
            EnemySpawnTimer -= 3;

            if (EnemySpawnTimer < 0)
            {
                EnemySpawnTimer = Rand.Next(500) + 300;

                int spawnX = 1300;
                bool spawnedEnemy = false;
                foreach (var credit in AttackCreditTexts)
                {
                    if (!credit.IsAlive && !spawnedEnemy)
                    {
                        spawnedEnemy = true;
                        credit.Reset(new CCPoint(spawnX, AppDelegate.GameHeight/2));
                    }
                }
            }

            foreach (var creditText in AttackCreditTexts)
            {
                creditText.Update();
            }
        }

        public void UpdateWalls()
        {
            foreach (var wall in Walls)
            {
                wall.Update();
            }
        }

        public void UpdateCreditTexts()
        {
            var toRemove = new List<CreditText>();

            foreach (var creditText in Credits)
            {
                if (creditText.CreditNames.Any())
                {
                    creditText.Update();
                }
                else
                {
                    toRemove.Add(creditText);
                }
            }

            toRemove.ForEach(credit => Credits.Remove(credit));
        }

        private void UpdateQuestions()
        {
            if (Credits.Any())
            {
                return;
            }

            Question toRemove = null;
            foreach (var question in Questions)
            {
                question.Update();

                if (question.PositionY > AppDelegate.GameHeight + 200)
                {
                    StartGameScene.RemoveChild(question);
                    toRemove = question;
                }
            }

            if (toRemove != null)
            {
                Questions.Remove(toRemove);
            }
        }

        private void UpdateGameStats()
        {
            DistanceCntTimer -= 2;
            DistanceCntTimer += PlatformVelocity / 10000f;
            CloudTimer -= 1;
            ObstacleTimer -= 1;
            SpikeTimer -= 1;

            if (DistanceCntTimer < 0)
            {
                DistanceCntTimer = 100;
                DistanceCnt++;
                DistanceScore++;
            }
        }

        private void UpdateClouds()
        {
            foreach (Cloud cloud in Clouds)
            {
                if (cloud.IsAlive)
                {
                    cloud.Update();
                }
            }

            if (CloudTimer < 0)
            {
                CloudTimer = 300;

                foreach (Cloud cloud in Clouds)
                {
                    if (!cloud.IsAlive)
                    {
                        cloud.Reset();
                        break;
                    }
                }
            }
        }

        private void UpdateBackground()
        {
            if (_layer1Vector.X < (-layers[0].BoundingBox.Size.Width))
            {
                _layer1Vector.X = _layer1Vector.X + (layers[0].BoundingBox.Size.Width * 3);
            }

            if (_ilayer1Vector.X < (-layers[0].BoundingBox.Size.Width))
            {
                _ilayer1Vector.X = _ilayer1Vector.X + (layers[0].BoundingBox.Size.Width * 3);
            }

            if (_jlayer1Vector.X < (-layers[0].BoundingBox.Size.Width))
            {
                _jlayer1Vector.X = _jlayer1Vector.X + (layers[0].BoundingBox.Size.Width * 3);
            }


            if (_layer2Vector.X < (-layers[3].BoundingBox.Size.Width / 2))
            {
                _layer2Vector.X = _layer2Vector.X + (layers[3].BoundingBox.Size.Width * 2);
            }

            if (_ilayer2Vector.X < (-layers[3].BoundingBox.Size.Width / 2))
            {
                _ilayer2Vector.X = _ilayer2Vector.X + (layers[3].BoundingBox.Size.Width * 2);
            }


            if (_layer3Vector.X < (-layers[5].BoundingBox.Size.Width))
            {
                _layer3Vector.X = _layer3Vector.X + (layers[5].BoundingBox.Size.Width * 3);
            }

            if (_ilayer3Vector.X < (-layers[5].BoundingBox.Size.Width))
            {
                _ilayer3Vector.X = _ilayer3Vector.X + (layers[5].BoundingBox.Size.Width * 3);
            }

            if (_jlayer3Vector.X < (-layers[5].BoundingBox.Size.Width))
            {
                _jlayer3Vector.X = _jlayer3Vector.X + (layers[5].BoundingBox.Size.Width * 3);
            }


            this.UpdateFrontLayer();

            _layer1Vector += Layer1Velocity;
            _ilayer1Vector += Layer1Velocity;
            _jlayer1Vector += Layer1Velocity;
            _layer2Vector += Layer2Velocity;
            _ilayer2Vector += Layer2Velocity;
            _layer3Vector += Layer3Velocity;
            _ilayer3Vector += Layer3Velocity;
            _jlayer3Vector += Layer3Velocity;
        }

        private void UpdateFrontLayer()
        {
            if (this._frontLayer1Vector.X < (-this.frontLayers[0].BoundingBox.Size.Width))
            {
                this._frontLayer1Vector.X = this._frontLayer3Vector.X + this.frontLayers[0].BoundingBox.Size.Width;
            }
            if (this._frontLayer2Vector.X < (-this.frontLayers[0].BoundingBox.Size.Width))
            {
                this._frontLayer2Vector.X = this._frontLayer1Vector.X + this.frontLayers[0].BoundingBox.Size.Width;
            }
            if (this._frontLayer3Vector.X < (-this.frontLayers[0].BoundingBox.Size.Width))
            {
                this._frontLayer3Vector.X = this._frontLayer2Vector.X + this.frontLayers[0].BoundingBox.Size.Width;
            }

            _frontLayer1Vector += FrontLayerVelocity;
            _frontLayer2Vector += FrontLayerVelocity;
            _frontLayer3Vector += FrontLayerVelocity;
        }

        #endregion

        #region Draw


        public void DrawLevel(float gameTime)
        {
            if (StartGameScene.HasGameStarted && !StartGameScene.IsGameOver || StartGameScene.IsUpdating && StartGameScene.HasGameStarted && !StartGameScene.IsGameOver)
            {
                layers[0].Position = _layer1Vector;
                layers[1].Position = _ilayer1Vector;
                layers[2].Position = _jlayer1Vector;

                layers[3].Position = _layer2Vector + MaxLayer2PositionY;
                layers[4].Position = _ilayer2Vector + MaxLayer2PositionY;

                foreach (Cloud cloud in Clouds)
                {
                    cloud.Visible = cloud.IsAlive;
                }

                layers[5].Position = _layer3Vector;
                layers[6].Position = _ilayer3Vector;
                layers[7].Position = _jlayer3Vector;

                frontLayers[0].Position = _frontLayer1Vector;
                frontLayers[1].Position = _frontLayer2Vector;
                frontLayers[2].Position = _frontLayer3Vector;
            }
        }

        #endregion
    }
}
