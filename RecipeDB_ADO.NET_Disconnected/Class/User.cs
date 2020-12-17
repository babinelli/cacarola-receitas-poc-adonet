using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDB_ADO.NET_Disconnected
{
    internal class User
    {

        internal static void MenuUser()
        {
            string option;
            do
            {
                Console.Clear();
                Console.WriteLine("------------------------------\nRECIPE DB MANAGER - USER\n------------------------------");
                Console.Write("1 - Inserir usuário\n2 - Atualizar dados de um usuário\n3 - Listar usuários\n4 - Deletar usuário\n0 - SAIR\n\nQual operação deseja efetuar? ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        InsertUser();
                        break;
                    case "2":
                        UpdateUser();
                        break;
                    case "3":
                        SelectUsers();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        break;
                }

            } while (option != "0");
            Console.ReadKey();
        }

        internal static void InsertUser()
        {
            #region New user from console
            Console.Clear();
            Console.WriteLine("------------------------------\nINSERIR USUÁRIO\n------------------------------");

            Console.Write("\nNome: ");
            string name = Console.ReadLine();

            Console.Write("\nApelido: ");
            string lastName = Console.ReadLine();

            Console.Write("\nE-mail: ");
            string email = Console.ReadLine(); // Checar se não existe na DB e se obedece regex

            Console.Write("\nUsername: ");
            string username = Console.ReadLine(); // Checar se não existe na DB

            Console.Write("\nPalavra-passe: ");
            string password = Console.ReadLine(); // Checar se obedece a regex

            Console.Write("\nDepartamento ID: ");
            string department = Console.ReadLine();
            #endregion

            #region Support variables
            string querySelect = "SELECT * FROM [dbo].[User];"; // replicar em memória a tabela com a qual vou trabalhar
            string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();
            #endregion

            #region ConnectionString

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                #region DataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(querySelect, sqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                #endregion

                #region Open Connections
                sqlConnection.Open();
                #endregion

                #region DataSet
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "User");
                #endregion

                #region DataRow
                DataRow dataRow;
                dataRow = dataSet.Tables[0].NewRow();
                dataRow["Name"] = name;
                dataRow["LastName"] = lastName;
                dataRow["Email"] = email;
                dataRow["Username"] = username;
                dataRow["Password"] = password;
                dataRow["Admin"] = false;
                dataRow["DepartmentID"] = department;
                dataSet.Tables[0].Rows.Add(dataRow);

                #endregion

                #region SqlCommandBuilder + Update
                commandBuilder.GetDeleteCommand();
                commandBuilder.DataAdapter.Update(dataSet.Tables[0]);
                Console.WriteLine("\nUsuário inserido com sucesso.");
                Console.ReadKey();
                #endregion
            }

            #endregion

        }

        internal static void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("------------------------------\nATUALIZAR USUÁRIO\n------------------------------");

            string option;
            do
            {

                Console.Write("Qual o UserID do usuário que deseja atualizar? ");
                string userID = Console.ReadLine();

                string querySelectByUserID = "SELECT * FROM [dbo].[User] WHERE [UserID] = '" + userID + "';";
                string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();

                Console.Write("\nCampos para atualização\n1 - Nome\n2 - Apelido\n3 - E-mail\n4 - Username\n5 - Departamento\n0 - Voltar ao menu inicial\n\nQual campo deseja atualizar? ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("Novo nome: ");
                        string name = Console.ReadLine();
                        string queryUpdateName = "UPDATE [dbo].[User] SET [Name] = '" + name + "' WHERE [UserID] = '" + userID + "';";

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            #region ConnectionString
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                            dataAdapter.UpdateCommand = new SqlCommand(queryUpdateName, sqlConnection);
                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                            #endregion

                            #region Open Connection
                            sqlConnection.Open();
                            #endregion

                            #region MyRegion
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            #endregion

                            #region Update to Server
                            dataSet.Tables[0].Rows[0]["Name"] = "";
                            dataAdapter.Update(dataSet);
                            Console.WriteLine("Alteração realizada com sucesso.");
                            #endregion

                        }
                        return;

                    case "2":
                        Console.Write("Novo apelido: ");
                        string lastName = Console.ReadLine();
                        string queryUpdateLastName = "UPDATE [dbo].[User] SET [LastName] = '" + lastName + "' WHERE [UserID] = '" + userID + "';";

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            #region ConnectionString
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                            dataAdapter.UpdateCommand = new SqlCommand(queryUpdateLastName, sqlConnection);
                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                            #endregion

                            #region Open Connection
                            sqlConnection.Open();
                            #endregion

                            #region MyRegion
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            #endregion

                            #region Update to Server
                            dataSet.Tables[0].Rows[0]["LastName"] = "";
                            dataAdapter.Update(dataSet);
                            Console.WriteLine("Alteração realizada com sucesso.");
                            #endregion

                        }
                        return;

                    case "3":
                        Console.Write("\nNovo e-mail: ");
                        string email = Console.ReadLine();
                        string queryUpdateEmail = "UPDATE [dbo].[User] SET [Email] = '" + email + "' WHERE [UserID] = '" + userID + "';";

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            #region ConnectionString
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                            dataAdapter.UpdateCommand = new SqlCommand(queryUpdateEmail, sqlConnection);
                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                            #endregion

                            #region Open Connection
                            sqlConnection.Open();
                            #endregion

                            #region MyRegion
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            #endregion

                            #region Update to Server
                            dataSet.Tables[0].Rows[0]["Email"] = "";
                            dataAdapter.Update(dataSet);
                            Console.WriteLine("Alteração realizada com sucesso.");
                            #endregion

                        }
                        return;

                    case "4":
                        Console.Write("Novo username: ");
                        string username = Console.ReadLine();
                        string queryUpdateUsername = "UPDATE [dbo].[User] SET [Username] = '" + username + "' WHERE [UserID] = '" + userID + "';";

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            #region ConnectionString
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                            dataAdapter.UpdateCommand = new SqlCommand(queryUpdateUsername, sqlConnection);
                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                            #endregion

                            #region Open Connection
                            sqlConnection.Open();
                            #endregion

                            #region MyRegion
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            #endregion

                            #region Update to Server
                            dataSet.Tables[0].Rows[0]["Username"] = "";
                            dataAdapter.Update(dataSet);
                            Console.WriteLine("Alteração realizada com sucesso.");
                            #endregion

                        }
                        return;

                    case "5": // DepartmentID -  implementar
                        Console.Write("Novo departamento: ");
                        string department = Console.ReadLine();
                        //string queryUpdateDepartmentID = "UPDATE [dbo].[User] SET [DepartmentID] = '" + oldPassword + "' WHERE [UserID] = '" + userID + "';";

                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            #region ConnectionString
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                            //dataAdapter.UpdateCommand = new SqlCommand(queryUpdateDepartmentID, sqlConnection);
                            SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(dataAdapter);
                            #endregion

                            #region Open Connection
                            sqlConnection.Open();
                            #endregion

                            #region MyRegion
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            #endregion

                            #region Update to Server
                            dataSet.Tables[0].Rows[0]["DepartmentID"] = "";
                            dataAdapter.Update(dataSet);
                            Console.WriteLine("Alteração realizada com sucesso.");
                            #endregion

                        }
                        return;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
                Console.ReadKey();


            } while (option != "0");

        }

        internal static void SelectUsers()
        {
            #region Support variables
            List<string> userList = new List<string>();

            string querySelect = "SELECT * FROM [dbo].[User] ORDER BY [UserID];";
            string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();

            #endregion

            #region Connection String
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                #region DataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelect, sqlConnection);
                #endregion

                #region Open connection
                sqlConnection.Open();
                #endregion

                #region DataTable
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                #endregion

                #region DataTableReader
                DataTableReader dataTableReader = new DataTableReader(dataTable);
                #endregion

                #region Read data
                while (dataTableReader.Read())
                {
                    userList.Add(dataTableReader[0].ToString() + "\t| " + dataTableReader[1].ToString() + "\t| " + dataTableReader[2].ToString() + "\t| " + dataTableReader[3].ToString() + "\t| " + dataTableReader[4].ToString() + "\t| " + dataTableReader[7].ToString());

                }
                #endregion

                #region Show data
                Console.Clear();
                Console.WriteLine("------------------------------\nLISTAR USUÁRIOS\n------------------------------");
                Console.WriteLine("UserID\t| Nome\t| Apelido\t| E-mail\t| Username\t| DepartmentID ");
                foreach (string item in userList)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
                #endregion

            }

            #endregion
        }

        internal static void DeleteUser()
        {
            #region Delete user from console
            Console.Clear();
            Console.WriteLine("------------------------------\nDELETAR USUÁRIO\n------------------------------");

            Console.Write("Qual o UserID do usuário que deseja deletar? ");
            string userID = Console.ReadLine();
            #endregion

            #region Support variables
            string querySelectByUserID = "SELECT * FROM [dbo].[User];";
            string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();
            #endregion

            #region ConectionString
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                #region DataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByUserID, sqlConnection);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                #endregion

                #region Open connection
                sqlConnection.Open();
                #endregion

                #region DataSet
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                #endregion

                #region Delete rolls
                string filter = "[UserID] = '" + userID + "'";
                DataRow[] rows = dataSet.Tables[0].Select(filter);

                if (rows == null || rows.Length == 0) // Se vetor rows for nulo ou vazio
                {
                    Console.WriteLine("\nNão existe nenhum utilizador com esse ID.");
                }
                else
                {
                    foreach (DataRow row in rows)
                    {
                        row.Delete();
                    }

                    #region Update to Server
                    dataAdapter.Update(dataSet);
                    Console.WriteLine("\nO utilizador foi deletado com sucesso.");
                    #endregion
                }
                Console.ReadKey();

                #endregion
            }
            #endregion

        }

    }
}
