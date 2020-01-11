using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace HangManTwo
{
    class Program
    {
        static List<char> lettersTried = new List<char>(); //List for letters that have been previously tried
        static int guessCount; //Count of guesses so far
        static bool wordGuessed = false; //Bool used to stop the while loop that keeps guess prompting
        static int attempts = 6; //Numbers of lives

        public static void Main()
        {
            //Welcome message and choose 1p or 2p
            WriteLine("Welcome to Hangman!");
            WriteLine("Would you like to play Single-Player or 2-Player? (1 for Single/2 for 2-Player)");
            char gameMode = ReadKey().KeyChar;
            WriteLine($"{Environment.NewLine}");

            if (gameMode == '1') //Single player gives you a word from a selected category
            {
                //Categories and randomization
                WriteLine("Please choose your category:" +
                    "\n1) Fruit" +
                    "\n2) Pizza Toppings");
                char category = ReadKey().KeyChar;
                string secretWord = Word(category);
               // int attempts = secretWord.Length - 2; //# of lives 

                StringBuilder hidWord = BuildBoard(Word(category));
                WhichGuess(secretWord, hidWord);
                while (guessCount < (secretWord.Length + 1) && attempts > 1 && wordGuessed == false)
                {
                    GuessAgain(secretWord, hidWord);
                }
            }
            else if (gameMode == '2')
            {
                WriteLine("Enter the secret word");
                string secretWord = ReadLine(); //the secret word
                Clear();
                StringBuilder hidWord = BuildBoard(secretWord);

                WhichGuess(secretWord, hidWord);
                while (guessCount < secretWord.Length && attempts > 1 && wordGuessed == false)
                {
                    GuessAgain(secretWord, hidWord);
                }
            }
            
        }
        public static string Word(char category)
        {
            string secretWord;

            string[] fruit = { "apple", "bananna", "orange", "pear","kiwi"};
            string[] pizzaToppings = { "pineapple", "pepperoni", "mushroom", "spinach", "sausage" };
            
            Random word = new Random();
            if (category == '1')
            {
                int n = word.Next(0, fruit.Length);
                secretWord = fruit[n];

                return secretWord;
            }
            else
            {
                int n = word.Next(0, pizzaToppings.Length);
                secretWord = pizzaToppings[n];

                return secretWord;
            }
        }
       
        public static void GuessAgain(string secretWord, StringBuilder hidWord)
        {
            WriteLine("Would you like to guess a letter again? (Y/N)");
            string goAgain = ReadLine().ToLower();
            if (goAgain == "y")
                Guess(secretWord, hidWord);

            else if (goAgain == "n")
                Guess(secretWord);          
        }

        public static StringBuilder BuildBoard(string secretWord)
        {
            StringBuilder hidWord = new StringBuilder(secretWord); //Builds new string to be replaced, figoure out later how to add a space to do "_ "

            for (int i = 0; i < secretWord.Length; i++) //for loop that assigns each index _
            {
                hidWord[i] = '_';
            }

            
         
            WriteLine($"The secret word is:{hidWord}");

            WriteLine("Console has been cleared so your word is now secret" +
                $"\n The secret word is:{hidWord}" +
                $"\n You start out with {attempts-1} attempts" +
                $"\n You can guess the entire word or just a letter at a time" +
                $"\n ");

            WriteLine($"The word is {secretWord.Length} letters long");

            return hidWord;
        }

        public static void Guess(string secretWord,StringBuilder hidWord) 
        {
            WriteLine($"\nGuess a letter:");
            char guessLetter = ReadKey().KeyChar;
            WriteLine(" ");
            bool found = false;

            int k;
            for (k = 0; k < secretWord.Length; k++)
            {
                if (guessLetter == secretWord[k])
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                WriteLine($"Your guess {guessLetter} was incorrect!");
                --attempts;
            }
            else
            {
                WriteLine($"Your guess {guessLetter} was correct!");
                for(int i = k; i< secretWord.Length; i++)
                {
                    if (guessLetter == secretWord[i])
                        hidWord[i] = guessLetter;
                }
                hidWord[k] = guessLetter;
            }
            if (attempts == 1)
            {
                WriteLine("Game over!");
            }
            else
            {
                guessCount++;
                AddLetterUsed(guessLetter);
                StatusUpdate();
                CurrentWord(hidWord, secretWord);
            }
        }
        public static void StatusUpdate()
        {
            WriteLine($"You currently have {attempts-1} attempts left" +
                $"\n You have used these letters");
            OutLettersUsed();
        } 
        public static void Guess(string secretWord)
        {
            WriteLine("Please enter your guess for the word:");
            string guess = ReadLine();
            if (guess == secretWord)
            {
                WriteLine($"Congrats you were correct! You finished the game with {attempts} attempts left! Great job!");
                wordGuessed = true;
            }
            else
            {
                WriteLine($"I'm sorry, you are incorrect!");
                //StatusUpdate();
            }
        }

        public static void AddLetterUsed(char guess)
        {
            lettersTried.Add(guess);
        }
        public static void OutLettersUsed()
        {
            lettersTried.ForEach(el => Write(el));
            WriteLine($"{Environment.NewLine}");

        }
        public static void WhichGuess(string secretWord, StringBuilder hidWord)
        {
            WriteLine("Press W to guess the whole word or L to guess a letter");
            string choice = ReadLine().ToLower();
            Write($"{Environment.NewLine}");
            if (choice == "w")
            {
                Guess(secretWord);
            }
            else if (choice == "l")
            {

                Guess(secretWord, hidWord);
            }
            else
                WriteLine("Your input was incorrect.");
        }
        public static void CurrentWord(StringBuilder hidWord, string secretWord)
        {
            WriteLine("The word is now:");
            for(int i = 0; i < hidWord.Length; i++)
            {
                if (hidWord.ToString() == secretWord)
                {
                    WriteLine("Congrats! You got the complete word!");
                    i = hidWord.Length - i;
                    wordGuessed = true;
                }
                Write($"{hidWord[i]}");
                
            }    
            WriteLine($"{Environment.NewLine}");
        }
    }
}
