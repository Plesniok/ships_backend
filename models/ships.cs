namespace Ships.Model; 


public record CreateGame 
 { 
    public string ? playerName { get; set; }
 }

public record AddPlayer 
 { 
   public string ? tableName { get; set; }
    public string ? playerName { get; set; }
 }

 public record TableName 
 { 
   public string ? tableName { get; set; }
 }
 public record Ships 
 { 
   public int ? name { get; set; }
 }

 public class PlayerName 
 { 
   public string ? name { get; set; }
 }

 