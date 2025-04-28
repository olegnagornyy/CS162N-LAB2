using System;
using System.Collections.Generic;

namespace YahtzeeGame
{
    class Program
    {
        // Constants
        const int NONE = -1;
        const int ONES = 0, TWOS = 1, THREES = 2, FOURS = 3, FIVES = 4, SIXES = 5,
                  THREE_OF_A_KIND = 6, FOUR_OF_A_KIND = 7, FULL_HOUSE = 8,
                  SMALL_STRAIGHT = 9, LARGE_STRAIGHT = 10, CHANCE = 11, YAHTZEE = 12,
                  SUBTOTAL = 13, BONUS = 14, TOTAL = 15;

        static void Main(string[] args)
        {
            int[] userScorecard = new int[16];
            int[] computerScorecard = new int[16];
            int userScorecardCount = 0;
            int computerScorecardCount = 0;

            ResetScorecard(userScorecard, ref userScorecardCount);
            ResetScorecard(computerScorecard, ref computerScorecardCount);

            while (userScorecardCount < 13 || computerScorecardCount < 13)
            {
                // User Turn
                if (userScorecardCount < 13)
                {
                    DisplayScoreCards(userScorecard, computerScorecard);
                    Console.WriteLine("\nYour Turn!");
                    UserPlay(userScorecard, ref userScorecardCount);
                    UpdateScorecard(userScorecard);
                }

                // Computer Turn
                if (computerScorecardCount < 13)
                {
                    DisplayScoreCards(userScorecard, computerScorecard);
                    Console.WriteLine("\nComputer's Turn!");
                    ComputerPlay(computerScorecard, ref computerScorecardCount);
                    UpdateScorecard(computerScorecard);
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            // Final Scores
            DisplayScoreCards(userScorecard, computerScorecard);
            Console.WriteLine();

            if (userScorecard[TOTAL] > computerScorecard[TOTAL])
                Console.WriteLine("You win!");
            else if (userScorecard[TOTAL] < computerScorecard[TOTAL])
                Console.WriteLine("Computer wins!");
            else
                Console.WriteLine("It's a tie!");

            Console.WriteLine("Game Over. Press Enter to exit.");
            Console.ReadLine();
        }

        #region Game Management
        static void ResetScorecard(int[] scorecard, ref int scorecardCount)
        {
            for (int i = 0; i < scorecard.Length; i++)
                scorecard[i] = NONE;
            scorecardCount = 0;
        }

        static void UpdateScorecard(int[] scorecard)
        {
            scorecard[SUBTOTAL] = 0;
            scorecard[BONUS] = 0;
            for (int i = ONES; i <= SIXES; i++)
                if (scorecard[i] != NONE)
                    scorecard[SUBTOTAL] += scorecard[i];

            if (scorecard[SUBTOTAL] >= 63)
                scorecard[BONUS] = 35;

            scorecard[TOTAL] = scorecard[SUBTOTAL] + scorecard[BONUS];
            for (int i = THREE_OF_A_KIND; i <= YAHTZEE; i++)
                if (scorecard[i] != NONE)
                    scorecard[TOTAL] += scorecard[i];
        }

        static void DisplayScoreCards(int[] uScorecard, int[] cScorecard)
        {
            string[] labels = {"Ones", "Twos", "Threes", "Fours", "Fives", "Sixes",
                "3 of a Kind", "4 of a Kind", "Full House", "Small Straight",
                "Large Straight", "Chance", "Yahtzee", "Sub Total", "Bonus", "Total Score"};
            string lineFormat = "| {3,2} {0,-15}|{1,8}|{2,8}|";
            string border = new string('-', 39);

            Console.Clear();
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, "", "  You   ", "Computer", ""));
            Console.WriteLine(border);
            for (int i = ONES; i <= SIXES; i++)
                Console.WriteLine(String.Format(lineFormat, labels[i], FormatCell(uScorecard[i]), FormatCell(cScorecard[i]), i));
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[SUBTOTAL], FormatCell(uScorecard[SUBTOTAL]), FormatCell(cScorecard[SUBTOTAL]), ""));
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[BONUS], FormatCell(uScorecard[BONUS]), FormatCell(cScorecard[BONUS]), ""));
            Console.WriteLine(border);
            for (int i = THREE_OF_A_KIND; i <= YAHTZEE; i++)
                Console.WriteLine(String.Format(lineFormat, labels[i], FormatCell(uScorecard[i]), FormatCell(cScorecard[i]), i));
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[TOTAL], FormatCell(uScorecard[TOTAL]), FormatCell(cScorecard[TOTAL]), ""));
            Console.WriteLine(border);
        }
        #endregion

        #region Playing
        static void Roll(int numberOfDice, List<int> dice)
        {
            Random rand = new Random();
            dice.Clear();
            for (int i = 0; i < numberOfDice; i++)
                dice.Add(rand.Next(1, 7));
        }

        static void DisplayDice(List<int> dice)
        {
            string border = "*------*";
            string empty = "|      |";
            string format = "|   {0}  |";

            foreach (var die in dice)
                Console.Write(border);
            Console.WriteLine();

            foreach (var die in dice)
                Console.Write(empty);
            Console.WriteLine();

            foreach (var die in dice)
                Console.Write(string.Format(format, die));
            Console.WriteLine();

            foreach (var die in dice)
                Console.Write(empty);
            Console.WriteLine();

            foreach (var die in dice)
                Console.Write(border);
            Console.WriteLine();
        }

        static int GetComputerScorecardItem(int[] scorecard, List<int> keeping)
        {
            int bestCategory = 0;
            int bestScore = -1;

            for (int i = ONES; i <= YAHTZEE; i++)
            {
                if (scorecard[i] == NONE)
                {
                    int currentScore = Score(i, keeping);
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestCategory = i;
                    }
                }
            }

            return bestCategory;
        }

        static void ComputerPlay(int[] scorecard, ref int scorecardCount)
        {
            List<int> dice = new List<int>();
            Roll(5, dice);
            List<int> keeping = new List<int>(dice);

            int chosenCategory = GetComputerScorecardItem(scorecard, keeping);
            scorecard[chosenCategory] = Score(chosenCategory, keeping);
            scorecardCount++;
        }

        static void UserPlay(int[] scorecard, ref int scorecardCount)
        {
            List<int> dice = new List<int>();
            List<int> keeping = new List<int>();

            Roll(5, dice);
            keeping.AddRange(dice);

            DisplayDice(dice);

            int chosenCategory;
            do
            {
                Console.Write("Enter category (0-12) to score: ");
                bool validInput = int.TryParse(Console.ReadLine(), out chosenCategory);
                if (!validInput) chosenCategory = -1;
            } while (chosenCategory < 0 || chosenCategory > 12 || scorecard[chosenCategory] != NONE);

            scorecard[chosenCategory] = Score(chosenCategory, keeping);
            scorecardCount++;
        }
        #endregion

        #region Utilities
        static string FormatCell(int cell)
        {
            return cell == NONE ? "-" : cell.ToString();
        }

        static void Count(List<int> dice, int[] counts)
        {
            Array.Clear(counts, 0, counts.Length);
            foreach (int die in dice)
                counts[die - 1]++;
        }

        static bool GetCounts(List<int> dice, out int[] counts)
        {
            counts = new int[6];
            Count(dice, counts);
            return true;
        }

        static int Score(int category, List<int> dice)
        {
            int[] counts;
            GetCounts(dice, out counts);

            switch (category)
            {
                case ONES: return counts[0] * 1;
                case TWOS: return counts[1] * 2;
                case THREES: return counts[2] * 3;
                case FOURS: return counts[3] * 4;
                case FIVES: return counts[4] * 5;
                case SIXES: return counts[5] * 6;
                case THREE_OF_A_KIND:
                    for (int i = 0; i < 6; i++)
                        if (counts[i] >= 3)
                            return Sum(dice);
                    return 0;
                case FOUR_OF_A_KIND:
                    for (int i = 0; i < 6; i++)
                        if (counts[i] >= 4)
                            return Sum(dice);
                    return 0;
                case FULL_HOUSE:
                    bool two = false, three = false;
                    for (int i = 0; i < 6; i++)
                    {
                        if (counts[i] == 2) two = true;
                        if (counts[i] == 3) three = true;
                    }
                    return (two && three) ? 25 : 0;
                case SMALL_STRAIGHT:
                    return HasStraight(counts, 4) ? 30 : 0;
                case LARGE_STRAIGHT:
                    return HasStraight(counts, 5) ? 40 : 0;
                case CHANCE:
                    return Sum(dice);
                case YAHTZEE:
                    for (int i = 0; i < 6; i++)
                        if (counts[i] == 5)
                            return 50;
                    return 0;
                default: return 0;
            }
        }

        static int Sum(List<int> dice)
        {
            int sum = 0;
            foreach (var die in dice)
                sum += die;
            return sum;
        }

        static bool HasStraight(int[] counts, int length)
        {
            int inARow = 0;
            for (int i = 0; i < 6; i++)
            {
                if (counts[i] > 0)
                {
                    inARow++;
                    if (inARow >= length)
                        return true;
                }
                else
                    inARow = 0;
            }
            return false;
        }
        #endregion
    }
}

