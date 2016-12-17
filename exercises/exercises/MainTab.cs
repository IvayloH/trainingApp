using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace exercisesProject
{
    
    public partial class MainTab : Form
    {
        string[] exercises = new string[50]; // array will hold all of the exercises from the file(fileLines)
        int duration, rest, durationCopy, restCopy;
        public string[] fileLines = System.IO.File.ReadAllLines(@"Exercises.txt");
        int currLine = 3;
        int repetitions;
        
        public MainTab()
        {
            InitializeComponent();
            //File.SetAttributes(@"Exercises.txt", FileAttributes.ReadOnly);
            //File.SetAttributes(@"Exercises.txt",FileAttributes.Hidden);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Image=exercisesProject.Properties.Resources.stop;
            updateComboBox();
        }

        private void updateComboBox()
        {
            fileLines = File.ReadAllLines(@"Exercises.txt");
            comboBox1.Items.Clear();
            richTextBox1.Clear(); 
            for (int i = 0; i < exercises.Length; i++) // clear exercises array
                exercises[i] = "";

            bool firstLoopWorkout = true; // first time it loops for each workout
            int ex = 0;
            foreach (string line in fileLines)
            {
                if (line.Equals("--") || line.Equals("!"))
                {
                    comboBox1.Items.Add("Workout " + (ex + 1).ToString());
                    ex++;
                    firstLoopWorkout = true;
                    continue;
                }
                if (firstLoopWorkout)
                {
                    exercises[ex] += line;
                    firstLoopWorkout = false;
                }
                else
                {
                    if (line.StartsWith("Duration:")) exercises[ex] += "\n";
                    exercises[ex] += "\n" + line;
                }
            }
            comboBox1.Items.Add("Stopwatch");   
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Enabled = true;
            if (stopwatchTimer.Enabled) stopwatchTimer.Enabled = false;
            if (workoutTimer.Enabled) workoutTimer.Stop();
            if (!(comboBox1.SelectedIndex == comboBox1.Items.Count - 1))
            {
                richTextBox1.Visible = true;
                timerTxtLabel.Text = "Timer:";
                richTextBox1.Text = exercises[comboBox1.SelectedIndex];
                duration = int.Parse(richTextBox1.Lines[0].Substring(richTextBox1.Lines[0].Length - 3, 2));
                rest = int.Parse(richTextBox1.Lines[1].Substring(richTextBox1.Lines[1].Length - 3, 2));
                repetitions = int.Parse(richTextBox1.Lines[2].Substring(richTextBox1.Lines[2].Length - 1));
                durationCopy = duration;
                restCopy = rest;
                var timespan = TimeSpan.FromSeconds(durationCopy);
                timerLabel.Text = timespan.ToString(@"mm\:ss");
                exerLabel.Text = richTextBox1.Lines[currLine];
            }
            else
            {
                richTextBox1.Visible = false;
                timerTxtLabel.Text = "Stopwatch";
                exerLabel.Text = "";
                timerLabel.Text = "0.0";
            }
        }

        private void workoutTimer_Tick(object sender, EventArgs e)
        {
            updateTimerText();
        }

        private void updateTimerText()
        {
            if (durationCopy != 0)
            {
                durationCopy--;
                timerLabel.Text = TimeSpan.FromSeconds(durationCopy).ToString(@"mm\:ss");
            }
            else
            {
                timerLabel.Text = TimeSpan.FromSeconds(restCopy).ToString(@"mm\:ss");
                restCopy--;
                exerLabel.Text = "REST";
            }

            if (restCopy == -1)
            {
                currLine++;
                if (currLine == richTextBox1.Lines.Length)
                {
                    repetitions--;
                    if (repetitions > 0)
                    {
                        currLine = 3;
                        restCopy = 60;
                    }
                    else
                    {
                        workoutTimer.Stop();
                        timerLabel.Text = "00:00";
                        exerLabel.Text = "Well Done!";
                    }
                }
                else
                {
                    if (richTextBox1.Lines[currLine].Equals("")) currLine++;
                    checkForMultipleDurations();
                    exerLabel.Text = richTextBox1.Lines[currLine];
                    durationCopy = duration+1; // 1 second already passes before it gets displayed
                    restCopy = rest;
                }
            }
        }

        private void checkForMultipleDurations()
        {
            if (richTextBox1.Lines[currLine].StartsWith("Duration:"))
            {
                duration = int.Parse(richTextBox1.Lines[currLine].Substring(richTextBox1.Lines[currLine].Length - 3, 2));
                currLine++;
                rest = int.Parse(richTextBox1.Lines[currLine].Substring(richTextBox1.Lines[currLine].Length - 3, 2));
                currLine++;
                repetitions = int.Parse(richTextBox1.Lines[currLine].Substring(richTextBox1.Lines[currLine].Length - 1));
                currLine++;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newWorkoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Add a new Workout 
            addWorkout workout = new addWorkout();
            workout.ShowDialog();
            updateComboBox();
        }

        private void MainTab_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void stopwatchTimer_Tick(object sender, EventArgs e)
        {
            timerLabel.Text = (double.Parse(timerLabel.Text) + 0.01).ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e) //start/pause
        {
            pictureBox2.Enabled = true;
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                if (stopwatchTimer.Enabled)
                    stopwatchTimer.Stop();
                else stopwatchTimer.Start();
            }
            else
                if (workoutTimer.Enabled) workoutTimer.Stop();
                else
                {
                    workoutTimer.Start();
                }
        }

        private void pictureBox2_Click(object sender, EventArgs e) //stop
        {
            if (stopwatchTimer.Enabled)
            {
                stopwatchTimer.Stop();
                timerLabel.Text = "0.0";
            }
            else
            {
                workoutTimer.Stop();
                int a = comboBox1.SelectedIndex;
                updateComboBox();
                comboBox1.SelectedIndex = a;
            }
        }
    }
}