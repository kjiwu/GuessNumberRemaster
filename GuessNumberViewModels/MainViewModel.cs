using GuessNumberCustomComponents.DialNumbers;
using GuessNumberCustomComponents.Popups;
using GuessNumberModels;
using System;
using System.Diagnostics;

namespace GuessNumberViewModels
{
    public class MainViewModel : ViewModelBase
    {
        Game game;
        DialNumbers dial;

        public MainViewModel()
        {
            game = Game.CreateInstance();
            game.Guess += game_Guess;
        }

        void game_Guess(object sender, GuessNumberGameArgs e)
        {
            if (e.IsWin)
            {

            }
            else
            {

            }

            if (null != dial)
            {
                dial.Reset();
            }

            game.Reset();
        }

        public void dialNumbers_Dial(object sender, DialNumberEventArgs e)
        {
            dial = sender as GuessNumberCustomComponents.DialNumbers.DialNumbers;

            Debug.WriteLine(e.Number);
            switch (e.Type)
            {
                case GuessNumberCustomComponents.DialNumbers.KeyType.Number:
                    InputNumbers = InputNumbers + e.Number;
                    break;
                case GuessNumberCustomComponents.DialNumbers.KeyType.Back:
                    InputNumbers = InputNumbers.Substring(0, InputNumbers.Length - 1);
                    break;
                case GuessNumberCustomComponents.DialNumbers.KeyType.Ok:
                    string result = game.GetYourGuessResult(InputNumbers);
                    GuessResultPopup popup = new GuessResultPopup();
                    popup.Show();

                    InputNumbers = String.Empty;
                    dial.Reset();
                    break;
            }
        }

        #region InputNumbers

        private string inputNumbers;
        public string InputNumbers
        {
            get
            {
                return inputNumbers;
            }
            set
            {
                inputNumbers = value;
                RaisePropertyChanged(() => InputNumbers);
            }
        }

        #endregion
    }
}
