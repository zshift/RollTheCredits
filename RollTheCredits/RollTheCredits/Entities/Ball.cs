using System;
using Cocos2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RollTheCredits.Common;

namespace RollTheCredits.Entities
{
    public class Ball : CCSprite
    {
        #region variables
        public bool IsAlive;
        
        private int _blinkCnt = 3;
        public bool IsBlinking = false;

        public int alpha = 255;

        public bool isAlive;

        public Level Level;

        private float previousBottom;

        private int jumpCnt = 0;
        public bool canJump = true;

        public bool IsBouncing;
        
        public CCPoint velocity;
        #endregion
        
        #region Constants for controlling vertical movement
        private const float MaxJumpTime = 0.8f;
        private float JumpLaunchVelocity = 3000.0f;
        private const float GravityAcceleration = 3600.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.08f;//0.26f;
        #endregion

        public bool IsOnGround;

        private float movement;

        #region Jumping/movement state
        public bool isJumping;
        public bool wasJumping;
        public float jumpTime;  

        public bool isMovingLeft;
        public bool isMovingRight;

        public bool isRunning;
        #endregion

        public bool isAction;
        public bool hasDoubleJump = false;

        #region Control consts
        private const Buttons JumpButton = Buttons.A;
        private const Buttons LeftButton = Buttons.LeftThumbstickLeft;
        private const Buttons RightButton = Buttons.LeftThumbstickRight;
        #endregion

        public bool YouWin = false;
        private bool startCredits;

        private CCSprite Carlos;

        private float twisting;

        /// <summary>
        /// Constructors a new GetPlayer().
        /// </summary>
        public Ball(Level level,CCPoint position)
        {
            base.InitWithFile("sprites/dot.png");
            Level = level;
            isRunning = false;
            LoadContent();

            Reset(position);
        }

        public bool ExitedTheGame { get; set; }

        public bool IsRolling { get; set; }

        public void WonGame(Action action)
        {
            action();
        }

        public void LoadContent()
        {
            Carlos = new CCSprite("sprites/carlos.png");
            AddChild(Carlos);
        }

        public void Reset(CCPoint position)
        {
            Position = position;
            velocity = CCPoint.Zero;
            isAlive = true;
            Carlos.Position = new CCPoint(BoundingBox.Size.Width/2, BoundingBox.Size.Height/2);
        }

        private bool CheckIfFell()
        {
            return (Position.Y > AppDelegate.GameHeight);
        }

        public void Update(float gameTime, KeyboardState keyboardState, GamePadState gamePadState)
        {
            //foreach (var life in Lives)
            //{
            //    life.ItemUpdate(gameTime);
            //}
            // Clear input.
            movement = 0.0f;
            isJumping = false;
            isAction = false;

            GetInput(keyboardState, gamePadState);

            if (startCredits)
            {
                Level.StartGameScene.StartCredits = true;
            }

            //if (CheckIfFell() && isAlive)
            //{
            //    //OnKilled();
            //}

            if (IsRolling)
            {
                Rotation += 5.0f;

                Rotation += twisting;
            }

            ApplyPhysics(gameTime);

            //switch (level.Anindiegame.GameType)
            //{
            //    case GameType.ClassicGame:
            //        ClassicGameUpdate(gameTime);
            //        break;
            //}

            if (isAlive && IsOnGround && !isRunning)
            {
                isRunning = true;
                //StopAllActions();
                //RunAction(PlayerRunAction);
            }

            //if (!isAlive && IsOnGround)
            //    Position = new CCPoint(Position.X + level.PlatformVelocity, Position.Y);


            if (IsBlinking)
            {
                if (alpha > 0)
                {
                    alpha = alpha - 20;
                }
                else if (_blinkCnt == 0)
                {
                    IsBlinking = false;
                    _blinkCnt = 3;
                    alpha = 255;
                }
                else
                {
                    _blinkCnt--;
                    alpha = 255;
                }
                Opacity = (byte)alpha;
            }

            JumpLaunchVelocity = 3000;

            if (ExitedTheGame)
            {
                if (PositionX + BoundingBox.Size.Width / 2 > AppDelegate.GameWidth)
                {
                    PositionX = AppDelegate.GameWidth - BoundingBox.Size.Width / 2;
                }

                if (PositionX - BoundingBox.Size.Width / 2 < 0)
                {
                    PositionX = BoundingBox.Size.Width / 2;
                }
            }
        }

