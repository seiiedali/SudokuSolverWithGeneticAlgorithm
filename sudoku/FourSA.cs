using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class FourSA
    {
        //attributes
        public int[,] firstState;
        public int[,] answer;
        public int tryCounter;
        public long timePeriod;
        private int[,] table;
        private int[,] neighbor;
        private int[] numberPool;
        private static Random rnd;
        private int currentScore;
        private int nextScore;
        private bool solved;
        private const int length = 4;
        private int trapCounter;
        //constructor
        public FourSA()
        {
            rnd = new Random();
            table = new int[length, length];
            neighbor = new int[length, length];
            firstState = new int[length, length];
            answer = new int[length, length];
            numberPool = new int[] { length, length, length, length};
            solved = false;
            tryCounter = 0;
        }
        //sudoku solver
        public void getTheAnswer()
        {
            Console.WriteLine("working on the solution of 4*4 sudoku table(SA) ... \n");
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (!solved)
            {
                Console.WriteLine("\n*******************\nNEW ITERATION\n*******************\n");
                trapCounter = 0;
                solve();
                if (!solved)
                {
                    Console.WriteLine("failed lets try again...");
                    Console.WriteLine("the final state is");
                    sudokuPrint(table);
                    Console.WriteLine("final score is: " + currentScore);
                    Console.WriteLine();
                    //press any key to start another iteration
                    Console.ReadKey();
                }

            }

            Console.WriteLine("\n*******************\nFOUND SOLUTION\n*******************\nTHE Answer is:\n");
            sudokuPrint(answer);
            Console.WriteLine("\n\nIt took " + tryCounter + " try");
            watch.Stop();
            timePeriod = watch.ElapsedMilliseconds;
            Console.WriteLine("ended in " + timePeriod + " ms");
        }

        private void solve()
        {
            initializeTable();
            Console.WriteLine("Starting state is : \n");
            sudokuPrint(firstState);
            //calculate first state score
            arrayCopy(ref table, ref neighbor);
            currentScore = scoreCalculator();
            //Console.WriteLine("starting score is: " + currentScore);
            //count neighbor with higher or lower score
            int good = 0;
            int noGood = 0;
            double neighborPosibality = 0;
            double chance = 0;

            while (trapCounter < 5000000)
            {
                trapCounter++;
                tryCounter++;
                //if it is an answer
                if (currentScore == 0)
                {
                    arrayCopy(ref table, ref answer);
                    solved = true;
                    break;
                }
                createNeighbor();
                nextScore = scoreCalculator();
                //if neighbor has better score
                if (nextScore <= currentScore)
                {
                    arrayCopy(ref neighbor, ref table);
                    currentScore = nextScore;
                    trapCounter++;
                    good++;
                }
                //if neighbor has lower score
                else
                {
                    chance = rnd.NextDouble();
                    neighborPosibality = Math.Pow(Math.E, -0.2 * ((double)(Math.Abs((1 / (double)nextScore + 1) - (1 / (double)currentScore + 1)) * trapCounter)));
                    if (neighborPosibality >= chance)
                    {
                        arrayCopy(ref neighbor, ref table);
                        currentScore = nextScore;
                        trapCounter++;
                        noGood++;
                    }
                }

            }
            //Console.WriteLine("this iteration took " + tryCounter + " try");
            //Console.WriteLine("there was: "+ good + " neighbors with better score and " + noGood + " with lower score");

        }
        private void initializeTable()
        {
            numberPool = new int[length] { length, length, length, length };
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    table[i, j] = getNumberOfPool() + 1;
                }
            }
            arrayCopy(ref table, ref firstState);
        }
        public int getNumberOfPool()
        {
            int[] poolCheck = new int[length];
            int index = 0;
            for (int i = 0; i < length; i++)
            {
                if (numberPool[i] != 0)
                {
                    poolCheck[index] = i;
                    index++;
                }
            }
            int[] availibleNums = new int[index];
            for (int i = 0; i < index; i++)
            {
                availibleNums[i] = poolCheck[i];
            }
            int position = rnd.Next(availibleNums.Length);
            int result = availibleNums[position];
            numberPool[availibleNums[position]]--;
            return result;
        }
        // to create a possible neighbor  
        private void createNeighbor2()
        {
            int temp = 0;
            int randomRow, randomColumn;
            arrayCopy(ref table, ref neighbor);
            for (int i = 0; i < 1; i++)
            {
                randomRow = rnd.Next(length);
                randomColumn = rnd.Next(length);
                for (int j = length - 1; j > 0; j--)
                {
                    if (j == length - 1)
                        temp = neighbor[randomRow, j];
                    neighbor[randomRow, j] = neighbor[randomRow, j - 1];
                }
                neighbor[randomRow, 0] = temp;
                for (int j = length - 1; j > 0; j--)
                {
                    if (j == length - 1)
                        temp = neighbor[j, randomColumn];
                    neighbor[j, randomColumn] = neighbor[j - 1, randomColumn];
                }
                neighbor[0, randomColumn] = temp;
            }
        }
        private void createNeighbor()
        {
            int temp, firstRow, firstColumn, secondRow, secondColumn;
            arrayCopy(ref table, ref neighbor);
            for (int i = 0; i < 1; i++)
            {
                firstRow = rnd.Next(length);
                firstColumn = rnd.Next(length);
                secondRow = rnd.Next(length);
                secondColumn = rnd.Next(length);
                temp = neighbor[firstRow, firstColumn];
                neighbor[firstRow, firstColumn] = neighbor[secondRow, secondColumn];
                neighbor[secondRow, secondColumn] = temp;
            }
        }
        private int scoreCalculator()
        {
            int scoreCounter = 0;
            //check row and column for duplication
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = j + 1; k < 4; k++)
                    {
                        //check row
                        if (neighbor[i, j] == neighbor[i, k])
                            scoreCounter++;
                        //check column
                        if (neighbor[j, i] == neighbor[k, i])
                            scoreCounter++;
                    }
                }
            }
            //get each region
            int[] tempCheck = new int[length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    tempCheck[j] = neighbor[(i / 2) * 2 + j / 2, i * 2 % 4 + j % 2];

                }
                //check each region for duplication
                for (int k = 0; k < length; k++)
                {
                    for (int l = k + 1; l < length; l++)
                    {
                        if (tempCheck[k] == tempCheck[l])
                            scoreCounter++;
                    }
                }
            }
            return scoreCounter;
        }

        private void arrayCopy(ref int[,] src, ref int[,] dst)
        {
            dst = new int[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    dst[i, j] = src[i, j];
                }
            }
        }
        private void sudokuPrint(int[,] matrix)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

}
