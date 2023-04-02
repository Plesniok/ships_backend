namespace Ships.Service;

using Ships.DB;
using Ships.Model;
using Random.Script;
public class ShipsService{

    ShipsDB DatabaseInstance = new ShipsDB();
    public ShipsService(){
        
    }
    public TableName CreateGame(string playerName, List<Point> ships){
        try{
            TableName NewTableName = new TableName();
            NewTableName.tableName = "1";
            List<TableName> allTables = DatabaseInstance.GetAllTables();
            int playerIndex = 1;

            NewTableName.tableName = RandomScript.GetRandomTableName();
            
            while(allTables.Contains(NewTableName)){
                NewTableName.tableName = RandomScript.GetRandomTableName();
            }

            this.DatabaseInstance.CreateGameInfo(NewTableName.tableName);
            this.DatabaseInstance.CreateGameShipsInfo(NewTableName.tableName);
            
            this.DatabaseInstance.AddPlayer(
                NewTableName.tableName,
                playerName,
                playerIndex
            );

            foreach(Point ship in ships){
                DatabaseInstance.AssignShip(
                    ship,
                    playerIndex,
                    NewTableName.tableName
                );
            }
            
            return NewTableName;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return new TableName();
        }
    }
    public bool ifTableExist(string tableName){
        try{
            List<TableName> ifTableExist = DatabaseInstance.GetTable(tableName);
            if(ifTableExist.Count == 0){
                return false;
            }

            return true;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return false;
        }
    }
    

    public bool ifTableIsLocked(string tableName){
        try{
            

            return DatabaseInstance.ifTableIsLocked(tableName);
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return false;
        }
    }
    public int? GetPlayerIdByName(string tableName, string playerName){
        try{
            int[] playerIndexes = {1,2}; 
            int correctIndex = 0;
            
            foreach (int id in playerIndexes){
                List<string> ifPlayerExists = DatabaseInstance.GetPlayerById(
                    tableName,
                    playerName,
                    id
                );
                if(ifPlayerExists.Count() > 0){
                    correctIndex = id;
                }
            }


            return correctIndex;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return null;
        }
    }
    public bool UpdatePlayer(string tableName, string playerName, List<Point> ships){
        try{
            int playerIndex = 2;

            DatabaseInstance.UpdatePlayer(
                tableName,
                playerName, 
                playerIndex
            );
            
            DatabaseInstance.LockGame(
                tableName
            );

            foreach(Point ship in ships){
                DatabaseInstance.AssignShip(
                    ship,
                    2,
                    tableName
                );
            }

            return true;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return false;
        }
    }
}