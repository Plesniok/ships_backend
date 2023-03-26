namespace Response.Service;

using Response.Model;

public class ResponseService {

    public static SuccessBodyResponse CreateSuccessBodyResponse(){
        return new SuccessBodyResponse();
    }
    public static InfoResponse InfoResponse(string message, string code){
        return new InfoResponse(){
            ErrorMessage = message,
            ErrorCode = code
        };
    }

    public static SuccessCreateGameResponse CreateGameResponse(string tableName){
        return new SuccessCreateGameResponse(){
            NewGameCode = tableName
        };
    }
    
    public static SuccessGetPlayerByName CreateGetPlayerByNameResponse(int? playerIndex){
        return new SuccessGetPlayerByName(){
            playerIndex = playerIndex
        };
    }

}