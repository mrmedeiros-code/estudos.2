using System.Data;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;


namespace testeBanco;


class Connection
{
    // localização da string de conexão
    private static string connection = "Data Source = localhost; Database = master; User ID = sa; Password = ADMIN58454!; Encrypt = True; TrustServerCertificate = True";

    private SqlConnection? conn;

    // objeto com metodo que cria ou retorna a conexão para conn
    public SqlConnection GetSqlConnection()
    {
        if (conn == null)
        {
            conn = new SqlConnection(connection);
        }
        return conn;
    }

    // metodo que testa o estado da conexão
    public void Connect()
    {
        try
        {
            var conn = GetSqlConnection();

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                Console.WriteLine("Conectado.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erro ao tentar se conectar. Erro: {e.Message}");
        }
    }
    // metodo que verifica se a tabela escolhida pelo usuário se encontra no banco
    public bool IfExists(string tableName) 
    {
        var conn = GetSqlConnection();

        using (var cmd = conn.CreateCommand()) {
            cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";

            cmd.Parameters.Add("@tableName", SqlDbType.NVarChar, 128).Value = tableName;

            int count = (int)cmd.ExecuteScalar();
            // se o ExecuteScalar encontrar, o count sera maior que zero
            return count > 0;
        }
    }
}

// classe que contem um construtor primario de conexão
class Create(Connection conn)
{
    // cria uma tabela pré-definida
    public void CreateSimpleTable(string? table)
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"CREATE TABLE {table} (ID INT PRIMARY KEY, Nome NVARCHAR(50), Email NVARCHAR(80))";
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine($"A tabela '{table}' que contem os valores: ID (INT PRYMARY KEY), Nome (NVarchar (50)) e Email (NVarchar (80)); foi criada com sucesso.");
            }
            catch (SqlException e)
            {
                // verifica se o erro é de tabela com o mesmo nome ou não
                if (e.Number == 2714)
                {
                    Console.WriteLine($"A tabela '{table}' já existe.");
                }
                else
                {
                    Console.WriteLine($"Erro: {e.Number}");
                    Console.WriteLine($"Erro: {e.Message}");
                } 
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {e.Message}");
            }
        }
    }
}


// classe de inserção com o construtor primario de conexão
class Insertion(Connection conn)
{
    // metodo com parametros que serão utilizados posteriormente
    public void Insert(string? table, int id, string? nome, string? email)
    {
        var getConn = conn.GetSqlConnection();
        // começo do using
        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"INSERT INTO {table} VALUES (@Id, @Nome, @Email)";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 50).Value = nome;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 80).Value = email;

            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                Console.WriteLine("As informações foram inseridas ao banco");
            }
        } // () Dispose (liberação de memoria)
    }
}

// classe com construtor primario
class Update(Connection conn)
{
    // metodo que atualiza o nome da tabela buscando por tabela e ID
    public void UpdateName(string table, int id, string? nome)
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"UPDATE {table} SET Nome = @nome WHERE ID = @id";
            cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 50).Value = nome;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                Console.WriteLine("Nome atualizado.");
            }
            else
            {
                Console.WriteLine("ID não encontrado. Nenhum valor foi atualizado.");
            }
        }
    }

    // metodo que atualiza o email da tabela buscando por tabela e ID
    public void UpdateEmail(string table, int id, string? email)
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"UPDATE {table} SET Email = @email WHERE ID = @id";
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 80).Value = email;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                Console.WriteLine("Email atualizado.");
            }
            else
            {
                Console.WriteLine("ID não encontrado. Nenhum valor foi atualizado.");
            }
        }
    }
    // metodo que atualiza o nome e email da tabela buscando por tabela e ID
    public void UpdateAll(string table, int id, string? nome, string? email) 
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"UPDATE {table} SET Nome = @nome, Email = @email WHERE ID = @id";
            cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 50).Value = nome;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 80).Value = email;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                Console.WriteLine("Informações atualizadas.");
            }
            else
            {
                Console.WriteLine("ID não encontrado. Nenhum valor foi atualizado.");
            }
        } // Dispose ()
    }
}

// classe com construtor primario
class Delete(Connection conn)
{
    // deleta o item da tabela pesquisando por ID
    public void DeleteOption(string table, int id)
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"DELETE FROM {table} WHERE ID = @id";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                Console.WriteLine($"Foi removido do banco de dados o ID: {id}");
            }
            else
            {
                Console.WriteLine("Nenhum ID foi encontrado.");
            }
        }
    }
    // deleta tabela
    public void DeleteTable(string table)
    {
        var getConn = conn.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"DROP TABLE {table}";
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Foi deletado do banco de dados a tabela: '{table}'");
        }
    }
}

// classe com construtor
class Visual(Connection connection)
{
    // metodo que le e ilustra no terminal os dados da tabela
    public void Visualization(string table)
    {
        var getConn = connection.GetSqlConnection();

        using (var cmd = getConn.CreateCommand())
        {
            cmd.CommandText = $"SELECT * FROM {table}";
            using (var reader = cmd.ExecuteReader()) {
                while(reader.Read()) {
                    int id = reader.GetInt32(0);
                    string nome = reader.GetString(1);
                    string email = reader.GetString(2);

                    Console.WriteLine($"ID: {id} | Nome: {nome} | Email: {email}");
                }
            }
        } // Dispose ()
    }
}
