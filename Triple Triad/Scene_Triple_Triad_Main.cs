using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using System.Threading;
using Triple_Triad.SpriteAnimation;

namespace Triple_Triad
{
    class Scene_Triple_Triad_Main : Scene
    {
        LogWriter logWriter = LogWriter.Instance;

        private Sprite spriteBoard;
        private Sprite selectedGrid;

        //private int simulateInterval = 1250;
        //private int simulateCount = 0;

        private VisibleEntity _PrevMouseTarget;
        private VisibleEntity _CurrMouseTarget;

        private Window _HelpWindow;

        private TripleTriadCard selectedCard;
        //private int selectedCardIndex;
        //private int selectedGridIndex;

        private Vector2[] _ScorePos;

        private Random rand;


        private List<Sprite> _ScreenText;

        private int winningMessageInterval = 4000;
        private int winningMessageCount = 0;

        public Scene_Triple_Triad_Main()
        {
            logWriter.WriteToLog(new string('=', 80));

            // Set random seed
            rand = new Random((int)DateTime.Now.Ticks);

            // Create the game board
            spriteBoard = new Sprite();
            spriteBoard.LoadContent("Triple Triad Board");
            spriteBoard.Depth = 0;
            //_SceneEntities.Add(spriteBoard);

            logWriter.WriteToLog("--- PLAYER " + TripleTriadGame.PlayerTurn + " ---");

            // Add card to SceneEntities so they can be updated and drawn
            AddCardsAsEntities();

            // Initialize help window
            InitHelpWindow();

            // Initialize selected grid highlight
            InitGridHighlight();

            // Initialize score position
            InitScorePosition();

            // Initialize text sprites
            InitScreenText();

            // Initialize hand animation
            InitInitialHandAnimations();

            // Initialize indicator
            InitTurnIndicator();
        }

        private void InitInitialHandAnimations()
        {
            RegisterAnimation("10", new SpriteAnimation_Fly(TripleTriadGame.Player1Cards[0], 0, 80));
            RegisterAnimation("11", new SpriteAnimation_Fly(TripleTriadGame.Player1Cards[1], 0, 80), "10", FireTime.AtEnd);
            RegisterAnimation("12", new SpriteAnimation_Fly(TripleTriadGame.Player1Cards[2], 0, 80), "11", FireTime.AtEnd);
            RegisterAnimation("13", new SpriteAnimation_Fly(TripleTriadGame.Player1Cards[3], 0, 80), "12", FireTime.AtEnd);
            RegisterAnimation("14", new SpriteAnimation_Fly(TripleTriadGame.Player1Cards[4], 0, 80), "13", FireTime.AtEnd);

            RegisterAnimation("20", new SpriteAnimation_Fly(TripleTriadGame.Player2Cards[0], 0, 80), "14", FireTime.AtEnd);
            RegisterAnimation("21", new SpriteAnimation_Fly(TripleTriadGame.Player2Cards[1], 0, 80), "20", FireTime.AtEnd);
            RegisterAnimation("22", new SpriteAnimation_Fly(TripleTriadGame.Player2Cards[2], 0, 80), "21", FireTime.AtEnd);
            RegisterAnimation("23", new SpriteAnimation_Fly(TripleTriadGame.Player2Cards[3], 0, 80), "22", FireTime.AtEnd);
            RegisterAnimation("24", new SpriteAnimation_Fly(TripleTriadGame.Player2Cards[4], 0, 80), "23", FireTime.AtEnd);

            StartAnimation("10");
        }

        private void InitTurnIndicator()
        {
            Sprite_Animate_Simple indicator = new Sprite_Animate_Simple();
            indicator.LoadContent("Indicator_Triangle", 4, 1);
            indicator.Depth = 1;
            indicator.Position = new Vector2((spriteBoard.ViewportWidth - indicator.ViewportWidth) / 2,
                                            (spriteBoard.ViewportHeight - indicator.ViewportHeight) / 2);
            Vector2 p1 = new Vector2(324, 0);
            Vector2 p2 = new Vector2(36, 0);
            RegisterAnimation("P12", new SpriteAnimation_Move(indicator, p1, p2, 1, 100));
            RegisterAnimation("P21", new SpriteAnimation_Move(indicator, p2, p1, 1, 100));
            AddEntity(indicator);

            RegisterAnimation("indicatorFadeIn", new SpriteAnimation_Fade(indicator, 0, 100), "24", FireTime.AtEnd);
            RegisterAnimation("indicatorMove", new SpriteAnimation_Move(indicator, indicator.Position, TripleTriadGame.PlayerTurn == 1 ? p1 : p2, 1, 100), "indicatorFadeIn", FireTime.AtEnd);
            RegisterAnimation("indicatorScale", new SpriteAnimation_Scale(indicator, 1, 100, 1f, 24f / 64), "indicatorMove", FireTime.AtStart);
        }

