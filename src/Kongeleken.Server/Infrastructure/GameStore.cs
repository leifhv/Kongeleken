using Kongeleken.Server.GameLogic;
using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.Infrastructure
{

    public interface IGameStore
    {
        Game CreateNew();
        void Delete(string id);
        Task<Game> GetAsync(string id);

        Task<Game> SaveAsync(Game game);

    }

    public class GameStore : IGameStore
    {
        //Todo: Create proper persistance of games

        internal class Entry<T>
        {
            public Entry(T item)
            {
                Item = item;
            }
            public DateTime RegTime { get; private set; } = DateTime.Now;
            public T Item { get; private set; }
        }

        private Dictionary<string, Entry<Game>> _games = new Dictionary<string, Entry<Game>>();
        private object _lockObject = new object();

        public Game CreateNew()
        {
            var newGame = new Game();
            newGame.Id = Guid.NewGuid().ToString();            
            lock (_lockObject)
            {
                if (_games.Count > 100) //Max 100 active games
                {
                    var firstEntry = _games.OrderBy(e => e.Value.RegTime).FirstOrDefault();
                    _games.Remove(firstEntry.Key);
                }

                _games.Add(newGame.Id, new Entry<Game>(newGame));
                return newGame;
            }
            Debug.Print("CreateNew: " + newGame.Id);
        }

        public void Delete(string id)
        {
            lock (_lockObject)
            {
                if (_games.ContainsKey(id))
                {
                    _games.Remove(id);
                }

            }
        }

        public async Task<Game> GetAsync(string id)
        {
            lock (_lockObject)
            {
                if (_games.ContainsKey(id))
                {
                    return _games[id].Item;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Game> SaveAsync(Game game)
        {
            lock (_lockObject)
            {
                if (!string.IsNullOrEmpty(game.Id)
                && _games.ContainsKey(game.Id))
                {
                    _games[game.Id] = new Entry<Game>(game);
                    return game;
                }

                if (_games.Count > 100)
                {
                    var firstEntry = _games.OrderBy(e => e.Value.RegTime).FirstOrDefault();
                    _games.Remove(firstEntry.Key);
                }

                if (string.IsNullOrEmpty(game.Id))
                {
                    game.Id = Guid.NewGuid().ToString();
                }

                _games.Add(game.Id, new Entry<Game>(game));
                return game;
            }
        }
    }
}
