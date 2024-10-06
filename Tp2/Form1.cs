using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tp2;

namespace Tp2
{
    public partial class Form1 : Form, IArticleView
    {
        private ArticleController _articleController;
       

        public Form1()
        {
            InitializeComponent();
           
            _articleController = new ArticleController(this);
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            RefreshDataGridView(); //charger les articles 
        }
        public void RefreshDataGridView()
        {
            _articleController.LoadArticles();
        }
        private void EditArticle(int articleId)
        {
            Form2 form2 = new Form2(articleId, RefreshDataGridView);
            form2.ShowDialog();
        }
        public void DisplayArticles(List<Article> articles)
        {
            //lier la liste des articles au DataGridView
            dataGridView1.DataSource = articles;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //recuperer la ligne selecionner
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                //recupere les valeurs des colonnes
                string articleDetails = $"ID: {selectedRow.Cells["Id"].Value}\n" +
                                        $"Code: {selectedRow.Cells["Code"].Value}\n" +
                                        $"Nom: {selectedRow.Cells["Name"].Value}\n" +
                                        $"Description: {selectedRow.Cells["Description"].Value}\n" +
                                        $"Marque: {selectedRow.Cells["Brand"].Value}\n" +
                                        $"Catégorie: {selectedRow.Cells["Category"].Value}\n" +
                                        $"Prix: {selectedRow.Cells["Price"].Value}";

                //affichage les détails dans le RichTextBox
                richTextBox1.Text = articleDetails;
            }
            else
            {
                //si aucune ligne na ete selectionner,effacer le contenu du RichTextBox
                richTextBox1.Clear();
            }
        }




        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //verefier que les champs ne sont pas vides
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            //creation d'un nouvel artcile
            Article newArticle = new Article
            {
                Id = int.Parse(textBox1.Text),
                Code = textBox2.Text,
                Name = textBox3.Text,
                Description = textBox4.Text,
                Brand = textBox5.Text,
                Category = textBox7.Text,
                Price = decimal.Parse(textBox6.Text) 
            };

            //ajouter l'article a la base de donnees et mettre a jour le DataGridView
            try
            {
                _articleController.AddArticle(newArticle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'article : " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérez l'ID de l'article sélectionné
                int selectedArticleId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                // Ouvrez Form2 pour modifier l'article sélectionné
                Form2 form2 = new Form2(selectedArticleId, RefreshDataGridView);

                form2.ShowDialog();

                // Après la fermeture de Form2, rechargez les articles
                try
                {
                    List<Article> articles = _articleController.GetArticles(); 
                    DisplayArticles(articles);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur  lors du rechargement des articles : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un article à modifier.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //recuperer l'id
                int selectedArticleId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                //confirmer la suppression
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet article ?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _articleController.DeleteArticle(selectedArticleId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression de l'article : " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un article à supprimer.");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Récupérez l'index
                int currentIndex = dataGridView1.SelectedRows[0].Index;

                //assurer que ce n'est pas la derniere ligne
                if (currentIndex < dataGridView1.Rows.Count - 1)
                {
                    //selectioner la prochiane ligne
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentIndex + 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Vous êtes déjà sur la dernière ligne.");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //recuperer l'index 
                int currentIndex = dataGridView1.SelectedRows[0].Index;

                //assurer que ce n'est pas la premiere ligne
                if (currentIndex > 0)
                {
                    //selectionner la ligne precidente
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[currentIndex - 1].Selected = true;
                }
                else
                {
                    MessageBox.Show("Vous êtes déjà sur la première ligne.");
                }
            }

        }
    }
}
