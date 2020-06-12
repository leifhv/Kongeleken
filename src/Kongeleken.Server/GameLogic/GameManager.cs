using Kongeleken.Server.Infrastructure;
using Kongeleken.Shared;
using Kongeleken.Shared.Constants;
using Kongeleken.Shared.DataObjects;
using Kongeleken.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{
    public interface IGameManager
    {
        Task<Result<StartNewGameResponse>> StartNewGameAsync(string initiatingPlayerName);
        Task<Result<AddPlayerResponse>> AddPlayerAsync(string gameId, string playerName);
        //Task<Result> TurnCardAsync(string gameId, string playerId, string cardId);
        Task<Result<GameDto>> GetGameAsync(string id,string forPlayerId);
        Task<GameDto> HandleGameEvent(GameEventDto gameEventDto);
    }

    public class GameManager : IGameManager
    {
        private IGameStore _gameStore;
        

        public GameManager(IGameStore gameStore)
        {
            _gameStore = gameStore;
        }

        public async Task<Result<StartNewGameResponse>> StartNewGameAsync(string initiatingPlayerName)
        {
            var newGame =  _gameStore.CreateNew();
            newGame.AddGameAction(initiatingPlayerName, $"{initiatingPlayerName} started the game", UserAction.None);

            newGame.CardDeck = new CardDeck();

            var newPlayerId =  AddPlayer(newGame, initiatingPlayerName);
            newGame.DealerPlayerId = newPlayerId;  //Player that starts the game is dealer

            var response = new StartNewGameResponse();
            response.NewPlayerId = newPlayerId;
            response.Game = DtoMapper.ToDto(newGame, newPlayerId);


            return Result<StartNewGameResponse>.Success(response);
        }

        private string AddPlayer(Game game, string playerName)
        {
            var player = new Player();
            var newPlayerId = Guid.NewGuid().ToString();
            player.Id = newPlayerId;
            player.Name = playerName;
            player.CurrentCard = null;
            player.PreviousCards = new List<Card>();

            game.Players.Add(player);

            game.AddGameAction(playerName, $"{playerName} joined the game", UserAction.None);

            return newPlayerId;
        }


        public async Task<Result<AddPlayerResponse>> AddPlayerAsync(string gameId,string playerName)
        {
            var game = await _gameStore.GetAsync(gameId);
            if(game == null)
            {
                return Result<AddPlayerResponse>.Failure("Unable to find game for id");
            }

            var newPlayerId = AddPlayer(game, playerName);
            //var player = new Player();
            //var newPlayerId = Guid.NewGuid().ToString();
            //player.Id = newPlayerId;
            //player.Name = playerName;
            //player.CurrentCard = null;
            //player.PreviousCards = new List<Card>();            

            //game.Players.Add(player);

            var response = new AddPlayerResponse();
            response.NewPlayerId = newPlayerId;
            response.Game = DtoMapper.ToDto(game,newPlayerId);

            return Result<AddPlayerResponse>.Success(response);
        }
        //public async Task<Result> TurnCardAsync(string gameId, string playerId, string cardId)
        //{
        //    var game = await _gameStore.GetAsync(gameId);
        //    if (game == null)
        //    {
        //        return Result.Failure("Unable to find game for id");
        //    }
        //    var player = game.Players.Single(p => p.Id == playerId);
        //    var card = player.CurrentCard;
        //    card.IsTurned = true;
        //    return Result.Success();
        //}

        public async Task<Result<GameDto>> GetGameAsync(string id, string forPlayerId)
        {
            var game = await _gameStore.GetAsync(id);

            if (game == null)
            {
                return Result<GameDto>.Failure("Unable to find a game with the id");
            }
            else
            {
                var gameDto = DtoMapper.ToDto(game,forPlayerId);
                return Result<GameDto>.Success(gameDto);
            }
        }



        public async Task<GameDto> HandleGameEvent(GameEventDto gameEventDto)
        {
            var game = await _gameStore.GetAsync(gameEventDto.GameId);
            if (game == null)
            {
                return null;
            }

            var initiatingPlayer = game.Players.Single(p => p.Id == gameEventDto.PlayerId);

            switch (gameEventDto.EventType)
            {
                case GameEventType.Nothing:
                    break;
                case GameEventType.Join:
                    break;
                case GameEventType.ShuffleDeck:
                    game.CardDeck.Shuffle();
                    game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} shuffled the deck", UserAction.None);
                    break;
                case GameEventType.Deal:
                { 
                    foreach(var player in game.Players)
                    {
                        player.CurrentCard = game.CardDeck.First();
                        player.CurrentCard.IsTurned = false;
                        game.CardDeck.RemoveAt(0);
                    }
                    game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} dealt cards", UserAction.None);
                }
                break;
                case GameEventType.TurnCard:
                    game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} turned his card", UserAction.None);
                    HandleTurnCardEvent(gameEventDto, game);
                    break;
                default:
                    throw new Exception("Unknown gameeventtype?");
                    break;
            }

            return DtoMapper.ToDto(game,gameEventDto.PlayerId);
        }

        
        private void HandleTurnCardEvent(GameEventDto gameEventDto, Game game)
        {
            var player = game.Players.Single(p => p.Id == gameEventDto.PlayerId);
            var card = player.CurrentCard;
            card.IsTurned = true;

            if(game.Players.All(p => p.CurrentCard.IsTurned))
            {
                var lowestCard = game.Players.Select(p => p.CurrentCard.Value).Min();
                var loosers = game.Players.Where(p => p.CurrentCard.Value == lowestCard);

                foreach (var loser in loosers)
                {
                    game.GameActions.Add(new GameActionDto(loser.Name, $"Lowest card is {lowestCard}. Looser this round is {loser.Name}.  DRINK!", UserAction.Drink));
                }

                //Handle king
                var playersWithKing = game.Players.Where(p => p.CurrentCard.Value == CardValue.King);
                foreach (var playerWithKing in playersWithKing)
                {
                    game.GameActions.Add(new GameActionDto(playerWithKing.Name, $"{playerWithKing.Name} got a king! ***DRINK!***", UserAction.DrinkKing));
                }

                //Handle queen
                var playersWithQueen = game.Players.Where(p => p.CurrentCard.Value == CardValue.Queen);
                foreach (var playerWithQueen in playersWithQueen)
                {
                    var playersWithPictureCard = game.Players.Where(p => p.CurrentCard.Value == CardValue.Queen
                    || p.CurrentCard.Value == CardValue.Jack
                    || p.CurrentCard.Value == CardValue.King).Where(p => p != playerWithQueen);

                    var playerNames = string.Join(",", playersWithPictureCard.Select(l => l.Name));

                    game.GameActions.Add(new GameActionDto(playerWithQueen.Name, $"{playerWithQueen.Name} got a queen! {playerNames} must DRINK!", UserAction.DrinkQueen));
                }

                //Handle jack
                var playersWithJack = game.Players.Where(p => p.CurrentCard.Value == CardValue.Jack);
                foreach (var playerWithJack in playersWithJack)
                {
                    var playersExceptCurrent = game.Players.Where(p => p != playerWithJack);
                    var playerNames = string.Join(",", playersExceptCurrent.Select(l => l.Name));
                    game.GameActions.Add(new GameActionDto(playerWithJack.Name, $"{playerWithJack.Name} got a jack! {playerNames} must DRINK!", UserAction.DrinkJack));
                }
                //TODO:
                //If a player receives a 6 of hearts he is to be given three new cards and all players must act according to these cards before a new round is started.
                //If a player receives a 6 of diamonds the player to the left of him is to be given three new cards and all players must act according  the these cards before a new round is started.
            }

        }


        /*
    Generally the player with the card of lowest value must drink some of his personal drink. (There is not standard measure of how much he must drink, the other players will normally complain if he drinks to little.)
    -The ace is the lowest card (valued as a 1), but:
    7 is considered to be lower than 1
    10 is considered to be lower than 7

    Special cards:
    If one player gets a king he must drink the "Kings drink"
    For each player that gets a queen every other player with a picture card (king,queen or jack) must drink some of his personal drink.
    For each player that get a jack every other player must drink some of his personal drink.
    If a player receives a 6 of hearts he is to be given three new cards and all players must act according to these cards before a new round is started.
    If a player receives a 6 of diamonds the player to the left of him is to be given three new cards and all players must act according the these cards before a new round is started.

    After each round new drinks should be mixed (if any of them have been drunk), before starting a new round. When all cards in the deck have been dealt, the deck is passed on to the player to the left of the current dealer.
    Draw situations
    If two players ends up with the lowest card, they both have to drink of their personal drink, and both receive a new card and have to act according to these two new cards.
    If two players receives a king, both players receives a new card and the one with the lowest valued card have to drink the "Kings drink"     
         */
    }
}
