using System;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;


namespace testeBanco;

class Program
{
    static void Main(string[] args)
    {

        int id;
        string? nome, email;

        var menu = new Menu();
        var connection = new Connection();
        var create = new Create(connection);
        var visual = new Visual(connection);
        var insertion = new Insertion(connection);
        var update = new Update(connection);
        var delete = new Delete(connection);
        connection.Connect();

        while (true)
        {
            int leitura = menu.ShowMenu();

            switch (leitura)
            {
                case 1:
                    Console.Write("Escolha um nome para a nova tabela: ");
                    string? tableName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(tableName) || !System.Text.RegularExpressions.Regex.IsMatch(tableName, "^[a-zA-Z_][a-zA-Z0-9_]*$"))
                    {
                        Console.WriteLine("Por favor, insira um nome válido.");
                    }
                    else
                    {
                        try
                        {
                            create.CreateSimpleTable(tableName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Erro: {e.Message}");
                        }
                    }
                    break;
                case 2:
                    Console.WriteLine("Escolha uma tabela: ");
                    tableName = Console.ReadLine();
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tableName))
                        {
                            Console.WriteLine("Por favor, selecione uma tabela para prosseguir com a inserção de valores.");
                        }
                        else if (!connection.IfExists(tableName))
                        {
                            Console.WriteLine("Essa tabela não existe.");
                        }
                        else
                        {
                            Console.WriteLine($"Você está em: {tableName}.");
                            visual.Visualization(tableName);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro: {e.Message}");
                    }
                    break;
                case 3:
                    Console.Write("Escolha uma tabela: ");
                    tableName = Console.ReadLine();
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tableName))
                        {
                            Console.WriteLine("Por favor, selecione uma tabela para prosseguir com a inserção de valores.");
                        }
                        else if (!connection.IfExists(tableName))
                        {
                            Console.WriteLine("O nome no qual escreveu não existe como tabela");
                        }
                        else
                        {
                            Console.WriteLine($"Você está em: {tableName}");
                            Console.WriteLine("Escreva o ID que deseja inserir: ");

                            string? input = Console.ReadLine();
                            if (int.TryParse(input, out id))
                            {
                                Console.WriteLine("Escreva o Nome que deseja inserir: ");
                                nome = Console.ReadLine();

                                Console.WriteLine("Escreva o Email que deseja inserir: ");
                                email = Console.ReadLine();
                                try
                                {
                                    insertion.Insert(tableName, id, nome, email);
                                }
                                catch (SqlException e)
                                {
                                    if (e.Number == 2627) // 2627 - Código de erro de chave primaria duplicada
                                    {
                                        Console.WriteLine("Este ID já existe.");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Erro inesperado: {e.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID inválido. Tente novamente!");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro: {e.Message}");
                    }
                    break;
                case 4:
                    Console.Write("Escolha uma tabela: ");
                    tableName = Console.ReadLine();
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tableName))
                        {
                            Console.WriteLine("Por favor, selecione uma tabela para prosseguir com a atualização de valores.");
                        }
                        else if (!connection.IfExists(tableName))
                        {
                            Console.WriteLine("O nome no qual escreveu não existe como tabela");
                        }
                        else
                        {
                            Console.WriteLine($"Você está em: {tableName}");
                            int leituraUpdate = menu.ShowUpdateMenu();

                            switch (leituraUpdate)
                            {
                                case 1:
                                    Console.Write("Procure por ID para modificar o nome: ");
                                    string? inputUpdateName = Console.ReadLine();
                                    if (int.TryParse(inputUpdateName, out id))
                                    {
                                        Console.Write("Escreva o novo nome: ");
                                        nome = Console.ReadLine();
                                        try
                                        {
                                            update.UpdateName(tableName, id, nome);
                                        }
                                        catch (SqlException e)
                                        {
                                            Console.WriteLine($"Erro: {e.Message}");
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine($"Erro inesperado: {e.Message}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID inválido. Tente novamente!");
                                    }
                                    break;
                                case 2:
                                    Console.Write("Procure por ID para modificar o Email: ");
                                    string? inputUpdateEmail = Console.ReadLine();
                                    if (int.TryParse(inputUpdateEmail, out id))
                                    {
                                        Console.Write("Escreva o novo Email: ");
                                        email = Console.ReadLine();

                                        try
                                        {
                                            update.UpdateEmail(tableName, id, email);
                                        }
                                        catch (SqlException e)
                                        {
                                            Console.WriteLine($"Erro: {e.Message}");
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine($"Erro inesperado: {e.Message}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID inválido. Tente novamente!");
                                    }
                                    break;
                                case 3:
                                    Console.Write("Procure por ID para modificar o nome e Email: ");
                                    string? inputUpdateAll = Console.ReadLine();
                                    if (int.TryParse(inputUpdateAll, out id))
                                    {
                                        Console.Write("Escreva o novo nome: ");
                                        nome = Console.ReadLine();

                                        Console.Write("Escreva o novo Email: ");
                                        email = Console.ReadLine();

                                        try
                                        {
                                            update.UpdateAll(tableName, id, nome, email);
                                        }
                                        catch (SqlException e)
                                        {
                                            Console.WriteLine($"Erro: {e.Message}");
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine($"Erro inesperado: {e.Message}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID inválido. Tente novamente!");
                                    }
                                    break;
                                case 4:
                                    Console.WriteLine("Retornando...");
                                    break;
                            }
                        }
                    }
                    catch (Exception eL)
                    {
                        Console.WriteLine($"Erro: {eL.Message}");
                    }
                    break;
                case 5:
                    int leituraDelete = menu.ShowDeleteMenu();

                    switch (leituraDelete)
                    {
                        case 1:
                            Console.Write("Escolha uma tabela: ");
                            tableName = Console.ReadLine();
                            try
                            {
                                if (string.IsNullOrWhiteSpace(tableName))
                                {
                                    Console.WriteLine("Por favor, selecione uma tabela para prosseguir com a remoção de valores.");
                                }
                                else if (!connection.IfExists(tableName))
                                {
                                    Console.WriteLine("O nome no qual escreveu não existe como tabela");
                                }
                                else
                                {
                                    Console.WriteLine($"Você está em: {tableName}");
                                    Console.Write("Procure o ID que deseja remover: ");
                                    string? inputDelete = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(inputDelete))
                                    {
                                        Console.WriteLine("Valor inválido. Tente novamente mais tarde!");
                                    }
                                    else
                                    {
                                        Console.Write("Tem certeza? S/n: ");
                                        string? inputCert = Console.ReadLine()?.ToLower();

                                        if (inputCert == "sim" || inputCert == "s")
                                        {
                                            if (int.TryParse(inputDelete, out id))
                                            {
                                                try
                                                {
                                                    delete.DeleteOption(tableName, id);
                                                }
                                                catch (SqlException e)
                                                {
                                                    Console.WriteLine($"Erro: {e.Message}");
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine($"Erro inesperado: {e.Message}");
                                                }
                                            }
                                        }
                                        else if (inputCert == "não" || inputCert == "nao" || inputCert == "n")
                                        {
                                            Console.WriteLine("Operação cancelada. Encerrando...");
                                            return;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Opção não reconhecida. Encerrando...");
                                            return;
                                        }

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Erro: {e.Message}");
                            }
                            break;
                        case 2:
                            Console.Write("Escreva o nome da tabela: ");
                            string? tableDeleted = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(tableDeleted))
                            {
                                Console.WriteLine("Valor inválido. Tente novamente mais tarde!");
                            }
                            else if (!connection.IfExists(tableDeleted))
                            {
                                Console.WriteLine("A tabela que inseriu não existe ou já foi deletada.");
                            }
                            else
                            {
                                Console.Write("Tem certeza? S/n: ");
                                string? inputDelete2 = Console.ReadLine()?.ToLower();
                                if (string.IsNullOrWhiteSpace(inputDelete2))
                                {
                                    Console.WriteLine("Resposta inválido. Tente novamente.");
                                }
                                else if (inputDelete2 == "s" || inputDelete2 == "sim")
                                {

                                    delete.DeleteTable(tableDeleted);
                                }
                                else
                                {
                                    Console.WriteLine("Operação cancelada. Encerrando...");
                                    return;
                                }
                            }
                            break;
                    }
                    break;
                case 6:
                    Console.WriteLine("Encerrando...");
                    return;
            }
        }
    }
}

