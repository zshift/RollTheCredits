using Cocos2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RollTheCredits.Entities;

namespace RollTheCredits.Scenes
{
    public class StartGameScene : CCLayerColor
    {

        public Level Level;

        private readonly CCSprite youWinText;
        private readonly CCSprite instructions;
        private readonly CCSprite title;

        public bool StartCredits = false;
        public bool IsGameOver;
        public bool IsUpdating;

        public bool HasGameStarted;

        public bool startCredits = false;
        public bool theBallHasLeftTheBuilding;

        public StartGameScene()
        {
            title = new CCSprite("sprites/rollthecredits.png")
            {
                Position = new CCPoint(AppDelegate.GameWidth / 2, AppDelegate.GameHeight - 60)
            };
            youWinText = new CCSprite("sprites/youwin.png")
            {
                Position = new CCPoint(AppDelegate.GameWidth / 2, AppDelegate.GameHeight - 190)
            };
            instructions = new CCSprite("sprites/presstostart.png")
            {
                Position = new CCPoint(AppDelegate.GameWidth / 2, AppDelegate.GameHeight - 390)
            };
                
            AddChild(this.youWinText, 4);
            AddChild(this.instructions, 4);
            AddChild(this.title, 4);

            this.youWinText.Visible = false;
            Level = new Level(this);

            Schedule(StartGameUpdate, 0.0333333f);
        }

        public void YouWin()
        {
            this.youWinText.Visible = true;
            this.instructions.Visible = false;
        }

        #region Init Scene

        /// <summary>
        /// there's no 'id' in cpp, so we recommand to return the exactly class pointer
        /// </summary>
        public static CCScene Scene()
        {
            // 'scene' is an autorelease object
            CCScene scene = new CCScene();

            // 'layer' is an autorelease object
            var layer = new StartGameScene();
            layer.Color = CCColor3B.Black;
            // add layer as a child to scene
            scene.AddChild(layer);

            // return the scene
            return scene;
        }

        #endregion

        public override void OnEnter()
        {
            base.OnEnter();
            //CCSimpleAudioEngine.SharedEngine.PlayBackgroundMusic("bgm/bgm2", true);
        }

        void StartGameUpdate(float gameTime)
        {
            var _keyboardState = Keyboard.GetState();
            var _gamePadState = GamePad.GetState(PlayerIndex.One);
            Level.UpdateLevel(gameTime, _keyboardState, _gamePadState);

            if (!StartCredits)
            {
                return;
            }
            
            UpdateTitleText();
        }

        private void UpdateTitleText()
        {
            title.Position += new CCPoint(0, 5);
            youWinText.Position += new CCPoint(0, 5);
            instructions.Position += new CCPoint(0, 5);

            if (title.PositionY < -200)
            {
                title.Visible = false;
            }

            if (youWinText.PositionY < -200)
            {
                youWinText.Visible = false;
            }

            if (instructions.PositionY < -200)
            {
                instructions.Visible = false;
            }
        }
    }
}
