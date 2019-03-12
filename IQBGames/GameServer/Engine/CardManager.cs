using GameModel;
using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class CardManager
    {
        public static  Dictionary<int, ECard> CardMap { get; set; }

        
        public static Dictionary<int, ECard> InitNewCards()
        {
            var Cards = new Dictionary<int, ECard>();
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
                Cards.Add(i, card);
            }
            return Cards;
        }

        public static Dictionary<int,ECard> NoToCard(List<int> noList)
        {
            var r = new Dictionary<int, ECard>();
            foreach(var no in noList)
            {
                var card = CardMap[no];
                r.Add(no, card);
            }
            return r;
        }
        static CardManager(){

            CardMap = InitNewCards();
        }
        public CardManager()
        {
           
        }

      
    }
}
