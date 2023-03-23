namespace Ships.Service;

using Ships.DB;
using Ships.Model;
using Random.Script;
public class ShipsService{

    ShipsDB DatabaseInstance = new ShipsDB();
    public ShipsService(){
        
    }
    public bool CreateGame(string playerName){
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
            
            return true;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return false;
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
    
    public bool UpdatePlayer(string tableName, string playerName){
        try{
            int playerIndex = 2;

            DatabaseInstance.UpdatePlayer(
                tableName,
                playerName, 
                playerIndex
            );

            return true;
        }
        catch(Exception err){
            Console.WriteLine(err.ToString());
            return false;
        }
    }
}