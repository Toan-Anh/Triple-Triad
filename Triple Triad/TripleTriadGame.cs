﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    static class TripleTriadGame
    {
        private static TripleTriadCardLib.Element[] _Elements;

        private static TripleTriadCard[] _Player1Cards;
        private static TripleTriadCard[] _Player2Cards;
        private static TripleTriadCard[] _PlayedCards;

        private static int _PlayerTurn = 1;
        private static int _P1Score = 5;

        internal static TripleTriadCardLib.Element[] Elements
        {
            get { return _Elements; }
            set { _Elements = value; }
        }

        internal static TripleTriadCard[] Player1Cards
        {
            get { return _Player1Cards; }
            set { _Player1Cards = value; }
        }

        internal static TripleTriadCard[] Player2Cards
        {
            get { return _Player2Cards; }
            set { _Player2Cards = value; }
        }

        internal static TripleTriadCard[] PlayedCards
        {
            get { return _PlayedCards; }
            set { _PlayedCards = value; }
        }

        public static int PlayerTurn
        {
            get { return _PlayerTurn; } 
            set{ _PlayerTurn = value; }
        }

        public static int P1Score
        {
            get { return _P1Score; }
            set { _P1Score = value; }
        }

        public static int NumberOfCardPlayed
        {
            get
            {
                int played;
                for (played = 0; played < 9 && _PlayedCards != null && PlayedCards[played] != null; ++played) ;
                return played;
            }
        }

        private static Random rand;

        public static void Initialize()
        {
            // Set random seed instance
            rand = new Random((int)DateTime.Now.Ticks);


            // Create random elements if needed
            _Elements = new TripleTriadCardLib.Element[9];
            for (int i = 0; i < 9; ++i)
            {
                if (GameRule.Elemental && rand.Next(10) > 6)
                    _Elements[i] = (TripleTriadCardLib.Element)(rand.Next(8) + 1);
                else
                    _Elements[i] = TripleTriadCardLib.Element.None;
            }

            // Set player turn
            _PlayerTurn = rand.Next(2) + 1;

            // Initialize card board
            _PlayedCards = new TripleTriadCard[9];
            //GenerateRandomHands();
        }

        public static void GenerateTestHands1()
        {
            _PlayerTurn = 2;
            _Player1Cards = new TripleTriadCard[5];
            _Player2Cards = new TripleTriadCard[5];

            int[] p1Cards = { 87, 3, 109, 0, 0};
            int[] p2Cards = { 82, 40, 34, 0, 0};

            for (int i = 0; i < 5; ++i)
            {
                TripleTriadCard card1 = TripleTriadCardLib.CardInfo[p1Cards [i]].GetInstance();
                TripleTriadCard card2 = TripleTriadCardLib.CardInfo[p2Cards[i]].GetInstance();
                card1.Position = TripleTriadCardLib.Player1HandPos[i];
                card2.Position = TripleTriadCardLib.Player2HandPos[i];
                card1.Depth = i * 0.1f + 0.1f;
                card2.Depth = i * 0.1f + 0.1f;
                card1.PlayerNumber = 1;
                card2.PlayerNumber = 2;
                _Player1Cards[i] = card1;
                _Player2Cards[i] = card2;
                _Player1Cards[i].IsOpen = (_PlayerTurn == 1 || GameRule.Open);
                _Player2Cards[i].IsOpen = (_PlayerTurn == 2 || GameRule.Open);
            }
        }

        public static void GenerateTestHands2()
        {
            _PlayerTurn = 1;
            _Player1Cards = new TripleTriadCard[5];
            _Player2Cards = new TripleTriadCard[5];

            int[] p1Cards = { 101, 86, 69, 34, 0 };
            int[] p2Cards = { 97, 7, 103, 0, 0 };

            for (int i = 0; i < 5; ++i)
            {
                TripleTriadCard card1 = TripleTriadCardLib.CardInfo[p1Cards[i]].GetInstance();
                TripleTriadCard card2 = TripleTriadCardLib.CardInfo[p2Cards[i]].GetInstance();
                card1.Position = TripleTriadCardLib.Player1HandPos[i];
                card2.Position = TripleTriadCardLib.Player2HandPos[i];
                card1.Depth = i * 0.1f + 0.1f;
                card2.Depth = i * 0.1f + 0.1f;
                card1.PlayerNumber = 1;
                card2.PlayerNumber = 2;
                _Player1Cards[i] = card1;
                _Player2Cards[i] = card2;
                _Player1Cards[i].IsOpen = (_PlayerTurn == 1 || GameRule.Open);
                _Player2Cards[i].IsOpen = (_PlayerTurn == 2 || GameRule.Open);
            }
        }

        public static void GenerateRandomHands()
        {
            _Player1Cards = new TripleTriadCard[5];
            _Player2Cards = new TripleTriadCard[5];

            for (int i = 0; i < 5; ++i)
            {
                TripleTriadCard card1 = TripleTriadCardLib.CardInfo[rand.Next(110)].GetInstance();
                TripleTriadCard card2 = TripleTriadCardLib.CardInfo[rand.Next(110)].GetInstance();
                card1.Position = TripleTriadCardLib.Player1HandPos[i];
                card2.Position = TripleTriadCardLib.Player2HandPos[i];
                card1.Depth = i * 0.1f + 0.1f;
                card2.Depth = i * 0.1f + 0.1f;
                card1.PlayerNumber = 1;
                card2.PlayerNumber = 2;
                _Player1Cards[i] = card1;
                _Player2Cards[i] = card2;
                _Player1Cards[i].IsOpen = (_PlayerTurn == 1 || GameRule.Open);
                _Player2Cards[i].IsOpen = (_PlayerTurn == 2 || GameRule.Open);
            }
        }

        public static void SuddenDeathHandRedistribution()
        {
            List<TripleTriadCard> newP1Cards = new List<TripleTriadCard>();
            List<TripleTriadCard> newP2Cards = new List<TripleTriadCard>();

            for (int i = 0; i < 5; ++i)
            {
                TripleTriadCard card = _Player1Cards[i];
                if (card.PlayerNumber == 1)
                {
                    if (newP1Cards.Count > 5)
                        newP1Cards.Add(card);
                    else
                        newP2Cards.Add(card);
                }
                else if (card.PlayerNumber == 2)
                {
                    if (newP2Cards.Count > 5)
                        newP2Cards.Add(card);
                    else
                        newP1Cards.Add(card);
                }
                card = _Player2Cards[i];
                if (card.PlayerNumber == 1)
                {
                    if (newP1Cards.Count > 5)
                        newP1Cards.Add(card);
                    else
                        newP2Cards.Add(card);
                }
                else if (card.PlayerNumber == 2)
                {
                    if (newP2Cards.Count > 5)
                        newP2Cards.Add(card);
                    else
                        newP1Cards.Add(card);
                }
            }

            _Player1Cards = newP1Cards.ToArray();
            _Player2Cards = newP2Cards.ToArray();
        }
    }
}
