using Kongeleken.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{
    public class CardDeck : List<Card>
    {
        private static Random rng = new Random();

        public CardDeck()
        {
            Init();
        }

        public void Shuffle()
        {
            Init();
            int n = this.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }

        private void Init()
        {
            Clear();
            //Add(NewCard(CardSuit.None, CardValue.Joker));
            Add(NewCard(CardSuit.Club, CardValue.Ace));
            Add(NewCard(CardSuit.Club, CardValue.Two));
            Add(NewCard(CardSuit.Club, CardValue.Three));
            Add(NewCard(CardSuit.Club, CardValue.Four));
            Add(NewCard(CardSuit.Club, CardValue.Five));
            Add(NewCard(CardSuit.Club, CardValue.Six));
            Add(NewCard(CardSuit.Club, CardValue.Seven));
            Add(NewCard(CardSuit.Club, CardValue.Eight));
            Add(NewCard(CardSuit.Club, CardValue.Nine));
            Add(NewCard(CardSuit.Club, CardValue.Ten));
            Add(NewCard(CardSuit.Club, CardValue.Jack));
            Add(NewCard(CardSuit.Club, CardValue.Queen));
            Add(NewCard(CardSuit.Club, CardValue.King));

            Add(NewCard(CardSuit.Spade, CardValue.Ace));
            Add(NewCard(CardSuit.Spade, CardValue.Two));
            Add(NewCard(CardSuit.Spade, CardValue.Three));
            Add(NewCard(CardSuit.Spade, CardValue.Four));
            Add(NewCard(CardSuit.Spade, CardValue.Five));
            Add(NewCard(CardSuit.Spade, CardValue.Six));
            Add(NewCard(CardSuit.Spade, CardValue.Seven));
            Add(NewCard(CardSuit.Spade, CardValue.Eight));
            Add(NewCard(CardSuit.Spade, CardValue.Nine));
            Add(NewCard(CardSuit.Spade, CardValue.Ten));
            Add(NewCard(CardSuit.Spade, CardValue.Jack));
            Add(NewCard(CardSuit.Spade, CardValue.Queen));
            Add(NewCard(CardSuit.Spade, CardValue.King));

            Add(NewCard(CardSuit.Heart, CardValue.Ace));
            Add(NewCard(CardSuit.Heart, CardValue.Two));
            Add(NewCard(CardSuit.Heart, CardValue.Three));
            Add(NewCard(CardSuit.Heart, CardValue.Four));
            Add(NewCard(CardSuit.Heart, CardValue.Five));
            Add(NewCard(CardSuit.Heart, CardValue.Six));
            Add(NewCard(CardSuit.Heart, CardValue.Seven));
            Add(NewCard(CardSuit.Heart, CardValue.Eight));
            Add(NewCard(CardSuit.Heart, CardValue.Nine));
            Add(NewCard(CardSuit.Heart, CardValue.Ten));
            Add(NewCard(CardSuit.Heart, CardValue.Jack));
            Add(NewCard(CardSuit.Heart, CardValue.Queen));
            Add(NewCard(CardSuit.Heart, CardValue.King));

            Add(NewCard(CardSuit.Diamond, CardValue.Ace));
            Add(NewCard(CardSuit.Diamond, CardValue.Two));
            Add(NewCard(CardSuit.Diamond, CardValue.Three));
            Add(NewCard(CardSuit.Diamond, CardValue.Four));
            Add(NewCard(CardSuit.Diamond, CardValue.Five));
            Add(NewCard(CardSuit.Diamond, CardValue.Six));
            Add(NewCard(CardSuit.Diamond, CardValue.Seven));
            Add(NewCard(CardSuit.Diamond, CardValue.Eight));
            Add(NewCard(CardSuit.Diamond, CardValue.Nine));
            Add(NewCard(CardSuit.Diamond, CardValue.Ten));
            Add(NewCard(CardSuit.Diamond, CardValue.Jack));
            Add(NewCard(CardSuit.Diamond, CardValue.Queen));
            Add(NewCard(CardSuit.Diamond, CardValue.King));

        }

        private Card NewCard(CardSuit suit, CardValue value)
        {
            var card = new Card();
            card.Id = Guid.NewGuid().ToString();
            card.Suit = suit;
            card.Value = value;
            return card;
        }
    }
}
