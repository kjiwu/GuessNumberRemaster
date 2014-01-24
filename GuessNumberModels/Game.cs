using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace GuessNumberModels
{
    public class GuessNumberGameArgs : EventArgs
    {
        public Boolean IsWin { get; set; }

        public GuessNumberGameArgs(bool isWin)
        {
            IsWin = isWin;
        }
    }

    public delegate void GuessNumberGameEventHandler(object sender, GuessNumberGameArgs e);


    public class Game
    {
        private static Game game;

        public static Game CreateInstance()
        {
            if (null == game)
            {
                game = new Game();
            }

            return game;
        }

        private Game()
        {
            GuessResult = GetGuessNumber();

            Historys = new ObservableCollection<string>();
            GuessTimes = 8;
        }

        public ObservableCollection<string> Historys { get; set; }

        private string GetGuessNumber()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            Stack<int> stack = new Stack<int>();
            while (stack.Count < 4)
            {
                int number = rand.Next(1, 9);
                while (stack.Contains(number))
                {
                    number = rand.Next(1, 9);
                }

                stack.Push(number);
            }

            return GetStackString(stack);
        }

        private string GetStackString(Stack<int> stack)
        {
            int result = 0;
            for (int i = 0; i < stack.Count; i++)
            {
                result += (int)(Math.Pow(10, i) * stack.ElementAt(i));
            }
            return result.ToString();
        }

        public string GetYourGuessResult(string value)
        {
            int A = 0, B = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Equals(GuessResult[i]))
                {
                    A++;
                }
                else if (GuessResult.Contains(value[i]))
                {
                    B++;
                }
            }

            if (A == GuessResult.Length)
            {
                if (null != Guess)
                {
                    Guess(this, new GuessNumberGameArgs(true));
                }
            }

            string result = String.Format("{0}A{1}B", A, B);
            Historys.Add(result);

            if (Historys.Count == GuessTimes)
            {
                if (null != Guess)
                {
                    Guess(this, new GuessNumberGameArgs(false));
                }
            }

            return result;
        }

        public string GuessResult { get; protected set; }

        public Boolean IsWin { get; protected set; }

        public int GuessTimes { get; set; }

        public void Reset()
        {
            GuessResult = GetGuessNumber();
            Historys = new ObservableCollection<string>();
        }

        public event GuessNumberGameEventHandler Guess;            
    }
}
