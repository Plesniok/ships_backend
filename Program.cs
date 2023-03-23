using Ships.Model;
using Response.Service;
using Ships.Service;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
ShipsService ShipsServiceInstance = new ShipsService();
// app.MapGet("/", () => {
//     var response = new {
//         message = ShipsDBInstance.GetPizzas()
//     };
//     return Results.Json(response);
// });

app.MapPost("/game", (CreateGame requestBody) => {
    
    bool serviceStatus = ShipsServiceInstance
        .CreateGame(requestBody.playerName);
    if(!serviceStatus){
        return Results.Json(
            ResponseService.InfoResponse(
                "Create game service unavailable",
                "10001"
            ),
            statusCode: 503
        );
    }
    return Results.Json(
        ResponseService.CreateSuccessBodyResponse(),
        statusCode:200
    );
});

app.MapPut("/player/two", (AddPlayer requestBody) => {
    
    if(!ShipsServiceInstance.ifTableExist(requestBody.tableName)){
        return Results.Json(
            ResponseService.InfoResponse(
                "Game does not exist",
                "10002"
            ),
            statusCode: 400
        );
    }

    bool serviceStatus = ShipsServiceInstance.UpdatePlayer(
        requestBody.tableName,
        requestBody.playerName
    );
    if(!serviceStatus){
        return Results.Json(
            ResponseService.InfoResponse(
                "Add 2nd player service unavailable",
                "10003"
            ),
            statusCode: 503
        );
    }
    return Results.Json(
        ResponseService.CreateSuccessBodyResponse(),
        statusCode:200
    );
});


app.Run();
