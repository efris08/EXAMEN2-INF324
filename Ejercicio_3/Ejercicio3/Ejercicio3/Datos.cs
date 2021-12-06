using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio3
{
    public partial class Datos : Form
    {
        public Datos()
        {
            InitializeComponent();
        }
        public int CR { get; set; }//RGB del color seleccionado
        public int CG { get; set; }//RGB del color seleccionado
        public int CB { get; set; }//RGB del color seleccionado
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            c = bmp.GetPixel(e.X, e.Y);
            CR = c.R;
            CG = c.G;
            CB = c.B;
            txtR.Text = CR.ToString();
            txtG.Text = CG.ToString();
            txtB.Text = CB.ToString();
            pictureBox3.BackColor = Color.FromArgb(int.Parse(txtR.Text), int.Parse(txtG.Text), int.Parse(txtB.Text));

        }
        private void txtR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox3.BackColor = Color.FromArgb(int.Parse(txtR.Text), int.Parse(txtG.Text), int.Parse(txtB.Text));
            }
            catch { }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Examen E = Owner as Examen;
            E.Recibir(CR, CG, CB);
            this.Hide();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
