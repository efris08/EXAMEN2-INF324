using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio3
{
    public partial class Examen : Form
    {
        private MySqlConnection cnx = null;
        MySqlCommand cmd = null;
        DataTable dt = null;
        Conexion C = null;
        int cR, cG, cB;//RGB que capta el mouse en el PictureBox de la imagen
        int RR, GG, BB;//RGB de la Base de Datos
        int meR, meG, meB;//RGB el Promedio de un segmento de pixeles
        int r2, g2, b2;//RGB del nuevo color que se pondrá
        public Examen()
        {
            InitializeComponent();
            C = new Conexion();
            cnx = C.Cnx();
            Listar();
        }

        #region BOTONES
        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                byte [] color1= photo(pb1, cR, cG, cB);
                byte [] color2=photo(pb2, r2, g2, b2);
                Guardar(txtName.Text, int.Parse(txtR.Text), int.Parse(txtG.Text), int.Parse(txtB.Text),color1 ,color2, r2, g2, b2);
                Listar();
            }
            else
                MessageBox.Show("Debe escribir un nombre para el color");
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Datos D = new Datos();
            AddOwnedForm(D);
            D.ShowDialog();

        }
        private void btnDetect_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            for (int i = 0; i < bmp.Width - 10; i += 10)
                for (int j = 0; j < bmp.Height - 10; j += 10)
                {
                    meR = 0;
                    meG = 0;
                    meB = 0;

                    for (int k = i; k < i + 10; k++)
                        for (int l = j; l < j + 10; l++)
                        {
                            c = bmp.GetPixel(k, l);
                            meR = meR + c.R;
                            meG = meG + c.G;
                            meB = meB + c.B;
                        }
                    meR = meR / 100;
                    meG = meG / 100;
                    meB = meB / 100;
                    bool sw = false;
                    for (int ii = 0; ii < dt.Rows.Count; ii++)
                    {
                        RR = int.Parse(dt.Rows[ii][1].ToString());
                        GG = int.Parse(dt.Rows[ii][2].ToString());
                        BB = int.Parse(dt.Rows[ii][3].ToString());
                        int RR2 = int.Parse(dt.Rows[ii][6].ToString());
                        int GG2 = int.Parse(dt.Rows[ii][7].ToString());
                        int BB2 = int.Parse(dt.Rows[ii][8].ToString());
                        if (((RR - 10) < meR) && (meR < (RR + 10)) &&
                        ((GG - 10) < meG) && (meG < (GG + 10)) &&
                        ((BB - 10) < meB) && (meB < (BB + 10)))
                            for (int k = i; k < i + 10; k++)
                                for (int l = j; l < j + 10; l++)
                                {
                                    bmp2.SetPixel(k, l, Color.FromArgb(RR2, GG2, BB2));
                                    sw = true;
                                }

                    }
                    if (sw == false)
                    {
                        for (int k = i; k < i + 10; k++)
                            for (int l = j; l < j + 10; l++)
                            {
                                c = bmp.GetPixel(k, l);
                                bmp2.SetPixel(k, l, c);

                            }
                    }
                }
            pictureBox2.Image = bmp2;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Borrar();
            Listar();
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int x, y, mR = 0, mG = 0, mB = 0;
            x = e.X; y = e.Y;
            for (int i = x; i < x + 10; i++)
                for (int j = y; j < y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    mR = mR + c.R;
                    mG = mG + c.G;
                    mB = mB + c.B;
                }
            mR = mR / 100;
            mG = mG / 100;
            mB = mB / 100;
            cR = mR;
            cG = mG;
            cB = mB;
            txtR.Text = cR.ToString();
            txtG.Text = cG.ToString();
            txtB.Text = cB.ToString();

            photo(pb1, cR, cG, cB);

        }
        #endregion

        #region FUNCIONES
        public void Borrar()
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "DELETE FROM segmentos";
                cnx.Open();
                MySqlDataReader lector = cmd.ExecuteReader();
                MessageBox.Show("Se eliminaron los registros.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { cnx.Close(); }
        }
        public void Guardar(string des, int r, int g, int b, byte[] f, byte[] f2, int r2, int g2, int b2)
        {
            try
            {

                MemoryStream ms = new MemoryStream();
                pb1.Image.Save(ms, ImageFormat.Png);
                byte[] pic_arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(pic_arr, 0, pic_arr.Length);

                cmd = new MySqlCommand();
                cmd.Connection = cnx;
                cmd.CommandText = "INSERT INTO segmentos(descripcion,rColor,gColor,bColor,color1,color2,r2,g2,b2 )  VALUES(@des,@r,@g,@b,@f,@f2,@r2,@g2,@b2 )";
                cmd.Parameters.Add(new MySqlParameter("@des", des));
                cmd.Parameters.Add(new MySqlParameter("@r", r));
                cmd.Parameters.Add(new MySqlParameter("@g", g));
                cmd.Parameters.Add(new MySqlParameter("@b", b));
                cmd.Parameters.Add(new MySqlParameter("@f", f));
                cmd.Parameters.Add(new MySqlParameter("@f2", f2));
                cmd.Parameters.Add(new MySqlParameter("@r2", r2));
                cmd.Parameters.Add(new MySqlParameter("@g2", g2));
                cmd.Parameters.Add(new MySqlParameter("@b2", b2));
                cnx.Open();
                MySqlDataReader lector = cmd.ExecuteReader();
                MessageBox.Show("Se guardó correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { cnx.Close(); }
        }
        private void Listar()
        {
            dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = "SELECT descripcion AS DESCRIPCION, rColor AS 'VALOR R', gColor AS 'VALOR G',bColor AS 'VALOR B',color1 AS 'COLOR ACTUAL',color2 AS 'COLOR QUE REEMPLAZARÁ',r2,g2,b2 FROM segmentos ORDER BY Id DESC"; ;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private byte[] photo(PictureBox pb,int r, int g, int b)
        {

            Bitmap color_rgb = new Bitmap(pb.Width, pb.Height);
            for (int i = 1; i < pb.Width; i++)
                for (int j = 1; j < pb.Height; j++)
                {
                    color_rgb.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            pb.Image = color_rgb;

            byte[] aByte;
            MemoryStream ms = new MemoryStream();
            if (pb.Image != null)
            {
                pb.Image.Save(ms, ImageFormat.Jpeg);
                aByte = ms.ToArray();
                return aByte;
            }
            else
                return null;
        }
        public void Recibir(int r, int g, int b)
        {
            r2 = r; g2 = g; b2 = b;
            photo(pb2, r2, g2, b2);
        }
        #endregion
    }
}

      
    
        

