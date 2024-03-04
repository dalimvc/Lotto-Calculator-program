using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lottoprogram
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        string errorMessage;
        int rounds;
        int fives = 0;
        int sixes = 0;
        int sevens = 0;
        bool checkInput;
        Random randomNumber = new Random();
        List<int> enteredLottoNumbers = new List<int>();
        List<List<int>> generatedLottoSeries = new List<List<int>>();

        //Method gets values from users' input and tries to parse it as an int and if successfull adds those values into a list
        //otherwise error message if uppdated
        //checkInput will be used to update result, when the first time user has entered correct input but some of the following times input is not correct
        //between different round list is erased and the same goes for the error message
        public void GetNumbers()
        {
            errorMessage = string.Empty;
            enteredLottoNumbers.Clear();
            try
            {
                int.TryParse(number1.Text, out int value1);
                int.TryParse(number2.Text, out int value2);
                int.TryParse(number3.Text, out int value3);
                int.TryParse(number4.Text, out int value4);
                int.TryParse(number5.Text, out int value5);
                int.TryParse(number6.Text, out int value6);
                int.TryParse(number7.Text, out int value7);
                enteredLottoNumbers.Add(value1);
                enteredLottoNumbers.Add(value2);
                enteredLottoNumbers.Add(value3);
                enteredLottoNumbers.Add(value4);
                enteredLottoNumbers.Add(value5);
                enteredLottoNumbers.Add(value6);
                enteredLottoNumbers.Add(value7);
            }
            catch
            {
                checkInput = false;
                errorMessage = "Alla fält är obligatoriska och du kan endast ange nummer mellan 1-35!";
                ErrorMessageTextBlock.Text = errorMessage;
            }
        }

        //works the same way as the previous method, only this time, the method tries to parse number of rounds
        public void GetNrOfRounds()
        {
            try
            {
                int.TryParse(nrDraws.Text, out rounds);
            }
            catch
            {
                checkInput = false;
                errorMessage = "Antal dragningar måste vara ett positivt nummer!";
                ErrorMessageTextBlock.Text = errorMessage;
            }
        }

        //even if parsing was successful, input might be wrong; 2 or more numbers can be the same or they can be 0 or less or bigger than 35
        //number of round can not be 0 or less than 0
        //method checks input and if it is not correct appropriate error message will be shown
        public void CheckNumberValidity()
        {
            for (int i = 0; i < enteredLottoNumbers.Count - 1; i++)
            {
                if (enteredLottoNumbers[i] < 1 || enteredLottoNumbers[i] > 35)
                {
                    checkInput = false;
                    errorMessage = "Numren måste vara mellan 1 och 35! Felaktig inmatning.";
                }
                else if (enteredLottoNumbers[i] == enteredLottoNumbers[i + 1])
                {
                    checkInput = false;
                    errorMessage = "Du kan inte ange samma nummer mer än en gång! Felaktig inmatning.";
                }
                else if (rounds <= 0)
                {
                    checkInput = false;
                    errorMessage = "Fältet är obligatoriskt och det måste vara ett positivt nummer! Felaktig inmatning.";
                }
            }
            ErrorMessageTextBlock.Text = errorMessage;
        }

        //method that generates one lotto series with 7 numbers between 1 and 35
        //method returns an list
        public List<int> generateLottoNumbers()
        {
            List<int> generatedLottoNumbers = new List<int>();
            generatedLottoNumbers.Add(randomNumber.Next(1, 36));
            int counter = 0;
            while (counter < 6)
            {
                int lottoNumber = randomNumber.Next(1, 36);
                if (!generatedLottoNumbers.Contains(lottoNumber))
                {
                    generatedLottoNumbers.Add(lottoNumber);
                    counter++;
                }
            }
            return generatedLottoNumbers;
        }

        //method that generates n-amount of lotto series based on users input "antal dragningar"
        //method returns list of list type int
        //between 2 rounds list is cleared
        public List<List<int>> generateLottoSeriers()
        {
            generatedLottoSeries.Clear();
            int counter = 0;
            while (rounds > counter)
            {
                generatedLottoSeries.Add(generateLottoNumbers());
                counter++;
            }
            return generatedLottoSeries;       
        }

        //counting number if matches between users input and computr's generated lotto series
        //it loops through outer list and than throught each inner list that contains 1 lotto series
        //then it checks how many mathes, same elements are in the inner list and the list that contains users numbers
        //than number of mathes will uppdate
        //at the end fives and sixes will be updated as fives contains also number of sixes and sevens and sixes contain both number of sixes and sevens
        public void CountOccurrences()
        {
            sevens = 0;
            sixes = 0;
            fives = 0;
            foreach (List<int> innerList in generatedLottoSeries)
            {
                int count = innerList.Intersect(enteredLottoNumbers).Count();
                if (count == 5)
                    fives++;
                else if (count == 6)
                    sixes++;
                else if (count == 7)
                    sevens++;
            }
            fives = fives - sixes - sevens;
            sixes = sixes - sevens;
        }

        //based on result from a method CountOccurrences(), text that shows matches updates
        //if...else statement is for the case user enters correct input than wrong input
        //when it's wrong input results will be 0
        public void updateTextresult()
        {
            if (checkInput == true)
            {
                fiveMatches.Text = fives.ToString();
                sixMatches.Text = sixes.ToString();
                SevenMatches.Text = sevens.ToString();
            }
            else
            {
                fiveMatches.Text = "0";
                sixMatches.Text = "0";
                SevenMatches.Text = "0";
            }

        }

        //"main method" that runs all previously declared methods
        //checkInput (user has entered correct input) is initialy true but gets changes in methods that check users' input
        //if input is false methods where conputer generates lotto series won't be running
        private void RunLotto(object sender, RoutedEventArgs e)
        {
            checkInput = true;
            updateTextresult();
            GetNumbers();
            GetNrOfRounds();
            CheckNumberValidity();
            if (checkInput == true)
            {
                generateLottoSeriers();
                CountOccurrences();
            }
            updateTextresult();
        }
    }
}
