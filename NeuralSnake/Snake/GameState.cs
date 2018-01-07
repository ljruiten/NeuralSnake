using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralSnake.Snake {
    class GameState {
        public IEnumerable<BodyPart> Snake { get; set; }
        public (int x, int y) Apple { get; set; }
    }
}
