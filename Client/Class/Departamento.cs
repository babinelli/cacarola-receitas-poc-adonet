using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Client
{

    internal class Departamento
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
            string department = string.Empty;
            do
            {
                // Pede na consola o departamento que o usuário quer inserir
                Console.Write("Novo departamento: ");
                department = Console.ReadLine();

            } while (department == "");
            

            // Consulta se aquele departamento já existe na DB
            List<Department> departments = ConsultDepartment(department);

            // Se departments não tiver elementos, ou seja, se o department ainda não existir na DB, adiciona o novo departamento
            if (departments.Count() == 0) 
            {
                Department departament = new Department
                {
                    DepartmentName = department
                };

                using (var context = new RecipeContext())
                {
                    context.Department.Add(departament);
                    context.SaveChanges();
                }

                Console.WriteLine("\nNovo departamento inserido com sucesso!");
                Console.ReadKey();
            }
            // Se departments tiver algum elemento, ou seja, se o department já existir na DB, não adiciona e mostra mensagem para o usuário
            else
            {
                Console.WriteLine("\nEste departamento já existe.\n\nNão foi possível inserir novo departamento.");
                Console.ReadKey();
            }

        }

        internal static void UpdateDepartment()
        {
            // Pede na consola o departamento a ser alterado
            Console.Write("Departamento antigo: ");
            string oldDepartment = Console.ReadLine();

            // Consulta se o departamento existe na DB
            List<Department> departmentsOld = ConsultDepartment(oldDepartment);

            // Se o oldDepartment existir na DB...
            if (departmentsOld.Count() != 0) 
            {
                // Pede na consola o novo departamento
                Console.Write("Novo departamento: ");
                string newDepartment = Console.ReadLine();

                // Consulta newDepartment departamento já existe
                List<Department> departmentsNew = ConsultDepartment(newDepartment);

                // Se o newDepartment não existir, atualiza a DB
                if (departmentsNew.Count() == 0)
                {
                    using (var context = new RecipeContext())
                    {
                        // Atualiza o dado
                        departmentsOld.First().DepartmentName = newDepartment;
                        context.Department.AddOrUpdate(departmentsOld.First());
                        context.SaveChanges();

                    }

                    Console.WriteLine("\nAlteração realizada com sucesso!");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nO departamento que deseja inserir já existe na DB.\n\nNão foi possível fazer a atualização.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("\nEste departamento não existe.\n\nNão é possível fazer a atualização.");
                Console.ReadKey();
            }

        }

        internal static void SelectDepartments()
        {
            using (var context = new RecipeContext())
            {
                var departments = context.Department.Select(r => r).OrderBy(r => r.DepartmentID);

                foreach (var department in departments)
                {
                    Console.WriteLine($"{department.DepartmentID} - {department.DepartmentName}");
                }
                Console.ReadKey();
            }
        }

        internal static void DeleteDepartment()
        {
            // Pede na consola o departamento a ser deletado
            Console.Write("Qual departamento deseja deletar (nome)? ");
            string department = Console.ReadLine();

            // Consulta se o department existe na DB
            List<Department> departments = ConsultDepartment(department);

            // Se o department existir na DB...
            if (departments.Count() != 0)
            {
                // Confirma se o usuário quer mesmo deletar o department da DB
                Console.WriteLine($"\n\nTem certeza que deseja deletar o departamento {department.ToUpper()} da base de dados?\n\nEssa ação NÃO poderá ser revertida posteriormente.\n\n1 - Sim\n0 - Cancelar ação e voltar ao menu\n\nOpção desejada: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    // Se sim, deleta
                    case "1":
                        using (var context = new RecipeContext())
                        {
                            var query = context.Department.Where(r => r.DepartmentName == department).Select(r => r);
                            context.Department.Remove(query.First());
                            context.SaveChanges();

                            Console.WriteLine("\n\nDepartamento deletado com sucesso.");
                            Console.ReadKey();
                        }
                        break;
                    // Se não, cancela e volta para o menu
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        return;
                }
            }
            // Se o department não existir na DB, informa o usuário
            else
            {
                Console.WriteLine("\nO departamento inserido não existe.");
                Console.ReadKey();
            }
            
        }

        internal static List<Department> ConsultDepartment(string department)
        {
            using (var context = new RecipeContext())
            {
                var departments = context.Department.Select(r => r).Where(r => r.DepartmentName == department);

                return departments.ToList();
            }
        }

    }
}
