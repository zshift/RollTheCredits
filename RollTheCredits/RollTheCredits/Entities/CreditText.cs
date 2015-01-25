using System;
using System.Collections.Generic;
using System.Linq;
using Cocos2D;
using RollTheCredits.Common;
using RollTheCredits.NameGenerator;
using RollTheCredits.Scenes;

namespace RollTheCredits.Entities
{
    public class CreditText
    {
        public CreditTypes CreditType;
        public List<NameText> CreditNames;
        private CCPoint Position;
        
        public bool IsAlive;

        private Level Level;

        private bool attacking;

        private Random randomizer;

        public CreditText(Level level, CCPoint position, CreditTypes creditType)
        {
            Level = level;
            randomizer = new Random();
            CreditType = creditType;
            Position = position;
            CreditNames = new List<NameText>();

            switch (creditType)
            {
                    case CreditTypes.RollingFirstLast:
                    case CreditTypes.RollingFirstMiddleLast:
                    GenerateRollingCreditName();
                    break;
                    case CreditTypes.AttackingFirstLast:
                    case CreditTypes.AttackingFirstMiddleLast:
                    GenerateAttackingCreditName();
                    break;
            }
            CreditNames.ForEach(platform => Level.StartGameScene.AddChild(platform, 4));
        }

        private void GenerateRollingCreditName()
        {
            CreditNames.Add(new NameText(Level, Names.Next(), RollingPosition));
        }

        private void GenerateAttackingCreditName()
        {
            CreditNames.Add(new NameText(Level, Names.Next(), AttackingPosition)
            {
                Visible = false
            });
        }

        private CCPoint RollingPosition
        {
            get { return new CCPoint((AppDelegate.GameWidth/2) + randomizer.Next(-350, 350), Position.Y); }
        }

        private CCPoint AttackingPosition
        {
            get { return new CCPoint(AppDelegate.GameWidth + AppDelegate.GameWidth/2, AppDelegate.GameHeight/2); }
        }

        private void RegenerateRollingCreditName()
        {
            CreditNames.First().Reset(Names.Next(), RollingPosition);
        }

        private void RegenerateAttackingCreditName()
        {
            CreditNames.First().Reset(Names.Next(), AttackingPosition);
        }

        public void Reset(CCPoint position)
        {
            Position = position;
            switch (CreditType)
            {
                case CreditTypes.RollingFirstLast:
                case CreditTypes.RollingFirstMiddleLast:
                    RegenerateRollingCreditName();
                    break;
                case CreditTypes.AttackingFirstLast:
                case CreditTypes.AttackingFirstMiddleLast:
                case CreditTypes.AttackingUpAndDownFirstLast:
                    RegenerateAttackingCreditName();
                    break;
            }
            IsAlive = true;
        }

        public void Update()
        {
            switch (CreditType)
            {
                case CreditTypes.RollingFirstLast:
                case CreditTypes.RollingFirstMiddleLast:
                    UpdateRollingCredits();
                    break;
                case CreditTypes.AttackingFirstLast:
                case CreditTypes.AttackingFirstMiddleLast:
                case CreditTypes.AttackingUpAndDownFirstLast:
                    UpdateAttackingCredits();
                    break;
            }
        }

        private void UpdateAttackingCredits()
        {
            foreach (var creditName in CreditNames)
            {
                creditName.UpdateAttackingName();
            }

            if (CreditNames.All(credit => credit.Position.X < -200))
            {
                IsAlive = false;
                CreditNames.All(credit=>credit.IsAlive = false);
            }
        }

        private void UpdateRollingCredits()
        {
            CreditNames.ForEach(x => x.UpdateRollingName());

            foreach (var creditName in CreditNames)
            {
                creditName.UpdateRollingName();
            }

            if (CreditNames.Any(credit => credit.Position.Y > AppDelegate.GameHeight + 200))
            {
                if (!Level.StartGameScene.theBallHasLeftTheBuilding)
                {
                    RegenerateRollingCreditName();
                }
                else
                {
                    RemoveName();
                }
            }
        }

        private void RemoveName()
        {
            CreditNames.ForEach(name => Level.StartGameScene.RemoveChild(name));
            CreditNames.Clear();
        }
    }
}
