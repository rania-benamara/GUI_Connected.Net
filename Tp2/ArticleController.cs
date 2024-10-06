using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace Tp2
{
    public class ArticleController
    {
        private List<Article> articles;
        private IArticleView view;

        public ArticleController(IArticleView view)
        {
            this.view = view;
            LoadArticles();
        }
        //methode pour obtenir un article par ID
        public Article GetArticleById(int id)
        {
            //recherchez  par ID
            return articles.FirstOrDefault(article => article.Id == id);
        }

        public SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder
            {
                DataSource = "(local)",
                InitialCatalog = "Boutique",
                UserID = "sa",
                Password = "sysadm"
            };
            return new SqlConnection(cs.ConnectionString);
        }

        public void LoadArticles()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Articles", conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    articles = new List<Article>();

                    while (reader.Read())
                    {
                        articles.Add(new Article
                        {
                            Id = (int)reader["Id"],
                            Code = reader["Code"].ToString(),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Category = reader["Category"].ToString(),
                            Price = (decimal)reader["Price"]
                        });
                    }
                }
                //affiche les articles recuperer 
                view.DisplayArticles(articles);
            }
            catch (SqlException ex)
            {
                //gerer les erreurs de connexion SQL
                Console.WriteLine("Erreur SQL : " + ex.Message);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
        public List<Article> GetArticles()
        {
            //retourner la liste des articles existants
            return articles;
        }

        public void AddArticle(Article article)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    string query = "INSERT INTO Articles (Id,Code, Name, Description, Brand, Category, Price) VALUES (@Id, @Code, @Name, @Description, @Brand, @Category, @Price)";
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

                //recharger les articles pour mettre a jour 
                LoadArticles();
            }
            catch (SqlException ex)
            {
                //gerer les erreurs de connexion SQL
                Console.WriteLine("Erreur SQL : " + ex.Message);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erreur : " + ex.Message);
            }

        }




        public void DeleteArticle(int articleId)
        {
          

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    string query = "DELETE FROM Articles WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", articleId);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadArticles(); //recharger les articles apres suppression
            }
            catch (SqlException ex)
            {
                throw new Exception("Erreur SQL lors de la suppression de l'article : " + ex.Message);
            }
        }
        //la methode UpdateArticle
        public void UpdateArticle(Article article)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
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

                //recharger les articles apres l mettere a jour
                LoadArticles();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erreur SQL lors de la mise à jour de l'article : " + ex.Message);
            }
        }

    }
}
