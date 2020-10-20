using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku {
    class Program {
        static void Main (string[] args) {
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            FourSimple test = new FourSimple ();
            test.getTheAnswer ();
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            //Since it can't be done in a reasonable time period
            //it will prevent other part of the program fro runnig.
            //TO TEST NineSimple JUST UNCOMMENT BELOW LINES

            //NineSimple test1 = new NineSimple();
            //test1.getTheAnswer();
            //Console.WriteLine("\n###############################################################################\n###############################################################################\n");

            FourSA test2 = new FourSA ();
            test2.getTheAnswer ();
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            NineSA test3 = new NineSA ();
            test3.getTheAnswer ();
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            FourGa test4 = new FourGa ();
            test4.getTheAnswer ();
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            NineGA test5 = new NineGA ();
            test5.getTheAnswer ();
            Console.WriteLine ("\n###############################################################################\n###############################################################################\n");

            Console.ReadKey ();
        }
    }
}