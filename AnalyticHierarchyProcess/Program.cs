using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyticHierarchyManager;


namespace AnalyticHierarchyProcess
{
    class Program
    {
        public static void PrintResult(AnalyticHierarchyManager.AnalyticHierarchyManager ahm)
        {
            Console.Clear();
            PrintLogo();
            try
            {
                Console.WriteLine($"Ваш выбор - {ahm.OptionsData[ahm.Process()]}"); //ahm.OptionsList[ahm.Process()]
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void PrintLogo()
        {
            Console.WriteLine("*******************");
            Console.WriteLine("TV SELECTOR v0.001");
            Console.WriteLine("*******************\n");
        }
        public static void PrintScale()
        {
            Console.WriteLine("ШКАЛА ОТНОСИТЕЛЬНОЙ ВАЖНОСТИ");
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("1-равнозначно\t3-умеренно\t5-существенно\t7-значительное\t9-очевидное");
            Console.WriteLine("2,4,6,8 - промежуточные значения");
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////\n");
        }
        public static void FillCriteriasCompares(AnalyticHierarchyManager.AnalyticHierarchyManager ahm)
        {
            for (int expert = 0; expert < ahm.NumExpert; expert++)
            {
                ahm.PairComparisonList.Add(new PairComparison(ahm.NumCriteria));
                Console.Clear();
                PrintLogo();
                PrintScale();
                Console.WriteLine("Отвечает эксперт №{0}", expert + 1);
                for (int i = 0; i < ahm.NumCriteria - 1; i++)
                {
                    for (int j = i + 1; j < ahm.NumCriteria; j++)
                    {
                        Console.WriteLine("Насколько {0} важнее, чем {1}?", ahm.CriteriaData[i], ahm.CriteriaData[j]);
                        double value = Convert.ToDouble(Console.ReadLine());
                        ahm.PairComparisonList[expert].AddElement(i, j, value);
                    }
                }
                Console.Clear();
            }
        }
        public static void FillOptionsCompares(AnalyticHierarchyManager.AnalyticHierarchyManager ahm)
        {
            for (int criteria = 0; criteria < ahm.NumCriteria; criteria++)
            {
                ahm.OptionsList.Add(new System.Collections.Generic.List<PairComparison>());
                for (int expert = 0; expert < ahm.NumExpert; expert++)
                {
                    Console.Clear();
                    PrintLogo();
                    PrintScale();
                    ahm.OptionsList[criteria].Add(new PairComparison(ahm.NumOptions));
                    Console.WriteLine("Отвечает эксперт №{0}", expert + 1);
                    for (int i = 0; i < ahm.NumOptions - 1; i++)
                    {
                        for (int j = i + 1; j < ahm.NumOptions; j++)
                        {
                            Console.WriteLine("Насколько {0} у {1} лучше, чем у {2}?", ahm.CriteriaData[criteria], ahm.OptionsData[i], ahm.OptionsData[j]);

                            double value = Convert.ToDouble(Console.ReadLine());
                            ahm.OptionsList[criteria][expert].AddElement(i, j, value);
                        }
                    }
                }
            }
        }



        public static void Main(string[] args)
        {
            int numExperts = 3;
            int numCriterias = 4;
            int numOptions = 3;
            AnalyticHierarchyManager.AnalyticHierarchyManager ahm = new AnalyticHierarchyManager.AnalyticHierarchyManager
            {
                NumExpert = numExperts,
                NumCriteria = numCriterias
            };
            ahm.CriteriaData.Add ("Диагональ");
            ahm.CriteriaData.Add("Разрешение");
            ahm.CriteriaData.Add("Производитель");
            ahm.CriteriaData.Add("Цена");
            //test data
            //ahm.CriteriaData.Add("A");
            //ahm.CriteriaData.Add("B");
            //ahm.CriteriaData.Add("C");
            //ahm.CriteriaData.Add("D");
            ahm.NumOptions = numOptions;
            ahm.OptionsData.Add("Samsung");
            ahm.OptionsData.Add("LG");
            ahm.OptionsData.Add("Sony");
            FillCriteriasCompares(ahm);
            FillOptionsCompares(ahm);
            PrintResult(ahm);



        }
    }

}
