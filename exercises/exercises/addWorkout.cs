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
    public partial class addWorkout : Form
    {
        public addWorkout()
        {
            InitializeComponent();
        }
        private void addWorkoutInfo()
        {
            string[] exerciseLines = System.IO.File.ReadAllLines(@"Exercises.txt");
            //using(FileStream fs = new FileStream("Exercises.txt",FileMode.Open))
            System.IO.File.WriteAllLines(@"Exercises.txt", exerciseLines.Take(exerciseLines.Length - 1)); // remove last line -> "!"

            StreamWriter writer = new StreamWriter(@"Exercises.txt",true);
            writer.WriteLine("--");
            writer.WriteLine("Duration: " + textBox1.Text + "s");
            writer.WriteLine("Rest: " + textBox2.Text + "s");
            writer.WriteLine("Repetitions: " + textBox3.Text);
            foreach (string line in richTextBox1.Lines)
                writer.WriteLine(line);

            if (checkBox1.Checked)
            {
                //writer.WriteLine("\n");
                writer.WriteLine("Duration: " + textBox4.Text + "s");
                writer.WriteLine("Rest: " + textBox5.Text + "s");
                writer.WriteLine("Repetitions: " + textBox6.Text);
                foreach (string line in richTextBox2.Lines)
                    writer.WriteLine(line);
            }

            if (checkBox2.Checked)
            {
                //writer.WriteLine("\n");
                writer.WriteLine("Duration: " + textBox7.Text + "s");
                writer.WriteLine("Rest: " + textBox8.Text + "s");
                writer.WriteLine("Repetitions: " + textBox9.Text);
                foreach (string line in richTextBox3.Lines)
                    writer.WriteLine(line);
            }

            writer.Write("!");
            writer.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                addWorkoutInfo();
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control obj in groupBox1.Controls)
            {
                if (obj.Enabled) obj.Enabled = false;
                else obj.Enabled = true;
            }
            if (checkBox2.Enabled) checkBox2.Enabled = false;
            else checkBox2.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control obj in groupBox2.Controls)
            {
                if (obj.Enabled) obj.Enabled = false;
                else obj.Enabled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar==(char) 8) e.Handled = false;
            else e.Handled = true;
        }

        private bool checkInput()
        {
            foreach (Control ctrl in groupBox3.Controls)
            {
                if (ctrl.Text.Length == 0)
                {
                    MessageBox.Show("Please enter all the information.");
                    return false;
                }
                if (ctrl is RichTextBox) 
                    if (!checkForEmptyLines(ctrl)) return false;
            }

            if (checkBox1.Checked)
            {
                foreach (Control ctrl in groupBox1.Controls)
                {
                    if (ctrl.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter all the information for the second round.");
                        return false;
                    }

                    if (ctrl is RichTextBox)
                        if (!checkForEmptyLines(ctrl)) return false;
                }
            }

            if (checkBox2.Checked)
            {
                foreach (Control ctrl in groupBox2.Controls)
                {
                    if (ctrl.Text.Length == 0)
                    {
                        MessageBox.Show("Please enter all the information for the third round.");
                        return false;
                    }
                    if (ctrl is RichTextBox) 
                        if (!checkForEmptyLines(ctrl)) return false;
                }
            }

            return true;
        }

        private bool checkForEmptyLines(Control ctrl)
        {
           foreach (string line in ((RichTextBox)ctrl).Lines)
               if (line.Equals(""))
               {
                   MessageBox.Show("Please do not leave any empty lines in thes exercises section.");
                   return false;
               }
           return true;
        }
    }
}