        private void InitScreenText()
        {
            string[] names = { "same", "plus", "samewall", "pluswall", "combo", "win", "lose", "draw" };

            _ScreenText = new List<Sprite>();

            for (int i = 0; i < names.Length; ++i)
            {
                Sprite tmp = new Sprite();
                tmp.LoadContent(names[i]);
                tmp.Position = new Vector2((Global.ScreenWidth - tmp.ViewportWidth) / 2, (Global.ScreenHeight - tmp.ViewportHeight) / 2);
                tmp.Depth = 1;
                _ScreenText.Add(tmp);
                RegisterAnimation(names[i] + "FlyIn", new SpriteAnimation_Fly(tmp, 0, 100, SpriteAnimation_Fly.FlyDirection.FromRight));
                if (i < 5)
                    RegisterAnimation(names[i] + "FlyOut", new SpriteAnimation_Fly(tmp, 500, 100, SpriteAnimation_Fly.FlyDirection.ToLeft), names[i] + "FlyIn", FireTime.AtEnd);
            }
        }

        private void InitScorePosition()
        {
            _ScorePos = new Vector2[2];
            _ScorePos[0] = new Vector2(304 + (64 - 40) / 2, 186);
            _ScorePos[1] = new Vector2(16 + (64 - 40) / 2, 186);
        }

        private void InitGridHighlight()
        {
            Color[] c = new Color[64 * 64];
            for (int i = 0; i < 64 * 64; ++i)
                c[i] = new Color(128, 128, 128, 128);
            selectedGrid = new Sprite();
            selectedGrid.Texture = new Texture2D(Global.Graphics.GraphicsDevice, 64, 64);
            selectedGrid.Texture.SetData(c);
            selectedGrid.Rectangle = new Rectangle(0, 0, 64, 64);
            selectedGrid.Depth = 0.99f;
        }

        private void InitHelpWindow()
        {
            _HelpWindow = new Window();
            _HelpWindow.Height = _HelpWindow.FittingHeight(1);
            _HelpWindow.CenterText = true;
        }

