﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;


namespace Client
{

    public class Program
    {

        static void Main(string[] args)
        {
            //try
            //{
                string option;
                do
                {
                    Console.Clear();
                    Console.WriteLine("------------------------------\nRECIPE DB MANAGER\n------------------------------");
                    Console.WriteLine("Tabelas de RecipeDb \n1 - User\n2 - Department\n0 - SAIR");
                    Console.Write("\nQual tabela deseja gerir? ");
                    option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            Utilizador.MenuUser();
                            break;
                        case "2":
                            Departamento.MenuDepartment();
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
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    Console.ReadKey();
            //}

        //}

    }
}
