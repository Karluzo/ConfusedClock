using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        byte[] clockFace;
        List<byte[]> history;
        private List<AllowedMove> moves ;


        int? oldIndex = null;
        int? newIndex = null;

        public Form1()
        {
            InitializeComponent();
            Restart();            
        }

        private string DStr(byte d)
        {
            return d == 0 ? "" : d.ToString();
        }


        private void RenderClockFace()
        {
            textBox0.Text = DStr(clockFace[0]);
            textBox1.Text = DStr(clockFace[1]);
            textBox2.Text = DStr(clockFace[2]);
            textBox3.Text = DStr(clockFace[3]);
            textBox4.Text = DStr(clockFace[4]);
            textBox5.Text = DStr(clockFace[5]);
            textBox6.Text = DStr(clockFace[6]);
            textBox7.Text = DStr(clockFace[7]);
            textBox8.Text = DStr(clockFace[8]);
            textBox9.Text = DStr(clockFace[9]);
            textBox10.Text = DStr(clockFace[10]);
            textBox11.Text = DStr(clockFace[11]);
            textBoxC.Text = DStr(clockFace[12]);
            textBox0.BackColor = oldIndex == 0 ? Color.Yellow : newIndex == 0 ? Color.LawnGreen: Color.LightBlue;
            textBox1.BackColor = oldIndex == 1 ? Color.Yellow : newIndex == 1 ? Color.LawnGreen : Color.White;
            textBox2.BackColor = oldIndex == 2 ? Color.Yellow : newIndex == 2 ? Color.LawnGreen : Color.White;
            textBox3.BackColor = oldIndex == 3 ? Color.Yellow : newIndex == 3 ? Color.LawnGreen : Color.LightBlue;
            textBox4.BackColor = oldIndex == 4 ? Color.Yellow : newIndex == 4 ? Color.LawnGreen : Color.White; ;
            textBox5.BackColor = oldIndex == 5 ? Color.Yellow : newIndex == 5 ? Color.LawnGreen : Color.White;
            textBox6.BackColor = oldIndex == 6 ? Color.Yellow : newIndex == 6 ? Color.LawnGreen : Color.LightBlue;
            textBox7.BackColor = oldIndex == 7 ? Color.Yellow : newIndex == 7 ? Color.LawnGreen : Color.White; ;
            textBox8.BackColor = oldIndex == 8 ? Color.Yellow : newIndex == 8 ? Color.LawnGreen : Color.White; 
            textBox9.BackColor = oldIndex == 9 ? Color.Yellow : newIndex == 9 ? Color.LawnGreen : Color.LightBlue;
            textBox10.BackColor = oldIndex == 10 ? Color.Yellow : newIndex == 10 ? Color.LawnGreen : Color.White;
            textBox11.BackColor = oldIndex == 11 ? Color.Yellow : newIndex == 11 ? Color.LawnGreen : Color.White;
            textBoxC.BackColor = oldIndex == 12 ? Color.Yellow : newIndex == 12 ? Color.LawnGreen : Color.White;
        }


        private void PlaySimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();
        }

        private void SetIndex(byte index)
        {
            if (clockFace[index] == 0) { PlaySimpleSound();  return; }
            oldIndex = index;
            newIndex = Array.IndexOf(clockFace, (byte)0);
            if (newIndex >= 0 && (oldIndex.Value == 12 || newIndex.Value == 12 || Math.Abs(newIndex.Value - oldIndex.Value) == 1) || (newIndex.Value == 11 && oldIndex == 0) || (newIndex.Value == 0 && oldIndex == 11))
            {
                RenderClockFace();
                Application.DoEvents();
                AllowedMove am = new AllowedMove(oldIndex.Value, newIndex.Value);
                if (!PerformMove(am))
                    MessageBox.Show("You are going in circles. Your move was cancelled.");
                else
                    Thread.Sleep(200);
            }
            else PlaySimpleSound();
            newIndex = null;
            oldIndex = null;
            
            RenderClockFace();
            
        }

        public bool PerformMove(AllowedMove am)
        {
            byte[] originalClockFace = (byte[])clockFace.Clone();

            byte tmp = clockFace[am.NewIndex];
            clockFace[am.NewIndex] = clockFace[am.OldIndex];
            clockFace[am.OldIndex] = tmp;

            int index = history.FindIndex(l => Enumerable.SequenceEqual(clockFace, l));
            if (index >= 0)
            {
                clockFace = originalClockFace;
                return false;
            }
            else
            {
                moves.Add(am);
                history.Add((byte[])clockFace.Clone());
                return true;
            }
        }

        public bool PerformUndo()
        {
            if (moves.Count == 0) return false;
            history.Remove(history.Last());
            moves.Remove(moves.Last());
            clockFace = (byte[])history.Last().Clone() ;
            newIndex = null;
            oldIndex = null;
            return true;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void textBox0_Click(object sender, EventArgs e)
        {
            SetIndex(0);
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            SetIndex(3);
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            SetIndex(6);
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            SetIndex(9);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!PerformUndo()) MessageBox.Show("You are at the beginning");
            else RenderClockFace();
        }

        public void Restart()
        {
            clockFace = new byte[] { 1, 12, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0 };
            history = new List<byte[]>() { (byte[])clockFace.Clone() };
            moves = new List<AllowedMove>();
            RenderClockFace();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Restart();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            SetIndex(1);
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            SetIndex(2);

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            SetIndex(4);

        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            SetIndex(5);

        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            SetIndex(7);

        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            SetIndex(8);

        }

        private void textBox10_Click(object sender, EventArgs e)
        {
            SetIndex(10);

        }

        private void textBox11_Click(object sender, EventArgs e)
        {
            SetIndex(11);

        }

        private void textBoxC_Click(object sender, EventArgs e)
        {
            SetIndex(12);

        }
        
        
    }


    public class AllowedMove
    {
        public int OldIndex;
        public int NewIndex;
        public int Distance
        {
            get { return Math.Abs(NewIndex - OldIndex); }
        }
        public AllowedMove(int oldIndex, int newIndex)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;

        }
    }


    public static class Tools
    {


        /// <summary>
        /// http://stackoverflow.com/questions/7242909/moving-elements-in-array-c-sharp
        /// Modified clockface edition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        public static void ShiftElement<T>(this T[] array, AllowedMove index, bool forward)
        {
            int oldIndex = index.OldIndex;
            int newIndex = index.NewIndex;
            // TODO: Argument validation
            if (oldIndex == newIndex)
            {
                return; // No-op
            }
            T tmp = array[oldIndex];

            if (forward)
            {
                if (newIndex < oldIndex)
                {
                    // Need to move part of the array "up" to make room
                    Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
                }
                else
                {
                    //clockwise movement
                    T last = array[array.Length - 1];
                    Array.Copy(array, newIndex, array, newIndex + 1, array.Length - newIndex - 1);
                    if (oldIndex > 0) Array.Copy(array, 0, array, 1, oldIndex);
                    array[0] = last;

                    //Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
                }
            }
            else
            {
                //TODO: dodělat
                if (newIndex < oldIndex)
                {
                    //Counterclockwise movement
                    T first = array[0];
                    Array.Copy(array, oldIndex + 1, array, oldIndex, array.Length - oldIndex - 1);
                    array[array.Length - 1] = first;
                    if (newIndex > 0) Array.Copy(array, 1, array, 0, newIndex);
                    //Array.Copy(array, newIndex, array, newIndex + 1, oldIndex - newIndex);
                }
                else
                {
                    // Need to move part of the array "up" to make room
                    Array.Copy(array, oldIndex + 1, array, oldIndex, newIndex - oldIndex);
                }

            }
            array[newIndex] = tmp;
        }
    }


}
