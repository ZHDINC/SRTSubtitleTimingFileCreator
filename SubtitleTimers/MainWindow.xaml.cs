using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace SubtitleTimers
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeSpan elapsed;
        TimeSpan elapsed2;
        String elapsedString;
        DateTime endTime;
        Timer timer;
        DateTime startingTime;
        StringBuilder total = new StringBuilder();
        bool clickedTime = false;
        bool clickedTimeSecond = false;
        int iteration = 1;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    IterationNumber.Text = iteration.ToString();
                    CurrentTime.Text = elapsedString;
                    elapsed2 = DateTime.Now - startingTime;
                    TotalTimeElapsed.Text = elapsed2.ToString();
                });
            } catch(TaskCanceledException ex)
            {
                // Catching this since on a millisecond timer interval it will likely be in the middle of running when program is closed.
            }
        }

        private void TimeClick(object sender, RoutedEventArgs e)
        {
            endTime = DateTime.Now;
            elapsed = endTime - startingTime;
            if (!clickedTime)
            {
                elapsedString = elapsed.ToString(@"hh\:mm\:ss\,fff");
                clickedTime = true;
            } else
            {
                elapsedString += " --> ";
                elapsedString += elapsed.ToString(@"hh\:mm\:ss\,fff");
                clickedTime = false;
                total.AppendLine(iteration.ToString());
                total.AppendLine(elapsedString);
                iteration++;
            }
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            timer = new Timer(1);
            timer.Elapsed += TimerElapsed;
            timer.Start();
            startingTime = DateTime.Now;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Subtitle file (*.srt) | *.srt";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, total.ToString());
            }
        }
    }
}