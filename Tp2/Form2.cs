using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tp2
{
    public partial class Form2 : Form
    {
        
        private int _articleId;
        private Action _refreshDataGridView;


        public Form2(int articleId, Action refreshDataGridView)
        {
            InitializeComponent();
            _articleId = articleId;
            _refreshDataGridView = refreshDataGridView;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadArticleDetails();
        }
        private void LoadArticleDetails()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder
            {
                DataSource = "(local)",
                InitialCatalog = "Boutique",
                UserID = "sa",
                Password = "sysadm"
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(cs.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Articles WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", _articleId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["Code"].ToString();
                        textBox2.Text = reader["Name"].ToString();
                        textBox3.Text = reader["Description"].ToString();
                        textBox4.Text = reader["Brand"].ToString();
                        textBox6.Text = reader["Category"].ToString();
                        textBox5.Text = reader["Price"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erreur SQL : " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //enregistrer les modifications de l'article
            Article updatedArticle = new Article
            {
                Id = _articleId,
                Code = textBox1.Text,
                Name = textBox2.Text,
                Description = textBox3.Text,
                Brand = textBox4.Text,
                Category = textBox6.Text,
                Price = decimal.Parse(textBox5.Text)
            };

            try
            {
                //mettre a jour dans la base de donnees
                UpdateArticle(updatedArticle);
                MessageBox.Show("Article modifié avec succès.");
                _refreshDataGridView();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification de l'article : " + ex.Message);
            }
        }
        private void UpdateArticle(Article article)
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder
            {
                DataSource = "(local)",
                InitialCatalog = "Boutique",
                UserID = "sa",
                Password = "sysadm"
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(cs.ConnectionString))
                {
                    string query = "UPDATE Articles SET Code = @Code, Name = @Name, Description = @Description, Brand = @Brand, Category = @Category, Price = @Price WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", article.Id);
                    cmd.Parameters.AddWithValue("@Code", article.Code);
                    cmd.Parameters.AddWithValue("@Name", article.Name);
                    cmd.Parameters.AddWithValue("@Description", article.Description);
                    cmd.Parameters.AddWithValue("@Brand", article.Brand);
                    cmd.Parameters.AddWithValue("@Category", article.Category);
                    cmd.Parameters.AddWithValue("@Price", article.Price);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erreur SQL : " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
