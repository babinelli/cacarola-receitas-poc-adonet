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

    internal class Department
    {

        internal static void MenuDepartment()
        {
            string option;
            do
            {
                Console.Clear();
                Console.WriteLine("------------------------------\nRECIPE DB MANAGER - DEPARTMENT\n------------------------------");
                Console.Write("1 - Inserir departamento\n2 - Atualizar departamento\n3 - Listar departamentos\n4 - Deletar departamento\n0 - SAIR\n\nQual operação deseja efetuar? ");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("------------------------------\nINSERIR DEPARTAMENTO\n------------------------------");
                        InsertDepartment();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("------------------------------\nATUALIZAR DEPARTAMENTO\n------------------------------");
                        UpdateDepartment();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("------------------------------\nLISTAR DEPARTAMENTO\n------------------------------");
                        SelectDepartments();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("------------------------------\nDELETAR DEPARTAMENTO\n------------------------------");
                        DeleteDepartment();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        break;
                }

            } while (option != "0");
        }

        internal static void InsertDepartment()
        {
            #region New department from console
            Console.Clear();
            Console.WriteLine("------------------------------\nINSERIR DEPARTAMENTO\n------------------------------");

            Console.Write("\nDepartamento: ");
            string department = Console.ReadLine();
            #endregion

            #region Support variables
            string querySelect = "SELECT * FROM [dbo].[Department];";
            string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();
            #endregion

            #region Connection String
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
                dataAdapter.Fill(dataSet, "Department");
                #endregion

                #region DataRow
                DataRow dataRow;
                dataRow = dataSet.Tables[0].NewRow();
                dataRow["DepartmentName"] = department;
                dataSet.Tables[0].Rows.Add(dataRow);
                #endregion

                #region SqlCommandBuilder + Update
                commandBuilder.GetDeleteCommand();
                commandBuilder.DataAdapter.Update(dataSet.Tables[0]);
                Console.WriteLine("\nDepartamento inserido com sucesso.");
                Console.ReadKey();
                #endregion

            }
            #endregion
        }

        internal static void UpdateDepartment()
        {
            Console.Clear();
            Console.WriteLine("------------------------------\nATUALIZAR DEPARTAMENTO\n------------------------------");
            Console.Write("Qual o DepartmentID do departamento que deseja atualizar? ");
            string departmentID = Console.ReadLine();

            string querySelectByDepartmentID = "SELECT * FROM [dbo].[Department] WHERE [DepartmentID] = '" + departmentID + "';";
            string connectionString = ConfigurationManager.AppSettings.Get("csRecipeDB").ToString();

            Console.Write("Novo departamento: ");
            string department = Console.ReadLine();
            string queryUpdateName = "UPDATE [dbo].[Department] SET [DepartmentName] = '" + department + "' WHERE [DepartmentID] = '" + departmentID + "';";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                #region ConnectionString
                SqlDataAdapter dataAdapter = new SqlDataAdapter(querySelectByDepartmentID, sqlConnection);
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
                dataSet.Tables[0].Rows[0]["DepartmentName"] = "";
                dataAdapter.Update(dataSet);
                Console.WriteLine("Alteração realizada com sucesso.");
                #endregion

            }
            Console.ReadKey();
        }

        internal static void SelectDepartments()
        {
            #region Support variables
            List<string> departmentList = new List<string>();

            string querySelect = "SELECT * FROM [dbo].[Department] ORDER BY [DepartmentID];";
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
                    departmentList.Add(dataTableReader[0].ToString() + "\t| " + dataTableReader[1].ToString());

                }
                #endregion

                #region Show data
                Console.Clear();
                Console.WriteLine("------------------------------\nLISTAR DEPARTAMENTOS\n------------------------------");
                Console.WriteLine("DepartmentID\t| Departamento ");
                foreach (string item in departmentList)
                {
                    Console.WriteLine(item);
                }
                Console.ReadKey();
                #endregion

            }
            #endregion
        }

        internal static void DeleteDepartment()
        {

            #region Delete user from console
            Console.Clear();
            Console.WriteLine("------------------------------\nDELETAR DEPARTAMENTO\n------------------------------");

            Console.Write("Qual o DepartmentID do departamento que deseja deletar? ");
            string departmentID = Console.ReadLine();
            #endregion

            #region Support variables
            string querySelectByUserID = "SELECT * FROM [dbo].[Department];";
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
                string filter = "[DepartmentID] = '" + departmentID + "'";
                DataRow[] rows = dataSet.Tables[0].Select(filter);

                if (rows == null || rows.Length == 0) // Se vetor rows for nulo ou vazio
                {
                    Console.WriteLine("\nNão existe nenhum departamento com esse ID.");
                }
                else
                {
                    foreach (DataRow row in rows)
                    {
                        row.Delete();
                    }

                    #region Update to Server
                    dataAdapter.Update(dataSet);
                    Console.WriteLine("\nO departamento foi deletado com sucesso.");
                    #endregion
                }
                Console.ReadKey();

                #endregion
            }
            #endregion


        }

    }
}
