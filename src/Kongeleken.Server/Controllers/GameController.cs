using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kongeleken.Server.GameLogic;
using Kongeleken.Shared.Constants;
using Kongeleken.Shared.DataObjects;
using Kongeleken.Shared.Messages;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kongeleken.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameManager _gameManager;
        public GameController(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }


        // GET api/<GameController>/5
        [HttpGet("")]
        public async Task<GameDto> Get(string id,string playerId)
        {
            var gameResult = await _gameManager.GetGameAsync(id,playerId);
            if(gameResult.Succeeded)
            {
                return gameResult.Value;
            }
            else
            {
                return null;
            }
        }

        // POST api/<GameController>
        [HttpPost("player")]
        public async Task<AddPlayerResponse> Post([FromBody] AddPlayerRequest addPlayerRequest)
        {
            var addPlayerResult = await _gameManager.AddPlayerAsync(addPlayerRequest.GameId, addPlayerRequest.PlayerName);
            return addPlayerResult.Value;
        }

        //[HttpPost("card")]
        //public async Task Post([FromBody] TurnCardRequest turnCardRequest)
        //{
        //    await _gameManager.TurnCardAsync(turnCardRequest.GameId, turnCardRequest.PlayerId, turnCardRequest.GameId);
        //}

        [HttpPost("event")]
        public async Task<GameDto> PostAsync([FromBody] NewGameEventRequest gameEventRequest)
        {
            var newGameState = await _gameManager.HandleGameEvent(gameEventRequest.GameEvent);
            return newGameState;
        }


        [HttpPost]
        public async Task<StartNewGameResponse> PostAsync([FromBody] StartNewGameRequest startNewGameRequest)
        {
            var newGameResult = await _gameManager.StartNewGameAsync(startNewGameRequest.PlayerName);
            return newGameResult.Value;
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
