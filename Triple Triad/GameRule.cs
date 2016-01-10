using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    static class GameRule
    {
        public enum TradingRules
        {
            One, Direct, Diff, All, 
        }

        private static bool _Open = false;
        private static bool _Elemental = false;
        private static bool _Same = false;
        private static bool _Plus = false;
        private static bool _SameWall = false;
        private static bool _PlusWall = false;
        private static bool _Random = false;
        private static bool _SuddenDeath = false;
        private static TradingRules _TradingRule = TradingRules.One;

        public static bool Open
        {
            get { return GameRule._Open; }
            set { GameRule._Open = value; }
        }
        public static bool Elemental
        {
            get { return GameRule._Elemental; }
            set { GameRule._Elemental = value; }
        }
        public static bool Same
        {
            get { return GameRule._Same; }
            set { GameRule._Same = value; }
        }
        public static bool Plus
        {
            get { return GameRule._Plus; }
            set { GameRule._Plus = value; }
        }
        public static bool SameWall
        {
            get { return GameRule._SameWall; }
            set { GameRule._SameWall = value; }
        }
        public static bool PlusWall
        {
            get { return GameRule._PlusWall; }
            set { GameRule._PlusWall = value; }
        }
        public static bool Random
        {
            get { return GameRule._Random; }
            set { GameRule._Random = value; }
        }
        public static bool SuddenDeath
        {
            get { return GameRule._SuddenDeath; }
            set { GameRule._SuddenDeath = value; }
        }
        internal static TradingRules TradingRule
        {
            get { return GameRule._TradingRule; }
            set { GameRule._TradingRule = value; }
        }
    }
}
