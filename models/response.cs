namespace Response.Model;

public record SuccessBodyResponse 
    { 
        // public string ? tableName { get; set; }
    }
public record InfoResponse 
    { 
        public string ? ErrorMessage { get; set; }
        public string ? ErrorCode { get; set; }
    }

public record SuccessCreateGameResponse 
    { 
        public string ? NewGameCode { get; set; }
    }

public record SuccessGetPlayerByName 
    { 
        public int ? playerIndex { get; set; }
    }