        private void GetInput(KeyboardState keyboardState, GamePadState gamePadState)
        {
            if (!YouWin)
            {
                return;
            }

            if (!startCredits)
            {
                startCredits = IsJumpPressed(keyboardState, gamePadState);
                return;
            }

            twisting = -5*gamePadState.Triggers.Left + 5*gamePadState.Triggers.Right;
            isJumping = IsJumpPressed(keyboardState, gamePadState) || this.IsBouncing;
            isMovingLeft = gamePadState.IsButtonDown(LeftButton) || keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left);
            isMovingRight = gamePadState.IsButtonDown(RightButton) || keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right);
        }

        private static bool IsJumpPressed(KeyboardState keyboardState, GamePadState gamePadState)
        {
            return gamePadState.IsButtonDown(Buttons.A)
                || keyboardState.IsKeyDown(Keys.Space)
                || keyboardState.IsKeyDown(Keys.W)
                || keyboardState.IsKeyDown(Keys.J);
        }

        public void ApplyPhysics(float gameTime)
        {
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y = MathHelper.Clamp(velocity.Y - GravityAcceleration * gameTime, -MaxFallSpeed, MaxFallSpeed);
            velocity.Y = this.Jump(velocity.Y, gameTime);

            velocity.X = this.Translate(this.velocity.X);

            // Apply velocity
            Position += velocity * gameTime;
            Position = new CCPoint((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollisions();
        }

        private void HandleCollisions()
        {
            //for animated sprites, bounds doubles the width, unsuer why, but boundingBox returns the correct values that are expected
            //animated sprites need to create a rect from boundingBox values, this only effects sprite's bottom and top bounds
            
            //new Rectangle((int)BoundingBox.Origin.X, (int)BoundingBox.Origin.Y, (int)BoundingBox.Size.Width,(int)BoundingBox.Size.Height);
            IsOnGround = false;

            if (!ExitedTheGame)
            {
                CheckPlatformCollisions();
                CheckWallCollisions();
                CheckCreditTextCollisions();
            }

            if(Level.StartGameScene.HasGameStarted)
                CheckTileCollisions();

            var bounds = GetSpriteBounds(this);

            // Save the new bounds bottom.
            previousBottom = bounds.Top;
        }

        private void CheckWallCollisions()
        {
            var bounds = GetSpriteBounds(this);

            foreach (var wall in this.Level.Walls)
            {
                var wallBounds = GetSpriteBounds(wall);
                var depth = bounds.GetIntersectionDepth(wallBounds);
                if (wall.BoundingBox.IntersectsRect(this.BoundingBox) && !wall.IsBroken)
                {
                    wall.Hitting();
                    this.Position = new CCPoint(this.Position.X + depth.X, this.Position.Y);
                }
                else if (!wall.IsBroken)
                {
                    wall.StopHitting();
                }

                bounds = GetSpriteBounds(this);
            }
        }

        private void CheckPlatformCollisions()
        {
            var bounds = GetSpriteBounds(this);

            foreach (var platform in this.Level.Platforms)
            {
                var platformBounds = GetSpriteBounds(platform);
                var depth = bounds.GetIntersectionDepth(platformBounds);
                if (platform.BoundingBox.IntersectsRect(this.BoundingBox)) //if (depth != Vector2.Zero)
                {
                    if (platform.PlatformType == PlatformTypes.YouWin)
                    {
                        YouWin = true;
                        Level.StartGameScene.YouWin();
                    }

                    this.IsOnGround = true;
                    this.jumpCnt = 0;
                    if (this.isAlive)
                    {
                        //WaitStep--;
                        //if (WaitStep < 0)
                        //{
                        //    if (level.Anindiegame.Appsettings.IsSoundEnabled)
                        //    {
                        //        if (IsMaximized)
                        //        {
                        //            WaitStep = 50;
                        //            int ran = level.Rand.Next(2) + 1;
                        //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_giant-00" + ran);
                        //        }
                        //        else
                        //        {
                        //            WaitStep = 100;
                        //            int ran = level.Rand.Next(12) + 1;
                        //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_grass_boots_jog-0" + (ran < 10 ? "0" + ran : ran.ToString()));
                        //        }
                        //    }
                        //}
                    }

                    this.Position = new CCPoint(this.Position.X, this.Position.Y + depth.Y);
                    bounds = GetSpriteBounds(this);
                }
            }
        }

        private void CheckTileCollisions()
        {
            var bounds = GetSpriteBounds(this);

            foreach (var platform in this.Level.TopPlatforms)
            {
                var platformBounds = GetSpriteBounds(platform);
                var depth = bounds.GetIntersectionDepth(platformBounds);
                if (platform.BoundingBox.IntersectsRect(this.BoundingBox)) //if (depth != Vector2.Zero)
                {
                    this.IsOnGround = true;
                    this.jumpCnt = 0;
                    if (this.isAlive)
                    {
                        //WaitStep--;
                        //if (WaitStep < 0)
                        //{
                        //    if (level.Anindiegame.Appsettings.IsSoundEnabled)
                        //    {
                        //        if (IsMaximized)
                        //        {
                        //            WaitStep = 50;
                        //            int ran = level.Rand.Next(2) + 1;
                        //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_giant-00" + ran);
                        //        }
                        //        else
                        //        {
                        //            WaitStep = 100;
                        //            int ran = level.Rand.Next(12) + 1;
                        //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_grass_boots_jog-0" + (ran < 10 ? "0" + ran : ran.ToString()));
                        //        }
                        //    }
                        //}
                    }

                    this.Position = new CCPoint(this.Position.X, this.Position.Y + depth.Y);
                    bounds = GetSpriteBounds(this);
                }
            }
        }

        private void CheckCreditTextCollisions()
        {
            var bounds = GetSpriteBounds(this);

            foreach (var credit in Level.Credits)
            {
                credit.CreditNames.ForEach(name =>
                {
                    var nameBounds = GetSpriteBounds(name);
                    var depth = bounds.GetIntersectionDepth(nameBounds);
                    if (name.BoundingBox.IntersectsRect(BoundingBox) && Position.X + BoundingBox.Size.Width / 2 > name.Position.X - name.BoundingBox.Size.Width / 2
                        && Position.X - BoundingBox.Size.Width / 2 < name.Position.X + name.BoundingBox.Size.Width / 2)
                    {
                        IsOnGround = true;
                        jumpCnt = 0;
                        if (isAlive)
                        {
                            //WaitStep--;
                            //if (WaitStep < 0)
                            //{
                            //    if (level.Anindiegame.Appsettings.IsSoundEnabled)
                            //    {
                            //        if (IsMaximized)
                            //        {
                            //            WaitStep = 50;
                            //            int ran = level.Rand.Next(2) + 1;
                            //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_giant-00" + ran);
                            //        }
                            //        else
                            //        {
                            //            WaitStep = 100;
                            //            int ran = level.Rand.Next(12) + 1;
                            //            CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/footstep_grass_boots_jog-0" + (ran < 10 ? "0" + ran : ran.ToString()));
                            //        }
                            //    }
                            //}
                        }

                        Position = new CCPoint(Position.X, Position.Y + depth.Y);
                        bounds = GetSpriteBounds(this);
                    }
                });

            }
        }

        private static Rectangle GetSpriteBounds(CCNode sprite)
        {
            return new Rectangle(
                (int)(sprite.Position.X - sprite.BoundingBox.Size.Width / 2f),
                (int)(sprite.Position.Y - sprite.BoundingBox.Size.Height / 2f),
                (int)sprite.BoundingBox.Size.Width,
                (int)sprite.BoundingBox.Size.Height);
        }

        private float Jump(float velocityY, float gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f || (canJump && !IsOnGround && hasDoubleJump && jumpCnt == 0 && jumpTime == 0))
                {
                    if (!IsOnGround && hasDoubleJump && jumpCnt == 0 && jumpTime == 0)
                    {
                        jumpTime = 0.0f;
                        jumpCnt++;
                    }


                    if (jumpTime == 0.0f)
                    {
                        //if (level.Anindiegame.Appsettings.IsSoundEnabled)
                        //{
                        //    int ran = level.Rand.Next(2) + 1;
                        //    CCSimpleAudioEngine.SharedEngine.PlayEffect("sound/jump" + ran);
                        //}

                        //Level.JumpCnt++;
                    }

                    canJump = false;

                    jumpTime += gameTime;//.ElapsedGameTime.TotalSeconds;
                    isRunning = false;
                    //StopAllActions();
                    //RunAction(PlayerJumpAction);
                }

                if (jumpTime > MaxJumpTime && !IsBouncing)
                {
                    //back to running
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                    IsBouncing = false;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
                canJump = true;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        private float Translate(float velocityX)
        {
            if (isMovingLeft)
            {
                velocityX = -1000.0f;
            }
            else if (isMovingRight)
            {
                velocityX = 1000.0f;
            }
            else
            {
                velocityX = 0f;
            }

            return velocityX;
        }
    }
}
