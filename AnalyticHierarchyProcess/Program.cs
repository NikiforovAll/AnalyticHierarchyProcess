using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticHierarchyProcess
{
    class Program
   {
		public static void printResult(AnalyticHierarchyManager AHM)
		{
			Console.Clear ();
			printLogo ();
			try
			{
				Console.WriteLine ("Ваш выбор - {0}",AHM.OptionsList[AHM.Process()]);
			}
			catch(Exception e) {
				Console.WriteLine (e);
			}
		}
		public static void printLogo()
		{
			Console.WriteLine ("*******************");
			Console.WriteLine ("TV SELECTOR v0.001");
			Console.WriteLine ("*******************\n");
		}
		public static void printScale()
		{
			Console.WriteLine ("ШКАЛА ОТНОСИТЕЛЬНОЙ ВАЖНОСТИ");
			Console.WriteLine ("//////////////////////////////////////////////////////////////////////////");
			Console.WriteLine ("1-равнозначно\t3-умеренно\t5-существенно\t7-значительное\t9-очевидное");
			Console.WriteLine ("2,4,6,8 - промежуточные значения");
			Console.WriteLine ("//////////////////////////////////////////////////////////////////////////\n");
		}
		public static void fillCriteriasCompares(AnalyticHierarchyManager AHM)
		{
			for(int expert=0;expert<AHM.NumExpert;expert++)
			{
				AHM.PairComparisonList.Add (new PairComparison (AHM.NumCriteria));
				Console.Clear ();
				printLogo ();
				printScale ();
				Console.WriteLine ("Отвечает эксперт №{0}",expert+1);
				for (int i = 0; i < AHM.NumCriteria-1; i++) 
				{
					for (int j = i+1; j < AHM.NumCriteria; j++) 
					{
						Console.WriteLine ("Насколько {0} важнее, чем {1}?",AHM.CriteriaData[i],AHM.CriteriaData[j]);
						double value = Convert.ToDouble (Console.ReadLine ());
						AHM.PairComparisonList [expert].AddElement (i, j, value);
					}
				}
				Console.Clear ();
			}
		}
		public static void fillOptionsCompares(AnalyticHierarchyManager AHM)
		{
			for (int criteria = 0; criteria < AHM.NumCriteria;criteria++) 
			{
				AHM.OptionsList.Add (new System.Collections.Generic.List<PairComparison>());
				for (int expert = 0; expert < AHM.NumExpert; expert++) 
				{
					Console.Clear ();
					printLogo ();
					printScale ();
					AHM.OptionsList [criteria].Add (new PairComparison (AHM.NumOptions));
					Console.WriteLine ("Отвечает эксперт №{0}",expert+1);
					for (int i = 0; i < AHM.NumOptions-1; i++) 
					{
						for (int j = i+1; j < AHM.NumOptions; j++) 
						{
							Console.WriteLine ("Насколько {0} у {1} лучше, чем у {2}?",AHM.CriteriaData[criteria],AHM.OptionsData[i],AHM.OptionsData[j]);

							double value = Convert.ToDouble (Console.ReadLine ());
							AHM.OptionsList [criteria] [expert].AddElement (i, j, value);
						}
					}
				}
			}
		}



		public static void Main (string[] args)
		{
			int NumExperts = 3;
			int NumCriterias = 4;
			int NumOptions = 3;
			AnalyticHierarchyManager AHM = new AnalyticHierarchyManager ();
			AHM.NumExpert = NumExperts;
			AHM.NumCriteria = NumCriterias;
			//AHM.CriteriaData.Add ("Диагональ");
			//AHM.CriteriaData.Add ("Разрешение");
			//AHM.CriteriaData.Add ("Производитель");
			//AHM.CriteriaData.Add ("Цена");
			//test data
			AHM.CriteriaData.Add ("A");
			AHM.CriteriaData.Add ("B");
			AHM.CriteriaData.Add ("C");
			AHM.CriteriaData.Add ("D");
			AHM.NumOptions = NumOptions;
			AHM.OptionsData.Add ("Samsung");
			AHM.OptionsData.Add ("LG");
			AHM.OptionsData.Add ("Sony");
			fillCriteriasCompares (AHM);
			fillOptionsCompares (AHM);
			printResult (AHM);



		}
	}
}
