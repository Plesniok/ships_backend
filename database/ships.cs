namespace Ships.DB; 

using Npgsql;
using Ships.Model;

public class ShipsDB
 {
   public NpgsqlConnection connection { get; set; }
   
   public ShipsDB(){
        var connString = "Server=localhost;Port=5432;Database=ships;User Id=postgres;Password=passwd123;";
        this.connection = new NpgsqlConnection(connString);
        this.connection.Open();
   }

    public List<TableName> GetAllTables(){
        
        using var cmd = new NpgsqlCommand(
            "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        List<TableName> result = new List<TableName>();
    
        while (reader.Read())
        {
            string tableName = reader.GetString(0);
            result.Add(
                new TableName{tableName = tableName}
            );
        }
        return result;
    }

    public List<TableName> GetTable(string tableName){
        using var cmd = new NpgsqlCommand(
            $@"SELECT table_name FROM information_schema.tables 
            WHERE table_schema = 'public' 
            AND table_type = 'BASE TABLE'
            AND table_name = '{tableName}_details';", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        List<TableName> result = new List<TableName>();
    
        while (reader.Read())
        {
            string tableNameRow = reader.GetString(0);
            result.Add(
                new TableName{tableName = $"{tableNameRow}"}
            );
        }
        return result;
    }
    
    public void CreateGameInfo(string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"CREATE TABLE {tableName}_details (
                player1 TEXT,
                player2 TEXT
            );", this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public void CreateGameShipsInfo(string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"CREATE TABLE {tableName}_ships (
                player1 TEXT,
                x INTEGER,
                y INTEGER
            );", this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public void AddPlayer(string tableName, string newPlayerName, int playerIndex){
        
        using var cmd = new NpgsqlCommand(
            $@"INSERT INTO {tableName}_details (player{playerIndex}) 
            VALUES ('{newPlayerName}');", this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public void UpdatePlayer(string tableName, string newPlayerName, int playerIndex){
        
        using var cmd = new NpgsqlCommand(
            $@"UPDATE {tableName}_details SET player{playerIndex} = '{newPlayerName}';", 
            this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public List<Ships> GetPizzas() 
        {
            using var cmd = new NpgsqlCommand("SELECT * FROM test_table", this.connection);
            using var reader = cmd.ExecuteReader();
            List<Ships> result = new List<Ships>();
            while (reader.Read())
            {
                int value = reader.GetInt32(0);
                result.Add(
                    new Ships{name = value}
                );
            }
            return result;
        } 
    }