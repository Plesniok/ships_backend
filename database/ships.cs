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
    
    public bool ifTableIsLocked(string tableName){

        using var cmd = new NpgsqlCommand(
            $@"SELECT is_locked FROM {tableName}_details;", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        bool ifTableIsLocked = false;
        
    
        while (reader.Read())
        {
            bool is_locked = reader.GetBoolean(0);
            ifTableIsLocked = is_locked;
        }
        return ifTableIsLocked;
    }
    public List<string> GetPlayerById(
        string tableName, 
        string playerName,
        int playerId
    ){

        using var cmd = new NpgsqlCommand(
            $@"SELECT player{playerId} FROM {tableName}_details 
            WHERE player{playerId} = '{playerName}';", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        List<string> result = new List<string>();
    
        while (reader.Read())
        {
            string playerNameRow = reader.GetString(0);
            result.Add(
                playerNameRow
            );
        }
        return result;
    }

    public List<Point> GetPlayerAvailableShips(
        string tableName, 
        int? playerId
    ){

        using var cmd = new NpgsqlCommand(
            $@"SELECT x,y FROM {tableName}_ships 
            WHERE player_id = {playerId}
            AND available = TRUE;", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        List<Point> result = new List<Point>();
    
        while (reader.Read())
        {
            Point shipPoint = 
                new Point(){
                    x = reader.GetInt32(0),
                    y = reader.GetInt32(1)
                };
            result.Add(
                shipPoint
            );
        }
        return result;
    }

    public List<Point> GetPlayerNotAvailableShips(
        string tableName, 
        int? playerId
    ){

        using var cmd = new NpgsqlCommand(
            $@"SELECT x,y FROM {tableName}_ships 
            WHERE player_id = {playerId}
            AND available = FALSE;", 
            this.connection
        );

        using var reader = cmd.ExecuteReader();
        List<Point> result = new List<Point>();
    
        while (reader.Read())
        {
            Point shipPoint = 
                new Point(){
                    x = reader.GetInt32(0),
                    y = reader.GetInt32(1)
                };
            result.Add(
                shipPoint
            );
        }
        return result;
    }
    

    public void CreateGameInfo(string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"CREATE TABLE {tableName}_details (
                player1 TEXT,
                player2 TEXT,
                is_locked BOOLEAN DEFAULT FALSE
            );", this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public void CreateGameShipsInfo(string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"CREATE TABLE {tableName}_ships (
                player_id INTEGER NOT NULL,
                x INTEGER NOT NULL,
                y INTEGER NOT NULL,
                available BOOLEAN DEFAULT TRUE
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

    public void AssignShip(Point ship, int playerIndex, string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"INSERT INTO {tableName}_ships (player_id, x, y) 
            VALUES ({playerIndex}, {ship.x}, {ship.y});", this.connection
        );
        cmd.ExecuteNonQuery();
    }

    public void LockGame(string tableName){
        
        using var cmd = new NpgsqlCommand(
            $@"UPDATE {tableName}_details SET is_locked = true;", 
            this.connection
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
    
    public int DestroyShip(
        string tableName, 
        int? enemyPlayer,
        Point ship
    ){
        
        using var cmd = new NpgsqlCommand(
            $@"UPDATE {tableName}_ships SET available = FALSE
            WHERE x = {ship.x}
            AND y = {ship.y}
            AND player_id = {enemyPlayer}
            AND available = TRUE;", 
            this.connection
        );
        return cmd.ExecuteNonQuery();
    }
}
