using System.Collections.Generic;

namespace Project.BattleShip.Models
{
    public interface IGame
    {
        event Game.ShipEvent RaiseEvent;

        void AddShipsToBoard(IEnumerable<Ship> ships, int maxWidth, int maxHeight);
        Shot TakeShot(char column, int row);
    }
}