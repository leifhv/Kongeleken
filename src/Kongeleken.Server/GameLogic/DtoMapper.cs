using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{
    public class DtoMapper
    {
        public static GameDto ToDto(Game game,string forPlayerId)
        {
            var dto = new GameDto();
            dto.Id = game.Id;
            dto.Players = game.Players.Select(p => ToDto(p)).ToList();

            var dealer = game.Players.FirstOrDefault(p => p.Id == game.DealerPlayerId);
            if(dealer != null)
            {
                dto.DealerName = dealer.Name;
                if(dealer.Id == forPlayerId)
                {
                    dto.PlayerIsDealer = true;
                }
                else
                {
                    dto.PlayerIsDealer = false;
                }
            }
            else
            {
                throw new Exception("No dealer in game!?");
            }

            dto.GameActions = new List<GameActionDto>();
            dto.GameActions.AddRange(game.GameActions);
            dto.GameActions.Reverse();
            return dto;
        }

        public static PlayerDto ToDto(Player player)
        {
            var dto = new PlayerDto();
            dto.Id = player.Id;
            dto.Name = player.Name;
            dto.FaceUpCards = player.PreviousCards.Select(c => ToCardDto(c)).ToList();
            dto.FaceDownCards = new List<FaceDownCardDto>();
            if (player.CurrentCard != null)
            {
                if (player.CurrentCard.IsTurned)
                {
                    dto.FaceUpCards.Add(ToCardDto(player.CurrentCard));
                }
                else
                {
                    dto.FaceDownCards.Add(ToFaceDownCardDto(player.CurrentCard));
                }
            }

            return dto;
        }

        public static FaceDownCardDto ToFaceDownCardDto(Card card)
        {
            if (card == null)
            {
                return null;
            }
            var faceDownCard = new FaceDownCardDto();
            faceDownCard.Id = card.Id;
            return faceDownCard;
        }

        public static CardDto ToCardDto(Card card)
        {
            var cardDto = new CardDto();
            cardDto.Id = card.Id;
            cardDto.Suit = card.Suit;
            cardDto.Value = card.Value;
            return cardDto;
        }
    }
}
