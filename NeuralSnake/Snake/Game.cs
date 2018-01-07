using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralSnake.Snake {
    class Game {
        private (int x, int y) size;

        private int fps;

        private Stack<BodyPart> snake;

        private (int x, int y) apple;

        private IPlayer player;

        private Heading heading;

        private Random random;

        private CancellationTokenSource source;

        private CancellationToken token;

        public event EventHandler<GameStateUpdatedEventArgs> OnUpdate;

        public Game(IPlayer player, int sizeX, int sizeY, int fps) {
            size.x = sizeX;
            size.y = sizeY;

            this.fps = fps;

            snake = new Stack<BodyPart>();

            heading = Heading.UP;

            this.player = player;

            random = new Random();

            source = new CancellationTokenSource();
            token = source.Token;

            Initialize();
        }

        public void Start() {
            Task.Run(() => RunMainGameLoop(token));
        }

        public void Stop() {
            source.Cancel();
        }

        private void RunMainGameLoop(CancellationToken token) {
            while (!token.IsCancellationRequested) {
                var move = player.GetMove();

                UpdateHeading(move);

                bool eatsApple = CheckApple();

                UpdateSnake(eatsApple);

                if (eatsApple) {
                    MakeApple();
                }

                if (IsColliding()) {
                    snake.Clear();
                    MakeApple();
                    MakeSnake();
                }

                var state = new GameState() {
                    Apple = apple,
                    Snake = snake
                };

                var args = new GameStateUpdatedEventArgs();
                args.State = state;

                OnUpdate?.Invoke(this, args);

                Thread.Sleep(1000 / fps);
            }
        }

        private void Initialize() {
            MakeApple();
            MakeSnake();
        }

        private void MakeApple() {
            var r = new Random();

            int x = r.Next(size.x - 1);
            int y = r.Next(size.y - 1);

            apple = (x, y);
        }

        private void MakeSnake() {
            int originX = 3;
            int originY = 5;

            for (int i = 0; i <= 2; i++) {
                var part = new BodyPart();

                part.Location = (originX, originY + i);

                snake.Push(part);
            }
        }

        private bool CheckApple() {
            var head = snake.Last();

            if (head.Location.X == apple.x &&
                head.Location.Y == apple.y) {
                return true;
            } else {
                return false;
            }
        }

        private void UpdateHeading(Heading move) {
            // Do not update the heading if the move is illegal.
            if (heading == Heading.LEFT && move == Heading.RIGHT ||
                heading == Heading.RIGHT && move == Heading.LEFT ||
                heading == Heading.UP && move == Heading.DOWN ||
                heading == Heading.DOWN && move == Heading.UP || 
                move == Heading.NONE) {

                return;
            } else {
                heading = move;
            }
        }

        private bool IsColliding() {
            var head = snake.Last();

            // Check for collisions with body
            foreach (var part in snake) {
                if (part != head && part.Location.X == head.Location.X &&
                    part.Location.Y == head.Location.Y) {
                    return true;
                }
            }

            // Check for collisions with borders
            if (head.Location.X < 0 || head.Location.X >size.x - 1) {
                return true;
            }

            if (head.Location.Y < 0 || head.Location.Y > size.y - 1) {
                return true;
            }

            return false;
        }

        private void UpdateSnake(bool addBodyPart) {
            if (addBodyPart) {
                var part = new BodyPart();
                part.Location = apple;
                snake.Push(part);
            }

            var head = snake.Last();

            (int x, int y) newLocation = (0, 0);

            if (heading == Heading.UP) {
                newLocation = (head.Location.X, head.Location.Y + 1);
            } else if (heading == Heading.DOWN) {
                newLocation = (head.Location.X, head.Location.Y - 1);
            } else if (heading == Heading.LEFT) {
                newLocation = (head.Location.X - 1, head.Location.Y);
            } else if (heading == Heading.RIGHT) {
                newLocation = (head.Location.X + 1, head.Location.Y);
            }

            for (int i = snake.Count() - 1; i >= 0; i--) {
                var current = snake.ElementAt(i);

                var temp = current.Location;

                snake.ElementAt(i).Location = newLocation;

                newLocation = temp;
            }
        }
    }
}
