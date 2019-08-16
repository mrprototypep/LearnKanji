using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace LearnKanji
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Locaton of folder containing all the terms.json files to load
        /// </summary>
        public static readonly string _appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "pSuite");

        static List<Term> masterList = null;
        static List<Term> instanceList = null;

        static Term currentTerm = null;

        /// <summary>
        /// Imports term lists from _appPath/TermLists and makes them available for selection
        /// </summary>
        private void PopulateListBox()
        {
            if (!Directory.Exists(_appPath))
                Directory.CreateDirectory(_appPath);
            if (!Directory.Exists(Path.Combine(_appPath, "TermLists")))
                Directory.CreateDirectory(Path.Combine(_appPath, "TermLists"));

            foreach (string path in Directory.GetFiles(Path.Combine(_appPath, "TermLists"))) {
                TermLists.Items.Add(new TermList(path));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            PopulateListBox();
        }

        void NextTerm()
        {
            if (instanceList.Count == 0)
                instanceList.AddRange(masterList.ToArray<Term>());

            Random rand = new Random();
            currentTerm = instanceList[rand.Next(0, instanceList.Count)];
            instanceList.Remove(currentTerm);

            if (QTerm.IsSelected)
                QuestionBox.Text = currentTerm.TermDefinition;
            else if (QAnswer.IsSelected)
                QuestionBox.Text = currentTerm.AnswerDefinition;
            else
                QuestionBox.Text = currentTerm.HiraganaDefinition;

            if (ATerm.IsSelected)
                HintBox.Text = currentTerm.TermDefinition;
            else if (AAnswer.IsSelected)
                HintBox.Text = currentTerm.AnswerDefinition;
            else
                HintBox.Text = currentTerm.HiraganaDefinition;
        }

        void AnswerCorrect()
        {
            ScoreBox.Text = (int.Parse(ScoreBox.Text) + 10).ToString();
            NextTerm();
        }

        void AnswerIncorrect()
        {
            ScoreBox.Text = (int.Parse(ScoreBox.Text) -3).ToString();
        }

        void EvalTermSucc()
        {
            if (ATerm.IsSelected)
            {
                if (AnswerBox.Text.Equals(currentTerm.TermDefinition))
                    AnswerCorrect();
                else
                    AnswerIncorrect();
            }
            else if (AAnswer.IsSelected)
            {
                if (AnswerBox.Text.Equals(currentTerm.AnswerDefinition))
                    AnswerCorrect();
                else
                    AnswerIncorrect();
            }
            else
            {
                if (AnswerBox.Text.Equals(currentTerm.HiraganaDefinition))
                    AnswerCorrect();
                else
                    AnswerIncorrect();
            }

            AnswerBox.Text = "";
        }

        private void SetsButton_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start("explorer.exe", Path.Combine(_appPath, "TermLists"));

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            masterList = new List<Term>();
            instanceList = new List<Term>();
            foreach(object list in TermLists.SelectedItems)
            {
                masterList.AddRange((list as TermList).Terms);
            }

            instanceList.AddRange(masterList.ToArray<Term>());
            NextTerm();
        }

        private void TermLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TermLists.SelectedItems.Count == 0)
                ReloadButton.IsEnabled = false;
            else
                ReloadButton.IsEnabled = true;
        }

        private void AnswerBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter)
                return;

            e.Handled = true;
            EvalTermSucc();
        }

        private void QuestionDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!QuestionBox.Text.Equals(""))
            {
                if (QTerm.IsSelected)
                    QuestionBox.Text = currentTerm.TermDefinition;
                else if (QAnswer.IsSelected)
                    QuestionBox.Text = currentTerm.AnswerDefinition;
                else
                    QuestionBox.Text = currentTerm.HiraganaDefinition;
            }
        }

        private void HintCheck_CheckChanged(object sender, RoutedEventArgs e)
        {
            if ((bool)HintCheck.IsChecked)
                HintBox.Visibility = Visibility.Visible;
            else
                HintBox.Visibility = Visibility.Collapsed;
        }

        private void AnswerDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ATerm.IsSelected)
                HintBox.Text = currentTerm.TermDefinition;
            else if (AAnswer.IsSelected)
                HintBox.Text = currentTerm.AnswerDefinition;
            else
                HintBox.Text = currentTerm.HiraganaDefinition;
        }
    }
}
