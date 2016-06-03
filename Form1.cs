using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        byte[] clockFace;
        List<byte[]> history;
        private List<AllowedMove> moves ;


        byte? oldIndex = null;
        byte? newIndex = null;

        public Form1()
        {
            InitializeComponent();
            Restart();
            
        }

        private void RenderClockFace()
        {
            textBox0.Text = clockFace[0].ToString();
            textBox1.Text = clockFace[1].ToString();
            textBox2.Text = clockFace[2].ToString();
            textBox3.Text = clockFace[3].ToString();
            textBox4.Text = clockFace[4].ToString();
            textBox5.Text = clockFace[5].ToString();
            textBox6.Text = clockFace[6].ToString();
            textBox7.Text = clockFace[7].ToString();
            textBox8.Text = clockFace[8].ToString();
            textBox9.Text = clockFace[9].ToString();
            textBox10.Text = clockFace[10].ToString();
            textBox11.Text = clockFace[11].ToString();
            textBox0.BackColor = oldIndex == 0 ? Color.Yellow : newIndex == 0 ? Color.Green : Color.White;
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = oldIndex == 3 ? Color.Yellow : newIndex == 3 ? Color.Green : Color.White;
            textBox4.BackColor = Color.White;
            textBox5.BackColor = Color.White;
            textBox6.BackColor = oldIndex == 6 ? Color.Yellow : newIndex == 6 ? Color.Green : Color.White;
            textBox7.BackColor = Color.White;
            textBox8.BackColor = Color.White;
            textBox9.BackColor = oldIndex == 9 ? Color.Yellow : newIndex == 9 ? Color.Green : Color.White;
            textBox10.BackColor = Color.White;
            textBox11.BackColor = Color.White;


            txtOldIndex.Text = oldIndex == null ? "---" : oldIndex.ToString();
            txtOldIndex.BackColor = oldIndex == null ? Color.White : Color.Yellow;
            txtNewIndex.Text = newIndex == null ? "---" : newIndex.ToString();
            txtNewIndex.BackColor = newIndex == null ? Color.White : Color.Green;
        }

        private void SetIndex(byte index)
        {
            if (oldIndex == null)
                oldIndex = index;
            else if (oldIndex != index && oldIndex.HasValue)
            {
                newIndex = index;
                AllowedMove am = new AllowedMove(oldIndex.Value, newIndex.Value);
                if (!PerformMove(am, chkForward.Checked))
                    MessageBox.Show("You are going in circles. Your move was cancelled.");

                newIndex = null;
                oldIndex = null;
            }
            RenderClockFace();
        }

        public bool PerformMove(AllowedMove am, bool forward)
        {
            byte[] originalClockFace = (byte[])clockFace.Clone();

            clockFace.ShiftElement(am, forward);
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
            clockFace = new byte[] { 1, 12, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            history = new List<byte[]>() { (byte[])clockFace.Clone() };
            moves = new List<AllowedMove>();
            RenderClockFace();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Restart();
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
