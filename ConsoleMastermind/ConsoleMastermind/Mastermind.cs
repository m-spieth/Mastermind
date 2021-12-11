using System;
using System.Collections.Generic;


namespace ConsoleMastermind
{
    public class Mastermind
    {
        private List<int> answerList = new List<int>();
        private int numOfRemainingAttempts = 10;
        private bool isGuessCorrect = false;
        
        public Mastermind()
        {
            GenerateAnswerList();
        }

        private void GenerateAnswerList()
        {
            Random randomGenerator = new Random();

            answerList.Add(randomGenerator.Next(1, 6));
            answerList.Add(randomGenerator.Next(1, 6));
            answerList.Add(randomGenerator.Next(1, 6));
            answerList.Add(randomGenerator.Next(1, 6));
        }

        public void Run()
        {
            //Build a string of the corect answer for use in our completion messages to the user
            //and for an easy compare check on an user answers
            string answerString = "";
            for (int i = 0; i < answerList.Count; i++)
            {
                answerString += answerList[i].ToString();
            }

            //Print the instuctions to the user and start prompting them for answers
            PrintStartMessage();

            while (numOfRemainingAttempts > 0 && !isGuessCorrect)
            {
                string validQuess = PromptTheUserForAGuess();
                CheckValidQuess(validQuess, answerString);
                numOfRemainingAttempts--;
            }

            //The user has either guessed correctly or run out of attempts so the game should be over
            if (isGuessCorrect)
            {
                //Print a message to congratulate the user
                Console.WriteLine();
                Console.WriteLine("CONGRATULATIONS!!! You have quessed the correct answer.");
                Console.WriteLine("Correct answer was: " + answerString);
            }
            else
            {
                //User should be out of attempts so print the failure messages
                Console.WriteLine();
                Console.WriteLine("Sorry you have failed to guess the correct answer and are out of attempts.");
                Console.WriteLine("Correct answer was: " + answerString);
            }
            
        }

        private void PrintStartMessage()
        {
            Console.WriteLine("Welcome to Mastermind!");
            Console.WriteLine();
            Console.WriteLine("A random 4 digit answer has been generated for you to guess. " + 
                "Each digit in the answer is between the number 1 and 6. You have 10 attempts " + 
                "to guess the answer correctly. After each guess you will be shown a '+' for each " + 
                "digit that is both correct and in the correct position. You will be shown a '-' " + 
                "for each digit that is correct but in the wrong position.");
            Console.WriteLine();
            Console.WriteLine("Good luck!");
            Console.WriteLine();
        }

        private string PromptTheUserForAGuess()
        {
            bool isValid = false;
            string userInput = "";

            //Repeadly prompt the user to enter a quess until they have entered a valid attempt
            while (!isValid)
            {
                Console.WriteLine("Attempts Remaining: " + numOfRemainingAttempts);
                Console.WriteLine("Please enter your guess as a 4 digit number, with each number being between 1 and 6 (ex. 3621): ");
                userInput = Console.ReadLine();

                isValid = ValidateUserInput(userInput);
            }    

            return userInput;
        }

        private bool ValidateUserInput(string userInput)
        {
            //Check that the user input is only 4 characters in length
            if (userInput.Length != 4)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Quess: Quess must be 4 digits in length");
                Console.WriteLine();
                return false;
            }

            //Check that each character of the user input is a number between 1 and 6
            foreach (char digit in userInput)
            {
                if (int.TryParse(digit.ToString(), out int result))
                {
                    if (result < 1 || result > 6)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid Guess: Each digit must be between 1 and 6");
                        Console.WriteLine();
                        return false;
                    }
                } 
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Quess: Quess must be made up of only numbers");
                    Console.WriteLine();
                    return false;
                }
            }

            //User entered a valid quess that we can proceed with
            return true;
        }

        private void CheckValidQuess(string validQuess, string answerString)
        {
            string correctDigitCorrectPostion = "";
            string correctDigitWrongPostion = "";

            //First check if the user correctly guessed the whole answer
            if (validQuess == answerString)
            {
                isGuessCorrect = true;
                return;
            }

            //Create a temp answer list that we can manipulate without affecting the answer
            List<int> tempAnswerList = new List<int>();
            tempAnswerList.AddRange(answerList);

            //Create a temp quess list that we can compare and manipulate
            List<int> tempQuessList = new List<int>();
            for (int i = 0; i < validQuess.Length; i++)
            {
                tempQuessList.Add(int.Parse(validQuess[i].ToString()));
            }


            //First check for full correctness
            for (int i = 0; i < tempQuessList.Count; i++)
            {
                if (tempQuessList[i] == tempAnswerList[i])
                {
                    correctDigitCorrectPostion += "+";

                    //Clear out the digit in the temp answer array so it is not used again
                    //Also clear the digit in the temp quess list so we do not consider it again
                    tempAnswerList[i] = 0;
                    tempQuessList[i] = 0;
                }
            }

            //Now check for partialy correct
            for (int i = 0; i < tempQuessList.Count; i++)
            {
                //Only consider quess digits that have not already been found fully correct
                if (tempQuessList[i] != 0 && tempAnswerList.Contains(tempQuessList[i]))
                {
                    correctDigitWrongPostion += "-";

                    //Clear out the matched digit from the temp answer array so it is not used again
                    tempAnswerList[tempAnswerList.IndexOf(tempQuessList[i])] = 0;
                }
            }

            //Print out the results of check the user quess
            Console.WriteLine();
            Console.WriteLine("Hints: [" + correctDigitCorrectPostion + correctDigitWrongPostion + "]");
            Console.WriteLine();
        }
    }
}
