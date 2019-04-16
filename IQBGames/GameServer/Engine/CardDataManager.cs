using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class CardDataManager
    {
       // public static  Dictionary<int, ECard> CardMap { get; set; }

        public static List<ECard> InitNewCards()
        {
            var Cards = new List<ECard>();
            for (int i = 1; i <= 52; i++)
            {
                int ct = (i / 13)+1;
                var type = (CardType)ct;
                var v = (i % 13);
                if (v == 0) v = 13;
                if (v == 1) v = 14;
                var card = new ECard
                {
                    No = i,
                    Value = v,
                    CardType = type,
                };
                Cards.Add(card);
            }
            return Cards;
        }

        //public static Dictionary<int,ECard> NoToCard(List<int> noList)
        //{
        //    var r = new Dictionary<int, ECard>();
        //    foreach(var no in noList)
        //    {
        //        var card = CardMap[no];
        //        r.Add(no, card);
        //    }
        //    return r;
        //}

        private Dictionary<int,ECard> _TableCards;
        public Dictionary<int, ECard> TableCards {
            get
            {
                if (_TableCards == null) _TableCards = new Dictionary<int, ECard>();
                return _TableCards;
            }
         }

       // private Dictionary<int, List<ECard>> _PlayCards;
        /// <summary>
        /// Key:座位号，Value 牌
        /// </summary>
        //public Dictionary<int, List<ECard>> PlayCards {
        //    get {
        //        if (_PlayCards == null)
        //        {
        //            _PlayCards = new Dictionary<int, List<ECard>>();
        //            for (int i= 1;i<=GameConfig.Room_Max_PlayerCount;i++)
        //            {
        //                _PlayCards.Add(i, new List<ECard>());
        //            }
        //        }
        //        return _PlayCards;
        //    }
        //}

        /// <summary>
        /// Key:有序编码
        /// </summary>
        private List<ECard> _RemainCards;
        public List<ECard> RemainCards
        {
            get
            {
                if (_RemainCards == null)
                {
                    _RemainCards = new List<ECard>();
                    //InitNewCards();
                }
                return _RemainCards;
            }
        }
        static CardDataManager(){

          //  CardMap = InitNewCards();
        }
        public CardDataManager()
        {
             
        }

        public void InitCardPile()
        {
            TableCards.Clear();

           // CleanPlayerCards();

            if (RemainCards.Count < 52)
                _RemainCards = InitNewCards();
        }

        //public void CleanPlayerCards()
        //{
        //    for (int i = 1; i <= GameConfig.Room_Max_PlayerCount; i++)
        //    {
        //        PlayCards[i].Clear();
        //    }
        //}
        public void ShuffleCard(List<ERoomUser> PlayerList)
        {
            InitCardPile();

            DealToTable();

            DealToAllPlayer(PlayerList);

        }

        /// <summary>
        /// 发牌桌的牌
        /// </summary>
        private void DealToTable()
        {
            TableCards.Clear();
            for (int i=1;i<= GameConfig.Table_Cards_Count; i++)
            {
                var card = PickCardFromPile();
                TableCards.Add(i, card);
            }
        }

        private void DealToAllPlayer(List<ERoomUser> PlayerList)
        {
            foreach(ERoomUser player in PlayerList)
            {
                if (player.CardList == null) player.CardList = new List<ECard>();
                else player.CardList.Clear();

                for (int j = 0; j < 2; j++)
                {
                    var card = PickCardFromPile();
                    player.CardList.Add(card);
                    
                }
            }
            //for (int i = 1; i <= 8; i++)
            //{
            //    PlayCards[i].Clear();
            //    for (int j =0;j<2;j++)
            //    {
            //        var card = PickCardFromPile();
            //        PlayCards[i].Add(card);
            //    }
               
            //}
        }

        public ECard PickCardFromPile()
        {
            var r = new Random();

            int remCount = RemainCards.Count;
            int hitnum = r.Next(1, remCount);

            var GetCard = RemainCards[hitnum];
            //排队取出
            RemainCards.RemoveAt(hitnum);
            return GetCard;
        }      
    }
}
