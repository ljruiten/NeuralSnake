using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralSnake.Snake {
    class GameStateUpdatedEventArgs : EventArgs {
        public GameState State { get; set; }
    }
}
