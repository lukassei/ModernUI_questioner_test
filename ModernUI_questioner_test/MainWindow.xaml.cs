using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;

namespace ModernUI_questioner_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private HDDWorker hdd;
        public MainWindow()
        {
            InitializeComponent();
            hdd = new HDDWorker(this);
            HideAllZones();
            actualZone_border_stats.Visibility = Visibility.Visible;
            DataContext = this;
        }
        public void HideAllZones()
        {
            actualZone_border_questionsSelect.Visibility = Visibility.Hidden;
            actualZone_border_selectDificulty.Visibility = Visibility.Hidden;
            actualZone_border_stats.Visibility = Visibility.Hidden;
            actualZone_border_Test.Visibility = Visibility.Hidden;
        }
        private void MainMenu_exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void TestSelectionButtonCreate(string _qSet)
        {
            Button b = new Button();
            b.Content = _qSet;
            b.Style = (Style)FindResource("ButtonFlatStyle");
            b.Height = 50;
            b.Width = 130;
            b.FontSize = 16;
            b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC8CBD3"));
            b.Click += (s, e) =>
            {
                //here comes the code which is going to start test.. 
                hdd.SelectedTestPath = @"QuestionSets\" + _qSet + ".txt";
                hdd.LoadAllQuestions();
                HideAllZones();
                DifficultySelectionButtonCreate();
                actualZone_border_selectDificulty.Visibility = Visibility.Visible;
            };
            b.ClickMode = ClickMode.Press;
            b.Cursor = Cursors.Hand;
            actualZone_wrap_allquestions.Children.Add(b);
        }
        private void DifficultySelectionButtonCreate()
        {
            hdd.LoadDifficultyOptions();

            foreach(string st in hdd.Difficulties)
            {
                Button b = new Button();
                b.Content = st.Split(':')[0] + " - " + st.Split(':')[1] + " Otázek";
                b.Style = (Style)FindResource("ButtonFlatStyle");
                b.Height = 50;
                b.Width = 170;
                b.FontSize = 16;
                b.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC8CBD3"));
                b.Click += (s, e) =>
                {
                    //here comes the code which is going to start test.. 
                    hdd.SelectedDiff = st;
                    HideAllZones();
                    //here comes code which loads questions to test grid
                    hdd.ChooseQuestionsForTest();
                    hdd.StartTest();
                    actualZone_border_Test.Visibility = Visibility.Visible;
                };
                b.ClickMode = ClickMode.Press;
                b.Cursor = Cursors.Hand;
                actualZone_wrap_dificulties.Children.Add(b);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void TestStart()
        {
            
        }
        private void Test_ans1_Click(object sender, RoutedEventArgs e)
        {
            hdd.AnswerQuestion(1);
        }

        private void Test_ans2_Click(object sender, RoutedEventArgs e)
        {
            hdd.AnswerQuestion(2);
        }

        private void Test_ans3_Click(object sender, RoutedEventArgs e)
        {
            hdd.AnswerQuestion(3);
        }

        private void MainMenu_collectedTest_button_Click(object sender, RoutedEventArgs e)
        {
            actualZone_wrap_dificulties.Children.Clear();
            HideAllZones();
            hdd.isTestCollected = true;
            actualZone_border_questionsSelect.Visibility = Visibility.Visible;
        }

        private void MainMenu_practiseTest_button_Click(object sender, RoutedEventArgs e)
        {
            actualZone_wrap_dificulties.Children.Clear();
            HideAllZones();
            hdd.isTestCollected = false;
            actualZone_border_questionsSelect.Visibility = Visibility.Visible;
        }

        private void MainMenu_stats_button_Click(object sender, RoutedEventArgs e)
        {
            HideAllZones();
            actualZone_border_stats.Visibility = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
