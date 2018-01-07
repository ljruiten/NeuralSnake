using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NeuralSnake.Snake {
    class KeyBoardPlayer : IPlayer {
        private Heading heading;

        public KeyBoardPlayer() {
            var thread = new Thread(() => RunKeyBoardThread());
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private void RunKeyBoardThread() {
            while(true) {
                if (Keyboard.IsKeyDown(Key.Up)) {
                    heading =  Heading.UP;
                } else if (Keyboard.IsKeyDown(Key.Down)) {
                    heading =  Heading.DOWN;
                } else if (Keyboard.IsKeyDown(Key.Left)) {
                    heading = Heading.LEFT;
                } else if (Keyboard.IsKeyDown(Key.Right)) {
                    heading = Heading.RIGHT;
                } else {
                    heading = Heading.NONE;
                }

                Thread.Sleep(50);
            }
        }

        public Heading GetMove() {
            return heading;
        }
    }
}
