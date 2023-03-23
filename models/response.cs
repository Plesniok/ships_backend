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