class Menu
{
    public int ShowMenu()
    {
        while (true)
        {
            Console.Write("""
            ======= Database Manager ========
            
            Escolha uma opção:
                1. Criar uma tabela
                2. Visualizar tabela
                3. Adicionar itens
                4. Atualizar itens
                5. Remover itens
                6. Sair

            Opção: 
            """);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int reader))
            {
                if (reader >= 1 && reader <= 6)
                {
                    return reader;
                }
                else
                {
                    Console.WriteLine("Valor inválido, por favor, digite novamente.");
                }
            }
        }

    }

    public int ShowUpdateMenu()
    {
        while (true)
        {
            Console.Write("""
            Escolha uma opção de atualização:
                1. Atualizar Nome
                2. Atualizar Email
                3. Atualizar Nome e Email
                4. Sair
            
            Opção: 
            """);

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int readerUpdate))
            {
                if (readerUpdate >= 1 && readerUpdate <= 4)
                {
                    return readerUpdate;
                }
                else
                {
                    Console.WriteLine("Valor inválido. Tente novamente!");
                }
            }
        }
    }

    public int ShowDeleteMenu()
    {
        while (true)
        {
            Console.Write("""
            Escolha o que deseja deletar:
                1. Deletar um valor dentro de uma tabela
                2. Deletar uma tabela
                3. Sair

            Opção: 
            """);

            string? input = Console.ReadLine();
            if (int.TryParse(input, out int readerDelete))
            {
                if (readerDelete >= 1 && readerDelete <= 3)
                {
                    return readerDelete;
                }
                else
                {
                    Console.WriteLine("Valor inválido. Tente novamente!");
                }
            }
        }
    }

}
