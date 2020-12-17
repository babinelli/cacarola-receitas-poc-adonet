using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Text.RegularExpressions;

namespace Client
{

    internal class Utilizador
    {
        internal static void MenuUser()
        {
            Utilizador user = new Utilizador();
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
                        user.InsertUser();
                        break;
                    case "2":
                        user.UpdateUser();
                        break;
                    case "3":
                        MenuSelectUsers();
                        break;
                    case "4":
                        user.DeleteUser();
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

        internal void InsertUser()
        {
            Console.Clear();
            Console.WriteLine("------------------------------\nINSERIR USUÁRIO\n------------------------------");


            string name = string.Empty;
            do
            {
                Console.Write("\nNome: ");
                name = Console.ReadLine();

            } while (name == ""); // Enquanto nome for vazio

            string lastName = string.Empty;
            do
            {
                Console.Write("\nApelido: ");
                lastName = Console.ReadLine();

            } while (lastName == ""); // Enquanto apelido for vazio

            string email;
            List<User> usersEmail;
            do
            {
                Console.Write("\nE-mail: ");
                email = Console.ReadLine();

                usersEmail = ConsultEmail(email);


            } while (!(ValidateEmail(email)) || usersEmail.Count() != 0); // Enquanto e-mail não for validado, ou já existir na base de dados

            string username = string.Empty;
            List<User> usersUsername;
            do
            {
                Console.Write("\nUsername: ");
                username = Console.ReadLine();
                usersUsername = ConsultUsername(username);

            } while (usersUsername.Count() != 0 || username == ""); // Enquanto username existir na base de dados ou username for vazio

            string password;
            do
            {
                Console.Write("\nPalavra-passe: ");
                password = Console.ReadLine();

            } while (!(ValidatePassword(password))); // Enquanto password não for validada

            // Falta pedir um departamento

            string department = string.Empty;
            int departmentID = 0;
            bool exist;
            do
            {
                Console.Write("\nNome do Departamento: ");
                department = Console.ReadLine();

                List<Department> departments = Departamento.ConsultDepartment(department);
                if (departments.Count() != 0)
                {
                    departmentID = departments.First().DepartmentID;
                    exist = true;
                }
                else
                {
                    Console.WriteLine("O departamento inserido não existe.");
                    Console.ReadKey();
                    exist = false;

                }

            } while (department == "" || !exist); // Enquanto department for vazio, ID não existir na tabela department

            User user = new User
            {
                Name = name,
                LastName = lastName,
                Email = email,
                Username = username,
                Password = password,
                Admin = false, // Por definição, usuários do tipo admin só podem ser inseridos internamente, no DAL
                DepartmentID = departmentID
            };

            using (var context = new RecipeContext())
            {
                context.User.Add(user);
                context.SaveChanges();
            }

            Console.WriteLine("\nNovo usuário inserido com sucesso!");
            Console.ReadKey();
        }

        internal void UpdateUser()
        {

            Console.Clear();
            // Pergunta na consola como deseja identificar o usuário? UserID, Username ou Email e retorna os users encontrados (deve ser apenas 1, ja que ID, username e e-mail devem ser únicos)
            List<User> users = IdentifyUserBy("------------------------------\nATUALIZAR DADOS DE USUÁRIO\n------------------------------");

            // Se retornar algum usuário...
            if (users != null && users.Count() != 0)
            {
                Console.Clear();
                Console.WriteLine("------------------------------\nATUALIZAR DADOS DE USUÁRIO\n------------------------------");

                // Mostra os dados do user na consola, exceto a Password
                Console.WriteLine("\nUser ID\t| Nome\t| Apelido\t| E-mail\t| Username");
                Console.WriteLine($"{users.First().UserID}\t| {users.First().Name}\t| {users.First().LastName}\t| {users.First().Email}\t| {users.First().Username}");

                string optionUpdate;
                do
                {
                    // Pergunta na consola qual o campo que deseja atualizar
                    Console.Write("\nCampos para atualização\n1 - Nome\n2 - Apelido\n3 - E-mail\n4 - Username\n5 - Password\n6 - Departamento\n0 - Voltar ao menu inicial\n\nQual campo deseja atualizar? ");
                    optionUpdate = Console.ReadLine();

                    // AddOrUpdate para atualizar os dados
                    switch (optionUpdate)
                    {
                        case "1":
                            Console.Write("Novo nome: ");
                            string name = Console.ReadLine();

                            using (var context = new RecipeContext())
                            {
                                users.First().Name = name;
                                context.User.AddOrUpdate(users.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("\nAlteração realizada com sucesso!");
                            break;

                        case "2":
                            Console.Write("Novo apelido: ");
                            string lastName = Console.ReadLine();

                            using (var context = new RecipeContext())
                            {
                                users.First().LastName = lastName;
                                context.User.AddOrUpdate(users.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("\nAlteração realizada com sucesso!");
                            break;

                        case "3":
                            string email;
                            List<User> usersEmail;
                            do
                            {
                                Console.Write("\nNovo e-mail: ");
                                email = Console.ReadLine();
                                usersEmail = ConsultEmail(email);

                            } while (!(ValidateEmail(email)) || usersEmail.Count() != 0); // Enquanto email não for validado, ou existir na BD

                            using (var context = new RecipeContext())
                            {
                                users.First().Email = email;
                                context.User.AddOrUpdate(users.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("\nAlteração realizada com sucesso!");
                            break;

                        case "4":
                            string username;
                            List<User> usersUsername;
                            do
                            {
                                Console.Write("Novo username: ");
                                username = Console.ReadLine();
                                usersUsername = ConsultUsername(username);

                            } while (usersUsername.Count() != 0); // Enquanto username existir na BD

                            using (var context = new RecipeContext())
                            {
                                users.First().Username = username;
                                context.User.AddOrUpdate(users.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("\nAlteração realizada com sucesso!");
                            break;

                        case "5":
                            Console.Write("Digite a senha atual: ");
                            string oldPassword = Console.ReadLine();

                            if (MatchPasswords(oldPassword, users))
                            {
                                string newPassword;
                                do
                                {
                                    Console.Write("Digite a nova senha: ");
                                    newPassword = Console.ReadLine();

                                } while (!(ValidatePassword(newPassword))); // Enquanto password não for validada

                                using (var context = new RecipeContext())
                                {
                                    users.First().Password = newPassword;
                                    context.User.AddOrUpdate(users.First());
                                    context.SaveChanges();
                                }

                                Console.WriteLine("\nAlteração realizada com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("\nNão foi possível realizar a alteração.");
                            }
                            break;
                        case "6":
                            string newDepartment;
                            List<Department> departments;
                            do
                            {
                                Console.Write("Novo departamento (nome): ");
                                newDepartment = Console.ReadLine();
                                departments = Departamento.ConsultDepartment(newDepartment);
                                if (departments.Count() == 0)
                                {
                                    Console.WriteLine("O departamento inserido não existe.");
                                    Console.ReadKey();
                                }

                            } while (departments.Count() == 0); // Enquanto o novo departamento não existir na DB

                            // Pega o ID do novo departamento
                            int departmentID = Convert.ToInt32(departments.First().DepartmentID);

                            using (var context = new RecipeContext())
                            {
                                users.First().DepartmentID = departmentID;
                                context.User.AddOrUpdate(users.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("\nAlteração realizada com sucesso!");

                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                    Console.ReadKey();
                } while (optionUpdate != "0");

            }

        }

        internal static void MenuSelectUsers()
        {
            string option;
            do
            {
                Console.Clear();
                Console.WriteLine("------------------------------\nLISTAR USUÁRIOS\n------------------------------");
                Console.WriteLine("O que deseja listar?\n1 - Todos os usuários\n2 - Apenas admin\n3 - Apenas usuários regulares\n0 - Voltar ao menu inicial");
                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SelectAllUsers();
                        break;
                    case "2":
                        SelectAdmins();
                        break;
                    case "3":
                        SelectRegularUsers();
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

        internal static void SelectAllUsers()
        {
            Console.Clear();
            using (var context = new RecipeContext())
            {
                IQueryable<User> users = context.User.Select(r => r).OrderBy(r => r.UserID);

                Console.WriteLine("------------------------------\nTODOS OS USUÁRIOS\n------------------------------");
                Console.WriteLine("User ID\t| Nome\t| Apelido\t| E-mail\t| Username");
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.UserID}\t| {user.Name}\t| {user.LastName}\t| {user.Email}\t| {user.Username}");
                }
            }
            Console.ReadKey();
        }

        internal static void SelectAdmins()
        {
            Console.Clear();
            using (var context = new RecipeContext())
            {
                IQueryable<User> users = context.User.Select(r => r).Where(r => r.Admin == true).OrderBy(r => r.UserID);

                Console.WriteLine("------------------------------\nUSUÁRIOS - ADMIN\n------------------------------");
                Console.WriteLine("User ID\t| Nome\t| Apelido\t| E-mail\t| Username");
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.UserID}\t| {user.Name}\t| {user.LastName}\t| {user.Email}\t| {user.Username}");
                }
            }
            Console.ReadKey();
        }

        internal static void SelectRegularUsers()
        {
            Console.Clear();
            using (var context = new RecipeContext())
            {
                IQueryable<User> users = context.User.Select(r => r).Where(r => r.Admin == false).OrderBy(r => r.UserID);

                Console.WriteLine("------------------------------\nUSUÁRIOS - NÃO ADMIN\n------------------------------");
                Console.WriteLine("User ID\t| Nome\t| Apelido\t| E-mail\t| Username");
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.UserID}\t| {user.Name}\t| {user.LastName}\t| {user.Email}\t| {user.Username}");
                }
            }
            Console.ReadKey();
        }

        internal static void SelectUsersByDepartment()
        {
            Console.Clear();
            Console.WriteLine("------------------------------\nUSUÁRIOS POR DEPARTAMENTO\n------------------------------");
            // Mostra todos os departamentos na consola
            Departamento.SelectDepartments();

            // Pergunta na consola de qual departamento o usuário deseja ver os users
            Console.WriteLine("Deseja ver os usuários de qual departamento?");
            string department = Console.ReadLine();

            // Verifica se o departamento desejado existe e retorna
            List<Department> departments = Departamento.ConsultDepartment(department);

            // Se o department existir
            if (departments.Count() != 0)
            {
                // Seleciona o ID do departamento desejado (primeiro e único item em departments)
                int departmentID = Convert.ToInt32(departments.First().DepartmentID);

                IQueryable<User> usersDepartment;
                using (var context = new RecipeContext())
                {
                    // Seleciona os users do departamento escolhido, a partir do DepartmentID
                    usersDepartment = context.User.Select(r => r).Where(r => r.DepartmentID == departmentID);
                }

                // Se existirem usuários daquele departamento
                if (usersDepartment.Count() != 0)
                {
                    Console.WriteLine("\nUser ID\t| Nome\t| Apelido\t| E-mail\t| Username\t| Departamento");
                    foreach (var user in usersDepartment)
                    {
                        Console.WriteLine($"{user.UserID}\t| {user.Name}\t| {user.LastName}\t| {user.Email}\t| {user.Username}\t| {departments.First().DepartmentName}");
                    }
                }

            }
            // Se o department não existir
            else
            {
                Console.WriteLine("O departamento inserido não existe.");
            }

            Console.ReadKey();
        }

        internal void DeleteUser()
        {
            Console.Clear();
            List<User> users = IdentifyUserBy("------------------------------\nDELETAR USUÁRIO\n------------------------------");

            if (users != null && users.Count() != 0)
            {
                string departmentName;
                int departmentId = users.First().DepartmentID;
                using (var context = new RecipeContext())
                {
                    // Seleciona o DepartmentName do usuário a partir da tabela Department
                    departmentName = context.Department.Where(r => r.DepartmentID == departmentId).Select(r => r.DepartmentName).First();
                }
                string option;
                do
                {
                    Console.Clear();
                    Console.WriteLine("------------------------------\nDELETAR USUÁRIO\n------------------------------");
                    Console.WriteLine("User ID\t| Nome\t| Apelido\t| E-mail\t| Username\t| Departamento");
                    Console.WriteLine($"{users.First().UserID}\t|{users.First().Name}\t|{users.First().LastName}\t|{users.First().Email}\t|{users.First().Username}\t|{departmentName}");

                    Console.WriteLine("\nTem certeza que deseja deletar o usuário acima da base de dados?\n\nEssa ação NÃO poderá ser revertida posteriormente.\n\n1 - Sim\n0 - Cancelar ação e voltar ao menu inicial\n\nOpção desejada: ");
                    option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            using (var context = new RecipeContext())
                            {
                                int userID = users.First().UserID;
                                var query= context.User.Where(r => r.UserID == userID).Select(r => r);
                                context.User.Remove(query.First());
                                context.SaveChanges();
                            }

                            Console.WriteLine("Usuário deletado com sucesso.");
                            Console.ReadKey();
                            return;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }

                } while (option != "0");
            }
        }

        internal static List<User> ConsultEmail(string email)
        {
            using (var context = new RecipeContext())
            {
                var users = context.User.Select(r => r).Where(r => r.Email == email);

                if (users.Any())
                {
                    Console.WriteLine("Esse e-mail já esta registado.");
                }
                return users.ToList();
            }
        }

        internal static List<User> ConsultUsername(string username)
        {
            using (var context = new RecipeContext())
            {
                var users = context.User.Select(r => r).Where(r => r.Username == username);
                if (users.Any())
                {
                    Console.WriteLine("Esse username já esta registado.");
                }
                return users.ToList();
            }
        }

        internal static bool MatchPasswords(string oldPassword, List<User> users)
        {
            string actualPassword = Convert.ToString(users.Select(r => r.Password));
            if (actualPassword == oldPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (regex.IsMatch(email))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Formato de e-mail inválido.");
                return false;
            }
        }

        internal static bool ValidatePassword(string password)
        {
            Regex regex = new Regex(@"(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

            if (regex.IsMatch(password))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Palavra-passe inválida.\nA palava-passe deve conter no mínimo 8 caracteres, incluindo letras maiúsculas e minúsculas, números e caracteres especiais.");
                return false;
            }
        }

        internal static List<User> IdentifyUserBy(string header)
        {
            string optionID;
            List<User> users;
            do
            {
                Console.Clear();
                Console.WriteLine(header);
                // Apenas ID, Username e E-mail, pois são unique, ou seja, não há mais de um usuário com esses dados iguais
                Console.Write("\n1 - UserID\n2 - Username\n3 - E-mail\n0 - SAIR\n\nComo deseja identificar o usuário? ");
                optionID = Console.ReadLine();

                switch (optionID)
                {
                    case "1":
                        users = ChooseUserByID();
                        break;
                    case "2":
                        users = ChooseUserByUsername();
                        break;
                    case "3":
                        users = ChooseUserByEmail();
                        break;
                    case "0":
                        return null;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Console.ReadKey();
                        users = null;
                        break;
                }
                if (users == null || users.Count() == 0)
                {
                    Console.WriteLine("Usuário não encontrado.");
                    Console.ReadKey();
                }
                
                return users;
            } while (optionID != "0");

        }

        internal static List<User> ChooseUserByID()
        {
            Console.Write("\nQual o ID do usuário? ");
            int userID;

            if (int.TryParse(Console.ReadLine(), out userID))
            {
                using (var context = new RecipeContext())
                {
                    var users = context.User.Select(r => r).Where(r => r.UserID == userID);

                    return users.ToList();
                }
            }
            else
            {
                Console.WriteLine("Valor inválido.");
                Console.ReadKey();
                return null;
            }

        }

        internal static List<User> ChooseUserByUsername()
        {
            Console.Write("\nQual o username do usuário? ");
            string username = Console.ReadLine();

            using (var context = new RecipeContext())
            {
                var users = context.User.Where(r => r.Username == username).Select(r => r);

                return users.ToList();


            }
        }

        internal static List<User> ChooseUserByEmail()
        {
            Console.Write("\nQual o e-mail do usuário? ");
            string email = Console.ReadLine();

            using (var context = new RecipeContext())
            {
                var users = context.User.Where(r => r.Email == email).Select(r => r);

                return users.ToList();


            }
        }

    }
}
