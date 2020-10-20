using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class NineGA {
        const int length = 9;
        static Random rnd = new Random ();
        private int[] numberPool;
        private bool solved;
        private int[] answer;
        private int tryCounter;
        public long timePeriod;
        List<chromosome> socity;
        List<chromosome> parent;
        class chromosome {
            public double fitnessValue;
            public double aggregationFittness;
            public int[] chrome;
            public chromosome () {
                chrome = new int[length * length];
                fitnessValue = 0;
                aggregationFittness = 0;
            }
            public chromosome (chromosome copy) {
                this.fitnessValue = copy.fitnessValue;
                this.chrome = new int[copy.chrome.Length];
                for (int i = 0; i < copy.chrome.Length; i++) {
                    this.chrome[i] = copy.chrome[i];
                }
                this.aggregationFittness = 0;
            }
        }

        public NineGA () {
            socity = new List<chromosome> ();
            parent = new List<chromosome> ();
            solved = false;
            tryCounter = 0;
            numberPool = new int[] { length, length, length, length, length, length, length, length, length };

        }
        public void getTheAnswer () {
            int trapCounter;
            Console.WriteLine ("working on the 9*9 sudoku solution using GA: ...");
            var watch = System.Diagnostics.Stopwatch.StartNew ();
            while (!solved) {
                if (tryCounter != 0) {
                    Console.WriteLine ("failed on the previous iteration.");
                    //printSocity();
                    //Console.WriteLine("with total fitness of: " + socity[socity.Count-1].aggregationFittness);
                    Console.WriteLine ("Press any key to start another iteration: ");
                    Console.ReadKey ();

                }
                Console.WriteLine ("\n*******************\nNEW ITERATION\n*******************\n");
                createSocity (20);
                Console.WriteLine ("Starting with this society: \n");
                printSocity ();
                //Console.WriteLine("with total fitness of: " + socity[socity.Count-1].aggregationFittness);
                trapCounter = 0;
                while (!checkForAnswer () && trapCounter < 100000) {
                    chooseParent (4);
                    crossOver (4);
                    mutation (2);
                    purgingSocity (20);
                    tryCounter++;
                    trapCounter++;
                }
            }
            Console.WriteLine ("\n*******************\nFOUND SOLUTION\n*******************\nTHE Answer is:\n");
            printTabe (answer);
            Console.WriteLine ("\n\nIt took " + tryCounter + " try");
            watch.Stop ();
            timePeriod = watch.ElapsedMilliseconds;
            Console.WriteLine ("ended in " + timePeriod + " ms");

        }
        private bool checkForAnswer () {
            for (int i = 0; i < socity.Count; i++) {
                if (socity[i].fitnessValue == 1) {
                    answer = socity[i].chrome;
                    solved = true;
                    return true;
                }
            }
            return false;
        }
        public void purgingSocity (int socityCount) {
            int itemToremove = socity.Count - socityCount;

            for (int i = 0; i < itemToremove; i++) {
                chromosome temp = (chromosome) (from x in socity where x.fitnessValue == socity.Min (chrome => chrome.fitnessValue) select x).First ();
                socity.Remove (temp);
            }
        }
        public void mutation (int mutationRate) {
            int randomChrome;
            int randomIndexOne;
            int randomIndexTwo;
            int temp;
            int mutationCount = (int) ((double) socity.Count * (double) mutationRate / 10);
            for (int i = 0; i < mutationCount; i++) {
                randomChrome = rnd.Next (socity.Count);
                randomIndexOne = rnd.Next (length * length);
                randomIndexTwo = rnd.Next (length * length);
                temp = socity[randomChrome].chrome[randomIndexOne];
                socity[randomChrome].chrome[randomIndexOne] = socity[randomChrome].chrome[randomIndexTwo];
                socity[randomChrome].chrome[randomIndexTwo] = temp;
                socity[randomChrome].fitnessValue = fittnessCalculate (socity[randomChrome].chrome);
            }
            socityAggregationFitness ();
        }
        private void crossOver (int childCount) {
            for (int j = 0; j < childCount; j++) {
                int randomIndexOne = rnd.Next (parent.Count);
                int randomIndexTwo = rnd.Next (parent.Count);
                chromosome tempOne = new chromosome ();
                chromosome tempTwo = new chromosome ();

                for (int i = 0; i < length * length; i++) {
                    if (i < (length * length / 2)) {
                        tempOne.chrome[i] = parent[randomIndexOne].chrome[i];
                        tempTwo.chrome[i] = parent[randomIndexTwo].chrome[i];
                    } else {
                        tempOne.chrome[i] = parent[randomIndexTwo].chrome[i];
                        tempTwo.chrome[i] = parent[randomIndexOne].chrome[i];

                    }
                }
                tempOne.fitnessValue = fittnessCalculate (tempOne.chrome);
                tempTwo.fitnessValue = fittnessCalculate (tempTwo.chrome);
                socity.Add (tempOne);
                socity.Add (tempTwo);
            }
            socityAggregationFitness ();
        }
        private void chooseParent (int parentCount) {
            parent = new List<chromosome> ();
            for (int i = 0; i < parentCount; i++) {
                double chance = rnd.NextDouble () * socity[socity.Count - 1].aggregationFittness;
                for (int j = 0; j < socity.Count; j++) {
                    if (chance <= socity[j].aggregationFittness) {
                        chromosome temp = new chromosome (socity[i]);
                        parent.Add (temp);
                        break;
                    }

                }
            }
        }
        private void createSocity (int socityPopulation) {

            for (int i = 0; i < socityPopulation; i++) {
                chromosome temp = new chromosome ();
                numberPool = new int[] { length, length, length, length, length, length, length, length, length };
                for (int j = 0; j < length * length; j++) {
                    temp.chrome[j] = getNumberOfPool () + 1;
                }
                temp.fitnessValue = fittnessCalculate (temp.chrome);
                socity.Add (temp);
            }
            socityAggregationFitness ();
        }
        public int getNumberOfPool () {
            int[] poolCheck = new int[length];
            int index = 0;
            for (int i = 0; i < length; i++) {
                if (numberPool[i] != 0) {
                    poolCheck[index] = i;
                    index++;
                }
            }
            int[] availibleNums = new int[index];
            for (int i = 0; i < index; i++) {
                availibleNums[i] = poolCheck[i];
            }
            int position = rnd.Next (availibleNums.Length);
            int result = availibleNums[position];
            numberPool[availibleNums[position]]--;
            return result;
        }

        private double fittnessCalculate (int[] chrome) {
            int counter = 0;
            int[, ] chromeMatrix = new int[length, length];
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < length; j++) {
                    chromeMatrix[i, j] = chrome[counter];
                    counter++;
                }
            }

            int scoreCounter = 0;
            //check row and column constraint
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < length; j++) {
                    for (int k = j + 1; k < length; k++) {
                        //check row
                        if (chromeMatrix[i, j] == chromeMatrix[i, k])
                            scoreCounter++;
                        //check column
                        if (chromeMatrix[j, i] == chromeMatrix[k, i])
                            scoreCounter++;
                    }
                }
            }
            //check regions constraint
            int[] tempCheck = new int[length];
            for (int i = 0; i < length; i++) {
                for (int j = 0; j < length; j++) {
                    tempCheck[j] = chromeMatrix[(i / 3) * 3 + j / 3, i * 3 % 9 + j % 3];

                }
                //check each region
                for (int k = 0; k < length; k++) {
                    for (int l = k + 1; l < length; l++) {
                        if (tempCheck[k] == tempCheck[l])
                            scoreCounter++;
                    }
                }
            }
            return (double) 1 / (scoreCounter + 1);
        }
        private void socityAggregationFitness () {
            double aggregatoinValue = 0;
            for (int i = 0; i < socity.Count; i++) {
                aggregatoinValue += socity[i].fitnessValue;
                socity[i].aggregationFittness = aggregatoinValue;
            }
        }
        private void printTabe (int[] array) {
            for (int i = 0; i < array.Length; i++) {
                if (i % 9 == 0)
                    Console.WriteLine ();
                Console.Write (array[i] + " ");
            }
        }
        public void printSocity () {
            for (int i = 0; i < socity.Count; i++) {
                for (int j = 0; j < length * length; j++) {
                    Console.Write (socity[i].chrome[j] + " ");

                }
                Console.WriteLine ();
                Console.WriteLine ();
            }
        }

    }
}