using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace _2nd_homework
{
    public partial class FallingBlockForm : Form
    {
        static  WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        // url: F:\\study\\4th year\\hurry\\3rd semester\\graphics\\practical\\second homework\\2nd homework\\
        static bool paused = false;
        public FallingBlockForm()
        {
            InitializeComponent();
            wplayer.URL = @"rah ensakee.m4a";
            for (int i = 1; i <= 6; i++)
            {
                comboBox1.Items.Add(i);
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if(!paused)
                wplayer.controls.play();
            int num = comboBox1.SelectedIndex +1 ;
            //the bigger the responcitivity the higher the game sense the keyboard strokes
            int resp = 3;
            int numOfCubes = 3;
            Console.WriteLine("the number of player is: "+ num);
            FallingBlocks tutorial = new FallingBlocks(num,resp,numOfCubes);
            int speed =int.Parse(speedTextBox.Text);
            tutorial.Run(resp*speed);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!paused)
            {
                wplayer.controls.pause();
                musicButton.Text = "turn on the voice";
                paused = true;
            }
            else
            {
                wplayer.controls.play();
                musicButton.Text = "mute the voice";
                paused = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
