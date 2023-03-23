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
    

}