        private void AddCardsAsEntities()
        {
            for (int i = 0; i < 5; ++i)
            {
                _SceneEntities.Add(TripleTriadGame.Player1Cards[i]);
                _SceneEntities.Add(TripleTriadGame.Player2Cards[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_Animations["indicatorScale"].AnimationState == AnimationState.End)
            {
                if (TripleTriadGame.NumberOfCardPlayed == 9)
                {
                    if (TripleTriadGame.P1Score > 5)
                    {
                        //_HelpWindow.Text = "Win";
                        if (winningMessageCount == 0)
                        {
                            Song BGM = Global.Content.Load<Song>("Audio/05");
                            MediaPlayer.Stop();
                            MediaPlayer.Play(BGM);
                            //StartAnimation("winFade");
                            StartAnimation("winFlyIn");
                        }
                    }
                    else if (TripleTriadGame.P1Score < 5)
                    {
                        //_HelpWindow.Text = "Lose";
                        if (winningMessageCount == 0)
                        {
                            StartAnimation("loseFlyIn");
                        }
                    }
                    else
                    {
                        //_HelpWindow.Text = "Draw";
                        if (winningMessageCount == 0)
                        {
                            StartAnimation("drawFlyIn");
                        }
                    }
                    //_HelpWindow.Open();
                    winningMessageCount += gameTime.ElapsedGameTime.Milliseconds;
                    if (winningMessageCount >= winningMessageInterval)
                        Global.SceneManager.CurrentScene = new Scene_Result();
                    return;
                }

                if (TripleTriadGame.PlayerTurn == 1)
                {
                    UpdatePlayersCardPositions(gameTime, TripleTriadGame.Player1Cards, 1);
                    PlayNextCard(1);
                }
                else if (TripleTriadGame.PlayerTurn == 2)
                {
                    //SimulateTraversingCards(gameTime, TripleTriadGame.Player2Cards, 2);
                    UpdatePlayersCardPositions(gameTime, TripleTriadGame.Player2Cards, 2);
                    PlayNextCard(2);
                }

                UpdateHelpWindow(gameTime);
            }
        }

        private void PlayNextCard(int player)
        {
            if (selectedCard != null)
            {
                int index = TripleTriadCardLib.SelectedGridPosition();
                if (index >= 0 && TripleTriadGame.PlayedCards[index] == null)
                {
                    selectedGrid.Position = TripleTriadCardLib.GameBoardPos[index];
                    if (!_SceneEntities.Contains(selectedGrid))
                        _SceneEntities.Add(selectedGrid);
                    if (Global.MouseManager.IsLeftMouseButtonClicked())
                    {
                        PlaceSelectedCard(index, player);
                        _SceneEntities.Remove(selectedGrid);
                        ChangePlayersTurn();
                    }
                }
                else
                    _SceneEntities.Remove(selectedGrid);

                if (Global.MouseManager.IsRightMouseButtonClicked())
                {
                    selectedCard = null;
                    _SceneEntities.Remove(selectedGrid);
                    Global.SFXManager.Cancel.Play();
                }
            }
        }

        private void ChangePlayersTurn()
        {
            StartAnimation("P" + TripleTriadGame.PlayerTurn + (3 - TripleTriadGame.PlayerTurn));
            TripleTriadGame.PlayerTurn = 3 - TripleTriadGame.PlayerTurn;
            logWriter.WriteToLog("--- PLAYER " + TripleTriadGame.PlayerTurn + " ---");
            if (!GameRule.Open)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (!TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player1Cards[i]))
                        TripleTriadGame.Player1Cards[i].IsOpen = (TripleTriadGame.PlayerTurn == 1 || GameRule.Open);
                    if (!TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player2Cards[i]))
                        TripleTriadGame.Player2Cards[i].IsOpen = (TripleTriadGame.PlayerTurn == 2 || GameRule.Open);
                }
            }
        }

        private void UpdateHelpWindow(GameTime gameTime)
        {
            _HelpWindow.Width = (int)(spriteBoard.ViewportWidth * Global.BoardScaleH - Global.BoardScaleH * 96 * 2);
            _HelpWindow.ScreenPosition = new Vector2(Global.BoardScaleH * 96, (int)(Global.ScreenHeight - _HelpWindow.Height - Global.BoardScaleV * 8));
            _HelpWindow.Update(gameTime);

            if (_CurrMouseTarget != null && _PrevMouseTarget != _CurrMouseTarget)
            {
                Global.SFXManager.Card.Play();
                _HelpWindow.Open();
                _HelpWindow.Text = ((TripleTriadCard)_CurrMouseTarget).Name;
            }
            else if (_CurrMouseTarget == null)
                _HelpWindow.Close();
        }


        private void UpdatePlayersCardPositions(GameTime gameTime, TripleTriadCard[] cards, int player)
        {
            TripleTriadCard tmp = null;
            int index = -1;
            ResetHandPositions(player);
            for (int i = 0; i < cards.Length; ++i)
            {
                if (selectedCard == null && !TripleTriadGame.PlayedCards.Contains(cards[i]) && (cards[i].IsMouseOver() || cards[i].IsMouseDown()))
                {
                    if (tmp == null || cards[i].Depth > tmp.Depth)
                    {
                        tmp = cards[i];
                        index = i;
                    }
                }
            }

            if (tmp != null && index != -1)
            {
                if (tmp.IsSelected())
                {
                    selectedCard = tmp;
                    Global.SFXManager.Card.Play();
                }

                if (player == 1)
                    tmp.Position = TripleTriadCardLib.Player1HandPos[index] + new Vector2(-16, 0);
                else if (player == 2)
                    tmp.Position = TripleTriadCardLib.Player2HandPos[index] + new Vector2(16, 0);
            }

            if (tmp != null)
            {
                _PrevMouseTarget = _CurrMouseTarget;
                _CurrMouseTarget = tmp;
            }
            else
                UpdateCurrentMouseTargetOnBoard(gameTime, player);
        }

        private void UpdateCurrentMouseTargetOnBoard(GameTime gameTime, int player)
        {
            TripleTriadCard tmp = null;
            for (int i = 0; i < TripleTriadGame.PlayedCards.Length; ++i)
            {
                if (TripleTriadGame.PlayedCards[i] != null && (TripleTriadGame.PlayedCards[i].IsMouseOver() || TripleTriadGame.PlayedCards[i].IsMouseDown()))
                {
                    tmp = TripleTriadGame.PlayedCards[i];
                    break;
                }
            }
            _PrevMouseTarget = _CurrMouseTarget;
            _CurrMouseTarget = tmp;
        }

        private void SimulateTraversingCards(GameTime gameTime, TripleTriadCard[] cards, int player)
        {
            //if (selectedCard == null)
            //{
            //    Thread cal = new Thread(new ThreadStart(CalculateMove));
            //    cal.Start();
            //}

            //simulateCount += gameTime.ElapsedGameTime.Milliseconds;
            //if (simulateCount >= simulateInterval && selectedCard == null)
            //{
            //    simulateCount = 0;
            //    ResetHandPositions(player);
            //    Random rand = new Random();
            //    int index = rand.Next(cards.Length);
            //    while (TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player2Cards[index]))
            //        index = rand.Next(cards.Length);
            //    if (player == 1)
            //        cards[index].Position = TripleTriadCardLib.Player1HandPos[index] + new Vector2(-16, 0);
            //    else if (player == 2)
            //        cards[index].Position = TripleTriadCardLib.Player2HandPos[index] + new Vector2(16, 0);
            //    Global.SFXManager.Card.Play();
            //}

            //if (selectedCard != null)
            //{
            //    ResetHandPositions(player);
            //    if (player == 1)
            //        selectedCard.Position = TripleTriadCardLib.Player1HandPos[selectedCardIndex] + new Vector2(-16, 0);
            //    else if (player == 2)
            //        selectedCard.Position = TripleTriadCardLib.Player2HandPos[selectedCardIndex] + new Vector2(16, 0);

            //    if (simulateCount >= simulateInterval)
            //    {
            //        PlaceSelectedCard(selectedGridIndex, player);
            //        TripleTriadGame.PlayerTurn = 3 - TripleTriadGame.PlayerTurn;
            //    }
            //}
        }

        /// <summary>
        /// Place the selected card on the board
        /// </summary>
        /// <param name="boardIndex">The index on the board</param>
        public void PlaceSelectedCard(int boardIndex, int player)
        {
            TripleTriadGame.PlayedCards[boardIndex] = selectedCard;
            if (TripleTriadGame.Elements[boardIndex] != TripleTriadCardLib.Element.None)
            {
                if (TripleTriadGame.Elements[boardIndex] == selectedCard.Element)
                {
                    Sprite plus = new Sprite();
                    plus.LoadContent("Plus_1");
                    plus.Position = TripleTriadCardLib.GameBoardPos[boardIndex];
                    plus.Depth = 0.99f;
                    _SceneEntities.Add(plus);
                }
                else
                {
                    Sprite minus = new Sprite();
                    minus.LoadContent("Minus_1");
                    minus.Position = TripleTriadCardLib.GameBoardPos[boardIndex];
                    minus.Depth = 0.99f;
                    _SceneEntities.Add(minus);
                }
            }

            logWriter.WriteToLog("PLAYER " + player + " places " + selectedCard.Name + " at cell number " + boardIndex);

            TripleTriadGame.PlayedCards[boardIndex].Position = TripleTriadCardLib.GameBoardPos[boardIndex];
            TripleTriadGame.PlayedCards[boardIndex].IsOpen = true;
            selectedCard = null;

            if (GameRule.Same)
            {
                PerformSameRuleCheck(boardIndex, player);
            }
            if (GameRule.Plus)
            {
                PerformPlusRuleCheck(boardIndex, player);
            }

            PerformNormalCheck(boardIndex, player);
        }

        private void PerformSameRuleCheck(int boardIndex, int player)
        {
            int up = boardIndex - 3;
            int down = boardIndex + 3;
            int left = boardIndex - 1;
            int right = boardIndex + 1;
            int count = 0;

            List<int> adjacentIndices = new List<int>();
            adjacentIndices.Add(down < 9 ? down : -1); // down
            adjacentIndices.Add(left >= 0 && left % 3 != 2 ? left : -1); // left
            adjacentIndices.Add(right < 9 && right % 3 != 0 ? right : -1); // right
            adjacentIndices.Add(up >= 0 ? up : -1); // up

            TripleTriadCard currentCard = TripleTriadGame.PlayedCards[boardIndex];

            List<int> comboCheckIndices = new List<int>();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = i + 1; j < 4; ++j)
                {
                    TripleTriadCard adjacentCard1 = adjacentIndices[i] >= 0 ? TripleTriadGame.PlayedCards[adjacentIndices[i]] : null;
                    TripleTriadCard adjacentCard2 = adjacentIndices[j] >= 0 ? TripleTriadGame.PlayedCards[adjacentIndices[j]] : null;
                    if (adjacentCard1 != null && adjacentCard2 != null &&
                        (currentCard.PlayerNumber != adjacentCard1.PlayerNumber || currentCard.PlayerNumber != adjacentCard2.PlayerNumber) &&
                        currentCard.CardValue[i] == adjacentCard1.CardValue[3 - i] && currentCard.CardValue[j] == adjacentCard2.CardValue[3 - j])
                    {
                        logWriter.WriteToLog("[SAME] " + currentCard.Name + " adjacent to " + adjacentCard1.Name + " and " + adjacentCard2.Name);
                        StartAnimation("sameFlyIn");
                        Global.SFXManager.FlyIn.Play();

                        if (adjacentCard1.PlayerNumber != player)
                        {
                            logWriter.WriteToLog("[SAME] " + adjacentCard1.Name + " taken by PLAYER " + player);
                            adjacentCard1.PlayerNumber = player;
                            comboCheckIndices.Add(adjacentIndices[i]);
                            ++count;
                        }
                        if (adjacentCard2.PlayerNumber != player)
                        {
                            logWriter.WriteToLog("[SAME] " + adjacentCard2.Name + " taken by PLAYER " + player);
                            adjacentCard2.PlayerNumber = player;
                            comboCheckIndices.Add(adjacentIndices[j]);
                            ++count;
                        }
                    }
                }
            }

            if (player == 1)
                TripleTriadGame.P1Score += count;
            else
                TripleTriadGame.P1Score -= count;


            // Perform Combo check
            while (comboCheckIndices.Count > 0)
            {
                List<int> tmp = new List<int>();
                for (int i = 0; i < comboCheckIndices.Count; ++i)
                {
                    tmp.AddRange(PerformNormalCheck(comboCheckIndices[i], player));
                }
                comboCheckIndices = tmp;
            }
        }

        private void PerformPlusRuleCheck(int boardIndex, int player)
        {
            int up = boardIndex - 3;
            int down = boardIndex + 3;
            int left = boardIndex - 1;
            int right = boardIndex + 1;
            int count = 0;

            List<int> adjacentIndices = new List<int>();
            adjacentIndices.Add(down < 9 ? down : -1); // down
            adjacentIndices.Add(left >= 0 && left % 3 != 2 ? left : -1); // left
            adjacentIndices.Add(right < 9 && right % 3 != 0 ? right : -1); // right
            adjacentIndices.Add(up >= 0 ? up : -1); // up

            TripleTriadCard currentCard = TripleTriadGame.PlayedCards[boardIndex];

            List<int> comboCheckIndices = new List<int>();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = i + 1; j < 4; ++j)
                {
                    TripleTriadCard adjacentCard1 = adjacentIndices[i] >= 0 ? TripleTriadGame.PlayedCards[adjacentIndices[i]] : null;
                    TripleTriadCard adjacentCard2 = adjacentIndices[j] >= 0 ? TripleTriadGame.PlayedCards[adjacentIndices[j]] : null;
                    if (adjacentCard1 != null && adjacentCard2 != null &&
                        (currentCard.PlayerNumber != adjacentCard1.PlayerNumber || currentCard.PlayerNumber != adjacentCard2.PlayerNumber) &&
                        currentCard.CardValue[i] + adjacentCard1.CardValue[3 - i] == currentCard.CardValue[j] + adjacentCard2.CardValue[3 - j])
                    {
                        logWriter.WriteToLog("[PLUS] " + currentCard.Name + " adjacent to " + adjacentCard1.Name + " and " + adjacentCard2.Name);
                        StartAnimation("plusFlyIn");
                        Global.SFXManager.FlyIn.Play();

                        if (adjacentCard1.PlayerNumber != player)
                        {
                            logWriter.WriteToLog("[PLUS] " + adjacentCard1.Name + " taken by PLAYER " + player);
                            adjacentCard1.PlayerNumber = player;
                            comboCheckIndices.Add(adjacentIndices[i]);
                            ++count;
                        }
                        if (adjacentCard2.PlayerNumber != player)
                        {
                            logWriter.WriteToLog("[PLUS] " + adjacentCard2.Name + " taken by PLAYER " + player);
                            adjacentCard2.PlayerNumber = player;
                            comboCheckIndices.Add(adjacentIndices[j]);
                            ++count;
                        }
                    }
                }
            }

            if (player == 1)
                TripleTriadGame.P1Score += count;
            else
                TripleTriadGame.P1Score -= count;


            // Perform Combo check
            while (comboCheckIndices.Count > 0)
            {
                List<int> tmp = new List<int>();
                for (int i = 0; i < comboCheckIndices.Count; ++i)
                {
                    tmp.AddRange(PerformNormalCheck(comboCheckIndices[i], player));
                }
                comboCheckIndices = tmp;
            }
        }

        private List<int> PerformNormalCheck(int boardIndex, int player)
        {
            List<int> possibleComboIndices = new List<int>();
            int up = boardIndex - 3;
            int down = boardIndex + 3;
            int left = boardIndex - 1;
            int right = boardIndex + 1;
            int count = 0;

            List<int> adjacentIndices = new List<int>();
            adjacentIndices.Add(down < 9 ? down : -1); // down
            adjacentIndices.Add(left >= 0 && left % 3 != 2 ? left : -1); // left
            adjacentIndices.Add(right < 9 && right % 3 != 0 ? right : -1); // right
            adjacentIndices.Add(up >= 0 ? up : -1); // up

            TripleTriadCard currentCard = TripleTriadGame.PlayedCards[boardIndex];

            int valueModifier1 = 0;
            if (TripleTriadGame.Elements[boardIndex] != TripleTriadCardLib.Element.None)
            {
                if (TripleTriadGame.Elements[boardIndex] == currentCard.Element)
                    valueModifier1 += 1;
                else
                    valueModifier1 -= 1;
            }

            for (int i = 0; i < 4; ++i)
            {
                int valueModifier2 = 0;
                TripleTriadCard adjacentCard = adjacentIndices[i] >= 0 ? TripleTriadGame.PlayedCards[adjacentIndices[i]] : null;
                if (adjacentIndices[i] >= 0 && TripleTriadGame.Elements[adjacentIndices[i]] != TripleTriadCardLib.Element.None && adjacentCard != null)
                {
                    if (TripleTriadGame.Elements[adjacentIndices[i]] == adjacentCard.Element)
                        valueModifier2 += 1;
                    else
                        valueModifier2 -= 1;
                }

                if (adjacentCard != null && currentCard.PlayerNumber != adjacentCard.PlayerNumber && 
                    currentCard.CardValue[i] + valueModifier1 > adjacentCard.CardValue[3 - i] + valueModifier2)
                {
                    logWriter.WriteToLog(adjacentCard.Name + " taken by PLAYER " + player + " using " + currentCard.Name);
                    adjacentCard.PlayerNumber = player;
                    possibleComboIndices.Add(adjacentIndices[i]);
                    ++count;
                    Global.SFXManager.CardWon.Play();
                }
            }

            if (player == 1)
                TripleTriadGame.P1Score += count;
            else
                TripleTriadGame.P1Score -= count;

            return possibleComboIndices;
        }

        private void CalculateMove()
        {
            //int tmp1 = 0, tmp2 = 0;
            //for (int i = 0; i < 9; ++i)
            //{
            //    if (TripleTriadGame.PlayedCards[i] != null && TripleTriadGame.PlayedCards[i].PlayerNumber == 1)
            //    {
            //        for (int j = 0; j < 5; ++j)
            //        {
            //            if (!TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player2Cards[j]))
            //            {
            //                if (i % 3 != 0 && TripleTriadGame.PlayedCards[i - 1] == null && TripleTriadGame.Player2Cards[j].CardValue[2] > TripleTriadGame.PlayedCards[i].CardValue[1])
            //                {
            //                    selectedCard = TripleTriadGame.Player2Cards[j];
            //                    selectedCardIndex = j;
            //                    selectedGridIndex = i - 1;
            //                    return;
            //                }
            //                else if (i % 3 != 2 && TripleTriadGame.PlayedCards[i + 1] == null && TripleTriadGame.Player2Cards[j].CardValue[1] > TripleTriadGame.PlayedCards[i].CardValue[2])
            //                {
            //                    selectedCard = TripleTriadGame.Player2Cards[j];
            //                    selectedCardIndex = j;
            //                    selectedGridIndex = i + 1;
            //                    return;
            //                }
            //                else if (i / 3 != 0 && TripleTriadGame.PlayedCards[i - 3] == null && TripleTriadGame.Player2Cards[j].CardValue[0] > TripleTriadGame.PlayedCards[i].CardValue[3])
            //                {
            //                    selectedCard = TripleTriadGame.Player2Cards[j];
            //                    selectedCardIndex = j;
            //                    selectedGridIndex = i - 3;
            //                    return;
            //                }
            //                else if (i / 3 != 2 && TripleTriadGame.PlayedCards[i + 3] == null && TripleTriadGame.Player2Cards[j].CardValue[3] > TripleTriadGame.PlayedCards[i].CardValue[0])
            //                {
            //                    selectedCard = TripleTriadGame.Player2Cards[j];
            //                    selectedCardIndex = j;
            //                    selectedGridIndex = i + 3;
            //                    return;
            //                }
            //                else if (TripleTriadGame.PlayedCards[i] == null)
            //                {
            //                    selectedCard = TripleTriadGame.Player2Cards[j];
            //                    selectedCardIndex = j;
            //                    selectedGridIndex = 8;
            //                    tmp2 = j;
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (TripleTriadGame.PlayedCards[i] == null)
            //        tmp1 = i;
            //}

            //selectedCard = TripleTriadGame.Player2Cards[tmp2];
            //selectedCardIndex = tmp2;
            //selectedGridIndex = tmp1;
        }

        private void ResetHandPositions(int player)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (player == 1 && TripleTriadGame.Player1Cards[i] != selectedCard && !TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player1Cards[i]))
                    TripleTriadGame.Player1Cards[i].Position = TripleTriadCardLib.Player1HandPos[i];
                else if (player == 2 && TripleTriadGame.Player2Cards[i] != selectedCard && !TripleTriadGame.PlayedCards.Contains(TripleTriadGame.Player2Cards[i]))
                    TripleTriadGame.Player2Cards[i].Position = TripleTriadCardLib.Player2HandPos[i];
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(Global.BoardScaleH, Global.BoardScaleV, 1f));
            spriteBoard.Draw(gameTime);
            for (int i = 0; i < 9; ++i)
                if (TripleTriadGame.Elements[i] != TripleTriadCardLib.Element.None)
                    Global.SpriteBatch.Draw(TripleTriadCardLib.Elements, new Vector2(118 + 64 * (i % 3), 39.5f + 64 * (i / 3)), TripleTriadCardLib.ElementRect[(int)TripleTriadGame.Elements[i] - 1], new Color(160, 160, 160, 160), 0, Vector2.Zero, 1f, SpriteEffects.None, 0.001f);
            foreach (VisibleEntity entity in _SceneEntities)
                entity.Draw(gameTime);

            Global.SpriteBatch.Draw(TripleTriadCardLib.Scores, _ScorePos[0], TripleTriadCardLib.ScoreRect[TripleTriadGame.P1Score - 1], Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
            Global.SpriteBatch.Draw(TripleTriadCardLib.Scores, _ScorePos[1], TripleTriadCardLib.ScoreRect[10 - TripleTriadGame.P1Score - 1], Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
            Global.SpriteBatch.End();


            Global.SpriteBatch.Begin();
            foreach (Sprite text in _ScreenText)
                text.Draw(gameTime);
            _HelpWindow.Draw(gameTime);
            Global.SpriteBatch.End();

            Global.MouseManager.DrawMouseCursor(gameTime);
        }
    }
}
