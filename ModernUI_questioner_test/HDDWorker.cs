using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ModernUI_questioner_test
{
    class HDDWorker
    {
        private List<string> QuestionSets { get; set; }
        public string SelectedTestPath { get; set; }
        public string SelectedDiff { get; set; }
        private int PercentageNeededToSucceed { get; set; }
        public bool isTestCollected { get; set; }
        private List<Question> QuestionsChoosenForTest { get; set; }
        private List<Question> Questions { get; set; }
        public List<string> Difficulties { get; set; }
        private MainWindow Mw { get; set; }
        private Question ActualQuestion { get; set; }
        public HDDWorker(MainWindow _mw)
        {
            QuestionSets = new List<string>();
            Difficulties = new List<string>();
            Questions = new List<Question>();
            QuestionsChoosenForTest = new List<Question>();
            Mw = _mw;
            SelectedTestPath = "";
            isTestCollected = false;
            CheckForExistingQuestionSets();
        }
        private void CheckForExistingQuestionSets()
        {
            QuestionSets.Clear();
            //Goes into folder QuestionSets and look for existing QuestionSets
            DirectoryInfo _questionFiles__directory = new DirectoryInfo(@"QuestionSets");
            FileInfo[] _questionFiles = _questionFiles__directory.GetFiles("*.txt");
            foreach (FileInfo _questionFile in _questionFiles)
            {
                string fileName = _questionFile.Name;
                int fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    fileName = fileName.Substring(0, fileExtPos);

                QuestionSets.Add(fileName);
            }
            //here goes method for creating the buttons inside select test wrapper
            CreateButtonsToSelectTest();
        }
        private void CreateButtonsToSelectTest()
        {
            foreach (string _qSet in QuestionSets)
            {
                Mw.TestSelectionButtonCreate(_qSet);
            }
        }
        public void LoadDifficultyOptions()
        {
            Difficulties.Clear();
            using (StreamReader sr = new StreamReader(SelectedTestPath))
            {
                string _questionLine;
                while ((_questionLine = sr.ReadLine()) != null)
                {
                    if(_questionLine.Split(';')[0] == "diff")
                    {
                        int n = 0;
                        foreach(string diff in _questionLine.Split(';'))
                        {
                            if(n > 1)
                                Difficulties.Add(diff);
                            n++;
                        }
                    }
                    else if(_questionLine.Split(';')[0] == "rightneeded")
                    {
                        PercentageNeededToSucceed = int.Parse(_questionLine.Split(';')[1]);
                    }
                }
            }
        }
        public void LoadAllQuestions()
        {
            Questions.Clear();

            using (StreamReader sr = new StreamReader(SelectedTestPath))
            {
                string _questionLine;
                while ((_questionLine = sr.ReadLine()) != null)
                {
                    if (_questionLine.Split(';')[0] != "diff" && _questionLine.Split(';')[0] != "rightneeded")
                    {
                        Question q = new Question(_questionLine.Split(';')[0], _questionLine.Split(';')[1], _questionLine.Split(';')[2], _questionLine.Split(';')[3], int.Parse(_questionLine.Split(';')[4]));
                        Questions.Add(q);
                    }
                }
            }

        }
        public void ChooseQuestionsForTest()
        {
            while (QuestionsChoosenForTest.Count < int.Parse(SelectedDiff.Split(':')[1]))
            {
                Random r = new Random();
                int _question = r.Next(Questions.Count);
                if (!QuestionsChoosenForTest.Contains(Questions[_question]))
                    QuestionsChoosenForTest.Add(Questions[_question]);
            }
        }

        //here comes the actual test system (teststart, nextquestion. evaluation, stats calculation etc..)
        int numberOfRuns = 0;
        public void StartTest()
        {
            numberOfRuns = 0;
            NextQuestion();
        }
        private void NextQuestion()
        {
            if(numberOfRuns < QuestionsChoosenForTest.Count)
            {
                ActualQuestion = QuestionsChoosenForTest[numberOfRuns];
                Mw.actualZone_Test_question.Text = ActualQuestion.QuestionTask;
                Mw.Test_ans1.Content = ActualQuestion.Answer1;
                Mw.Test_ans2.Content = ActualQuestion.Answer2;
                Mw.Test_ans3.Content = ActualQuestion.Answer3;
            }
            else
            {
                //evaluate

                //hide test part
                //check every question and after that add it as colorded button into stack panel in evuluation grid
                //if collected - write the result
                //show evaluation grid
                Mw.HideAllZones();
                foreach(Question q in QuestionsChoosenForTest)
                {
                    Button b = new Button();
                    b.Content = q;
                    b.Style = (Style)Application.Current.FindResource("ButtonFlatStyle");
                    b.Width = 130;
                    b.FontSize = 16;
                    if (q.Evaluate())
                        b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4B762D"));
                    else
                        b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF762D2D"));
                    b.Click += (s, e) =>
                    {
                        //This will open grid with right answer
                    };
                    b.ClickMode = ClickMode.Press;
                    b.Cursor = Cursors.Hand;
                }
                Mw.actualZone_border_evaluation.Visibility = Visibility.Visible;
            }
            numberOfRuns++;
        }
        public void AnswerQuestion(int _choosenAnswer)
        {
            ActualQuestion.ChoosenAnswer = _choosenAnswer;
            NextQuestion();
        }
        private void EvaluateTest()
        {
            foreach(Question q in QuestionsChoosenForTest) //This will 
            {
                q.Evaluate();
            }

            //podmínka, která podle druhu testu rozhedne o tom, zda bude výsledek uložen
            //bude se ukládat pouze procentuální úspěšnost a název testu + procenta potřebná pro úspěšné složení
        }























    }
}
